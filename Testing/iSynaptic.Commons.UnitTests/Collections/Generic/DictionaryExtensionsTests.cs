using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class DictionaryExtensionsTests
    {
        [Test]
        public void ToReadOnlyWithNull()
        {
            IDictionary<int, int> dict = null;

            Assert.Throws<ArgumentNullException>(() => dict.ToReadOnlyDictionary());
        }

        [Test]
        public void ToReadOnlyTwice()
        {
            IDictionary<int, int> dict = new Dictionary<int, int>();
            dict = dict.ToReadOnlyDictionary();

            Assert.IsTrue(object.ReferenceEquals(dict, dict.ToReadOnlyDictionary()));
            Assert.IsTrue(dict.IsReadOnly);
        }
    }
}
