using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas {
    public class DataFrame {
        private readonly Dictionary<String, Series<Object>> _data;

        public DataFrame(Dictionary<String, Series<Object>> data) {
            _data = data;
        }

        public Series<Object> this[String seriesIndex] {
            get { return _data[seriesIndex]; }
        }

        public static DataFrame FromCsvData(String data, char delimiter = ',', char quoteChar = '"') {
            throw new NotImplementedException();
        }

        public static int GetColumnCount(String data, char delimiter = ',', char quoteChar = '"') {
            return CsvReader.FromString(data, delimiter, quoteChar).First().Count();
        }
    }
}