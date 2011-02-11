using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace iSynaptic.Commons.Collections.Generic
{
    public class MultiMap<TKey, TValue>
    {
        private readonly IDictionary<TKey, ICollection<TValue>> _Dictionary = null;

        public MultiMap(IDictionary<TKey, ICollection<TValue>> dictionary)
        {
            Guard.NotNull(dictionary, "dictionary");
            _Dictionary = dictionary;
        }

        public void Add(TKey key, TValue value)
        {
            var values = GetCollection(key);
            values.Add(value);
        }

        public void Remove(TKey key, TValue value)
        {
            var values = GetCollection(key);
            values.Remove(value);
        }

        public IEnumerable<TValue> this[TKey key]
        {
            get { return GetCollection(key); }
        }

        protected virtual ICollection<TValue> GetCollection(TKey key)
        {
            var values = _Dictionary.TryGetValue(key);
            if (values.HasValue != true)
            {
                values = new List<TValue>();
                _Dictionary.Add(key, values.Value);
            }

            return values.Value;
        }
    }
}
