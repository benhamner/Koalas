using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas
{
    public class DataFrame
    {
        private List<List<String>> series;

        public DataFrame (List<List<String>> series)
        {
            this.series = series;
        }

        public List<String> this [int seriesIndex]
        {
            get {return series[seriesIndex];}
        }

        public static DataFrame FromCsvData(String data, char delimiter=',', char quoteChar='"')
        {
            var csv = new CsvReader(new StringReader(data), delimiter, quoteChar);
            var series = Enumerable.Range(0, csv.First().Count()).Select(i => new List<string>()).ToList();
            csv = new CsvReader(new StringReader(data), delimiter, quoteChar);

            foreach (var row in csv)
                for (var i = 0; i < series.Count; i++)
                    series[i].Add(row[i]);

            return new DataFrame(series);
        }

        public static int GetColumnCount(String data, char delimiter=',', char quoteChar='"')
        {
            return new CsvReader(new StringReader(data), delimiter, quoteChar).First().Count();
        }
    }
}