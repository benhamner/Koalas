using Koalas;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KoalaTests
{
    class SeriesTests
    {
        [Test]
        public void MeanTest() {
            var series = new List<double> {1, 2, 3, 4}.ToSeries();
            Assert.AreEqual(2.5, series.Mean());

            series = new List<double> { 1.0 }.ToSeries();
            Assert.AreEqual(1.0, series.Mean());

            series = new List<double> { 1.0, 100.0 }.ToSeries();
            Assert.AreEqual(50.5, series.Mean());

            series = new List<double> { 1.0, Double.NaN }.ToSeries();
            Assert.AreEqual(Double.NaN, series.Mean());
        }
    }
}
