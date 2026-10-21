using GarageApp.Vehicles;
using System;

namespace GarageApp
{
    public class Garage
    {
        private readonly Vehicle?[] _vehicles;
        private readonly string[] _allowedVehicleTypes;

        public string Name { get; }

        public int Capacity
        {
            get { return _vehicles.Length; }
        }

        // Returns a copy, so nobody outside can change the garage's rules
        public string[] AllowedVehicleTypes
        {
            get { return _allowedVehicleTypes[..]; }
        }

        public int ParkedVehicleCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _vehicles.Length; i++)
                {
                    if (_vehicles[i] != null)
                        count++;
                }
                return count;
            }
        }

        public int FreeSpaces
        {
            get { return Capacity - ParkedVehicleCount; }
        }

        public bool IsFull
        {
            get { return ParkedVehicleCount == Capacity; }
        }

        public Garage(string name, int capacity, string[] allowedVehicleTypes)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Garage name is required.", nameof(name));

            if (capacity < 1)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");

            ArgumentNullException.ThrowIfNull(allowedVehicleTypes);

            if (allowedVehicleTypes.Length == 0)
                throw new ArgumentException("At least one vehicle type is required.", nameof(allowedVehicleTypes));

            for (int i = 0; i < allowedVehicleTypes.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(allowedVehicleTypes[i]))
                    throw new ArgumentException("Vehicle types cannot be empty.", nameof(allowedVehicleTypes));
            }

            Name = name.Trim();
            _vehicles = new Vehicle?[capacity];
            _allowedVehicleTypes = allowedVehicleTypes[..];   // keep our own copy
        }

        // Can this garage take this kind of vehicle?
        public bool Accepts(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            string typeName = vehicle.GetType().Name;

            for (int i = 0; i < _allowedVehicleTypes.Length; i++)
            {
                if (string.Equals(_allowedVehicleTypes[i], typeName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public ParkResult Park(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            // The UI only offers allowed types, but Garage must protect its own rules
            if (!Accepts(vehicle))
                return ParkResult.WrongVehicleType;

            int freeIndex = -1;

            for (int i = 0; i < _vehicles.Length; i++)
            {
                Vehicle? current = _vehicles[i];

                if (current == null)
                {
                    // Remember the first free space, but keep looking for duplicates
                    if (freeIndex == -1)
                        freeIndex = i;
                }
                else if (current.RegistrationNumber == vehicle.RegistrationNumber)
                {
                    return ParkResult.DuplicateRegistrationNumber;
                }
            }

            if (freeIndex == -1)
                return ParkResult.GarageFull;

            _vehicles[freeIndex] = vehicle;
            return ParkResult.Success;
        }

        public bool Remove(string registrationNumber)
        {
            string normalizedNumber = NormalizeRegistrationNumber(registrationNumber);

            for (int i = 0; i < _vehicles.Length; i++)
            {
                Vehicle? current = _vehicles[i];

                if (current != null && current.RegistrationNumber == normalizedNumber)
                {
                    _vehicles[i] = null;
                    return true;
                }
            }
            return false;
        }

        public Vehicle? FindByRegistrationNumber(string registrationNumber)
        {
            string normalizedNumber = NormalizeRegistrationNumber(registrationNumber);

            for (int i = 0; i < _vehicles.Length; i++)
            {
                Vehicle? current = _vehicles[i];

                if (current != null && current.RegistrationNumber == normalizedNumber)
                    return current;
            }
            return null;
        }

        public Vehicle[] GetAllVehicles()
        {
            Vehicle[] temp = new Vehicle[_vehicles.Length];
            int count = 0;

            for (int i = 0; i < _vehicles.Length; i++)
            {
                Vehicle? current = _vehicles[i];
                if (current != null)
                {
                    temp[count] = current;
                    count++;
                }
            }

            return temp[..count];
        }

        public VehicleTypeCount[] GetVehicleTypeCounts()
        {
            // There can never be more different types than parking spaces
            string[] typeNames = new string[_vehicles.Length];
            int[] counts = new int[_vehicles.Length];
            int numberOfTypes = 0;

            for (int i = 0; i < _vehicles.Length; i++)
            {
                Vehicle? current = _vehicles[i];
                if (current == null)
                    continue;

                string typeName = current.GetType().Name;

                int typeIndex = -1;
                for (int t = 0; t < numberOfTypes; t++)
                {
                    if (typeNames[t] == typeName)
                    {
                        typeIndex = t;
                        break;
                    }
                }

                if (typeIndex == -1)
                {
                    typeNames[numberOfTypes] = typeName;
                    counts[numberOfTypes] = 1;
                    numberOfTypes++;
                }
                else
                {
                    counts[typeIndex]++;
                }
            }

            VehicleTypeCount[] result = new VehicleTypeCount[numberOfTypes];
            for (int t = 0; t < numberOfTypes; t++)
            {
                result[t] = new VehicleTypeCount(typeNames[t], counts[t]);
            }
            return result;
        }

        // null means "ignore this property"
        public Vehicle[] Search(string? vehicleType, string? color, int? numberOfWheels,
                                FuelType? fuelType, int? numberOfSeats, int? cylinderVolume,
                                int? numberOfEngines, double? length)
        {
            Vehicle[] temp = new Vehicle[_vehicles.Length];
            int count = 0;

            for (int i = 0; i < _vehicles.Length; i++)
            {
                Vehicle? current = _vehicles[i];
                if (current != null && Matches(current, vehicleType, color, numberOfWheels,
                                               fuelType, numberOfSeats, cylinderVolume,
                                               numberOfEngines, length))
                {
                    temp[count] = current;
                    count++;
                }
            }

            return temp[..count];
        }

        private static bool Matches(Vehicle vehicle, string? vehicleType, string? color, int? numberOfWheels,
                                    FuelType? fuelType, int? numberOfSeats, int? cylinderVolume,
                                    int? numberOfEngines, double? length)
        {
            // ---------- Common properties (all vehicles) ----------
            if (!string.IsNullOrWhiteSpace(vehicleType) &&
                !string.Equals(vehicle.GetType().Name, vehicleType.Trim(), StringComparison.OrdinalIgnoreCase))
                return false;

            if (!string.IsNullOrWhiteSpace(color) &&
                !string.Equals(vehicle.Color, color.Trim(), StringComparison.OrdinalIgnoreCase))
                return false;

            if (numberOfWheels.HasValue && vehicle.NumberOfWheels != numberOfWheels.Value)
                return false;

            // ---------- Specific properties (only one vehicle type has each) ----------
            // If a filter is set, the vehicle must be that type AND have the right value

            if (fuelType.HasValue &&
                !(vehicle is Car car && car.FuelType == fuelType.Value))
                return false;

            if (numberOfSeats.HasValue &&
                !(vehicle is Bus bus && bus.NumberOfSeats == numberOfSeats.Value))
                return false;

            if (cylinderVolume.HasValue &&
                !(vehicle is Motorcycle motorcycle && motorcycle.CylinderVolume == cylinderVolume.Value))
                return false;

            if (numberOfEngines.HasValue &&
                !(vehicle is Airplane airplane && airplane.NumberOfEngines == numberOfEngines.Value))
                return false;

            // double: compare with a small tolerance, never with == (8.5 can be stored as 8.4999999...)
            if (length.HasValue &&
                !(vehicle is Boat boat && Math.Abs(boat.Length - length.Value) < 0.01))
                return false;

            return true;
        }

        private static string NormalizeRegistrationNumber(string registrationNumber)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber))
                return string.Empty;

            // Remove spaces and make uppercase so " abc123 " matches ABC123
            return registrationNumber.Trim().ToUpperInvariant();
        }
    }
}