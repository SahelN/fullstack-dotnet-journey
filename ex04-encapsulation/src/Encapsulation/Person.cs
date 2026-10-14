using System;
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

        // constructor goes through the properties -> validation runs here too
        public Person(string firstName, string lastName, int age, decimal salary)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Salary = salary;
        }

        // public get (Program can read), private set (only Person can change) + validation
        public string FirstName
        {
            get { return _firstName; }
            private set
            {
                if (value == null || value.Length < 3)
                    throw new ArgumentException("First name cannot contain fewer than 3 symbols!");
                _firstName = value;
            }
        }

        public string LastName
        {
            get { return _lastName; }
            private set
            {
                if (value == null || value.Length < 3)
                    throw new ArgumentException("Last name cannot contain fewer than 3 symbols!");
                _lastName = value;
            }
        }

        public int Age
        {
            get { return _age; }
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Age cannot be zero or a negative integer!");
                _age = value;
            }
        }

        public decimal Salary
        {
            get { return _salary; }
            private set
            {
                if (value < 460)
                    throw new ArgumentException("Salary cannot be less than 460 dollar!");
                _salary = value;
            }
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