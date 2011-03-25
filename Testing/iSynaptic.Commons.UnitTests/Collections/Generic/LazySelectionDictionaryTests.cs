using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class LazySelectionDictionaryTests
    {
        [Test]
        public void Count_Initially_IsZero()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => x.ToString());
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void ContainsKey_OnlyReturnsTrue_IfValueCanBeLoaded()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => x == 1 ? Maybe<string>.NoValue : x.ToString());
            Assert.IsFalse(dictionary.ContainsKey(1));
            Assert.IsTrue(dictionary.ContainsKey(42));
        }

        [Test]
        public void GetValue_ReturnsCorrectly()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => x.ToString());
            string value = dictionary[1];

            Assert.AreEqual("1", value);
        }

        [Test]
        public void GetValue_EvaluatesOnce()
        {
            int executed = 0;
            var dictionary = new LazySelectionDictionary<int, string>(x => { executed++; return x.ToString(); });
            
            string value = dictionary[1];
            value = dictionary[1];

            Assert.AreEqual("1", value);
            Assert.AreEqual(1, executed);
        }

        [Test]
        public void Enumerator_IsInitialyEmpty()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => x.ToString());
            var array = dictionary
                .Select(x => x)
                .ToArray();

            Assert.AreEqual(0, array.Length);
        }

        [Test]
        public void TryGetValue_ReturnsCorrectly()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => x % 2 == 0 ? x.ToString() : Maybe<string>.NoValue);
            string value = null;

            Assert.IsFalse(dictionary.TryGetValue(1, out value));
            Assert.IsTrue(dictionary.TryGetValue(2, out value));
            Assert.AreEqual("2", value);
        }

        [Test]
        public void Add_StoresValueInDictionary()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => Maybe<string>.NoValue);
            dictionary.Add(42, "42");

            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("42", dictionary[42]);
        }

        [Test]
        public void Remove_TakesValueOutOfDictionary()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => Maybe<string>.NoValue)
                                 {
                                     {1, "1"},
                                     {2, "2"},
                                     {42, "42"}
                                 };

            Assert.AreEqual(3, dictionary.Count);
            
            dictionary.Remove(2);

            Assert.AreEqual(2, dictionary.Count);
            Assert.IsFalse(dictionary.ContainsKey(2));
        }

        [Test]
        public void Clear_RemovesAllItemsFromDictionary()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => Maybe<string>.NoValue)
                                 {
                                     {1, "1"},
                                     {2, "2"},
                                     {42, "42"}
                                 };

            Assert.AreEqual(3, dictionary.Count);

            dictionary.Clear();
            Assert.AreEqual(0, dictionary.Count);

            Assert.IsFalse(dictionary.TryGetValue(1)
                       .Or(dictionary.TryGetValue(2))
                       .Or(dictionary.TryGetValue(42))
                       .HasValue);
        }

        [Test]
        public void Indexer_AddsValueToDictionary()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => Maybe<string>.NoValue);
            dictionary[42] = "42";

            Assert.AreEqual(1, dictionary.Count);
            Assert.AreEqual("42", dictionary[42]);
        }
    }
}
