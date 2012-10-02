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

        public object[] Row(int index) {
            return ColumnNames.Select(column => _data[column][index]).ToArray();
        }

        public DataFrame Rows(int[] indices)
        {
            throw new NotImplementedException();
            //return new DataFrame(_data.Values.Select(series => series[indices]));
        }

        public Series<T> GetTypedSeries<T> (String seriesName) {
            return _data[seriesName] as Series<T>;
        }
    
        public static DataFrame FromCsvData(String data) {
            return FromCsvData(data, new CsvSchema());
        }

        public static DataFrame FromCsvData(String data, CsvSchema schema) {
            var csvParser = CsvParser.FromString(data, schema);
            var seriesList = csvParser.ColumnNames.Zip(csvParser.ColumnTypes, (name, type) => (Activator.CreateInstance(typeof(Series<>).MakeGenericType(type), name, csvParser.NumRows) as Series)).ToList();

            var irow = 0;
            foreach (var row in csvParser) {
                for (var i = 0; i < seriesList.Count; i++) {
                    seriesList[i][irow] = row[i];
                }
                irow++;
            }
            return new DataFrame(seriesList);
        }
    }
} 