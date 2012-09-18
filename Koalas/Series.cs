using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Koalas
{
    public class Series<T> : IEnumerable<T> {
        public List<T> Data;

        public Series () {

            Data = new List<T>();
        }

        public Series(int length)
        {

            Data = new List<T>(length);
        } 

        public IEnumerator<T> GetEnumerator() {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
