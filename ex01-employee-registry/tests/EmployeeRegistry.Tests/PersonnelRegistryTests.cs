using EmployeeRegistry;
using Xunit;

namespace EmployeeRegistry.Tests
{
    public class PersonnelRegistryTests
    {
        [Fact]
        public void PrintEmployees_EmptyRegistry_PrintsEmptyMessage()
        {
            // Arrange
            PersonnelRegistry registry = new PersonnelRegistry();
            StringWriter output = new StringWriter();
            Console.SetOut(output);

            // Act
            registry.PrintEmployees();

            // Assert
            string text = output.ToString();
            Assert.Contains("The registry is empty.", text);
        }

        [Fact]
        public void PrintEmployees_WithEmployees_PrintsNames()
        {
            // Arrange
            PersonnelRegistry registry = new PersonnelRegistry();
            registry.AddEmployee(new Employee("Anna", 32000m));
            registry.AddEmployee(new Employee("Bertil", 28500m));

            StringWriter output = new StringWriter();
            Console.SetOut(output);

            // Act
            registry.PrintEmployees();

            // Assert
            string text = output.ToString();
            Assert.Contains("Anna", text);
            Assert.Contains("Bertil", text);
        }
    }
}