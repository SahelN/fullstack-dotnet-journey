namespace EmployeeRegistry
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PersonnelRegistry registry = new PersonnelRegistry();

            Console.WriteLine("=== Personnel Registry ===");

            while (true)
            {
                string? name = ReadName();
                if (name is null)
                    break;

                decimal salary = ReadSalary();

                registry.AddEmployee(new Employee(name, salary));
                Console.WriteLine($"{name} was added.");
                Console.WriteLine();
            }

            Console.WriteLine();
            registry.PrintEmployees();
        }

        // Asks for a name until it is valid. Returns null if the user wants to quit.
        static string? ReadName()
        {
            while (true)
            {
                Console.Write("Name (or 'q' to quit): ");
                string input = Console.ReadLine() ?? "q";

                if (input.Trim().Equals("q", StringComparison.OrdinalIgnoreCase))
                    return null;

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Name cannot be empty. Try again.");
            }
        }

        // Asks for a salary until it is a valid, non-negative number.
        static decimal ReadSalary()
        {
            while (true)
            {
                Console.Write("Salary: ");
                string input = Console.ReadLine() ?? "";

                if (decimal.TryParse(input, out decimal salary) && salary >= 0)
                    return salary;

                Console.WriteLine("Please enter a valid salary (a number, 0 or more).");
            }
        }
    }
}