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
        }
    }
}
