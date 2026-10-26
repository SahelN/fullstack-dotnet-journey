using GarageApp.Vehicles;
using System;
using System.Collections.Generic;

namespace GarageApp
{
    public class Garage
    {
        private readonly List<Vehicle> _vehicles = new List<Vehicle>();
        private readonly List<string> _allowedVehicleTypes = new List<string>();

        public string Name { get; }

        public int Capacity { get; }

        // Returns a copy, so nobody outside can change the garage's rules
        public List<string> AllowedVehicleTypes
        {
            get { return new List<string>(_allowedVehicleTypes); }
        }

        public int ParkedVehicleCount
        {
            get { return _vehicles.Count; }
        }

        public int FreeSpaces
        {
            get { return Capacity - ParkedVehicleCount; }
        }

        public bool IsFull
        {
            get { return ParkedVehicleCount == Capacity; }
        }

        public Garage(string name, int capacity, List<string> allowedVehicleTypes)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Garage name is required.", nameof(name));

            if (capacity < 1)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");

            ArgumentNullException.ThrowIfNull(allowedVehicleTypes);

            if (allowedVehicleTypes.Count == 0)
                throw new ArgumentException("At least one vehicle type is required.", nameof(allowedVehicleTypes));

            foreach (string type in allowedVehicleTypes)
            {
                if (string.IsNullOrWhiteSpace(type))
                    throw new ArgumentException("Vehicle types cannot be empty.", nameof(allowedVehicleTypes));
            }

            Name = name.Trim();
            Capacity = capacity;
            _allowedVehicleTypes.AddRange(allowedVehicleTypes);   // keep our own copy
        }

        // Can this garage take this kind of vehicle?
        public bool Accepts(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            string typeName = vehicle.GetType().Name;

            foreach (string allowedType in _allowedVehicleTypes)
            {
                if (string.Equals(allowedType, typeName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public ParkResult Park(Vehicle vehicle)
        {
            ArgumentNullException.ThrowIfNull(vehicle);

            if (IsFull)
                return ParkResult.GarageFull;

            // The UI only offers allowed types, but Garage must protect its own rules
            if (!Accepts(vehicle))
                return ParkResult.WrongVehicleType;

            if (IndexOf(vehicle.RegistrationNumber) != -1)
                return ParkResult.DuplicateRegistrationNumber;

            _vehicles.Add(vehicle);
            return ParkResult.Success;
        }

        public bool Remove(string registrationNumber)
        {
            int index = IndexOf(registrationNumber);
            if (index == -1)
                return false;   // not found

            _vehicles.RemoveAt(index);
            return true;
        }

        public Vehicle? FindByRegistrationNumber(string registrationNumber)
        {
            int index = IndexOf(registrationNumber);
            if (index == -1)
                return null;

            return _vehicles[index];
        }

        public List<Vehicle> GetAllVehicles()
        {
            return new List<Vehicle>(_vehicles);
        }

        public List<VehicleTypeCount> GetVehicleTypeCounts()
        {
            // Key = type name (e.g. "Car"), value = how many of that type are parked
            Dictionary<string, int> counts = new Dictionary<string, int>();

            foreach (Vehicle vehicle in _vehicles)
            {
                string typeName = vehicle.GetType().Name;

                if (counts.ContainsKey(typeName))
                    counts[typeName]++;
                else
                    counts[typeName] = 1;
            }

            List<VehicleTypeCount> result = new List<VehicleTypeCount>();
            foreach (KeyValuePair<string, int> pair in counts)
            {
                result.Add(new VehicleTypeCount(pair.Key, pair.Value));
            }
            return result;
        }

        // null means "ignore this property"
        public List<Vehicle> Search(string? vehicleType, string? color, int? numberOfWheels,
                                    FuelType? fuelType, int? numberOfSeats, int? cylinderVolume,
                                    int? numberOfEngines, double? length)
        {
            List<Vehicle> result = new List<Vehicle>();

            foreach (Vehicle vehicle in _vehicles)
            {
                if (Matches(vehicle, vehicleType, color, numberOfWheels,
                            fuelType, numberOfSeats, cylinderVolume,
                            numberOfEngines, length))
                {
                    result.Add(vehicle);
                }
            }

            return result;
        }

        // One place for "find the position of this registration number" (used by Park, Remove and Find)
        private int IndexOf(string registrationNumber)
        {
            string normalizedNumber = NormalizeRegistrationNumber(registrationNumber);

            for (int i = 0; i < _vehicles.Count; i++)
            {
                if (_vehicles[i].RegistrationNumber == normalizedNumber)
                    return i;
            }
            return -1;
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