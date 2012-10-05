using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Koalas;

namespace KoalaTests {
    public class CsvSchemaTests {
        [Test]
        // This option is handled by CsvReader
        public void DelimiterTest() {
            var schema = new CsvSchema() { Delimiter = '|' };
            var csv = CsvReader.FromString("a|b|c", schema).ToList();
            Assert.AreEqual("a", csv[0][0]);
            Assert.AreEqual("b", csv[0][1]);
            Assert.AreEqual("c", csv[0][2]);

            csv = CsvReader.FromString("1|2|3|4\r5|6|7|8", schema).ToList();
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("4", csv[0][3]);
            Assert.AreEqual("5", csv[1][0]);
            Assert.AreEqual("7", csv[1][2]);

            csv = CsvReader.FromString("1|2|\"3a\"\"\nbc\"|4\r5|6|7|8", schema).ToList();
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3a\"\nbc", csv[0][2]);
            Assert.AreEqual("5", csv[1][0]);
            Assert.AreEqual("7", csv[1][2]);
        }

        [Test]
        // This option is handled by CsvReader
        public void QuoteTest() {
            var schema = new CsvSchema() { Quote = '|' };
            var csv = CsvReader.FromString("|a,b,c|,b,c\r\nd,e,f", schema).ToList();
            Assert.AreEqual("a,b,c", csv[0][0]);
            Assert.AreEqual("b", csv[0][1]);
            Assert.AreEqual("e", csv[1][1]);

            schema = new CsvSchema() { Quote = '|' };
            csv = CsvReader.FromString("|a,b,c|,b,c\r\nd,|e\r\rf,a||bc\r1|,f", schema).ToList();
            Assert.AreEqual("a,b,c", csv[0][0]);
            Assert.AreEqual("b", csv[0][1]);
            Assert.AreEqual("e\r\rf,a|bc\r1", csv[1][1]);
        }

        [Test]
        // This requirement is handled by CsvParser
        public void DontInferHeaderTest() {
            var schema = new CsvSchema() {InferHeader = false, HasHeader = true};
            var csv = CsvParser.FromString("1,2\n3,4", schema);
            Assert.AreEqual(3, csv.ToList()[0][0]);
            Assert.AreEqual(4, csv.ToList()[0][1]);
            Assert.AreEqual("1", csv.ColumnNames[0]);
            Assert.AreEqual(1, csv.NumRows);
            Assert.AreEqual(2, csv.NumColumns);

            schema = new CsvSchema() { InferHeader = false, HasHeader = false };
            csv = CsvParser.FromString("1,2\n3,4", schema);
            Assert.AreEqual(1, csv.ToList()[0][0]);
            Assert.AreEqual(2, csv.ToList()[0][1]);
            Assert.AreEqual("0", csv.ColumnNames[0]);
            Assert.AreEqual(2, csv.NumRows);
            Assert.AreEqual(2, csv.NumColumns);

            schema = new CsvSchema() { InferHeader = true, HasHeader = false };
            csv = CsvParser.FromString("1,2\n3,4", schema);
            Assert.AreEqual(1, csv.ToList()[0][0]);
            Assert.AreEqual(2, csv.ToList()[0][1]);
            Assert.AreEqual("0", csv.ColumnNames[0]);
            Assert.AreEqual(2, csv.NumRows);
            Assert.AreEqual(2, csv.NumColumns);

            schema = new CsvSchema() { InferHeader = true, HasHeader = true };
            csv = CsvParser.FromString("1,2\n3,4", schema);
            Assert.AreEqual(1, csv.ToList()[0][0]);
            Assert.AreEqual(2, csv.ToList()[0][1]);
            Assert.AreEqual("0", csv.ColumnNames[0]);
            Assert.AreEqual(2, csv.NumRows);
            Assert.AreEqual(2, csv.NumColumns);

        }

        [Test]
        // Enforced in CsvParser
        public void ExpectedRowCountTests() {
            var schema = new CsvSchema() {ExpectedRowCount = 4, EnforceExpectedRowCount = true};
            Assert.AreEqual(1, CsvParser.FromString("1,2\n3,4\n5,6\n7,8\n", schema).ToList()[0][0]);
            Assert.AreEqual(1, CsvParser.FromString("1,2\n3,4\n5,6\r\n\r\n\n7,8\n", schema).ToList()[0][0]);
            Assert.Throws<Exception>(() => CsvParser.FromString("1,2\n3,4\n5,6\n7,8\n9,10\n", schema));
            Assert.Throws<Exception>(() => CsvParser.FromString("1,2\n3,4\n5,6\n\n", schema));
        }
    }
}