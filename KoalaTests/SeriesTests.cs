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
        public void InstantiationTest() {
            var array = new double[] {1, 2, 3, 4, 5};
            var series = new Series<double>("Test", array);
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

        [Test]
        public void IndexTest() {
            var series = new List<double> { 1, 2, 3, 4 }.ToSeries();
            Assert.AreEqual(2, series[1]);
            series[1] = 100;
            Assert.AreEqual(100, series[1]);
        }

        [Test]
        public void SliceTest() {
            var series = new List<double> { 1, 2, 3, 4 }.ToSeries();
            Assert.AreEqual(2, series[1]);
            var seriesSlice = series[0, 3];
            Assert.AreEqual(1, seriesSlice[0]);
            Assert.AreEqual(4, seriesSlice[1]);
            seriesSlice[0] = 100;
            seriesSlice[1] = 400;
            Assert.AreEqual(100, seriesSlice[0]);
            Assert.AreEqual(400, seriesSlice[1]);
            Assert.AreEqual(1, series[0]);
            Assert.AreEqual(4, series[3]);
        }

        [Test]
        public void SliceTestString() {
            var series = new List<String> { "Cat", "Dog", "Fish", "Bird" }.ToSeries();
            Assert.AreEqual("Dog", series[1]);
            var seriesSlice = series[0, 3];
            Assert.AreEqual("Cat", seriesSlice[0]);
            Assert.AreEqual("Bird", seriesSlice[1]);
            seriesSlice[0] = "Tuna";
            seriesSlice[1] = "Whale";
            Assert.AreEqual("Tuna", seriesSlice[0]);
            Assert.AreEqual("Whale", seriesSlice[1]);
            Assert.AreEqual("Cat", series[0]);
            Assert.AreEqual("Bird", series[3]);
        }

        [Test]
        public void AbsTest() {
            var series = new List<double> {-2, -1.5, 0, 1, 2}.ToSeries().Abs();
            Assert.AreEqual(2, series[0]);
            Assert.AreEqual(1.5, series[1]);
            Assert.AreEqual(0, series[2]);
            Assert.AreEqual(1, series[3]);
        }

        [Test]
        public void AddTest() {
            var series = new List<double> { -2, -1.5, 0, 1, 2 }.ToSeries().Add(1);
            Assert.AreEqual(-1, series[0]);
            Assert.AreEqual(-0.5, series[1]);
            Assert.AreEqual(1, series[2]);
            Assert.AreEqual(2, series[3]);
        }

        [Test]
        public void PlusTest() {
            var s1 = new List<double> {-2, -1.5, 0}.ToSeries("s1");
            var s2 = s1 + 1.5;
            Assert.AreEqual(-0.5, s2[0]);
            Assert.AreEqual(0, s2[1]);

            var s3 = 1 + s2;
            Assert.AreEqual(0.5, s3[0]);
            Assert.AreEqual(1, s3[1]);
        }
    }
}
