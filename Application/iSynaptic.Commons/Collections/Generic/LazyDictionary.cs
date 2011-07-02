using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class LazyDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, Lazy<TValue>> _Underlying;

        public LazyDictionary() : this(EqualityComparer<TKey>.Default)
        {
        }

        public LazyDictionary(IEqualityComparer<TKey> comparer)
            : this(new Dictionary<TKey, Lazy<TValue>>(comparer))
        {
        }

        public LazyDictionary(IDictionary<TKey, Lazy<TValue>> underlying)
        {
            _Underlying = Guard.NotNull(underlying, "underlying");
        }

        public override int Count
        {
            get { return _Underlying.Count; }
        }

        public override void Clear()
        {
            _Underlying.Clear();
        }

        public override void Add(TKey key, TValue value)
        {
            _Underlying.Add(key, new Lazy<TValue>(() => value));
        }

        public void Add(TKey key, Func<TValue> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");
            Add(key, new Lazy<TValue>(valueFactory));
        }

        public void Add(TKey key, Lazy<TValue> lazyValue)
        {
            Guard.NotNull(lazyValue, "lazyValue");
            _Underlying.Add(key, lazyValue);
        }

        public override bool Remove(TKey key)
        {
            return _Underlying.Remove(key);
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            var results = _Underlying
                .TryGetValue(key)
                .Select(x => x.Value);

            value = results.ValueOrDefault();
            return results.HasValue;
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Underlying.Select(x => KeyValuePair.Create(x.Key, x.Value.Value)).GetEnumerator();
        }

        protected override void SetValue(TKey key, TValue value)
        {
            _Underlying[key] = new Lazy<TValue>(() => value);
        }

        public override bool ContainsKey(TKey key)
        {
            return _Underlying.ContainsKey(key);
        }
    }
}
