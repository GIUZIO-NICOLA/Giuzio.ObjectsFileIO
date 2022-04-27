using Exercise.Lists.Models;
using Exercise.Lists.Utils;
using System;
using System.Collections.Generic;

namespace Exercise.Lists
{
    internal class Program
    {
        static void Main(string[] args)
        {    // string path = "C:\temp\people.csv"


            // 1 -  SAVE a list o PEOPLE  from DataList to file csv
            // 2 -  Use the the same file to Load data from FILE to a List of PEOPLE 


            // 3 -  Print out all the PEOPLE properties from file 

            // use a utility class to create STANDARD METHODS to manager both situation !!
            // USE FILE STATIC FILE PATH !!!

            // Create a method to populate MockData
            
            List<People> people = OriginalTextFileProcessor.loadFromFile<People>();
            
            Console.WriteLine(String.Empty);

            if (people != null)
            {
                foreach (People p in people)
                { Console.WriteLine(p.ToString()); }
            }

            Console.WriteLine(String.Empty);

            OriginalTextFileProcessor.writeToFile<People>(people);
            

            //People person = new People(" 2, Marco, false".Split(','));
            //Console.WriteLine(person.Name);

        }



    }
}
