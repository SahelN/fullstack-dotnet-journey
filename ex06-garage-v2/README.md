# Garage 2.0 – Code Review

A console application that simulates a parking facility with several garages. You can park and remove vehicles, list what is parked, find a vehicle by registration number, and filter on one or more properties.

## How to run

**Visual Studio:** open `GarageApp.slnx` and press **F5**.

**Terminal:** `dotnet run --project ex06-garage-v2/src/GarageApp` (requires .NET 10)

### Quick test

1. Enter a size for each garage, for example `2` for all four.
2. Answer `y` to add 6 test vehicles automatically.
3. Choose a garage and try the menu.

| Reg. no. | Type | Color | Specific property |
|---|---|---|---|
| ABC123 | Car | Black | Gasoline |
| DEF456 | Car | Red | Electric |
| MCP001 | Motorcycle | Pink | 600 cc |
| BUS100 | Bus | Yellow | 50 seats |
| BOAT01 | Boat | White | 8.5 m |
| SEABC1 | Airplane | White | 2 engines |

## Classes and responsibilities

| Class | Responsibility |
|---|---|
| `Vehicle` (abstract) | Shared properties: `RegistrationNumber`, `Color`, `NumberOfWheels`. Validates input in the constructor. |
| `Car`, `Motorcycle`, `Bus`, `Boat`, `Airplane` | One specific property each (`FuelType`, `CylinderVolume`, `NumberOfSeats`, `Length`, `NumberOfEngines`). |
| `Garage` | Owns a private `List<Vehicle>` and the garage's rules: capacity, allowed types and unique registration numbers. Parks, removes, searches and counts. |
| `ConsoleUI` | All user interaction: menus, input, output and error messages. Holds the list of garages. |
| `ParkResult`, `VehicleTypeCount`, `Program` | Small helpers: why parking failed, type + count for the type list, and the entry point. |

## Design decisions

- **`Garage` never writes to the console.** It returns data and `ConsoleUI` decides how to show it.
- **Encapsulation:** `GetAllVehicles()` and `AllowedVehicleTypes` return copies, so the UI cannot bypass the checks in `Park()`.
- **No arrays, generics or LINQ in `Garage`**, as the assignment requires. `Contains`, `AddRange` and `RemoveAt` are `List<T>`'s own methods.

## Extra features

- Four garages with different allowed types: Garage (Car, Bus), Motorcycle garage, Hangar and Harbor. You can switch between them.
- Filtering on vehicle-specific properties as well.
- The filter only asks about properties that can exist in the selected garage (no fuel type in the Hangar, no wheels in the Harbor).

## Known limitations

- Decimal numbers follow the computer's culture: on Swedish Windows, type a boat's length as `8,5`.
- The registration number format is not validated yet, only that it is not empty. I am aware of this and plan to add the validation later.

## I would like feedback on

1. `ConsoleUI` is about 630 lines in one static class. Would you split it, and into which classes?
2. Adding a new vehicle type requires changes in five places (subclass, `ParkVehicle`, `Matches()`, `Search()`, filter questions). How would you make that easier?
3. Allowed vehicle types are strings like `"Car"` compared with `GetType().Name`. Would an `enum` or `Type` be better?
4. `Search()` takes 8 nullable parameters. Would a `SearchCriteria` class be clearer?
5. Capacity is counted in number of vehicles. How would you handle vehicles that take up different amounts of space?
6. Can you find any input that crashes the program or gives an unclear error message?
