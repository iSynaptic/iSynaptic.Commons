using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public sealed class WeakKeyMemberDictionary<TKey, TKeyMember, TValue> : BaseWeakDictionary<TKey, TValue, TKey, TValue> where TKeyMember : class
    {
        private readonly Func<TKey, WeakReference<TKeyMember>> _MemberSelector = null;

        public WeakKeyMemberDictionary(Func<TKey, WeakReference<TKeyMember>> memberSelector, int capacity = 0, IEqualityComparer<TKey> comparer = null)
            : base(capacity, comparer)
        {
             _MemberSelector = memberSelector;
        }

        protected override TKey WrapKey(TKey key, IEqualityComparer<TKey> comparer)
        {
            return key;
        }

        protected override Maybe<TKey> UnwrapKey(TKey key)
        {
            return key
                .ToMaybe()
                .Coalesce(x => _MemberSelector(x))
                .SelectMaybe(x => x.TryGetTarget())
                .Select(x => key);
        }

        protected override TValue WrapValue(TValue value)
        {
            return value;
        }

        protected override Maybe<TValue> UnwrapValue(TValue value)
        {
            return value;
        }
    }
}
