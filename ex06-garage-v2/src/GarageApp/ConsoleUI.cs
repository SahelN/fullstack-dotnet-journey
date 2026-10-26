using GarageApp.Vehicles;
using System;
using System.Collections.Generic;

namespace GarageApp
{
    public static class ConsoleUI
    {
        public static void Run()
        {
            Console.WriteLine("=== Garage App ===");
            Console.WriteLine();

            List<Garage> garages = CreateGarages();

            Console.WriteLine();
            Console.WriteLine("You can start with 6 ready-made test vehicles (you don't need to type anything).");
            if (ReadYesNo("Do you want me to add them? (y/n): "))
            {
                AddSampleVehicles(garages);
            }

            Console.WriteLine();
            int currentIndex = SwitchGarage(garages, -1);   // -1 = no garage chosen yet

            bool running = true;
            while (running)
            {
                Garage current = garages[currentIndex];

                PrintMenu(current);
                int choice = ReadInt("Choose an option: ", 0, 7);
                Console.WriteLine();

                switch (choice)
                {
                    case 1:
                        ListAllVehicles(current);
                        break;
                    case 2:
                        ListVehicleTypes(current);
                        break;
                    case 3:
                        ParkVehicle(garages, current);
                        break;
                    case 4:
                        RemoveVehicle(current);
                        break;
                    case 5:
                        FindVehicle(garages, current);
                        break;
                    case 6:
                        SearchVehicles(current);
                        break;
                    case 7:
                        currentIndex = SwitchGarage(garages, currentIndex);
                        break;
                    case 0:
                        running = false;
                        break;
                }

                if (running)
                {
                    Console.WriteLine();
                    Console.Write("Press Enter to continue...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Goodbye!");
        }

        // ---------- Setup ----------
        private static List<Garage> CreateGarages()
        {
            // Fixed garages: the user only chooses the size
            List<string> names = new List<string> { "Garage", "Motorcycle garage", "Hangar", "Harbor" };
            List<List<string>> allowedTypes = new List<List<string>>
            {
                new List<string> { "Car", "Bus" },
                new List<string> { "Motorcycle" },
                new List<string> { "Airplane" },
                new List<string> { "Boat" }
            };

            Console.WriteLine($"This parking facility has {names.Count} garages. Each garage accepts different vehicle types:");
            Console.WriteLine();

            Console.WriteLine($"     {"Name",-18} {"Vehicle types"}");
            Console.WriteLine($"     {new string('-', 18)} {new string('-', 13)}");

            for (int i = 0; i < names.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {names[i],-18} {FormatTypes(allowedTypes[i])}");
            }

            Console.WriteLine();
            Console.WriteLine("Please choose the number of parking spaces for each garage (1-1000).");
            Console.WriteLine();

            List<Garage> garages = new List<Garage>();
            int totalSpaces = 0;

            for (int i = 0; i < names.Count; i++)
            {
                int capacity = ReadInt($"  {names[i] + ":",-19}", 1, 1000);
                garages.Add(new Garage(names[i], capacity, allowedTypes[i]));
                totalSpaces += capacity;
            }

            Console.WriteLine();
            Console.WriteLine($"All {garages.Count} garages were created ({totalSpaces} parking spaces in total).");
            return garages;
        }

        // ---------- Menu ----------
        private static void PrintMenu(Garage current)
        {
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine($" {current.Name.ToUpperInvariant()}  ({current.FreeSpaces} of {current.Capacity} spaces free)");
            Console.WriteLine($" Accepts: {FormatTypes(current.AllowedVehicleTypes)}");
            Console.WriteLine("========================================");
            Console.WriteLine(" 1. List all parked vehicles");
            Console.WriteLine(" 2. List vehicle types and counts");
            Console.WriteLine(" 3. Park a vehicle");
            Console.WriteLine(" 4. Remove a vehicle");
            Console.WriteLine(" 5. Find a vehicle by registration number");
            Console.WriteLine(" 6. Filter vehicles by properties");
            Console.WriteLine(" 7. Switch garage");
            Console.WriteLine(" 0. Exit");
            Console.WriteLine("----------------------------------------");
        }

        // ---------- Menu actions ----------
        private static void ListAllVehicles(Garage garage)
        {
            Console.WriteLine($"Parked vehicles in {garage.Name}:");
            PrintVehicles(garage.GetAllVehicles());
        }

        private static void ListVehicleTypes(Garage garage)
        {
            List<VehicleTypeCount> typeCounts = garage.GetVehicleTypeCounts();

            if (typeCounts.Count == 0)
            {
                Console.WriteLine($"{garage.Name} is empty.");
                return;
            }

            Console.WriteLine($"Vehicle types in {garage.Name}:");
            foreach (VehicleTypeCount typeCount in typeCounts)
            {
                Console.WriteLine($"  {typeCount.TypeName,-11} {typeCount.Count}");
            }
        }

        private static void ParkVehicle(List<Garage> garages, Garage garage)
        {
            // Check early so the user doesn't type everything for nothing
            if (garage.IsFull)
            {
                Console.WriteLine($"{garage.Name} is full. Remove a vehicle or switch garage first.");
                return;
            }

            // Only show the vehicle types this garage accepts
            List<string> allowedTypes = garage.AllowedVehicleTypes;
            string vehicleType;

            if (allowedTypes.Count == 1)
            {
                vehicleType = allowedTypes[0];
                Console.WriteLine($"Vehicle type: {vehicleType}");
            }
            else
            {
                Console.WriteLine($"Vehicle types in {garage.Name}:");
                for (int i = 0; i < allowedTypes.Count; i++)
                {
                    Console.WriteLine($"  {i + 1}. {allowedTypes[i]}");
                }
                int typeChoice = ReadInt($"Choose vehicle type (1-{allowedTypes.Count}): ", 1, allowedTypes.Count);
                vehicleType = allowedTypes[typeChoice - 1];
            }

            string registrationNumber = ReadRequiredString("Registration number: ");

            // Registration numbers must be unique in ALL garages, not just this one
            Garage? garageWithVehicle = FindGarageWithVehicle(garages, registrationNumber);
            if (garageWithVehicle != null)
            {
                Console.WriteLine($"A vehicle with registration number {registrationNumber.ToUpperInvariant()} is already parked in {garageWithVehicle.Name}.");
                return;
            }

            string color = ReadRequiredString("Color: ");

            Vehicle vehicle;
            try
            {
                switch (vehicleType)
                {
                    case "Car":
                        int carWheels = ReadInt("Number of wheels (0-20): ", 0, 20);
                        FuelType fuelType = ReadFuelType();
                        vehicle = new Car(registrationNumber, color, carWheels, fuelType);
                        break;
                    case "Motorcycle":
                        int motorcycleWheels = ReadInt("Number of wheels (0-20): ", 0, 20);
                        int cylinderVolume = ReadInt("Cylinder volume in cc (1-10000): ", 1, 10000);
                        vehicle = new Motorcycle(registrationNumber, color, motorcycleWheels, cylinderVolume);
                        break;
                    case "Bus":
                        int busWheels = ReadInt("Number of wheels (0-20): ", 0, 20);
                        int numberOfSeats = ReadInt("Number of seats (1-200): ", 1, 200);
                        vehicle = new Bus(registrationNumber, color, busWheels, numberOfSeats);
                        break;
                    case "Boat":
                        double length = ReadPositiveDouble("Length in meters: ");
                        vehicle = new Boat(registrationNumber, color, length);
                        break;
                    case "Airplane":
                        int airplaneWheels = ReadInt("Number of wheels (0-20): ", 0, 20);
                        int numberOfEngines = ReadInt("Number of engines (1-10): ", 1, 10);
                        vehicle = new Airplane(registrationNumber, color, airplaneWheels, numberOfEngines);
                        break;
                    default:
                        Console.WriteLine($"Unknown vehicle type: {vehicleType}");
                        return;
                }
            }
            catch (ArgumentException ex)
            {
                // Safety net: the Vehicle constructors validate their input
                Console.WriteLine($"Could not create the vehicle: {ex.Message}");
                return;
            }

            ParkResult result = garage.Park(vehicle);
            PrintParkResult(result, vehicle, garage);
        }

        private static void RemoveVehicle(Garage garage)
        {
            string registrationNumber = ReadRequiredString("Registration number of the vehicle to remove: ");

            if (garage.Remove(registrationNumber))
            {
                Console.WriteLine($"{registrationNumber.ToUpperInvariant()} was removed from {garage.Name}.");
            }
            else
            {
                Console.WriteLine($"No vehicle with registration number {registrationNumber.ToUpperInvariant()} was found in {garage.Name}.");
            }
        }

        private static void FindVehicle(List<Garage> garages, Garage garage)
        {
            string registrationNumber = ReadRequiredString("Registration number: ");
            Vehicle? vehicle = garage.FindByRegistrationNumber(registrationNumber);

            if (vehicle != null)
            {
                Console.WriteLine($"Vehicle found in {garage.Name}:");
                Console.WriteLine($"  {vehicle}");
                return;
            }

            Console.WriteLine($"No vehicle with registration number {registrationNumber.ToUpperInvariant()} was found in {garage.Name}.");

            // Helpful hint: is it parked somewhere else?
            Garage? otherGarage = FindGarageWithVehicle(garages, registrationNumber);
            if (otherGarage != null)
            {
                Console.WriteLine($"Tip: it is parked in {otherGarage.Name}. Use option 7 to switch garage.");
            }
        }

        private static void SearchVehicles(Garage garage)
        {
            Console.WriteLine($"Filter vehicles in {garage.Name}. Leave a field empty to ignore it.");
            Console.WriteLine();

            List<string> allowedTypes = garage.AllowedVehicleTypes;

            Console.WriteLine("Common properties:");

            // Only ask for type if this garage has more than one type to choose from
            string? vehicleType = null;
            if (allowedTypes.Count > 1)
                vehicleType = ReadOptionalVehicleType(allowedTypes);

            // Which types can still match? The chosen one, or all types this garage accepts
            List<string> typesToAsk = new List<string>();
            if (vehicleType != null)
                typesToAsk.Add(vehicleType);
            else
                typesToAsk.AddRange(allowedTypes);

            string? color = ReadOptionalString("  Color: ");

            // Boats always have 0 wheels, so the question is pointless if only boats can match
            int? numberOfWheels = null;
            bool onlyBoats = typesToAsk.Count == 1 && typesToAsk[0] == "Boat";
            if (!onlyBoats)
                numberOfWheels = ReadOptionalInt("  Number of wheels: ");

            // Only ask for specific properties of types that can actually be in this garage
            Console.WriteLine();
            Console.WriteLine("Specific properties:");

            FuelType? fuelType = null;
            int? numberOfSeats = null;
            int? cylinderVolume = null;
            int? numberOfEngines = null;
            double? length = null;

            if (typesToAsk.Contains("Car"))
                fuelType = ReadOptionalFuelType();

            if (typesToAsk.Contains("Bus"))
                numberOfSeats = ReadOptionalInt("  Number of seats (Bus): ");

            if (typesToAsk.Contains("Motorcycle"))
                cylinderVolume = ReadOptionalInt("  Cylinder volume in cc (Motorcycle): ");

            if (typesToAsk.Contains("Airplane"))
                numberOfEngines = ReadOptionalInt("  Number of engines (Airplane): ");

            if (typesToAsk.Contains("Boat"))
                length = ReadOptionalPositiveDouble("  Length in meters (Boat): ");

            if (vehicleType == null && color == null && numberOfWheels == null &&
                fuelType == null && numberOfSeats == null && cylinderVolume == null &&
                numberOfEngines == null && length == null)
            {
                Console.WriteLine("You did not enter any filter. Use option 1 to list all vehicles.");
                return;
            }

            List<Vehicle> result = garage.Search(vehicleType, color, numberOfWheels,
                                                 fuelType, numberOfSeats, cylinderVolume,
                                                 numberOfEngines, length);

            Console.WriteLine();
            Console.WriteLine($"Search result in {garage.Name} ({result.Count} found):");
            PrintVehicles(result);
        }

        private static int SwitchGarage(List<Garage> garages, int currentIndex)
        {
            Console.WriteLine("Choose a garage:");
            Console.WriteLine();

            Console.WriteLine($"     {"Name",-18} {"Vehicle types",-13} Free spaces");
            Console.WriteLine($"     {new string('-', 18)} {new string('-', 13)} {new string('-', 18)}");

            for (int i = 0; i < garages.Count; i++)
            {
                Garage garage = garages[i];
                string marker = i == currentIndex ? "  <- current" : "";
                Console.WriteLine($"  {i + 1}. {garage.Name,-18} {FormatTypes(garage.AllowedVehicleTypes),-13} " +
                                  $"({garage.FreeSpaces} of {garage.Capacity}){marker}");
            }

            Console.WriteLine();
            int choice = ReadInt($"Choose garage (1-{garages.Count}): ", 1, garages.Count);
            int newIndex = choice - 1;

            Console.WriteLine($"You are now working in: {garages[newIndex].Name}");
            return newIndex;
        }

        // ---------- Sample data ----------
        private static void AddSampleVehicles(List<Garage> garages)
        {
            List<Vehicle> samples = new List<Vehicle>
            {
                new Car("ABC123", "Black", 4, FuelType.Gasoline),
                new Car("DEF456", "Red", 4, FuelType.Electric),
                new Motorcycle("MCP001", "Pink", 3, 600),
                new Bus("BUS100", "Yellow", 6, 50),
                new Boat("BOAT01", "White", 8.5),
                new Airplane("SEABC1", "White", 3, 2)
            };

            int parkedCount = 0;
            foreach (Vehicle sample in samples)
            {
                // Park each vehicle in the first garage that accepts its type
                foreach (Garage garage in garages)
                {
                    if (garage.Accepts(sample))
                    {
                        if (garage.Park(sample) == ParkResult.Success)
                            parkedCount++;
                        break;
                    }
                }
            }

            Console.WriteLine($"{parkedCount} of {samples.Count} test vehicles were added automatically:");
            foreach (Garage garage in garages)
            {
                List<Vehicle> vehicles = garage.GetAllVehicles();
                if (vehicles.Count > 0)
                {
                    Console.WriteLine($" {garage.Name}:");
                    PrintVehicles(vehicles);
                }
            }
        }

        // ---------- Helpers ----------
        private static Garage? FindGarageWithVehicle(List<Garage> garages, string registrationNumber)
        {
            foreach (Garage garage in garages)
            {
                if (garage.FindByRegistrationNumber(registrationNumber) != null)
                    return garage;
            }
            return null;
        }

        private static string FormatTypes(List<string> types)
        {
            return string.Join(", ", types);
        }

        // ---------- Output helpers ----------
        private static void PrintVehicles(List<Vehicle> vehicles)
        {
            if (vehicles.Count == 0)
            {
                Console.WriteLine("  No vehicles found.");
                return;
            }

            foreach (Vehicle vehicle in vehicles)
            {
                Console.WriteLine($"  {vehicle}");
            }
        }

        private static void PrintParkResult(ParkResult result, Vehicle vehicle, Garage garage)
        {
            switch (result)
            {
                case ParkResult.Success:
                    Console.WriteLine($"{vehicle.RegistrationNumber} was parked in {garage.Name}.");
                    break;
                case ParkResult.GarageFull:
                    Console.WriteLine($"Could not park {vehicle.RegistrationNumber}: {garage.Name} is full.");
                    break;
                case ParkResult.DuplicateRegistrationNumber:
                    Console.WriteLine($"Could not park {vehicle.RegistrationNumber}: a vehicle with that registration number is already parked.");
                    break;
                case ParkResult.WrongVehicleType:
                    Console.WriteLine($"Could not park {vehicle.RegistrationNumber}: {garage.Name} only accepts {FormatTypes(garage.AllowedVehicleTypes)}.");
                    break;
            }
        }

        // ---------- Input helpers (never crash on bad input) ----------
        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                if (int.TryParse(input, out int value) && value >= min && value <= max)
                    return value;

                Console.WriteLine($"Please enter a whole number between {min} and {max}.");
            }
        }

        private static double ReadPositiveDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                if (double.TryParse(input, out double value) && value > 0)
                    return value;

                Console.WriteLine("Please enter a number greater than zero.");
            }
        }

        private static string ReadRequiredString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("This field is required.");
            }
        }

        private static string? ReadOptionalString(string prompt)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(input))
                return null;

            return input.Trim();
        }

        // Numbered list instead of free text, so a typo like "Cars" can't happen
        private static string? ReadOptionalVehicleType(List<string> types)
        {
            for (int i = 0; i < types.Count; i++)
            {
                Console.WriteLine($"    {i + 1}. {types[i]}");
            }

            while (true)
            {
                Console.Write($"  Vehicle type (1-{types.Count}, empty = all): ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (input == string.Empty)
                    return null;

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= types.Count)
                    return types[choice - 1];

                Console.WriteLine($"  Please enter a number between 1 and {types.Count}, or leave it empty.");
            }
        }

        private static int? ReadOptionalInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (int.TryParse(input, out int value) && value >= 0)
                    return value;

                Console.WriteLine("Please enter a whole number, or leave it empty.");
            }
        }

        private static double? ReadOptionalPositiveDouble(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (double.TryParse(input, out double value) && value > 0)
                    return value;

                Console.WriteLine("  Please enter a number greater than zero, or leave it empty.");
            }
        }

        private static bool ReadYesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

                if (input == "y" || input == "yes")
                    return true;
                if (input == "n" || input == "no")
                    return false;

                Console.WriteLine("Please answer y or n.");
            }
        }

        private static FuelType ReadFuelType()
        {
            Console.WriteLine("Fuel type: 1. Gasoline  2. Diesel  3. Electric");
            int choice = ReadInt("Choose fuel type: ", 1, 3);

            switch (choice)
            {
                case 1:
                    return FuelType.Gasoline;
                case 2:
                    return FuelType.Diesel;
                default:
                    return FuelType.Electric;
            }
        }

        private static FuelType? ReadOptionalFuelType()
        {
            while (true)
            {
                Console.Write("  Fuel type (Car) 1. Gasoline  2. Diesel  3. Electric: ");
                string input = (Console.ReadLine() ?? string.Empty).Trim();

                if (input == string.Empty)
                    return null;

                switch (input)
                {
                    case "1":
                        return FuelType.Gasoline;
                    case "2":
                        return FuelType.Diesel;
                    case "3":
                        return FuelType.Electric;
                }

                Console.WriteLine("  Please enter 1, 2 or 3, or leave it empty.");
            }
        }
    }
}