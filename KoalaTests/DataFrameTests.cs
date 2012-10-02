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
        public void ColumnIndexByNumberTest() {
            var data = "1,2,3\r4,5,6";
            var df = DataFrame.FromCsvData(data);
            Assert.AreEqual(6, df[-1][1]);
            Assert.AreEqual(2, df[-2][0]);
            Assert.AreEqual(2, df[1][0]);
            Assert.AreEqual(1, df[0][0]);
        }

        [Test]
        public void ColumnIndexByNameTest() {
            var data = "lion,tiger,bear\r1,2,3\r4,5,6";
            var df = DataFrame.FromCsvData(data);
            Assert.AreEqual(6, df["bear"][1]);
            Assert.AreEqual(2, df["tiger"][0]);
            Assert.AreEqual(2, df[-2][0]);
            Assert.AreEqual(1, df["lion"][0]);
        }

        [Test]
        public void AddRowTest() {
            var data = "1,2,Ben\n4,5,Will";
            var df = DataFrame.FromCsvData(data);
            df.AddRow(1,2,"Margit");
            Assert.AreEqual("Margit", df[2][2]);
            Assert.AreEqual("Will", df[2][1]);
            Assert.AreEqual(1, df[0][0]);
            Assert.AreEqual(1, df[0][2]);
        }

        [Test]
        public void ColumnSubsetByNamesTest() {
            var data = "Animal,Legs,Furry\nCat,4,1\nDog,4,1\nHuman,2,0";
            var df = DataFrame.FromCsvData(data);
            var dfSubset = df["Animal", "Legs"];
            Assert.AreEqual(2, dfSubset.ColumnCount);
            Assert.AreEqual("Cat", dfSubset["Animal"][0]);
            Assert.Throws<KeyNotFoundException>(() => { var x = dfSubset["Furry"]; });
        }

        [Test]
        public void ColumnSubsetByIndexTest() {
            var data = "Animal,Legs,Furry\nCat,4,1\nDog,4,1\nHuman,2,0";
            var df = DataFrame.FromCsvData(data);
            var dfSubset = df[0, 1];
            Assert.AreEqual(2, dfSubset.ColumnCount);
            Assert.AreEqual("Cat", dfSubset["Animal"][0]);
            Assert.Throws<KeyNotFoundException>(() => {var x=dfSubset["Furry"]; } );
        }

        [Test]
        // THIS MEANS THAT DOING AN ASSIGNMENT ON A SUBSET ALSO EFFECTS THE ORIGINAL DATAFRAME IF IT'S NOT EXPLICITLY COPIED
        public void DataFrameNotCopiedOnSubsetTest() {
            var data = "Animal,Legs,Furry\nCat,4,1\nDog,4,1\nHuman,2,0";
            var df = DataFrame.FromCsvData(data);
            var dfSubset = df[0, 1];
            df["Animal"][0] = "Kitten";
            Assert.AreEqual("Kitten", dfSubset["Animal"][0]);
            dfSubset["Animal"][0] = "Puppy";
            Assert.AreEqual("Puppy", df["Animal"][0]);
        }
    }
}