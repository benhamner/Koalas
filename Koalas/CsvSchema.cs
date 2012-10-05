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
        public bool HasHeader = false;
        public bool InferHeader = true;

        //public List<string> RequiredColumnNames;
        //public List<string> OptionalColumnNames;
        //public List<int> RequiredColumnIds;
        //public List<int> OptionalColumnIds;
        //public bool AllowAdditionalColumns = true;

        // These two function together and determine whether to error if the actual and expected number of rows don't match
        public int ExpectedRowCount = 0; // <=0 means unknown, header does not count
        public bool EnforceExpectedRowCount = false;

        //public bool RequireHeader = false;
        //public bool InferRowId = true;
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
