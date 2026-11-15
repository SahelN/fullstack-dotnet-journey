# Databaser – Övning 7: Triggers

Skapa en tabell testTrigger och gör en INSERT-trigger, en UPDATE-trigger och en INSTEAD OF-trigger. Testa att de fungerar.

## Lösning

Jag skapade tabellen i databasen Shop.

```sql
CREATE TABLE testTrigger (
    FirstName VARCHAR(50),
    LastName  VARCHAR(50)
);
```

## 1. INSERT-trigger

![Skapa triggern info](images/01-create-info.png)

![INSERT - Results](images/02-insert-results.png)

![INSERT - Messages](images/03-insert-messages.png)

## 2. UPDATE-trigger

![Skapa triggern modify](images/04-create-modify.png)

![UPDATE - Results](images/05-update-results.png)

![UPDATE - Messages](images/06-update-messages.png)

## 3. INSTEAD OF-trigger

![Skapa triggern stopInsert](images/07-create-stopinsert.png)

![INSTEAD OF - Results](images/08-insteadof-results.png)

![INSTEAD OF - Messages](images/09-insteadof-messages.png)
