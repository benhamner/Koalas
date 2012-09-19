using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Koalas;

namespace KoalaTests {
    public class CsvReaderTests {
        [Test]
        public void LineEndingsTest() {
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
        public void ResetTest() {
            var data = "1,2\r3,4";
            var csv = CsvReader.FromString(data);
            Assert.AreEqual("1", csv.First().First());
            Assert.AreEqual("3", csv.First().First());

            data = "1,2\r3,4";
            csv = CsvReader.FromString(data);
            Assert.AreEqual("1", csv.ToList().First().First());
            Assert.AreEqual(2, csv.Count());
            Assert.AreEqual("1", csv.First().First());
        }

        [Test]
        public void BlankLineTest() {
            var data = "1,2\r\n\r\n3,4\r\n";
            var csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("1", csv[0][0]);
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3", csv[1][0]);
            Assert.AreEqual("4", csv[1][1]);
            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual(2, csv[0].Count);
            Assert.AreEqual(2, csv[1].Count);
        }

        [Test]
        public void QuotedFieldTest() {
            var data = "1,\"2\"\n3,4";
            var csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("1", csv[0][0]);
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3", csv[1][0]);
            Assert.AreEqual("4", csv[1][1]);
            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual(2, csv[0].Count);
            Assert.AreEqual(2, csv[1].Count);
        }

        [Test]
        public void QuoteInQuotedFieldTest() {
            var data = "1,\"\"\"2\"\n3,4";
            var csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("1", csv[0][0]);
            Assert.AreEqual("\"2", csv[0][1]);
            Assert.AreEqual("3", csv[1][0]);
            Assert.AreEqual("4", csv[1][1]);
            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual(2, csv[0].Count);
            Assert.AreEqual(2, csv[1].Count);
        }

        [Test]
        public void NewLineInQuotedFieldTest() {
            var data = "1,\"\n2\"\n3,4";
            var csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("1", csv[0][0]);
            Assert.AreEqual("\n2", csv[0][1]);
            Assert.AreEqual("3", csv[1][0]);
            Assert.AreEqual("4", csv[1][1]);
            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual(2, csv[0].Count);
            Assert.AreEqual(2, csv[1].Count);
        }

        [Test]
        public void SingleColumnTest() {
            var data = "1\n0\n";
            var csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("1", csv[0][0]);
            Assert.AreEqual("0", csv[1][0]);
            Assert.AreEqual(2, csv.Count);
            Assert.AreEqual(1, csv[0].Count);
            Assert.AreEqual(1, csv[1].Count);
        }

        [Test]
        public void ChrisClarkTest()
        {
            // PlainText
            Assert.AreEqual("this is my result", CsvReader.FromString("this is my result").First().First());

            // PlainTextCsv
            var data = "this is my result,this is my result";
            var csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("this is my result", csv[0][0]);
            Assert.AreEqual("this is my result", csv[0][1]);
            Assert.AreEqual(1, csv.Count());
            Assert.AreEqual(2, csv[0].Count());

            // PlainTextCsvSpace
            // Modification of Chris's initial test: we want to preserve whitespace
            data = "this is my result, this is my result";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("this is my result", csv[0][0]);
            Assert.AreEqual(" this is my result", csv[0][1]);
            Assert.AreEqual(1, csv.Count());
            Assert.AreEqual(2, csv[0].Count());

            // PlainTextCsvSimpleQuote
            data = "this is my result,\"this is my result\"";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("this is my result", csv[0][0]);
            Assert.AreEqual("this is my result", csv[0][1]);
            Assert.AreEqual(1, csv.Count());
            Assert.AreEqual(2, csv[0].Count());

            // PlainTextCsvQuoteWithComma
            data = "this is my result,\"this, that, another is my result\"";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("this is my result", csv[0][0]);
            Assert.AreEqual("this, that, another is my result", csv[0][1]);
            Assert.AreEqual(1, csv.Count());
            Assert.AreEqual(2, csv[0].Count());

            // PlainTextCsvQuoteWithCommaMultiline
            data = "this is my result,\"this, that, another is my result\"\nthis is my result,\"this, that, another is my result\"";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("this is my result", csv[0][0]);
            Assert.AreEqual("this, that, another is my result", csv[0][1]);
            Assert.AreEqual("this is my result", csv[1][0]);
            Assert.AreEqual("this, that, another is my result", csv[1][1]);
            Assert.AreEqual(2, csv.Count());
            Assert.AreEqual(2, csv[0].Count());

            // QuotesInAQuotedField
            data = "the inseam is 7\" which is quite normal,2,3,4";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("the inseam is 7\" which is quite normal", csv[0][0]);
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3", csv[0][2]);
            Assert.AreEqual("4", csv[0][3]);
            Assert.AreEqual(1, csv.Count());
            Assert.AreEqual(4, csv[0].Count());

            // QuotesFollwedByCommaInAQuotedField
            data = "\"the inseam is 7\"\", which is quite normal\",2,3,4";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("the inseam is 7\", which is quite normal", csv[0][0]);
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3", csv[0][2]);
            Assert.AreEqual("4", csv[0][3]);
            Assert.AreEqual(1, csv.Count());
            Assert.AreEqual(4, csv[0].Count());

        
            // Public Sub ManyQuotesFollwedByCommaInAQuotedField()
            data = "\"the inseam is 7\"\"\"\", which is quite normal\",2,3,4";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("the inseam is 7\"\", which is quite normal", csv[0][0]);
            Assert.AreEqual("2", csv[0][1]);
            Assert.AreEqual("3", csv[0][2]);
            Assert.AreEqual("4", csv[0][3]);
            Assert.AreEqual(1, csv.Count());
            Assert.AreEqual(4, csv[0].Count());         

            // ManyQuotesLaterInLineFollwedByCommaInAQuotedField()
            data = "test,\"the inseam is 7\"\"\"\", which is quite normal\",2,3,4";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("test", csv[0][0]);
            Assert.AreEqual("the inseam is 7\"\", which is quite normal", csv[0][1]);
            Assert.AreEqual("2", csv[0][2]);
            Assert.AreEqual("3", csv[0][3]);
            Assert.AreEqual("4", csv[0][4]);
            Assert.AreEqual(1, csv.Count());
            Assert.AreEqual(5, csv[0].Count());   

            // CommaList
            data = "\"Drill, Ye Tarriers, Drill (1900)\",9/1/1900";
            csv = CsvReader.FromString(data).ToList();
            Assert.AreEqual("Drill, Ye Tarriers, Drill (1900)", csv[0][0]);
            Assert.AreEqual("9/1/1900", csv[0][1]);
            Assert.AreEqual(1, csv.Count());
            Assert.AreEqual(2, csv[0].Count());   
        }
    }
}