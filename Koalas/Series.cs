using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koalas
{
    public class Series<T> : IEnumerable<T> {
        public List<T> Data;
        public String Name;

        public Series (String name) {
            Name = name;
            Data = new List<T>();
        }

        public Series(String name, int length)
        {
            Data = new List<T>(length);
            Name = name;
        }

        public T this[int index] {
            get { return Data[index]; }
        }

        public IEnumerator<T> GetEnumerator() {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
