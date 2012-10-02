using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koalas {
    public static class SeriesExtensions {
        public static Series<T> ToSeries<T>(this IEnumerable<T> enumerable) {
            return enumerable.ToSeries("");
        }
        
        public static Series<T> ToSeries<T>(this IEnumerable<T> enumerable, String name) {
            return new Series<T>(name, enumerable.ToArray());
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
