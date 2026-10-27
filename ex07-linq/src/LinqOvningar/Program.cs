using System;
using System.Collections.Generic;
using System.Linq;
namespace LinqOvningar
{
    public class Student
    {
        public int Id { get; set; }
        public string Namn { get; set; } = "";
        public int Alder { get; set; }
        public string Klass { get; set; } = "";
        public int Poang { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Exempeldata 
            List<Student> studenter = new List<Student>
            {
                new Student { Id = 1, Namn = "Anna", Alder = 19, Klass = "A", Poang = 85 },
                new Student { Id = 2, Namn = "Bertil", Alder = 22, Klass = "B", Poang = 72 },
                new Student { Id = 3, Namn = "Cecilia", Alder = 20, Klass = "A", Poang = 91 },
                new Student { Id = 4, Namn = "David", Alder = 24, Klass = "C", Poang = 64 },
                new Student { Id = 5, Namn = "Eva", Alder = 19, Klass = "B", Poang = 85 },
                new Student { Id = 6, Namn = "Felix", Alder = 21, Klass = "A", Poang = 77 },
                new Student { Id = 7, Namn = "Gustav", Alder = 23, Klass = "C", Poang = 58 },
                new Student { Id = 8, Namn = "Hanna", Alder = 20, Klass = "B", Poang = 95 }
            };
            Console.WriteLine("LINQ-ÖVNINGAR MED STUDENTER\n");

            // ===== Where =====
            Console.WriteLine("--- Where ---");

            // Hämta alla studenter som är äldre än 20
            var aldreAn20 = studenter.Where(s => s.Alder > 20);
            Console.WriteLine("Studenter äldre än 20:");
            foreach (var s in aldreAn20)
                Console.WriteLine($"  {s.Namn} ({s.Alder} år)");

            // Hämta alla studenter som har mer än 80 poäng
            var merAn80 = studenter.Where(s => s.Poang > 80);
            Console.WriteLine("Studenter med mer än 80 poäng:");
            foreach (var s in merAn80)
                Console.WriteLine($"  {s.Namn} ({s.Poang} poäng)");
            Console.WriteLine();

            // ===== Select =====
            Console.WriteLine("--- Select ---");

            // Visa bara namnen på alla studenter
            var namn = studenter.Select(s => s.Namn);
            Console.WriteLine("Namnen på alla studenter:");
            foreach (var s in namn)
                Console.WriteLine($"  {s}");

            // Visa namn och poäng i en ny anonym typ
            var namnOchPoang = studenter.Select(s => new { s.Namn, s.Poang });
            Console.WriteLine("Namn och poäng (anonym typ):");
            foreach (var x in namnOchPoang)
                Console.WriteLine($"  {x.Namn}: {x.Poang}");
            Console.WriteLine();

            // ===== OrderBy =====
            Console.WriteLine("--- OrderBy ---");

            // Sortera studenter efter ålder
            var efterAlder = studenter.OrderBy(s => s.Alder);
            Console.WriteLine("Sorterade efter ålder:");
            foreach (var s in efterAlder)
                Console.WriteLine($"  {s.Namn} {s.Alder} år");

            // Sortera studenter efter poäng, högst först
            var efterPoang = studenter.OrderByDescending(s => s.Poang);
            Console.WriteLine("Sorterade efter poäng, högst först:");
            foreach (var s in efterPoang)
                Console.WriteLine($"  {s.Namn} {s.Poang} poäng");
            Console.WriteLine();

            // ===== Count =====
            Console.WriteLine("--- Count ---");

            // Räkna hur många studenter som finns
            int antal = studenter.Count();
            Console.WriteLine($"Antal studenter: {antal}");

            // Räkna hur många som går i klass A
            int antalKlassA = studenter.Count(s => s.Klass == "A");
            Console.WriteLine($"Antal i klass A: {antalKlassA}");
            Console.WriteLine();

            // ===== Take =====
            Console.WriteLine("--- Take ---");

            // Ta ut de tre första studenterna
            var treForsta = studenter.Take(3);
            Console.WriteLine("De tre första studenterna:");
            foreach (var s in treForsta)
                Console.WriteLine($"  {s.Namn}");

            // Ta ut de två bästa poängen efter sortering
            var tvaBasta = studenter.OrderByDescending(s => s.Poang).Select(s => s.Poang).Take(2);
            Console.WriteLine("De två bästa poängen:");
            foreach (var s in tvaBasta)
                Console.WriteLine($"  {s}");
            Console.WriteLine();

            // ===== Distinct =====
            Console.WriteLine("--- Distinct ---");

            // Visa alla unika klassnamn
            var unikaKlasser = studenter.Select(s => s.Klass).Distinct();
            Console.WriteLine("Unika klasser:");
            foreach (var s in unikaKlasser)
                Console.WriteLine($"  {s}");

            // Visa alla unika åldrar
            var unikaAldrar = studenter.Select(s => s.Alder).Distinct();
            Console.WriteLine("Unika åldrar:");
            foreach (var s in unikaAldrar)
                Console.WriteLine($"  {s}");
            Console.WriteLine();

            // ===== Any / All =====
            Console.WriteLine("--- Any / All ---");

            // Finns det någon student med 100 poäng?
            bool nagonMed100 = studenter.Any(s => s.Poang == 100);
            Console.WriteLine($"Finns någon med 100 poäng? {nagonMed100}");

            // Har alla studenter minst 50 poäng?
            bool allaMinst50 = studenter.All(s => s.Poang >= 50);
            Console.WriteLine($"Har alla minst 50 poäng? {allaMinst50}");
            Console.WriteLine();

            // ===== FirstOrDefault / Last / Single =====
            Console.WriteLine("--- FirstOrDefault / Last / Single ---");

            // Hämta första studenten i klass B
            Student? forstaIB = studenter.FirstOrDefault(s => s.Klass == "B");
            Console.WriteLine($"Första i klass B: {forstaIB?.Namn ?? "ingen"}");

            // Hämta sista studenten i listan
            Student sista = studenter.Last();
            Console.WriteLine($"Sista i listan: {sista.Namn}");

            // Hämta studenten med ett visst Id
            int id = 4;
            Student medId = studenter.Single(s => s.Id == id);
            Console.WriteLine($"Student med Id {id}: {medId.Namn}");
            Console.WriteLine();

            // ===== GroupBy =====
            Console.WriteLine("--- GroupBy ---");

            // Gruppera studenter per klass
            var perKlass = studenter.GroupBy(s => s.Klass);

            // Skriv ut varje klass och namnen i gruppen
            foreach (var grupp in perKlass)
            {
                Console.WriteLine($"Klass {grupp.Key}:");
                foreach (var s in grupp)
                    Console.WriteLine($"  {s.Namn}");
            }
            Console.WriteLine();

            // ===== Sum / Average / Min / Max =====
            Console.WriteLine("--- Sum / Average / Min / Max ---");

            // Summera alla poäng
            int summa = studenter.Sum(s => s.Poang);
            Console.WriteLine($"Summa poäng: {summa}");

            // Räkna ut medelpoäng
            double medel = studenter.Average(s => s.Poang);
            Console.WriteLine($"Medelpoäng: {medel:F2}");

            // Hitta lägsta och högsta poäng
            int lagst = studenter.Min(s => s.Poang);
            int hogst = studenter.Max(s => s.Poang);
            Console.WriteLine($"Lägsta poäng: {lagst}");
            Console.WriteLine($"Högsta poäng: {hogst}");
        }
    }
}