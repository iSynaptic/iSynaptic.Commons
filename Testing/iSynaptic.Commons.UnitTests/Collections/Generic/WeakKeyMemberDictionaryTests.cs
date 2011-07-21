// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
