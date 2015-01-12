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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public partial class FuncExtensionsTests
    {
        [Test]
        public void ToComparer()
        {
            Func<string, string, int> strategy = (x, y) => x.CompareTo(y);
            var comparer = strategy.ToComparer();

            string foo = "Foo";
            string bar = "Bar";

            Assert.AreEqual(foo.CompareTo(bar), comparer.Compare(foo, bar));
        }

        [Test]
        public void ToEqualityComparer_WithSelector()
        {
            Func<string, int> strategy = x => x.Length;
            var lengthComparer = strategy.ToEqualityComparer();

            Assert.IsTrue(lengthComparer.Equals("Foo", "Bar"));
            Assert.AreEqual(lengthComparer.GetHashCode("Foo"), lengthComparer.GetHashCode("Bar"));

            Assert.IsFalse(lengthComparer.Equals("Foo", "Quix"));
            Assert.AreNotEqual(lengthComparer.GetHashCode("Foo"), lengthComparer.GetHashCode("Quix"));
        }

        [Test]
        public void ToEqualityComparer_WithEqualsAndHashCodeStrategy()
        {
            Func<string, string, bool> equalsStrategy = (x, y) => x.Length == y.Length;
            Func<string, int> hashCodeStrategy = x => x.Length.GetHashCode();

            var lengthComparer = equalsStrategy.ToEqualityComparer(hashCodeStrategy);

            Assert.IsTrue(lengthComparer.Equals("Foo", "Bar"));
            Assert.AreEqual(lengthComparer.GetHashCode("Foo"), lengthComparer.GetHashCode("Bar"));

            Assert.IsFalse(lengthComparer.Equals("Foo", "Quix"));
            Assert.AreNotEqual(lengthComparer.GetHashCode("Foo"), lengthComparer.GetHashCode("Quix"));
        }

        [Test]
        public void Synchronize_PreventsConcurrentAccess()
        {
            int count = 0;
            Func<int> func = () => { count++; return count; };
            func = func.Synchronize(() => true);

            var random = new Random(DateTime.UtcNow.Second);
            int start = random.Next(10, 30);
            int end = random.Next(50, 100);

            Parallel.For(start, end, x => func());

            Assert.AreEqual(end - start, count);
        }
    }
}
