namespace ExceptionsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Start av programmet ===");

            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("Välj ett scenario:");
                Console.WriteLine("1. Giltig fil (valid.txt)");
                Console.WriteLine("2. Filen saknas (missing.txt)");
                Console.WriteLine("3. Text i stället för tal (text.txt)");
                Console.WriteLine("4. Division med noll (zero.txt)");
                Console.WriteLine("5. Tom fil (empty.txt)");
                Console.WriteLine("6. Tomt filnamn");
                Console.WriteLine("7. För stort tal (overflow.txt)");
                Console.WriteLine("0. Avsluta");
                Console.Write("Ditt val: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RunScenario(GetTestFilePath("valid.txt"));
                        break;
                    case "2":
                        RunScenario(GetTestFilePath("missing.txt"));
                        break;
                    case "3":
                        RunScenario(GetTestFilePath("text.txt"));
                        break;
                    case "4":
                        RunScenario(GetTestFilePath("zero.txt"));
                        break;
                    case "5":
                        RunScenario(GetTestFilePath("empty.txt"));
                        break;
                    case "6":
                        RunScenario(""); // Ingen Path.Combine, då blir det inte tomt
                        break;
                    case "7":
                        RunScenario(GetTestFilePath("overflow.txt"));
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
            }

            Console.WriteLine("Programmet avslutas normalt.");
        }

        static string GetTestFilePath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, "TestData", fileName);
        }

        // Exempel på metod som själv kastar ett undantag (throw)
        static int ProcessFile(string fileName)
        {
            // Om filnamnet är tomt: logiskt fel vi vill signalera
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("Filnamn får inte vara tomt eller null.", nameof(fileName));
            }

            string? line = null;
            try
            {
                using StreamReader reader = new StreamReader(fileName);

                line = reader.ReadLine() ?? throw new InvalidOperationException("Filen är tom.");

                // Försöker omvandla text till tal
                int number = int.Parse(line); // Kan ge FormatException

                // Heltalsdivision: kastar DivideByZeroException om number är 0
                // (med double, t.ex. 100.0 / 0, blir resultatet ∞ i stället för ett undantag)
                return 100 / number;
            }
            catch (FormatException)
            {
                // Vi kan logga eller omformulera felet
                Console.WriteLine($"[LOG] Formatfel i ProcessFile. Fil: {Path.GetFileName(fileName)}, innehåll: \"{line}\"");
                // Vi kan välja att låta metoden "kasta upp" felet
                throw; // När du i `catch` bara vill logga/analysera,
                       // men låta anroparen (t.ex. en högre nivå i applikationen)
                       // bestämma hur man ska återhämta sig. 
            }
        }

        static void RunScenario(string fileName)
        {
            try
            {
                Console.WriteLine("Försöker läsa fil och räkna...");
                var result = ProcessFile(fileName);

                Console.WriteLine($"\nResultat: {result}");
            }
            catch (FileNotFoundException ex)
            {
                // Specifikt fel om filen inte finns
                Console.WriteLine($"Filen hittades inte: {Path.GetFileName(ex.FileName)}");
            }
            catch (FormatException)
            {
                // Specifikt fel om texten inte kan tolkas som tal
                Console.WriteLine("Formatfel: Filen innehåller inte ett giltigt heltal.");
            }
            catch (DivideByZeroException)
            {
                // Specifikt fel om nolldivision
                Console.WriteLine("Kan inte dividera med noll: filen innehåller 0.");
            }
            catch (InvalidOperationException ex)
            {
                // Val 5: empty.txt är tom, ReadLine() returnerar null
                Console.WriteLine($"Tom fil: {ex.Message}");
            }
            catch (ArgumentException)
            {
                // Val 6: tomt filnamn stoppas av kontrollen i början av ProcessFile
                Console.WriteLine("Ogiltigt filnamn: filnamnet får inte vara tomt.");
            }
            catch (Exception ex)
            {
                // Fallback för alla övriga obekanta fel
                Console.WriteLine($"Okänt fel: {ex.Message}");
            }
            finally
            {
                // Körs ALLTID, även om det blev undantag
                Console.WriteLine("Cleanup: Logging avslutat anrop.");
            }
        }
    }
}

