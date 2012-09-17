using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Koalas;

namespace KoalaTests
{
    public class DataFrameTests
    {
        [Test]
        public void GetColumnCountTest()
        {
            var data = @"1,2,3
4,5,6
7,8,9";
            Assert.AreEqual(3, DataFrame.GetColumnCount(data));
        }

        [Test]
        public void FromCsvDataTest()
        {
            var data = "1,2,3\r4,5,6";
            var df = DataFrame.FromCsvData(data);
            Assert.AreEqual("5", df[1][1]);

            data = "1,2,\"asdf\"\n,4,5,6";
            df = DataFrame.FromCsvData(data);
            Assert.AreEqual("asdf", df[2][0]);

            data = "1,2,\"as\"\"df\"\n,4,5,6";
            df = DataFrame.FromCsvData(data);
            Assert.AreEqual("as\"df", df[2][0]);
        }
    }

}
