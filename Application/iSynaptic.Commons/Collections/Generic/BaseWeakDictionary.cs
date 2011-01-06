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
                if (comparer == null)
                    comparer = EqualityComparer<T>.Default;

                _Comparer = comparer;
            }

            public int GetHashCode(object obj)
            {
                var weakKey = obj as WeakKeyReference<T>;

                if (weakKey != null)
                    return weakKey.HashCode;

                return _Comparer.GetHashCode((T)obj);
            }

            public new bool Equals(object x, object y)
            {
                bool xIsDead, yIsDead;

                T first = GetTarget(x, out xIsDead);
                T second = GetTarget(y, out yIsDead);

                if (xIsDead)
                    return yIsDead ? x == y : false;

                if (yIsDead)
                    return false;

                return _Comparer.Equals(first, second);
            }

            private static T GetTarget(object obj, out bool isDead)
            {
                var weakKey = obj as WeakKeyReference<T>;
                T target;
                if (weakKey != null)
                {
                    target = weakKey.Target;
                    isDead = !weakKey.IsAlive;
                }
                else
                {
                    target = (T)obj;
                    isDead = false;
                }
                return target;
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

        public BaseWeakDictionary()
            : this(0) { }

        public BaseWeakDictionary(int capacity)
            : this(capacity, null) { }

        public BaseWeakDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer) { }

        public BaseWeakDictionary(int capacity, IEqualityComparer<TKey> comparer)
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
        protected abstract bool UnwrapKey(TWrappedKey key, ref TKey destination);
        protected abstract TWrappedValue WrapValue(TValue value);
        protected abstract bool UnwrapValue(TWrappedValue value, ref TValue destination);

        protected static bool UnwrapWeakReference<T>(WeakReference<T> value, ref T destination) where T : class
        {
            var target = value.Target;

            bool isAlive = value.IsAlive;
            if (isAlive)
                destination = target;

            return isAlive;
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
            TWrappedValue dictValue;
            if (_Dictionary.TryGetValue(WrapKey(key), out dictValue))
            {
                TValue retreivedValue = default(TValue);

                if (UnwrapValue(dictValue, ref retreivedValue))
                {
                    value = retreivedValue;
                    return true;
                }
            }

            value = default(TValue);
            return false;
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
                TKey key = default(TKey);
                TValue value = default(TValue);

                if (UnwrapKey(pair.Key, ref key) && UnwrapValue(pair.Value, ref value))
                    yield return new KeyValuePair<TKey, TValue>(key, value);
            }
        }

        public void PurgeGarbage()
        {
            _Dictionary.RemoveAll(pair =>
            {
                TKey key = default(TKey);
                TValue value = default(TValue);

                return !UnwrapKey(pair.Key, ref key) || !UnwrapValue(pair.Value, ref value);
            });
        }
    } 

}
