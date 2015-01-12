// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
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
