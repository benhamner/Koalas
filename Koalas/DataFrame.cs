﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Koalas {

    public class DataFrame<T> {
        private readonly Dictionary<String, Series<T>> _data;
        public List<String> ColumnNames;

        public DataFrame(Dictionary<String, Series<T>> data)
        {
            _data = data;
            ColumnNames = data.Select(x => x.Key).ToList();
        }

        public DataFrame(List<Series<T>> seriesList)
        {
            _data = seriesList.ToDictionary(series => series.Name);
            ColumnNames = seriesList.Select(series => series.Name).ToList();
        }

        public Series<T> this[String seriesIndex]
        {
            get { return _data[seriesIndex]; }
        }

        public Series<T> this[int index]
        {
            get { return _data[ColumnNames[index]]; }
        }

    }

    public class DataFrame {
        private readonly Dictionary<String, Series> _data;
        public List<String> ColumnNames;

        public DataFrame(Dictionary<String, Series> data) {
            _data = data;
            ColumnNames = data.Select(x => x.Key).ToList();
        }

        public DataFrame(List<Series> seriesList) {
            _data = seriesList.ToDictionary(series => series.Name);
            ColumnNames = seriesList.Select(series => series.Name).ToList();
        }

        public Series this[String seriesIndex] {
            get { return _data[seriesIndex]; }
        }

        public Series this[int index]
        {
            get { return _data[ColumnNames[index]]; }
        }

        public Series<T> GetSeriesByType<T>() {
            if (_data != null) {
                var first = _data.Values.FirstOrDefault(series => series.Type == typeof (T));
                if (first != null) {
                    var series = first.Cast<T>().ToSeries();
                    series.Name = first.Name;
                    return series;
                }
                throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }

        public DataFrame<T> GetDataFrameByType<T> ()
        {
            if (_data != null)
            {
                var seriesList =
                    _data.Values.Where(series => series.Type == typeof (T)).Select(series => series.Cast<T>().ToSeries(series.Name)).
                        ToList();
                return new DataFrame<T>(seriesList);
            }
            throw new NotImplementedException();
        }

        public static DataFrame FromCsvData(String data) {
            return FromCsvData(data, new CsvSchema());
        } 

        public static DataFrame FromCsvData(String data, CsvSchema schema) {
            var csvParser = CsvParser.FromString(data, schema);

            var seriesList = csvParser.ColumnNames.Select(name => new Series(name)).ToList();
            foreach (var row in csvParser) {
                for (var i=0; i<seriesList.Count; i++) {
                    seriesList[i].Add(row[i]);
                }
            }
            return new DataFrame(seriesList);
        }
    }
} 