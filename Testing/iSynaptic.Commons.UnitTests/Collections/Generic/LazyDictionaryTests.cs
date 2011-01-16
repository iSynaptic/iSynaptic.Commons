using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class LazyDictionaryTests
    {
        [Test]
        public void Count_Initially_IsZero()
        {
            var dictionary = new LazyDictionary<int, string>(x => x.ToString());
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void ContainsKey_OnlyReturnsTrue_AfterValueIsLoaded()
        {
            var dictionary = new LazyDictionary<int, string>(x => x.ToString());
            Assert.IsFalse(dictionary.ContainsKey(1));
        }

        [Test]
        public void GetValue_ReturnsCorrectly()
        {
            var dictionary = new LazyDictionary<int, string>(x => x.ToString());
            string value = dictionary[1];

            Assert.AreEqual("1", value);
        }

        [Test]
        public void GetValue_EvaluatesOnce()
        {
            int executed = 0;
            var dictionary = new LazyDictionary<int, string>(x => { executed++; return x.ToString(); });
            
            string value = dictionary[1];
            value = dictionary[1];

            Assert.AreEqual("1", value);
            Assert.AreEqual(1, executed);
        }

        [Test]
        public void Enumerator_IsInitialyEmpty()
        {
            var dictionary = new LazyDictionary<int, string>(x => x.ToString());
            var array = dictionary
                .Select(x => x)
                .ToArray();

            Assert.AreEqual(0, array.Length);
        }

        [Test]
        public void TryGetValue_ReturnsCorrectly()
        {
            var dictionary = new LazyDictionary<int, string>(x => x % 2 ==0 ? x.ToString() : Maybe<string>.NoValue);
            string value = null;

            Assert.IsFalse(dictionary.TryGetValue(1, out value));
            Assert.IsTrue(dictionary.TryGetValue(2, out value));
            Assert.AreEqual("2", value);
        }
    }
}
