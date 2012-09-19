using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas {
    public class DataFrame {
        private readonly Dictionary<String, Series<Object>> _data;
        public List<String> ColumnNames; 

        public DataFrame(Dictionary<String, Series<Object>> data) {
            _data = data;
            ColumnNames = data.Select(x => x.Key).ToList();
        }

        public DataFrame(List<Series<Object>> seriesList) {
            _data = seriesList.ToDictionary(series => series.Name);
            ColumnNames = seriesList.Select(series => series.Name).ToList();
        }

        public Series<Object> this[String seriesIndex] {
            get { return _data[seriesIndex]; }
        }

        public Series<Object> this[int index]
        {
            get { return _data[ColumnNames[index]]; }
        }

        public static DataFrame FromCsvData(String data, char delimiter = ',', char quote = '"') {
            var csvParser = CsvParser.FromString(data, delimiter, quote);
            var dictData = new Dictionary<String, Series<Object>>();
            var seriesList = csvParser.ColumnNames.Zip(csvParser.ColumnTypes,
                                                   (colName, colType) =>
                                                   (Series<Object>) Activator.CreateInstance(typeof (Series<>).MakeGenericType(colType),
                                                                            colName, csvParser.NumColumns)).ToList();
            return new DataFrame(seriesList);
        }
    }
}