using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koalas
{
    public class Series<T> : List<T> {
        public String Name { get; set; }

        public Series (String name) {
            Name = name;
        } 
    }

    // Janky, one way to get around type checking for the DataFrame's collection of Series<T>
    public class Series : Series<object> {
        private Type _type;

        public Type Type {
            get { return _type; }
            set {
                if (Type == null) {
                    _type = value;
                }
                else if (Type != value) {
                    throw new ArrayTypeMismatchException();
                }
            }
        }

        public Series(String name) : base(name) {}

        public new void Add(object item) {
            Type = item.GetType();
            base.Add(item);
        }

        public new void AddRange(IEnumerable<object> collection) {
            throw new NotImplementedException();
        }

        public new void Insert(int index, object item) {
            Type = item.GetType();
            base.Insert(index, item);
        }

        public new void InsertRange(int index, IEnumerable<object> collection) {
            throw new NotImplementedException();
        }
    }

    public static class MyExtensions {
        public static Series<T> ToSeries<T>(this IEnumerable<T> enumerable) {
            var series = new Series<T>("");
            foreach (var elem in enumerable) {
                series.Add(elem);
            }
            return series;
        }  
    }
}