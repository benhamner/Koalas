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
            Assert.AreEqual(true, CsvParser.FromString("id,val\n1,2\n3,4").HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("1,2\r3,4").HasHeader);
            Assert.AreEqual(true, CsvParser.FromString("id,1\n1,2\n3,4").HasHeader);
            Assert.AreEqual(true, CsvParser.FromString("id,\"1\"\n1,2\n3,4").HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("c1,2\nc3,4").HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("1,2\nc3,c4").HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("1.5,2.5\n1,2\n3,4\n").HasHeader);
            Assert.AreEqual(true, CsvParser.FromString("a,b,c,d\n1,2,3,4\n4,5,6,7\n").HasHeader);
            Assert.AreEqual(true, CsvParser.FromString("a,b,c\nc,d,e").HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("a,b,b\nc,d,e").HasHeader);
            Assert.AreEqual(false, CsvParser.FromString("id,id\n1,2\n3,4").HasHeader);
        }
    }
}
