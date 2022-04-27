using System;

namespace Exercise.Lists.Models
{
    internal class People
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public People(string[] args)
        {
            if (args == null || args.Length != 3)
            {
                Console.WriteLine("Non riesco ad istanziare la classe con questi parametri!");
                return;
            }

            try
            {
                Id = int.Parse(args[0]);
                Name = args[1];
                IsActive = bool.Parse(args[2]);
            }
            catch (FormatException fe) { Console.WriteLine("Conversione non effettuata correttamente!"); Console.WriteLine(fe.Message); }
            catch (Exception e) { Console.WriteLine("Non riesco ad istanziare la classe con questi parametri!"); Console.WriteLine(e.ToString()); }
        }

        public override string ToString()
        {
            return $"Sono l'oggetto {this.GetType().Name} e i miei campi sono {this.Id}, {this.Name}, {this.IsActive}";
        }

    }
}
