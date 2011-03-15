using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public abstract class BaseWeakDictionary<TKey, TValue, TWrappedKey, TWrappedValue> : BaseDictionary<TKey, TValue>, IWeakDictionary<TKey, TValue>
    {
        private readonly IEqualityComparer<TKey> _Comparer;
        private readonly Dictionary<TWrappedKey, TWrappedValue> _Dictionary;

        protected BaseWeakDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _Comparer = comparer ?? EqualityComparer<TKey>.Default;

            var wrappedKeyComparer = comparer as IEqualityComparer<TWrappedKey>;

            _Dictionary = wrappedKeyComparer != null
                ? new Dictionary<TWrappedKey, TWrappedValue>(capacity, wrappedKeyComparer)
                : new Dictionary<TWrappedKey, TWrappedValue>(capacity);
        }

        protected abstract TWrappedKey WrapKey(TKey key, IEqualityComparer<TKey> comparer);
        protected abstract Maybe<TKey> UnwrapKey(TWrappedKey key);
        protected abstract TWrappedValue WrapValue(TValue value);
        protected abstract Maybe<TValue> UnwrapValue(TWrappedValue value);

        public override int Count
        {
            get { return _Dictionary.Count; }
        }

        public override void Add(TKey key, TValue value)
        {
            Guard.NotNull(key, "key");

            var weakKey = WrapKey(key, _Comparer);
            var weakValue = WrapValue(value);

            _Dictionary.Add(weakKey, weakValue);
        }

        public override bool ContainsKey(TKey key)
        {
            return _Dictionary.ContainsKey(WrapKey(key, _Comparer));
        }

        public override bool Remove(TKey key)
        {
            return _Dictionary.Remove(WrapKey(key, _Comparer));
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            var result = _Dictionary
                .TryGetValue(WrapKey(key, _Comparer))
                .Bind(UnwrapValue);

            value = result.Return();
            return result.HasValue;
        }

        protected override void SetValue(TKey key, TValue value)
        {
            var weakKey = WrapKey(key, _Comparer);
            _Dictionary[weakKey] = WrapValue(value);
        }

        public override void Clear()
        {
            _Dictionary.Clear();
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return (_Dictionary
                .Select(pair => UnwrapKey(pair.Key)
                    .Select(k => UnwrapValue(pair.Value)
                        .Select(v => KeyValuePair.Create(k, v))))
                .Where(x => x.HasValue)
                .Select(x => x.Value))
                .GetEnumerator();
        }

        public void PurgeGarbage()
        {
            _Dictionary.RemoveAll(pair =>
                !(UnwrapKey(pair.Key)
                    .Select(k => UnwrapValue(pair.Value))
                    .HasValue));
        }
    } 
}
