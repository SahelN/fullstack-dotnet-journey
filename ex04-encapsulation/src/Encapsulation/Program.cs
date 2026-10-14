using System;
using System.Collections.Generic;
using System.Globalization;

namespace Encapsulation
{
    internal class Program
    {
        public static void Main()
        {
            // ! = null-forgiving operator: tells the compiler ReadLine() won't return null here (removes warning CS8604)
            int lines = int.Parse(Console.ReadLine()!);

            var persons = new List<Person>();
            for (int i = 0; i < lines; i++)
            {
                // TODO: validate input – assumes correct input for now
                // ! = trust ReadLine() is not null (removes warning CS8602)
                var cmdArgs = Console.ReadLine()!.Split();

                // InvariantCulture = "." is always the decimal separator, regardless of Windows language
                var person = new Person(
                    cmdArgs[0],
                    cmdArgs[1],
                    int.Parse(cmdArgs[2]),
                    decimal.Parse(cmdArgs[3], CultureInfo.InvariantCulture));

                persons.Add(person);
            }

            decimal bonus = decimal.Parse(Console.ReadLine()!, CultureInfo.InvariantCulture);

            foreach (Person person in persons)
            {
                person.IncreaseSalary(bonus);
                Console.WriteLine(person);
            }
        }
    }
}