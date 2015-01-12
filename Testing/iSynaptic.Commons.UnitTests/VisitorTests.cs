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
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class VisitorTests
    {
        private class TestSubject : IVisitableChildren
        {
            public TestSubject(Int32 value)
                : this(value, null)
            {
            }

            public TestSubject(Int32 value, params TestSubject[] children)
            {
                Value = value;
                Children = children ?? Enumerable.Empty<TestSubject>();
            }

            public void AcceptChildren(Action<IEnumerable<Object>> dispatch)
            {
                dispatch(Children);
            }

            public Int32 Value { get; private set; }
            private IEnumerable<TestSubject> Children { get; set; }
        }

        private class DerivedStatefulTestVisitor : StatefulTestVisitor
        {
            protected override Int32 Visit(TestSubject subject, Int32 value)
            {
                return DispatchChildren(subject, value + subject.Value + 1);
            }
        }

        private class StatefulTestVisitor : Visitor<Int32>
        {
            protected void Visit(TestSubject subject)
            {
                throw new InvalidOperationException("This should never be invoked.");
            }

            protected virtual Int32 Visit(TestSubject subject, Int32 value)
            {
                return DispatchChildren(subject, value + subject.Value);
            }
        }

        private class StatelessTestVisitor : Visitor
        {
            public Int32 Result { get; private set; }

            protected void Visit(TestSubject subject)
            {
                Result += subject.Value;
                DispatchChildren(subject);
            }
        }

        private class WriterVisitor : Visitor
        {
            protected void Visit(TestSubject subject, TextWriter writer)
            {
                writer.Write(subject.Value);
                DispatchChildren(subject, writer);
            }
        }

        [Test]
        public void Visitor_ThreadsStateCorrectly()
        {
            var subject = new TestSubject(1,
                    new TestSubject(2),
                    new TestSubject(3,
                        new TestSubject(4),
                        new TestSubject(5)));

            var visitor = new StatefulTestVisitor();
            var result = visitor.Dispatch(subject, 0);
            
            Assert.AreEqual(15, result);
        }

        [Test]
        public void DerivedVisitor_ThreadsStateCorrectly()
        {
            var subject = new TestSubject(1,
                    new TestSubject(2),
                    new TestSubject(3,
                        new TestSubject(4),
                        new TestSubject(5)));

            var visitor = new DerivedStatefulTestVisitor();
            var result = visitor.Dispatch(subject, 0);
            
            Assert.AreEqual(20, result);
        }        
        
        [Test]
        public void Visitor_WhenNotThreadingState_WorksCorrectly()
        {
            var subject = new TestSubject(1,
                    new TestSubject(2),
                    new TestSubject(3,
                        new TestSubject(4),
                        new TestSubject(5)));

            var visitor = new StatelessTestVisitor();

            visitor.Dispatch(subject);

            Assert.AreEqual(15, visitor.Result);
        }

        [Test]
        public void Visitor_WhenNotReturningState_ThreadsPreviousStateThrough()
        {
            var subject = new TestSubject(1,
                    new TestSubject(2),
                    new TestSubject(3,
                        new TestSubject(4),
                        new TestSubject(5)));

            var writer = new StringWriter();
            var visitor = new WriterVisitor();
            visitor.Dispatch(subject, writer);

            Assert.AreEqual("12345", writer.GetStringBuilder().ToString());
        }
    }
}
