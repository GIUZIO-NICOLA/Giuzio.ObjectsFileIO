using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Exercise.Lists.Utils
{
    public static class OriginalTextFileProcessor
    {
        public const string dirPath = @"C:\temp";

        
        // READ LOGIC

        // Lettura raw del file CSV
        private static string loadCSV(string csvToLoad)
        {
            try
            {
                string pathFile = Path.Combine(dirPath, $"{csvToLoad}.csv");
                // Console.WriteLine (pathFile);
                var text = File.ReadAllText(pathFile);
                return text;
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); return string.Empty; }
        }

        // Parse del contenuto del CSV ed elaborazione
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
                        T currObj = (T)Activator.CreateInstance(typeof(T), new object[] { fields });
                        list.Add(currObj);
                    }
                }
            }

            return list;
            // throw new NotImplementedException();
        }

        // Wrapper per le funzionalità
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

        // WRITE LOGIC

        // Scrittura del contenuto nel CSV
        private static void writeCSV(string csvToWrite, string content)
        {
            try
            {
                string pathFile = Path.Combine(dirPath, $"{csvToWrite}_w.csv");
                // Console.WriteLine (pathFile);
                File.WriteAllText(pathFile, content);
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }

        // Costruzione del contenuto da scrivere nel CSV
        private static string elaborateContent<T>(List<T> list)
        {
            string[] properties = GetProperties<T>();
            string content = String.Join(",", properties);
            content = String.Concat(content, "\n");

            string tmpStr = string.Empty;

            foreach (T item in list)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    tmpStr = item.GetType().GetProperty(properties[i]).GetValue(item)?.ToString();
                    if (i != properties.Length - 1)
                        content = String.Concat(content, tmpStr, ",");
                    else
                        content = String.Concat(content, tmpStr);
                }
                content = String.Concat(content, "\n");
            }

            // Console.WriteLine(content);
            return content;


            /*
            string str = string.Empty;
            try
            {
                Console.WriteLine(
                    item.GetType()
                    .GetProperties()[0]
                    .GetValue(item)
                    //.GetProperty("Id")
                    //.GetValue(item)
                    .ToString()
                    );
                    // GetField(properties[0]).GetValue(T).ToString();                 
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
            finally { Console.WriteLine(""); }


            // FieldInfo myFieldInfo = (T).GetField(properties[0]);
            // Console.WriteLine("The value of the public field is: '{0}'", myFieldInfo.GetValue(list.ElementAt<T>(i)));

            // FieldInfo fi = item.GetType().GetField(properties[0]);
            // string str = fi.GetValue(item);
            //Console.WriteLine(str);
        }

            return content;
*/
        }

        // Wrapper per le funzionalità
        public static void writeToFile<T>(List<T> list)
        {
            // Prepara contenuto
            string content = elaborateContent<T>(list);

            // Esamina se c'è contenuto
            if (string.IsNullOrEmpty(content))
            {
                Console.WriteLine("Nessun contenuto da scrivere");
            }
            else
            {
                // Individua quale file csv da scrivere
                string csvToWrite = typeof(T).Name.ToLower();

                // Se c'è contenuto costruisce scrive nel file
                writeCSV(csvToWrite, content);
            }
        }

        // PROPERTY INSPECTOR
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
