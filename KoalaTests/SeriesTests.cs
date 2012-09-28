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
        public void ContainsTest() {
            var series = new List<double> {1, 2, 3, 4}.ToSeries();
            Assert.IsTrue(series.Contains(3));
            Assert.IsFalse(series.Contains(5));
            var untypedSeries = (Series) series;
            Assert.IsTrue(untypedSeries.Contains(3));
            Assert.IsFalse(untypedSeries.Contains(10));
        }

        [Test]
        public void AddTest() {
            var series = new List<double> { 1, 2, 3, 4 }.ToSeries();
            series.Add(5);
            Assert.AreEqual(5, series[4]);
            var untypedSeries = (Series)series;
            untypedSeries.Add(6);
            Assert.AreEqual(6, untypedSeries[5]);
            Assert.AreEqual(6, series[5]);
        }
        
        [Test]
        public void InstantiationTest()
        {
            var array = new double[] {1, 2, 3, 4, 5};
            var series = new Series<double>("Test", array.ToList());
        }

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
