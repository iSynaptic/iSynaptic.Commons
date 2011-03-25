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

        protected abstract IWeakDictionary<TKey, TValue> CreateWeakDictionary();
        protected abstract IWeakDictionary<TKey, TValue> CreateWeakDictionary(IEqualityComparer<TKey> comparer);
    
        [Test]
        public void PurgeGarbage_RemovesEntriesForGarbageCollectedKeys()
        {
            var dictionary = CreateWeakDictionary();

            var key = CreateKey(true);
            var value = CreateValue(true);

            dictionary.Add(key, value);
            dictionary.Add(CreateKey(), CreateValue());

            GC.Collect();
            dictionary.PurgeGarbage();

            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey(key));
            Assert.AreEqual(value, dictionary[key]);
        }
    }
}