using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class ValueCollection<TKey, TValue> : ICollection<TValue>
    {
        private static NotSupportedException _ReadOnlyException = new NotSupportedException("Mutating a value collection derived from a dictionary is not allowed.");

        private readonly ICollection<KeyValuePair<TKey, TValue>> _Underlying;

        public ValueCollection(ICollection<KeyValuePair<TKey, TValue>> underlying)
        {
            if (underlying == null)
                throw new ArgumentNullException("underlying");

            _Underlying = underlying;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return _Underlying.Select(x => x.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TValue item)
        {
            throw _ReadOnlyException;
        }

        public void Clear()
        {
            throw _ReadOnlyException;
        }

        public bool Contains(TValue item)
        {
            return _Underlying.Any(x => x.Value.Equals(item));
        }

        public void CopyTo(TValue[] destination, int index)
        {
            _Underlying
                .Select(x => x.Value)
                .CopyTo(destination, index);
        }

        public bool Remove(TValue item)
        {
            throw _ReadOnlyException;
        }

        public int Count
        {
            get { return _Underlying.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }
    }
}
