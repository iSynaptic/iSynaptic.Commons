using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class WeakKeyMemberDictionaryTests : WeakDictionaryTestsBase<TestKey, object>
    {
        private readonly List<object> _MemberObjects = new List<object>();

        [TearDown]
        public void TearDown()
        {
            _MemberObjects.Clear();
        }

        protected override TestKey CreateKey(bool keepReference = false)
        {
            var obj = new object();
            
            if(keepReference)
                _MemberObjects.Add(obj);

            return new TestKey("Test Message", obj);
        }

        protected override object CreateValue(bool keepReference = false)
        {
            return new object();
        }

        protected override IWeakDictionary<TestKey, object> CreateWeakDictionary(IEqualityComparer<TestKey> comparer = null)
        {
            return new WeakKeyMemberDictionary<TestKey, object, object>(x => x.WeakObject, comparer: comparer);
        }
    }

    public struct TestKey
    {
        public TestKey(string message, object weakObject) : this()
        {
            Message = message;
            WeakObject = WeakReference<object>.Create(weakObject);
        }

        public string Message { get; private set; }
        public WeakReference<object> WeakObject { get; private set; }
    }
}
