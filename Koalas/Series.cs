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
        public abstract void Append(object value);
        public abstract bool Contains(object value);
        public abstract void Clear();
        public abstract int IndexOf(object value);
        public abstract void Insert(int index, object value);
        public abstract void Remove(object value);
        public abstract void RemoveAt(int index);
        public abstract object this[int index] { get; set; }
        public abstract Series this[params int[] indices] { get; set; }
    }

    public class Series<T> : Series, IEnumerable<T> {
        public override String Name { get; set; }

        public override Type Type {
            get { return typeof (T); }
        }

        private readonly List<T> _list;

        public Series(String name, List<T> list) {
            Name = name;
            _list = list;
        }

        public Series(String name) {
            Name = name;
            _list = new List<T>();
        }

        public IEnumerator<T> GetEnumerator() {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _list.GetEnumerator();
        }

        public void Append(T item) {
            _list.Add(item);
        }

        public override void Append(object value) {
            _list.Add((T) Convert.ChangeType(value, typeof (T)));
        }

        public override bool Contains(object value) {
            return _list.Contains((T) Convert.ChangeType(value, typeof (T)));
        }

        public override void Clear() {
            _list.Clear();
        }

        public override int IndexOf(object value) {
            return _list.IndexOf((T) Convert.ChangeType(value, typeof (T)));
        }

        public override void Insert(int index, object value) {
            _list.Insert(index, (T) value);
        }

        public override void Remove(object value) {
            _list.Remove((T) Convert.ChangeType(value, typeof (T)));
        }

        public bool Contains(T item) {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            return _list.Remove(item);
        }

        public override int Count {
            get { return _list.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public int IndexOf(T item) {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item) {
            _list.Insert(index, item);
        }

        public override void RemoveAt(int index) {
            _list.RemoveAt(index);
        }

        public override object this[int index] {
            get { return _list[index]; }
            set { _list[index] = (T) Convert.ChangeType(value, typeof(T)); }
        }

        public override Series this[params int[] indices] {
            get { return new Series<T>(Name, indices.Select(index => _list[index]).ToList()); }
            set { throw new NotImplementedException(); }
        }

    }


}