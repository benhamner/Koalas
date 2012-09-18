using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas {
    public class CsvParser {
        public List<Type> ColumnTypes;
        private Int64 _tempInt64;
        private Double _tempDouble;
        private int _numColumns;
        private readonly CsvReader _csvReader;
        public bool HasHeader;

        public CsvParser(Stream stream, char delimiter=',', char quote='"', bool hasHeader=false, bool inferHeader=true) {
            _csvReader = new CsvReader(stream, delimiter, quote);
            if (inferHeader)
                InferHeaderAndTypes();
            else
                InferTypes();
        }

        public static CsvParser FromString(String data, char delimiter = ',', char quote = '"', bool hasHeader = false, bool inferHeader = true)
        {
            return new CsvParser(CsvReader.StringToStream(data), delimiter, quote);
        }

        // Header requirements
        //  - Each name in the header is unique
        //  - Contains at least one String
        //  - At least one of the following is true:
        //    - At least one column name is of a strictly greater type
        //    - All elements are strings
        private void InferHeaderAndTypes() {
            var header = _csvReader.First();
            var headerTypes = header.Select(GetType).ToList();
            _numColumns = headerTypes.Count;
            HasHeader = true;
            InferTypes();
            HasHeader = header.Distinct().Count() == header.Count()
                && headerTypes.Contains(typeof(String))
                && (headerTypes.Zip(ColumnTypes, IsGreaterType).Contains(true) 
                    || headerTypes.All(x => x==typeof(String)));
        }

        private void InferTypes() {
            var columnTypes = Enumerable.Range(0, _numColumns).Select(el => typeof(Int64)).ToList();
            foreach (var row in _csvReader.Skip(HasHeader ? 1:0))
                foreach (var i in Enumerable.Range(0, row.Count))
                    columnTypes[i] = MaxType(columnTypes[i], GetType(row[i]));
            ColumnTypes = columnTypes;
        }

        private Type GetType(String s) {
            if (Int64.TryParse(s, out _tempInt64))
                return typeof (Int64);
            if (Double.TryParse(s, out _tempDouble))
                return typeof (Double);
            return typeof (String);
        }

        public static bool IsGreaterType(Type a, Type b) {
            return a != b && a == MaxType(a, b);
        }

        public static Type MaxType(Type a, Type b) {
            if (a == typeof(String) || b == typeof(String))
                return typeof (String);
            if (a == typeof(Double) || b == typeof(Double))
                return typeof (Double);
            return typeof (Int64);
        }

    }
}