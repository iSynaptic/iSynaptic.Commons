using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Collections.Generic
{
    public sealed class WeakKeyMemberDictionary<TKey, TKeyMember, TValue> : BaseWeakDictionary<TKey, TValue, TKey, TValue> where TKeyMember : class
    {
        private readonly Func<TKey, WeakReference<TKeyMember>> _MemberSelector = null;

        public WeakKeyMemberDictionary(Func<TKey, WeakReference<TKeyMember>> memberSelector)
            : this(memberSelector, 0)
        {
        }

        public WeakKeyMemberDictionary(Func<TKey, WeakReference<TKeyMember>> memberSelector, int capacity)
            : this(memberSelector, capacity, null) { }

        public WeakKeyMemberDictionary(Func<TKey, WeakReference<TKeyMember>> memberSelector, IEqualityComparer<TKey> comparer)
            : this(memberSelector, 0, comparer) { }

        public WeakKeyMemberDictionary(Func<TKey, WeakReference<TKeyMember>> memberSelector, int capacity, IEqualityComparer<TKey> comparer)
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
            return Maybe.Value(key)
                .Coalesce(x => _MemberSelector(x))
                .Select(x => x.TryGetTarget())
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
