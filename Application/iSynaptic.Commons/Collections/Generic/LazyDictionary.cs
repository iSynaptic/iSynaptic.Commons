using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class LazyDictionary<TKey, TValue> : BaseReadOnlyDictionary<TKey, TValue>
    {
        private readonly Func<TKey, Maybe<TValue>> _LazyEvaluator;
        private readonly Dictionary<TKey, TValue> _Values = null;

        public LazyDictionary(Func<TKey, Maybe<TValue>> lazyEvaluator)
        {
            Guard.NotNull(lazyEvaluator, "lazyEvaluator");

            _LazyEvaluator = lazyEvaluator;
            _Values = new Dictionary<TKey, TValue>();
        }

        public override int Count
        {
            get { return _Values.Count; }
        }

        public override bool ContainsKey(TKey key)
        {
            return _Values.ContainsKey(key);
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            if (_Values.TryGetValue(key, out value))
                return true;

            var results = _LazyEvaluator(key);
            if(results.HasValue)
            {
                value = results.Value;
                _Values.Add(key, value);
                return true;
            }

            value = default(TValue);
            return false;
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _Values.GetEnumerator();
        }
    }
}
