using System;

namespace Encapsulation
{
    public class Person
    {
        // private fields: nobody outside the class can touch them directly
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private int _age;

        // constructor goes through the properties
        public Person(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        // public get (Program can read and sort), private set (only Person can change)
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

        // how a Person is shown as text
        public override string ToString()
        {
            return $"{FirstName} {LastName} is {Age} years old.";
        }
    }
}
