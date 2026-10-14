using System.Globalization;

namespace Encapsulation
{
    public class Person
    {
        // private fields: nobody outside the class can touch them directly
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private int _age;
        private decimal _salary;

        // constructor goes through the properties
        public Person(string firstName, string lastName, int age, decimal salary)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Salary = salary;
        }

        // public get (Program can read), private set (only Person can change)
        public string FirstName
        {
            get { return _firstName; }
            private set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            private set { _lastName = value; }
        }

        public int Age
        {
            get { return _age; }
            private set { _age = value; }
        }

        public decimal Salary
        {
            get { return _salary; }
            private set { _salary = value; }
        }

        // the only way to change salary from outside – the rule lives inside Person
        public void IncreaseSalary(decimal percentage)
        {
            // under 30 gets half the increase
            decimal actualPercentage = Age < 30 ? percentage / 2 : percentage;
            Salary += Salary * actualPercentage / 100;
        }

        // InvariantCulture -> "2640.00" (not "2640,00" on a Swedish computer)
        public override string ToString()
        {
            return $"{FirstName} {LastName} receives {Salary.ToString("F2", CultureInfo.InvariantCulture)} dollars.";
        }
    }
}