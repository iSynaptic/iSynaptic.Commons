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
using iSynaptic.Commons.Linq;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class SmartLoopTests
    {
        [Test]
        public void Each_ExecuteCorrectNumberOfTimesWithCorrectValue()
        {
            var items = new List<int>();

            Enumerable.Range(1, 10)
                .SmartLoop()
                .Each(items.Add)
                .Execute();

            Assert.AreEqual(10, items.Count);
            Assert.IsTrue(items.SequenceEqual(Enumerable.Range(1, 10)));
        }

        [Test]
        public void OnlyOne_ExecutesWhenOnlyOneItemIsNotIgnored()
        {
            bool executed = false;

            Enumerable.Range(1, 2)
                .SmartLoop()
                .OnlyOne(x => executed = true)
                .Execute();

            Assert.IsFalse(executed);

            Enumerable.Range(1, 2)
                .SmartLoop()
                .Ignore(2)
                .OnlyOne(x => executed = true)
                .Execute();

            Assert.IsTrue(executed);
        }

        [Test]
        public void MoreThanOne_ExecutesWhenOnlyWithItemIsNotIgnored()
        {
            bool executed = false;

            Enumerable.Range(1, 2)
                .SmartLoop()
                .Ignore(x => x == 2)
                .MoreThanOne(x => executed = true)
                .Execute();

            Assert.IsFalse(executed);

            Enumerable.Range(1, 2)
                .SmartLoop()
                .MoreThanOne(x => executed = true)
                .Execute();

            Assert.IsTrue(executed);
        }

        [Test]
        public void When_ItemMatches()
        {
            var items = new List<int>();

            Enumerable.Range(1, 10)
                .SmartLoop()
                .When(5, items.Add)
                .Execute();

            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(5, items[0]);
        }

        [Test]
        public void When_PredicateIsTrue()
        {
            var items = new List<int>();

            Enumerable.Range(1, 10)
                .SmartLoop()
                .When(x => x % 3 == 0, items.Add)
                .Execute();

            Assert.AreEqual(3, items.Count);
            Assert.IsTrue(items.SequenceEqual(new[] { 3, 6, 9 }));
        }

        [Test]
        public void BeforeAll_ExecutesAtTheCorrectTime()
        {
            bool beforeAllExecutedCorrectly = false;
            bool itemExecuted = false;

            Enumerable.Range(1, 2)
                .SmartLoop()
                .BeforeAll(x => beforeAllExecutedCorrectly = !itemExecuted)
                .Each(x => itemExecuted = true)
                .Execute();

            Assert.IsTrue(beforeAllExecutedCorrectly);
            Assert.IsTrue(itemExecuted);
        }

        [Test]
        public void AfterAll_ExecutesAtTheCorrectTime()
        {
            bool afterAllExecutedCorrectly = false;
            bool itemExecuted = false;

            Enumerable.Range(1, 2)
                .SmartLoop()
                .AfterAll(x => afterAllExecutedCorrectly = itemExecuted)
                .Each(x => itemExecuted = true)
                .Execute();

            Assert.IsTrue(afterAllExecutedCorrectly);
            Assert.IsTrue(itemExecuted);
        }

        [Test]
        public void None_ExecutesWhenAllItemsAreIgnored()
        {
            bool executed = false;

            Enumerable.Range(1, 2)
                .SmartLoop()
                .None(() => executed = true)
                .Execute();

            Assert.IsFalse(executed);

            Enumerable.Range(1, 2)
                .SmartLoop()
                .Ignore(x => true)
                .None(() => executed = true)
                .Execute();

            Assert.IsTrue(executed);
        }

        [Test]
        public void Between_ExecutesAtTheCorrectTime()
        {
            int lastNum = 0;
            var results = new List<int>();

            Enumerable.Range(1, 3)
                .SmartLoop()
                .Each(x => lastNum = x)
                .Between((x, y) => results.Add(x + y))
                .Execute();

            Assert.IsTrue(results.SequenceEqual(new[] { 3, 5 }));
        }
    }
}
