using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Koalas;

namespace KoalaTests {
    public class DataFrameTests {
        [Test]
        public void FromCsvDataTest() {
            var data = "1,2,3\r4,5,6";
            var df = DataFrame.FromCsvData(data);
            Assert.AreEqual(5, df[1][1]);

            data = "a,b,c\n1,2,\"asdf\"\n4,5,6";
            df = DataFrame.FromCsvData(data);
            Assert.AreEqual("asdf", df[2][0]);

            data = "a,b,c\n1,2,\"as\"\"df\"\n4,5,6";
            df = DataFrame.FromCsvData(data);
            Assert.AreEqual("as\"df", df[2][0]);

            data = "a,b,c\n1,2.0,3\r4,5,6";
            df = DataFrame.FromCsvData(data);
            Assert.AreEqual(5.0, df[1][1]);
        }

        [Test]
        public void TypeTests() {
            var data = "1,2\n3,4.0\n5,6\n";
            var df = DataFrame.FromCsvData(data);
            Assert.AreEqual(4.0, df.GetSeriesByType<double>()[1]);
            Assert.AreEqual(1, df.GetSeriesByType<long>()[0]);
        }

    }
}