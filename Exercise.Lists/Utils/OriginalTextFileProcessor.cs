using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Exercise.Lists.Utils
{
    public static class OriginalTextFileProcessor
    {
        public const string dirPath = @"C:\temp";

        // 2 -  Use the the same file to Load data from FILE to a List of PEOPLE 

        // load() PEOPLE objets from  FILE   
        // --> extract lines from file  
        // ->  create a List of objects from each column 
        // -> each object must have PROPERTIES VALUES  taht match with each FILE COLUMN

        private static string loadCSV(string name)
        {
            try
            {
                string pathFile = Path.Combine(dirPath, $"{name}.csv");
                // Console.WriteLine (pathFile);
                var text = File.ReadAllText(pathFile);
                return text;
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); return string.Empty; }
        }

        private static List<T> buildList<T>(string content)
        {
            // 1) Crea Lista
            List<T> list = new List<T>();

            // 1.1) Legge le proprietà di T
            string[] properties = GetProperties<T>();
            int propertiesLength = properties.Length;

            // 2) Prende il contenuto di content e lo divide per righe, rimuovendo eventuali caratteri "white-space"
            string[] lines = content.Split("\n");
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }

            // 3) Lavora con ogni singola riga
            bool first = true;  // Gestione prima riga
            int currLine = 0;   // Enumera le righe
            foreach (string line in lines)
            {
                // Divide le stringhe in base alla virgola
                string[] fields = line.Split(",");

                if (first)
                {
                    if (properties.SequenceEqual(fields))       // Se le proprietà lette coincidono con quelle del tipo T
                    {
                        first = false;
                        currLine++;
                        Console.WriteLine("Same Type!");
                    }
                    else
                    {
                        Console.WriteLine("Incompatible Type!");
                        return list = null;
                    }
                }
                else
                {
                    // Console.WriteLine("Fetching...");
                    if (fields.Length != propertiesLength)
                    {
                        Console.WriteLine("Skipping line n. {0}", currLine++);
                    }
                    else
                    {
                        Console.WriteLine("Fetching line n. {0}", currLine++);
                        T currObj = (T)Activator.CreateInstance(typeof(T), new object[] {fields});
                        list.Add(currObj);
                    }
                }
            }

            return list;
            // throw new NotImplementedException();
        }

        public static List<T> loadFromFile<T>()
        {
            // Individua quale file csv da caricare
            string csvToLoad = typeof(T).Name.ToLower();

            // Carica contenuto
            string content = loadCSV(csvToLoad);

            // Console.WriteLine(content);
            // return null;

            // Se non c'è contenuto ritorna niente
            if (string.IsNullOrEmpty(content))
                return null;

            // Se c'è contenuto costruisce lista tramite metodo e restituisce
            return buildList<T>(content);

        }

        public static void writeToFile<T> (List<T> list)
        {

        }

        public static string[] GetProperties<T>()
        {
            var properties = typeof(T).GetProperties();

            string[] propertiesNames = new string[properties.Length];
            int index = 0;

            foreach (var property in properties)
            {
                // Console.WriteLine(property.Name.ToString());
                propertiesNames.SetValue(property.Name.ToString(), index++);
            }

            return propertiesNames;
        }
    }
}
