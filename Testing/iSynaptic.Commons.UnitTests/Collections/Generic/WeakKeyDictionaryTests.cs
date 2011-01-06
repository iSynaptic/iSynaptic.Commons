using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class WeakKeyDictionaryTests : WeakDictionaryTestsBase
    {
        protected override IWeakDictionary<object, object> CreateDictionary()
        {
            return new WeakKeyDictionary<object, object>();
        }
    }
}
