using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace iSynaptic.Commons.Collections.Generic
{
    public class MultiMap<TKey, TValue>
    {
        private readonly Func<TKey, ICollection<TValue>> _CollectionFactory = null;

        public MultiMap()
            : this(new Dictionary<TKey, ICollection<TValue>>())
        {
        }

        public MultiMap(IDictionary<TKey, ICollection<TValue>> dictionary)
            : this(GetDictionaryBackedCollectionFactory(dictionary))
        {
        }

        public MultiMap(Func<TKey, ICollection<TValue>> collectionFactory)
        {
            Guard.NotNull(collectionFactory, "collectionFactory");
            _CollectionFactory = collectionFactory;
        }

        private static Func<TKey, ICollection<TValue>> GetDictionaryBackedCollectionFactory(IDictionary<TKey, ICollection<TValue>> dictionary = null)
        {
            Guard.NotNull(dictionary, "dictionary");
            Guard.MustSatisfy(dictionary, x => !x.IsReadOnly, "dictionary", "Dictionary must not be read-only.");

            return x => dictionary
                .TryGetValue(x)
                .OnNoValue((Func<ICollection<TValue>>)(() =>
                {
                    var values = new List<TValue>();
                    dictionary.Add(x, values);

                    return values;
                }))
                .Value;
        }

        public void Add(TKey key, TValue value)
        {
            var collection = GetCollection(key);
            collection.Add(value);
        }

        public void AddRange(TKey key, IEnumerable<TValue> values)
        {
            Guard.NotNull(values, "values");
            
            var collection = GetCollection(key);
            collection.AddRange(values);
        }

        public void Remove(TKey key, TValue value)
        {
            var collection = GetCollection(key);
            collection.Remove(value);
        }

        public ICollection<TValue> this[TKey key]
        {
            get { return GetCollection(key); }
        }

        protected virtual ICollection<TValue> GetCollection(TKey key)
        {
            return _CollectionFactory(key);
        }
    }
}
