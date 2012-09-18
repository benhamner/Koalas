using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Koalas;

namespace KoalasExe {
    internal class Program {
        private static void Main(string[] args) {
            const string data = @"1,2,3
4,5,6
7,8,9";
            Console.WriteLine(data);
            Console.WriteLine("***************************");
            var df = DataFrame.FromCsvData(data);
            df[1].ForEach(Console.WriteLine);
            Console.WriteLine(DataFrame.GetColumnCount(data));
            Console.WriteLine("***************************");
            var csv = CsvReader.FromString(data);

            foreach (var row in csv) {
                foreach (var s in row) {
                    Console.WriteLine(s);
                }
            }
            Console.ReadLine();
        }
    }
}