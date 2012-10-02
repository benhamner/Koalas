using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koalas {
    public static class SeriesExtensions {
        public static Series<T> ToSeries<T>(this IEnumerable<T> enumerable) {
            var series = new Series<T>("");
            foreach (var item in enumerable) {
                series.Append(item);
            }
            return series;
        }

        public static Series<T> ToSeries<T>(this IEnumerable<T> enumerable, String name) {
            var series = new Series<T>(name);
            foreach (var item in enumerable) {
                series.Append(item);
            }
            return series;
        }

        public static double Mean(this Series<double> series) {
            return series.Sum() / series.Count;
        }

        public static Series<double> Abs(this Series<double> series) {
            return series.Select(Math.Abs).ToSeries(series.Name);
        }

        public static Series<double> Add(this Series<double> series, double value) {
            return series.Select(x => x + value).ToSeries(series.Name);
        }

    }
}
