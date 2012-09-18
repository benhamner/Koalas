using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas {
    internal class CsvParser : CsvReader {
        public CsvParser(Stream stream, char delimiter, char quote) : base(stream, delimiter, quote) {
        }
    }
}