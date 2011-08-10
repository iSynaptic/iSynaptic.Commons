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

namespace iSynaptic.Commons
{
    [TestFixture]
    public class KeyedReaderWriterTests
    {
        [Test]
        public void Getter_IsCalled()
        {
            bool executed = false;
            var krw = new KeyedReaderWriter<object, object>(k => { executed = true; return Maybe<object>.NoValue; }, (k, v) => false);

            krw.TryGet(null);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Getter_ResultIsReturned()
        {
            var krw = new KeyedReaderWriter<object, object>(k => 42.ToMaybe<object>(), (k, v) => false);
            var result = krw.TryGet(null);

            Assert.IsTrue(42.ToMaybe<object>() == result);
        }

        [Test]
        public void Setter_IsCalled()
        {
            bool executed = false;
            var krw = new KeyedReaderWriter<object, object>(k => Maybe<object>.NoValue, (k, v) => { executed = true; return false; });

            krw.TrySet(null, Maybe<object>.NoValue);
            Assert.IsTrue(executed);
        }

        [Test]
        public void Setter_KeyIsUsed()
        {
            object result = null;

            var krw = new KeyedReaderWriter<object, object>(k => Maybe<object>.NoValue, (k, v) => { result = k; return true; });
            krw.TrySet(42, Maybe<object>.NoValue);

            Assert.IsTrue(42 == (int)result);
        }

        [Test]
        public void Setter_ValueIsUsed()
        {
            var result = Maybe<object>.NoValue;

            var krw = new KeyedReaderWriter<object, object>(k => Maybe<object>.NoValue, (k, v) => { result = v; return true; });
            krw.TrySet(null, 42.ToMaybe<object>());

            Assert.IsTrue(42.ToMaybe<object>() == result);
        }

        [Test]
        public void Setter_ResultIsReturned()
        {
            bool returnValue = false;

            var krw = new KeyedReaderWriter<object, object>(k => Maybe<object>.NoValue, (k, v) => returnValue);
            Assert.IsFalse(krw.TrySet(null, Maybe<object>.NoValue));

            returnValue = true;
            Assert.IsTrue(krw.TrySet(null, Maybe<object>.NoValue));
        }
    }
}
