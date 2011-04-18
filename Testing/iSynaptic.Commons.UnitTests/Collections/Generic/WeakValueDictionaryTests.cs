using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class WeakValueDictionaryTests : WeakDictionaryTestsBase<object, object>
    {
        protected override object CreateKey(bool keepReference = false)
        {
            return new object();
        }

        protected override object CreateValue(bool keepReference = false)
        {
            return new object();
        }

        protected override IWeakDictionary<object, object> CreateWeakDictionary(IEqualityComparer<object> comparer = null, Action<Maybe<object>, Maybe<object>> onGarbagePurge = null)
        {
            return new WeakValueDictionary<object, object>(comparer: comparer, onGarbagePurge: onGarbagePurge);
        }
    }
}
