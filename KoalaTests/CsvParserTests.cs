using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Koalas;

namespace KoalaTests {
    class CsvParserTests {
        [Test]
        public void IsTypeGreaterTest() {
            Assert.AreEqual(true, CsvParser.IsGreaterType(typeof(String), typeof(Double)));
            Assert.AreEqual(true, CsvParser.IsGreaterType(typeof(String), typeof(Int64)));
            Assert.AreEqual(false, CsvParser.IsGreaterType(typeof(String), typeof(String)));
            Assert.AreEqual(true, CsvParser.IsGreaterType(typeof(Double), typeof(Int64)));
            Assert.AreEqual(false, CsvParser.IsGreaterType(typeof(Double), typeof(Double)));
            Assert.AreEqual(false, CsvParser.IsGreaterType(typeof(Double), typeof(String)));
            Assert.AreEqual(false, CsvParser.IsGreaterType(typeof(Int64), typeof(Double)));
            Assert.AreEqual(false, CsvParser.IsGreaterType(typeof(Int64), typeof(Int64)));
            Assert.AreEqual(false, CsvParser.IsGreaterType(typeof(Int64), typeof(String)));
        }

        [Test]
        public void HeaderInferenceTest() {
            Assert.AreEqual(true, CsvParser.FromString("id,val\n1,2\n3,4").Schema.HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("1,2\r3,4").Schema.HasHeader);
            Assert.AreEqual(true, CsvParser.FromString("id,1\n1,2\n3,4").Schema.HasHeader);
            Assert.AreEqual(true, CsvParser.FromString("id,\"1\"\n1,2\n3,4").Schema.HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("c1,2\nc3,4").Schema.HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("1,2\nc3,c4").Schema.HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("1.5,2.5\n1,2\n3,4\n").Schema.HasHeader);
            Assert.AreEqual(true, CsvParser.FromString("a,b,c,d\n1,2,3,4\n4,5,6,7\n").Schema.HasHeader);
            Assert.AreEqual(true, CsvParser.FromString("a,b,c\nc,d,e").Schema.HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("a,b,b\nc,d,e").Schema.HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("id,id\n1,2\n3,4").Schema.HasHeader);
        }

        [Test]
        public void ColumnInferenceTest() {
            Assert.AreEqual(typeof(String), CsvParser.FromString("id, val\ra,b\rc,d").ColumnTypes[0]);
            Assert.AreEqual(typeof(Int64), CsvParser.FromString("id, val\r1,b\r2,d").ColumnTypes[0]);
            Assert.AreEqual(typeof(Double), CsvParser.FromString("id, val\r1.,b\r2,d").ColumnTypes[0]);
            Assert.AreEqual(typeof(String), CsvParser.FromString("id, val\r1.,b\r2,d").ColumnTypes[1]);
            Assert.AreEqual(typeof(Int64), CsvParser.FromString("id, val\r1.,1\r2,1000000000").ColumnTypes[1]);
        }

        [Test]
        public void ParsingTest() {
            var data = "1,2,3\n4,5,6";
            var csv = CsvParser.FromString(data).ToList();
            Assert.AreEqual(1, csv[0][0]);
            Assert.AreEqual(2, csv[0][1]);
            Assert.AreEqual(3, csv[0][2]);
            Assert.AreEqual(4, csv[1][0]);
            Assert.AreEqual(5, csv[1][1]);
            Assert.AreEqual(6, csv[1][2]);

            data = "1,2,3\n4,5,6";
            csv = CsvParser.FromString(data).ToList();
            Assert.AreEqual(1, csv[0][0]);
            Assert.AreEqual(2, csv[0][1]);
            Assert.AreEqual(3, csv[0][2]);
            Assert.AreEqual(4, csv[1][0]);
            Assert.AreEqual(5, csv[1][1]);
            Assert.AreEqual(6, csv[1][2]);
        }
    }
}
