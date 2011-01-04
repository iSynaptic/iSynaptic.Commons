using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public class WeakDictionary<TKey, TValue> : BaseDictionary<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        private readonly Dictionary<object, object> _Dictionary;
        private readonly WeakKeyComparer<TKey> _Comparer;

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
            public WeakKeyReference(T key, WeakKeyComparer<T> comparer)
                : base(key)
            {
                HashCode = comparer.GetHashCode(key);
            }

            public int HashCode { get; private set; }
        }

        #endregion

        public WeakDictionary()
            : this(0) { }

        public WeakDictionary(int capacity)
            : this(capacity, null) { }

        public WeakDictionary(IEqualityComparer<TKey> comparer)
            : this(0, comparer) { }

        public WeakDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _Comparer = new WeakKeyComparer<TKey>(comparer);
            _Dictionary = new Dictionary<object, object>(capacity, _Comparer);
        }

        protected virtual object WrapKey(TKey key)
        {
            return new WeakKeyReference<TKey>(key, _Comparer);
        }

        protected virtual bool UnwrapKey(object key, ref TKey destination)
        {
            return Unwrap(key, ref destination);
        }

        protected virtual object WrapValue(TValue value)
        {
            return WeakReference<TValue>.Create(value);
        }

        protected virtual bool UnwrapValue(object value, ref TValue destination)
        {
            return Unwrap(value, ref destination);
        }

        protected static bool Unwrap<T>(object value, ref T destination) where T : class
        {
            var weakValue = value as WeakReference<T>;
            var target = weakValue.Target;

            bool isAlive = weakValue.IsAlive;
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
            if (key == null)
                throw new ArgumentNullException("key");

            var weakKey = WrapKey(key);
            var weakValue = WrapValue(value);

            _Dictionary.Add(weakKey, weakValue);
        }

        public override bool ContainsKey(TKey key)
        {
            return _Dictionary.ContainsKey(key);
        }

        public override bool Remove(TKey key)
        {
            return _Dictionary.Remove(key);
        }

        public override bool TryGetValue(TKey key, out TValue value)
        {
            object dictValue;
            if (_Dictionary.TryGetValue(key, out dictValue))
            {
                TValue retreivedValue = null;

                if (UnwrapValue(dictValue, ref retreivedValue))
                {
                    value = retreivedValue;
                    return true;
                }
            }

            value = null;
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
            foreach (KeyValuePair<object, object> pair in _Dictionary)
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
