using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Koalas;

namespace KoalaTests
{
    public class CsvReaderTests
    {
        [Test]
        public void LineEndingsTest()
        {
            var data = "1,2\r3,4";
            var csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("1", csv[0][0]);
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3", csv[1][0]);
            Assert.AreEqual("4", csv[1][1]);
            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual(2, csv[0].Count);
            Assert.AreEqual(2, csv[1].Count);

            data = "1,2\r3,4\r";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("1", csv[0][0]);
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3", csv[1][0]);
            Assert.AreEqual("4", csv[1][1]);
            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual(2, csv[0].Count);
            Assert.AreEqual(2, csv[1].Count);

            data = "1,2\n3,4\n";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("1", csv[0][0]);
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3", csv[1][0]);
            Assert.AreEqual("4", csv[1][1]);
            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual(2, csv[0].Count);
            Assert.AreEqual(2, csv[1].Count);

            data = "1,2\r\n3,4\r\n";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("1", csv[0][0]);
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3", csv[1][0]);
            Assert.AreEqual("4", csv[1][1]);
            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual(2, csv[0].Count);
            Assert.AreEqual(2, csv[1].Count);
        }

        [Test]
        public void ResetTest()
        {

            var data = "1,2\r3,4";
            var csv = CsvReader.FromString(data);
            Assert.AreEqual("1", csv.First().First());
            Assert.AreEqual("1", csv.First().First());
        }
    }
}
