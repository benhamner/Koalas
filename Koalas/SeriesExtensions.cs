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

        public static Series<double> Add(this Series<double> s1, Series<double> s2) {
            return s1.Zip(s2, (x1, x2) => x1 + x2).ToSeries(s1.Name + "+" + s2.Name);
        }

    }
}
