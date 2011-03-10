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
            var dictionary = new LazyDictionary<int, string>();
            Assert.AreEqual(0, dictionary.Count);
        }

        [Test]
        public void ContainsKey_OnlyReturnsTrue_IfValueCanBeFound()
        {
            var dictionary = new LazyDictionary<int, string>
                                 {
                                     {42, () => "42"}
                                 };

            Assert.IsFalse(dictionary.ContainsKey(1));
            Assert.IsTrue(dictionary.ContainsKey(42));
        }

        [Test]
        public void GetValue_ReturnsCorrectly()
        {
            var dictionary = new LazyDictionary<int, string>
                                 {
                                     {42, () => "42"}
                                 };

            Assert.AreEqual("42", dictionary[42]);
        }

        [Test]
        public void Enumerator_IsInitialyEmpty()
        {
            var dictionary = new LazyDictionary<int, string>();
            var array = dictionary
                .Select(x => x)
                .ToArray();

            Assert.AreEqual(0, array.Length);
        }

        [Test]
        public void TryGetValue_ReturnsCorrectly()
        {
            var dictionary = new LazyDictionary<int, string>
                                 {
                                     {42, () => "42"}
                                 };

            string value = null;

            Assert.IsFalse(dictionary.TryGetValue(1, out value));
            Assert.IsTrue(dictionary.TryGetValue(42, out value));
            Assert.AreEqual("42", value);
        }
    }
}
