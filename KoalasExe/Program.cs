using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Koalas;

namespace KoalasExe
{
    class Program
    {
        static void Main(string[] args)
        {
            const string data = @"1,2,3
4,""5
as""""df"",6
7,8,9";
            Console.WriteLine(data);
            Console.WriteLine("***************************");
            var df = DataFrame.FromCsvData(data);
            df[1].ForEach(Console.WriteLine);
            Console.WriteLine(DataFrame.GetColumnCount(data));
            Console.ReadLine();
        }
    }
}
