using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class WeakDictionaryTests : WeakDictionaryTestsBase<object, object>
    {
        protected override object CreateKey(bool keepReference = false)
        {
            return new object();
        }

        protected override object CreateValue(bool keepReference = false)
        {
            return new object();
        }

        protected override IWeakDictionary<object, object> CreateWeakDictionary()
        {
            return new WeakDictionary<object, object>();
        }

        protected override IWeakDictionary<object, object> CreateWeakDictionary(IEqualityComparer<object> comparer)
        {
            return new WeakDictionary<object, object>(comparer);
        }
    }
}
