using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    public abstract class WeakDictionaryTestsBase<TKey, TValue> : DictionaryTestsBase<TKey, TValue>
    {
        protected override IDictionary<TKey, TValue> CreateDictionary()
        {
            return CreateWeakDictionary();
        }

        protected override IDictionary<TKey, TValue> CreateDictionary(IEqualityComparer<TKey> comparer)
        {
            return CreateWeakDictionary(comparer);
        }

        protected abstract IWeakDictionary<TKey, TValue> CreateWeakDictionary(IEqualityComparer<TKey> comparer = null);
    
        [Test]
        public void PurgeGarbage_RemovesEntriesForGarbageCollectedKeys()
        {
            var dictionary = CreateWeakDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);
            dictionary.Add(CreateKey(), CreateValue());

            GC.Collect();
            dictionary.PurgeGarbage(null);

            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey(key));
            Assert.AreEqual(value, dictionary[key]);
        }
        
        [Test]
        public void PurgeGarbage_OnGarbagePurgeCalled()
        {
            bool onGarbagePurgeExecuted = false;
            Action<Maybe<TKey>, Maybe<TValue>> withPurgedPair = (k, v) => onGarbagePurgeExecuted = true;

            var dictionary = CreateWeakDictionary();
            dictionary.Add(CreateKey(), CreateValue());

            GC.Collect();
            dictionary.PurgeGarbage(withPurgedPair);

            Assert.IsTrue(onGarbagePurgeExecuted);
        }
    }
}