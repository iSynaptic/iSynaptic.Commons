using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class LazySelectionDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>
    {
        private readonly Func<TKey, Maybe<TValue>> _Selector;
        private readonly IDictionary<TKey, TValue> _Underlying = null;

        public LazySelectionDictionary(Func<TKey, Maybe<TValue>> selector) : this(new Dictionary<TKey, TValue>(), selector)
        {
        }

        public LazySelectionDictionary(IDictionary<TKey, TValue> underlying, Func<TKey, Maybe<TValue>> selector)
        {
            _Selector = Guard.NotNull(selector, "selector");

            Guard.NotNull(underlying, "underlying");
            _Underlying = Guard.MustSatisfy(underlying, x => !x.IsReadOnly, "underlying", "Underlying dictionary must not be read-only.");
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
            _Underlying.Add(key, value);
        }

        public override bool Remove(TKey key)
        {
            return _Underlying.Remove(key);
        }

        public override bool ContainsKey(TKey key)
        {
            return _Underlying.ContainsKey(key);
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            if (_Underlying.TryGetValue(key, out value))
                return true;

            var results = _Selector(key);
            if(results.HasValue)
            {
                value = results.Value;
                _Underlying.Add(key, value);
                return true;
            }

            value = default(TValue);
            return false;
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Underlying.GetEnumerator();
        }

        protected override void SetValue(TKey key, TValue value)
        {
            _Underlying[key] = value;
        }
    }
}
