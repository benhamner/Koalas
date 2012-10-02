using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas {

    public class DataFrame {
        private readonly Dictionary<String, Series> _data;
        public List<String> ColumnNames;
        private int _columnCount;
        private int _rowCount;
        public int ColumnCount { get { return _columnCount; } }
        public int RowCount { get { return _rowCount; } }
        
        public DataFrame(Dictionary<String, Series> data) {
            _columnCount = data.Count;
            _data = data;
            ColumnNames = data.Select(x => x.Key).ToList();
        }

        public DataFrame(List<Series> seriesList) {
            _columnCount = seriesList.Count;
            _data = seriesList.ToDictionary(series => series.Name);
            ColumnNames = seriesList.Select(series => series.Name).ToList();
        }

        public Series this[String seriesIndex] {
            get { return _data[seriesIndex]; }
        }

        public Series this[int index] {
            get {
                return index < 0 ? _data[ColumnNames[ColumnNames.Count + index]] : _data[ColumnNames[index]];
            }
        }

        public DataFrame this[params String[] columnNames] {
            get {
                return new DataFrame(columnNames.Select(name => _data[name]).ToList());
            }            
        }

        public DataFrame this[params int[] indices] {
            get {
                return new DataFrame(indices.Select(index => _data[ColumnNames[index < 0 ? ColumnNames.Count + index : index]]).ToList());
            }
        }

        public Series<T> GetTypedSeries<T> (String seriesName) {
            return _data[seriesName] as Series<T>;
        }
    
        public void AddRow(params object[] row) {
            if (row.Count() != ColumnCount) {
                throw new ArgumentException(String.Format("Column Count Mismatch: There are {0} columns in the row, but {1} columns in the dataframe", row.Count(), ColumnCount));
            }
            for (var i=0; i<ColumnCount; i++)
            {
                _data[ColumnNames[i]].Add(row[i]);
            }
            _rowCount++;
        }

        public static DataFrame FromCsvData(String data) {
            return FromCsvData(data, new CsvSchema());
        }

        public static DataFrame FromCsvData(String data, CsvSchema schema) {
            var csvParser = CsvParser.FromString(data, schema);

            //var seriesList = csvParser.ColumnNames.Select(name => new Series(name)).ToList();

            var seriesList = csvParser.ColumnNames.Zip(csvParser.ColumnTypes, (name, type) => (Activator.CreateInstance(typeof(Series<>).MakeGenericType(type), name) as Series)).ToList();
            
            //dynamic seriesList = csvParser.ColumnNames.Zip(csvParser.ColumnTypes, (name, type) => (Activator.CreateInstance(typeof(Series<>).MakeGenericType(type), name))).ToList();

            foreach (var row in csvParser) {
                for (var i = 0; i < seriesList.Count; i++) {
                    seriesList[i].Add(row[i]);
                }
            }
            return new DataFrame(seriesList);
        }
    }
} 