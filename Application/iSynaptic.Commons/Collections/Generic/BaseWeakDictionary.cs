using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public abstract class BaseWeakDictionary<TKey, TValue, TWrappedKey, TWrappedValue> : BaseDictionary<TKey, TValue>, IWeakDictionary<TKey, TValue>
    {
        private readonly IEqualityComparer<TKey> _BaseComparer;
        private readonly IEqualityComparer<TWrappedKey> _Comparer;
        private readonly Dictionary<TWrappedKey, TWrappedValue> _Dictionary;

        #region WeakKeyComparer

        protected sealed class WeakKeyComparer<T> : IEqualityComparer<object> where T : class
        {
            private IEqualityComparer<T> _Comparer;

            public WeakKeyComparer(IEqualityComparer<T> comparer)
            {
                _Comparer = comparer ?? EqualityComparer<T>.Default;
            }

            public int GetHashCode(object obj)
            {
                var weakKey = (WeakKeyReference<T>) obj;
                return weakKey.HashCode;
            }

            public new bool Equals(object x, object y)
            {
                var xTarget = GetTarget(x);
                var yTarget = GetTarget(y);

                return xTarget.Equals(yTarget, _Comparer);
            }

            private static Maybe<T> GetTarget(object obj)
            {
                var weakKey = (WeakKeyReference<T>)obj;
             
                T target = weakKey.Target;
                return weakKey.IsAlive ? target : Maybe<T>.NoValue;
            }
        }

        #endregion

        #region WeakKeyReference

        protected sealed class WeakKeyReference<T> : WeakReference<T> where T : class
        {
            public WeakKeyReference(T key, IEqualityComparer<T> comparer)
                : base(key)
            {
                HashCode = comparer.GetHashCode(key);
            }

            public int HashCode { get; private set; }
        }

        #endregion

        protected BaseWeakDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _BaseComparer = comparer ?? EqualityComparer<TKey>.Default;

            _Comparer = BuildComparer(_BaseComparer);
            _Dictionary = new Dictionary<TWrappedKey, TWrappedValue>(capacity, _Comparer);
        }

        protected abstract IEqualityComparer<TWrappedKey> BuildComparer(IEqualityComparer<TKey> comparer);

        private TWrappedKey WrapKey(TKey key)
        {
            return WrapKey(key, _BaseComparer);
        }

        protected abstract TWrappedKey WrapKey(TKey key, IEqualityComparer<TKey> comparer);
        protected abstract Maybe<TKey> UnwrapKey(TWrappedKey key);
        protected abstract TWrappedValue WrapValue(TValue value);
        protected abstract Maybe<TValue> UnwrapValue(TWrappedValue value);

        protected static Maybe<T> UnwrapWeakReference<T>(WeakReference<T> value) where T : class
        {
            var target = value.Target;

            return value.IsAlive ? target : Maybe<T>.NoValue;
        }

        public override int Count
        {
            get { return _Dictionary.Count; }
        }

        public override void Add(TKey key, TValue value)
        {
            Guard.NotNull(key, "key");

            var weakKey = WrapKey(key);
            var weakValue = WrapValue(value);

            _Dictionary.Add(weakKey, weakValue);
        }

        public override bool ContainsKey(TKey key)
        {
            return _Dictionary.ContainsKey(WrapKey(key));
        }

        public override bool Remove(TKey key)
        {
            return _Dictionary.Remove(WrapKey(key));
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            var result = _Dictionary
                .TryGetValue(WrapKey(key))
                .Bind(UnwrapValue);

            value = result.HasValue ? result.Value : default(TValue);
            return result.HasValue;
        }

        protected override void SetValue(TKey key, TValue value)
        {
            var weakKey = WrapKey(key);
            _Dictionary[weakKey] = WrapValue(value);
        }

        public override void Clear()
        {
            _Dictionary.Clear();
        }

        public override IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (KeyValuePair<TWrappedKey, TWrappedValue> pair in _Dictionary)
            {
                var key = UnwrapKey(pair.Key);
                var value = UnwrapValue(pair.Value);

                if (key.HasValue && value.HasValue)
                    yield return new KeyValuePair<TKey, TValue>(key.Value, value.Value);
            }
        }

        public void PurgeGarbage()
        {
            _Dictionary.RemoveAll(pair =>
            {
                var key = UnwrapKey(pair.Key);
                var value = UnwrapValue(pair.Value);

                return !key.HasValue || !value.HasValue;
            });
        }
    } 

}
