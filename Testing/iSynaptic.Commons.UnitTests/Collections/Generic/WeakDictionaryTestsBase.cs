using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    public abstract class WeakDictionaryTestsBase
    {
        protected abstract IWeakDictionary<object, object> CreateDictionary();

        [Test]
        public void PurgeGarbage_RemovesEntriesForGarbageCollectedKeys()
        {
            var dictionary = CreateDictionary();

            var key = new object();
            var value = new object();

            dictionary.Add(key, value);
            dictionary.Add(new object(), new object());

            GC.Collect();
            dictionary.PurgeGarbage();

            Assert.AreEqual(1, dictionary.Count);
            Assert.IsTrue(dictionary.ContainsKey(key));
            Assert.AreEqual(value, dictionary[key]);
        }

        [Test]
        public void Keys_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();
            var keys = dictionary.Keys;

            var key = new object();
            var value = new object();

            dictionary.Add(key, value);

            Assert.IsTrue(keys.SequenceEqual(new []{ key }));
        }

        [Test]
        public void Values_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();
            var values = dictionary.Values;

            var key = new object();
            var value = new object();

            dictionary.Add(key, value);

            Assert.IsTrue(values.SequenceEqual(new[] { value }));
        }

        [Test]
        public void Count_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();
            dictionary.Add(new object(), new object());
            dictionary.Add(new object(), new object());
            dictionary.Add(new object(), new object());

            Assert.AreEqual(3, dictionary.Count);
        }

        [Test]
        public void Indexer_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = new object();
            var value = new object();

            dictionary.Add(key, value);
            Assert.AreEqual(value, dictionary[key]);
        }

        [Test]
        public void Indexer_WithInvalidKey_ThrowsException()
        {
            var dictionary = CreateDictionary();
            var key = new object();

            Assert.Throws<KeyNotFoundException>(() => { var result = dictionary[key]; });
        }

        [Test]
        public void ContainsKey_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = new object();
            var value = new object();

            dictionary.Add(key, value);

            Assert.IsTrue(dictionary.ContainsKey(key));
            Assert.IsFalse(dictionary.ContainsKey(new object()));
        }

        [Test]
        public void Contains_ReturnsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = new object();
            var value = new object();

            dictionary.Add(key, value);

            Assert.IsTrue(dictionary.Contains(new KeyValuePair<object, object>(key, value)));
            Assert.IsFalse(dictionary.Contains(new KeyValuePair<object, object>(new object(), new object())));
        }

        [Test]
        public void Remove_ByPair_FunctionsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = new object();
            var value = new object();

            dictionary.Add(key, value);
            ((ICollection<KeyValuePair<object, object>>)dictionary).Remove(new KeyValuePair<object, object>(key, value));

            Assert.IsFalse(dictionary.ContainsKey(key));
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void Remove_ByKey_FunctionsCorrectly()
        {
            var dictionary = CreateDictionary();

            var key = new object();
            var value = new object();

            dictionary.Add(key, value);
            dictionary.Remove(key);

            Assert.IsFalse(dictionary.ContainsKey(key));
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void IsReadOnly_ReturnsFalse()
        {
            var dictionary = CreateDictionary();
            Assert.IsFalse(dictionary.IsReadOnly);
        }

        [Test]
        public void SetValue_ViaIndexer()
        {
            var dictionary = CreateDictionary();

            var key = new object();
            var value = new object();

            dictionary[key] = value;

            Assert.IsTrue(ReferenceEquals(dictionary[key], value));
        }
    }
}
