using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koalas {
    public abstract class Series {
        public abstract String Name { get; set; }
        public abstract Type Type { get; }

        public abstract int Count { get; }
        public abstract bool Contains(object value);
        public abstract int IndexOf(object value);
        public abstract object this[int index] { get; set; }
        public abstract Series this[params int[] indices] { get; set; }
    }

    public class Series<T> : Series, IEnumerable<T> {
        public override String Name { get; set; }

        public override Type Type {
            get { return typeof (T); }
        }

        private readonly T[] _array;

        public Series(String name, T[] array) {
            Name = name;
            _array = array;
        }

        public Series(String name, int length) {
            Name = name;
            _array = new T[length];
        }
        
        public IEnumerator<T> GetEnumerator() {
            return _array.Cast<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _array.GetEnumerator();
        }

        public override bool Contains(object value) {
            return _array.Contains((T) Convert.ChangeType(value, typeof (T)));
        }

        public override int IndexOf(object value) {
            return Array.IndexOf(_array, value);
        }

        public bool Contains(T item) {
            return _array.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _array.CopyTo(array, arrayIndex);
        }

        public override int Count {
            get { return _array.Count(); }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public int IndexOf(T item) {
            return Array.IndexOf(_array, item);
        }

        public override object this[int index] {
            get { return _array[index]; }
            set { _array[index] = (T) Convert.ChangeType(value, typeof(T)); }
        }

        public override Series this[params int[] indices] {
            get { return new Series<T>(Name, indices.Select(index => _array[index]).ToArray()); }
            set { throw new NotImplementedException(); }
        }
        
        public static Series<T> operator +(Series<T> s1, Series<T> s2) {
            if (typeof(T)==typeof(double)) {
                return (s1 as Series<double>).Add(s2 as Series<double>) as Series<T>;
            }
            throw new NotImplementedException();
        }

        public static Series<T> operator +(Series<T> s1, T x2) {
            if (typeof(T) == typeof(double)) {
                return (s1 as Series<double>).Add((double) Convert.ChangeType(x2, typeof(double))) as Series<T>;
            }
            throw new NotImplementedException();
        }

        public static Series<T> operator +(T x1, Series<T> s2) { return s2 + x1; }



    
    }
    
}