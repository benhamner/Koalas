using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas {
    public class DataFrame {
        private readonly Dictionary<String, Series> _data;
        public List<String> ColumnNames;

        public DataFrame(Dictionary<String, Series> data) {
            _data = data;
            ColumnNames = data.Select(x => x.Key).ToList();
        }

        public DataFrame(List<Series> seriesList) {
            _data = seriesList.ToDictionary(series => series.Name);
            ColumnNames = seriesList.Select(series => series.Name).ToList();
        }

        public Series this[String seriesIndex] {
            get { return _data[seriesIndex]; }
        }

        public Series this[int index]
        {
            get { return _data[ColumnNames[index]]; }
        }

        public Series<T> GetSeriesByType<T>() {
            if (_data != null) {
                var first = _data.Values.FirstOrDefault(series => series.Type == typeof (T));
                if (first != null) {
                    var series = first.Cast<T>().ToSeries();
                    series.Name = first.Name;
                    return series;
                }
                throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }

        public static DataFrame FromCsvData(String data, char delimiter = ',', char quote = '"') {
            var csvParser = CsvParser.FromString(data, delimiter, quote);

            var seriesList = csvParser.ColumnNames.Select(name => new Series(name)).ToList();
            foreach (var row in csvParser) {
                for (var i=0; i<seriesList.Count; i++) {
                    seriesList[i].Add(row[i]);
                }
            }
            return new DataFrame(seriesList);
        }
    }
} 