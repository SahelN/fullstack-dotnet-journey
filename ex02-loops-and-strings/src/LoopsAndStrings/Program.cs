using System;
using System.Text;

namespace LoopsAndStrings
{
    internal class Program
    {
        const int FreePrice = 0;
        const int YouthPrice = 80;
        const int SeniorPrice = 90;
        const int StandardPrice = 120;

        const int ChildFreeAgeLimit = 5;    // Under 5 years old enters for free
        const int SeniorFreeAgeLimit = 100; // Over 100 years old enters for free
        const int YouthAgeLimit = 20;       // Under 20 years old = youth
        const int SeniorAgeLimit = 64;      // Over 64 years old = senior

        const int MinAge = 0;
        const int MaxAge = 120;
        const int MaxGroupSize = 50;
        const int RepeatCount = 10;

        /// <summary>Runs the main menu loop until the user chooses 0.</summary>
        static void Main(string[] args)
        {
            Console.Clear();
            var isRunning = true;

            while (isRunning)
            {
                ShowMainMenu();
                string? selectedMenu = Console.ReadLine();

                switch (selectedMenu)
                {
                    case "1":
                        HandleTicketPrice();
                        break;
                    case "2":
                        HandleRepeatText();
                        break;
                    case "3":
                        HandleThirdWord();
                        break;
                    case "0":
                        isRunning = false;
                        Console.WriteLine("Exiting the program...");
                        break;

                    default:
                        // Anything that is not a valid menu choice ends up here, including an empty line and null.
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        /// <summary>Prints the main menu.</summary>
        static void ShowMainMenu()
        {
            Console.WriteLine();
            Console.WriteLine("==================================");
            Console.WriteLine("            MAIN MENU");
            Console.WriteLine("==================================");
            Console.WriteLine("You are now in the main menu. Navigate by typing a number and pressing Enter.");
            Console.WriteLine();
            Console.WriteLine("  1. Cinema – calculate ticket price");
            Console.WriteLine("  2. Repeat a text ten times");
            Console.WriteLine("  3. Find the third word in a sentence");
            Console.WriteLine("  0. Exit the program");
            Console.WriteLine();
            Console.Write("Your choice: ");
        }

        /// <summary>Menu option 1. Asks for the number of people and calculates the ticket price for one person or a group.</summary>
        static void HandleTicketPrice()
        {
            int numberOfPeople = ReadValidInt("How many people are in your group? ", 1, MaxGroupSize);

            if (numberOfPeople > 1)
            {
                HandleGroupPrice(numberOfPeople);
            }
            else
            {
                HandleSinglePrice();
            }
        }

        /// <summary>
        /// One person: prints the price category and the price.
        /// </summary>
        static void HandleSinglePrice()
        {
            int age = ReadValidInt("Enter your age: ", MinAge, MaxAge);
            var ticket = GetTicket(age);

            Console.WriteLine($"{ticket.Category}: {ticket.Price} SEK");
        }

        /// <summary>
        /// A group: asks for each person's age, prints a summary,
        /// and optionally shows the details for each person.
        /// </summary>
        /// <param name="numberOfPeople">How many people are in the group.</param>
        static void HandleGroupPrice(int numberOfPeople)
        {
            // The ages are saved so the details can be shown after the total.
            int[] ages = new int[numberOfPeople];
            int sumPrice = 0;

            for (int i = 0; i < numberOfPeople; i++)
            {
                ages[i] = ReadValidInt($"Enter the age of person {i + 1}: ", MinAge, MaxAge);
                sumPrice += GetTicket(ages[i]).Price;
            }

            Console.WriteLine();
            Console.WriteLine("--- Summary ---");
            Console.WriteLine($"Number of people: {numberOfPeople}");
            Console.WriteLine($"Total cost: {sumPrice} SEK");

            if (ReadValidYesNo("Do you want to see the details? (Y/N): "))
            {
                Console.WriteLine();
                Console.WriteLine("--- Details ---");

                for (int i = 0; i < ages.Length; i++)
                {
                    var ticket = GetTicket(ages[i]);
                    Console.WriteLine($"Person {i + 1} (age {ages[i]}): {ticket.Category}: {ticket.Price} SEK");
                }
            }
        }

        /// <summary>Menu option 2. Repeats the user's text ten times on the same line.</summary>
        static void HandleRepeatText()
        {
            string text = ReadNonEmptyText("Enter a text: ");
            Console.WriteLine(BuildRepeatedText(text, RepeatCount));
        }

        /// <summary>Menu option 3. Prints the third word of the user's sentence.</summary>
        static void HandleThirdWord()
        {
            // Asks again until the sentence has at least three words.
            while (true)
            {
                string sentence = ReadNonEmptyText("Enter a sentence with at least 3 words: ");

                if (TryGetThirdWord(sentence, out string thirdWord))
                {
                    Console.WriteLine($"The third word is: {thirdWord}");
                    return;
                }

                Console.WriteLine("The sentence must contain at least 3 words.");
            }
        }

        /// <summary>Asks until the user enters a whole number between min and max.</summary>
        static int ReadValidInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                // TryParse does not crash on "abc" – it returns false instead.
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine($"Invalid input. Enter a whole number between {min} and {max}.");
            }
        }

        /// <summary>
        /// Asks until the user answers Y or N (upper or lower case).
        /// Returns true for Y and false for N.
        /// </summary>
        static bool ReadValidYesNo(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "Y")
                {
                    return true;
                }

                if (input == "N")
                {
                    return false;
                }

                Console.WriteLine("Invalid input. Please enter Y or N.");
            }
        }

        /// <summary>Asks until the user enters something that is not empty.</summary>
        static string ReadNonEmptyText(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("The input cannot be empty.");
            }
        }

        /// <summary>
        /// Returns the price category and the price for an age.
        /// </summary>
        /// <param name="age">The person's age.</param>
        /// <returns>The price category and the price in SEK.</returns>
        internal static (string Category, int Price) GetTicket(int age)
        {
            if (age < ChildFreeAgeLimit || age > SeniorFreeAgeLimit)
            {
                return ("Free entry", FreePrice);
            }

            if (age < YouthAgeLimit)
            {
                return ("Youth price", YouthPrice);
            }
            else
            {
                // Nested if: only checked if the person is not a youth.
                if (age > SeniorAgeLimit)
                {
                    return ("Senior price", SeniorPrice);
                }
                else
                {
                    return ("Standard price", StandardPrice);
                }
            }
        }

        /// <summary>
        /// Builds "1. text, 2. text, … n. text" using a for loop.
        /// A comma is added BEFORE each repetition except the first,
        /// so there is no comma after the last one.
        /// </summary>
        internal static string BuildRepeatedText(string text, int times)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 1; i <= times; i++)
            {
                if (i > 1)
                {
                    result.Append(", ");
                }

                result.Append($"{i}. {text}");
            }

            return result.ToString();
        }

        /// <summary>
        /// Splits the sentence on spaces and returns the third word.
        /// RemoveEmptyEntries removes empty parts caused by several
        /// spaces in a row (extra task 3).
        /// Returns false if the sentence has fewer than three words – the same pattern as int.TryParse.
        /// </summary>
        internal static bool TryGetThirdWord(string sentence, out string thirdWord)
        {
            var words = sentence.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 3)
            {
                thirdWord = string.Empty;
                return false;
            }

            thirdWord = words[2]; // Index 2 = the third word, because indexing starts at 0
            return true;
        }
    }
}
