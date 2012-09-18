using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas {
    public class CsvParser : CsvReader {
        public List<Type> ColumnTypes;
        private Int64 _tempInt64;
        private Double _tempDouble;
        public bool HasHeader;

        public CsvParser(Stream stream, char delimiter, char quote)
            : base(stream, delimiter, quote) {
            InferHeaderAndTypes();
        }

        public static new CsvParser FromString(String data, char delimiter=',', char quote='"') {
            return new CsvParser(StringToStream(data), delimiter, quote);
        }

        private void InferHeaderAndTypes(int numRows = 10) {
            var headerTypes = this.First().Select(GetType).ToList();
            var columnTypes = headerTypes.Select(el => typeof (Int64)).ToList();
            foreach (var row in this.Skip(1).Take(numRows - 1))
                foreach (var i in Enumerable.Range(0, row.Count))
                    columnTypes[i] = MaxType(columnTypes[i], GetType(row[i]));
            HasHeader = headerTypes.Zip(columnTypes, IsGreaterType).Contains(true);
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