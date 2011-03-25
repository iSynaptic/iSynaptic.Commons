using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class LazyDictionaryTests : DictionaryTestsBase<int, string>
    {
        private readonly Random _Random = new Random();

        protected override int CreateKey(bool keepReference = false)
        {
            return _Random.Next();
        }

        protected override string CreateValue(bool keepReference = false)
        {
            return _Random.Next().ToString();
        }

        protected override IDictionary<int, string> CreateDictionary()
        {
            return new LazyDictionary<int, string>();
        }

        protected override IDictionary<int, string> CreateDictionary(IEqualityComparer<int> comparer)
        {
            return new LazyDictionary<int, string>(comparer);
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
