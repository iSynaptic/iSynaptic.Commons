using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class KeyCollection<TKey, TValue> : ICollection<TKey>
    {
        private static NotSupportedException _ReadOnlyException = new NotSupportedException("Mutating a key collection derived from a dictionary is not allowed.");

        private readonly ICollection<KeyValuePair<TKey, TValue>> _Underlying;
        

        public KeyCollection(ICollection<KeyValuePair<TKey, TValue>> underlying)
        {
            if (underlying == null)
                throw new ArgumentNullException("underlying");

            _Underlying = underlying;
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            return _Underlying.Select(x => x.Key).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(TKey item)
        {
            throw _ReadOnlyException;
        }

        public void Clear()
        {
            throw _ReadOnlyException;
        }

        public bool Contains(TKey item)
        {
            var dictionary = _Underlying as IDictionary<TKey, TValue>;
            if (dictionary != null)
                return dictionary.ContainsKey(item);

            return _Underlying.Any(x => x.Key.Equals(item));
        }

        public void CopyTo(TKey[] array, int arrayIndex)
        {
            _Underlying
                .Select(x => x.Key)
                .CopyTo(array, arrayIndex);
        }

        public bool Remove(TKey item)
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
