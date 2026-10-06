using EmployeeRegistry;
using Xunit;

namespace EmployeeRegistry.Tests
{
    public class EmployeeTests
    {
        [Fact]
        public void Constructor_ValidInput_SetsNameAndSalary()
        {
            var employee = new Employee("Eva Nilsson", 36000m);

            Assert.Equal("Eva Nilsson", employee.Name);
            Assert.Equal(36000m, employee.Salary);
        }

        [Fact]
        public void Constructor_NameWithSpaces_IsTrimmed()
        {
            var employee = new Employee("  Eva Nilsson   ", 36000m);

            Assert.Equal("Eva Nilsson", employee.Name);
        }

        [Fact]
        public void Constructor_ZeroSalary_IsAllowed()
        {
            var employee = new Employee("Eva Nilsson", 0m);

            Assert.Equal(0m, employee.Salary);
        }

        [Fact]
        public void Constructor_EmptyName_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                Employee employee = new Employee("", 36000m);
            });
        }

        [Fact]
        public void Constructor_NegativeSalary_ThrowsException()
        {
            string name = "Eva Nilsson";
            decimal negativeSalary = -1m;

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Employee employee = new Employee(name, negativeSalary);
            });
        }
    }
}