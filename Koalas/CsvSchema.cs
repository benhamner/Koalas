using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koalas
{
    // Uncommented properties mean that it is supported
    public class CsvSchema {
        public int Delimiter = ',';
        public int Quote = '"';
        //public List<string> RequiredColumnNames;
        //public List<string> OptionalColumnNames;
        //public List<int> RequiredColumnIds;
        //public List<int> OptionalColumnIds;
        public bool HasHeader = false;
        public bool InferHeader = true;
        //public bool AllowAdditionalColumns = true;
        //public int ExpectedRowCount = 0; // <=0 means unknown
        //public bool RequireRowCount = false;
        //public bool RequireHeader = false;
        //public bool InferRowId = true;
        //public int SkipRows = 0;
        //public int SkipLines = 0;
        //public List<object> RowIndex;
        //public List<object> ColumnIndex;
        //public List<string> NaValues;
        //public bool MissingToDouble = true;

        public CsvSchema() {
            //RequiredColumnNames = new List<string>();
            //RequiredColumnIds = new List<int>();
            //OptionalColumnNames = new List<string>();
            //OptionalColumnIds = new List<int>();
        }
    }
}
