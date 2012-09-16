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
    }
}
