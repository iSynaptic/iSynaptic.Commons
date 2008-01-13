using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    internal struct SimpleArray<T>
    {
        private int _Count;
        private T[] _Items;

        public SimpleArray(IEnumerable<T> source)
        {
            _Count = 0;
            _Items = null;

            ICollection<T> col = source as ICollection<T>;
            if (col != null)
            {
                _Count = col.Count;
                if (_Count > 0)
                {
                    _Items = new T[_Count];
                    col.CopyTo(_Items, 0);
                }
            }
            else
            {
                foreach (T item in source)
                {
                    if (_Items == null)
                        _Items = new T[4];
                    else if (_Items.Length == _Count)
                    {
                        T[] newItems = new T[_Count * 2];
                        Array.Copy(_Items, 0, newItems, 0, _Count);
                        _Items = newItems;
                    }

                    _Items[_Count] = item;
                    _Count++;
                }
            }
        }

        public T[] ToArray()
        {
            if (_Count == 0)
                return new T[0];

            T[] results = new T[_Count];
            Array.Copy(_Items, 0, results, 0, _Count);

            return results;
        }
    }
}
