namespace EmployeeRegistry
{
    public class Employee
    {
        public string Name { get; }
        public decimal Salary { get; }

        public Employee(string name, decimal salary)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (salary < 0)
                throw new ArgumentOutOfRangeException(nameof(salary), "Salary cannot be negative.");

            Name = name.Trim();
            Salary = salary;
        }
    }
}
