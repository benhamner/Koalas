using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas {
    public class CsvParser : IEnumerable<List<Object>> {
        public List<Type> ColumnTypes;
        public List<String> ColumnNames; 
        private Int64 _tempInt64;
        private Double _tempDouble;
        public int NumColumns;
        public int NumRows;
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
            NumColumns = headerTypes.Count;
            HasHeader = false;
            InferTypes();
            HasHeader = header.Distinct().Count() == header.Count()
                && headerTypes.Contains(typeof(String))
                && (headerTypes.Zip(ColumnTypes, IsGreaterType).Contains(true) 
                    || headerTypes.All(x => x==typeof(String)));
            ColumnNames = HasHeader ? header : Enumerable.Range(0, NumColumns).Select(i => i.ToString()).ToList();
        }

        private void InferTypes() {
            var columnTypes = Enumerable.Range(0, NumColumns).Select(el => typeof(Int64)).ToList();
            NumRows = 0;
            foreach (var row in _csvReader.Skip(HasHeader ? 1 : 0)) {
                NumRows++;
                foreach (var i in Enumerable.Range(0, row.Count))
                    columnTypes[i] = MaxType(columnTypes[i], GetType(row[i]));
            }
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

        public static Object ParseType(Type type, String s) {
            if (type==typeof(Int64))
                return Int64.Parse(s);
            if (type==typeof(Double))
                return Double.Parse(s);
            return s;
        }

        public IEnumerator<List<Object>> GetEnumerator() {
            return _csvReader.Select(readerRow => ColumnTypes.Zip(readerRow, ParseType).ToList()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}