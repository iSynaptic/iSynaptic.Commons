using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class LazySelectionDictionaryTests : DictionaryTestsBase<int, string>
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
            return new LazySelectionDictionary<int, string>(x => Maybe<string>.NoValue);
        }

        protected override IDictionary<int, string> CreateDictionary(IEqualityComparer<int> comparer)
        {
            return new LazySelectionDictionary<int, string>(x => Maybe<string>.NoValue, comparer);
        }

        [Test]
        public void ContainsKey_OnlyReturnsTrue_IfValueCanBeLoaded()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => x.ToMaybe().Where(y => y != 1).Select(y => y.ToString()));
            Assert.IsFalse(dictionary.ContainsKey(1));
            Assert.IsTrue(dictionary.ContainsKey(42));
        }

        [Test]
        public void GetValue_EvaluatesOnce()
        {
            int executed = 0;
            var dictionary = new LazySelectionDictionary<int, string>(x => { executed++; return x.ToString().ToMaybe(); });

            string value = dictionary[1];
            value = dictionary[1];

            Assert.AreEqual("1", value);
            Assert.AreEqual(1, executed);
        }

        [Test]
        public void TryGetValue_InvokesSelectionFunc()
        {
            var dictionary = new LazySelectionDictionary<int, string>(x => x.ToMaybe().Where(y => y % 2 == 0).Select(y => x.ToString()));
            string value = null;

            Assert.IsFalse(dictionary.TryGetValue(1, out value));
            Assert.IsTrue(dictionary.TryGetValue(2, out value));
            Assert.AreEqual("2", value);
        }
    }
}
