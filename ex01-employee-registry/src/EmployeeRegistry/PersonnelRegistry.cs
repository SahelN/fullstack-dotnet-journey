namespace EmployeeRegistry
{
    public class PersonnelRegistry
    {
        private readonly List<Employee> _employees = [];

        public void AddEmployee(Employee employee)
        {
            _employees.Add(employee);
        }

        public void PrintEmployees()
        {
            if (_employees.Count == 0)
            {
                Console.WriteLine("The registry is empty.");
                return;
            }

            Console.WriteLine($"{"Name",-20}{"Salary",12}");
            Console.WriteLine(new string('-', 32));

            foreach (Employee emp in _employees)
            {
                Console.WriteLine($"{emp.Name, -20}{emp.Salary,12:C}");
            }
        }
    }
}
