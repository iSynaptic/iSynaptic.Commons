using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.UnitTests.Extensions
{
    [TestFixture]
    public class DictionaryExtensionsTests : BaseTestFixture
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
