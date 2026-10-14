using System;
using System.Collections.Generic;
using System.Globalization;

namespace Encapsulation
{
    internal class Program
    {
        public static void Main()
        {
            // ?? "" = if ReadLine() returns null, use an empty string instead (no crash, no warning)
            if (!int.TryParse(Console.ReadLine() ?? "", out int lines) || lines < 0)
            {
                Console.WriteLine("Invalid number of persons.");
                return;
            }

            var persons = new List<Person>();
            for (int i = 0; i < lines; i++)
            {
                // input validation lives in Program: format and types
                var cmdArgs = (Console.ReadLine() ?? "").Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (cmdArgs.Length != 4)
                {
                    Console.WriteLine("Invalid input. Expected: firstName lastName age salary");
                    continue;
                }

                if (!int.TryParse(cmdArgs[2], out int age))
                {
                    Console.WriteLine($"Invalid age: {cmdArgs[2]}");
                    continue;
                }

                // InvariantCulture = "." is always the decimal separator, regardless of Windows language
                if (!decimal.TryParse(cmdArgs[3], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal salary))
                {
                    Console.WriteLine($"Invalid salary: {cmdArgs[3]}");
                    continue;
                }

                // data validation lives in Person: the setters throw ArgumentException
                try
                {
                    persons.Add(new Person(cmdArgs[0], cmdArgs[1], age, salary));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (!decimal.TryParse(Console.ReadLine() ?? "", NumberStyles.Number, CultureInfo.InvariantCulture, out decimal bonus) || bonus < 0)
            {
                Console.WriteLine("Invalid bonus.");
                return;
            }

            foreach (Person person in persons)
            {
                person.IncreaseSalary(bonus);
                Console.WriteLine(person);
            }
        }
    }
}