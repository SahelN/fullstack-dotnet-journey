# API – Övning 1: Lagersystem (StorageApi)

ASP.NET Core Web API (.NET 10) med Entity Framework Core och SQL Server (LocalDB). API:et hanterar produkter i ett lager med full CRUD via `/api/products`.

## Struktur

| Mapp/fil | Innehåll |
|---|---|
| `Models/Product.cs` | Entiteten Product (Id, Name, Price, Category, Shelf, Count, Description) |
| `Data/StorageContext.cs` | DbContext med `DbSet<Product> Products` |
| `Controllers/ProductsController.cs` | API-controller med CRUD, statistik och sökning (scaffoldad, sedan refaktorerad till DTOs) |
| `DTOs/` | `CreateProductDto`, `ProductDto` och `ProductStatsDto` |
| `Migrations/` | Migrationen `Init` som skapar tabellen `Products` |

## Git-arbetsflöde
Varje del gjordes i en egen feature-gren som mergades till `main` via pull request:
- `feature/model-storage` – modellen (Del 2)
- `feature/controller-storage` – controller och DbContext (Del 3)
- `feature/migration-database` – migration och databas (Del 4)

Del 6–9 committades direkt på `main`, en commit per del.

## Del 4 – Migration och databas
Jag körde `Add-Migration Init` och `Update-Database` i Package Manager Console. EF skapade databasen `StorageContext` med tabellerna `Products` och `__EFMigrationsHistory`. Sedan lade jag in några produkter manuellt.

![Databasen och tabellen Products i SQL Server Object Explorer](images/01-sql-object-explorer.png)

![Produkter i tabellen Products](images/02-products-data.png)

## Del 5 – Efter scaffold

### Vilka metoder har genererats?

| Metod | HTTP | Route | Svar |
|---|---|---|---|
| `GetProduct()` | GET | `/api/products` | 200 OK med alla produkter |
| `GetProduct(int id)` | GET | `/api/products/{id}` | 200 OK, eller 404 om den inte finns |
| `PutProduct(int id, Product product)` | PUT | `/api/products/{id}` | 204 NoContent, 400 om id inte matchar, 404 om den inte finns |
| `PostProduct(Product product)` | POST | `/api/products` | 201 Created |
| `DeleteProduct(int id)` | DELETE | `/api/products/{id}` | 204 NoContent, eller 404 |
| `ProductExists(int id)` | – | privat hjälpmetod | används av PUT |

### Hur används StorageContext?
`StorageContext` registreras i `Program.cs` med `AddDbContext` och `UseSqlServer`, med connection-strängen från `appsettings.json`. Den är scoped, vilket betyder en ny instans per HTTP-anrop. Controllern får den via constructor injection.

`_context.Products` motsvarar tabellen `Products`. EF översätter anropen till SQL: `ToListAsync()` blir en SELECT, `FindAsync(id)` en SELECT med WHERE, och `Add`/`Remove` ändrar bara i minnet tills `SaveChangesAsync()` skickar INSERT/UPDATE/DELETE till databasen.

### Hur fungerar CreatedAtAction, Ok, NotFound osv?
Det är hjälpmetoder i `ControllerBase` som returnerar ett HTTP-svar med rätt statuskod:
- `Ok(x)` – 200. När en metod returnerar `ActionResult<T>` räcker `return product;`, det blir automatiskt 200.
- `NotFound()` – 404, resursen finns inte.
- `BadRequest()` – 400, felaktigt anrop.
- `NoContent()` – 204, lyckades men inget innehåll att skicka tillbaka.
- `CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product)` – 201, skickar med den nya produkten och en `Location`-header med adressen där den kan hämtas.

### Test i Postman

GET alla produkter – 200 OK:

![GET /api/products](images/03-get-all.png)

GET en produkt – 200 OK:

![GET /api/products/3](images/04-get-by-id.png)

GET en produkt som inte finns – 404 Not Found:

![GET /api/products/999](images/05-get-not-found.png)

POST ny produkt – 201 Created:

![POST /api/products](images/06-post-created.png)

### Debuggning
Jag satte en brytpunkt i `PostProduct` och såg att `product.Id` är 0 innan produkten sparas. Id sätts först av databasen (IDENTITY) när `SaveChangesAsync()` körs, och syns sedan i svaret (201 Created).

![Debuggning av PostProduct](images/07-debug-post.png)

## Del 6 – DTOs

I mappen `DTOs/` finns:

| Klass | Används till |
|---|---|
| `CreateProductDto` | Input vid POST och PUT. Samma fält som `Product` men utan `Id`, eftersom databasen sätter det. |
| `ProductDto` | Output till klienten: `Id`, `Name`, `Price`, `Count` och det beräknade fältet `InventoryValue => Price * Count`. |
| `ProductStatsDto` | Svaret från `/api/products/stats`. |

`CreateProductDto` har valideringsattribut: `[Required]` och `[StringLength]` på textfälten, och `[Range]` på `Price` och `Count` så att de inte kan vara negativa. Eftersom controllern har `[ApiController]` valideras modellen automatiskt. Är något fel returneras `400 Bad Request` med felmeddelandena, och koden i action-metoden körs inte.

## Del 7 – Använd DTOs

Controllern tar nu emot `CreateProductDto` och returnerar `ProductDto` i stället för entiteten `Product`:
- **GET**: `Select(p => new ProductDto { ... })` gör att EF bara hämtar de kolumner som behövs.
- **POST**: DTO:n mappas till en ny `Product`, som sparas. Svaret är `201 Created` med en `ProductDto`.
- **PUT**: Den befintliga produkten hämtas och fälten uppdateras från DTO:n. Id tas från URL:en, så klienten kan inte ändra det (skydd mot overposting).
- Mappningen entitet → DTO ligger i hjälpmetoden `ToDto()`.

Fördelar: klienten ser bara det den behöver (`Shelf`, `Category` och `Description` skickas inte ut i svaren), klienten kan inte sätta `Id` själv, och databasmodellen kan ändras utan att API-kontraktet ändras.

## Del 8 – Swagger UI

1. Nugetpaketet `Swashbuckle.AspNetCore.SwaggerUi` är installerat.
2. I `Program.cs` pekar `app.UseSwaggerUI(...)` på OpenAPI-dokumentet `/openapi/v1.json`, som `MapOpenApi()` redan publicerar.
3. I `launchSettings.json` har `"launchBrowser": true` och `"launchUrl": "swagger"` lagts till, så att Swagger öppnas direkt när projektet startar.

## Del 9 – Mer funktionalitet

### Statistik – `GET /api/products/stats`
Returnerar:
- `totalProducts` – antal produkter (rader i tabellen)
- `totalItemsInStock` – totalt antal varor i lager (summan av `Count`)
- `totalInventoryValue` – totalt lagervärde (summan av `Price * Count`)
- `averagePrice` – snittpris, avrundat till två decimaler

Beräkningarna görs i databasen med `CountAsync`, `SumAsync` och `AverageAsync`. `AverageAsync` på en tom tabell kastar annars ett exception, så jag castar till `double?` och använder `?? 0`.

### Sökning via querystring
- `GET /api/products?category=Verktyg` – filtrerar på kategori
- `GET /api/products?name=ham` – produkter vars namn innehåller texten (valfri del)
- `GET /api/products?category=Verktyg&name=ham` – kombinerad sökning (valfri del)

Filtren läggs på en `IQueryable` bara när parametern har ett värde. Därför skickas allt som en enda SQL-fråga med `WHERE`. SQL Servers standardkollation gör att sökningen inte är skiftlägeskänslig.

Routen `{id:int}` har en int-constraint, så att `stats` aldrig kan tolkas som ett id.
