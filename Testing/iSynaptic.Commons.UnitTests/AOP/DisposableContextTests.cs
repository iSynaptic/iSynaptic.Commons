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

using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons.AOP
{
    [TestFixture]
    public class DisposableContextTests
    {
        [Test]
        public void Dispose_WhenHasEnlistent_DisposesEnlistment()
        {
            bool disposed = false;
            Action dispose = () => disposed = true;

            using (DisposableContext dc = new DisposableContext())
            {
                dc.Enlist(dispose.ToDisposable());
            }

            Assert.IsTrue(disposed);
        }

        [Test]
        public void Dispose_WhenHasNestedEnlistment_DisposesEnlistmentOnOuterContextDispose()
        {
            bool disposed = false;
            Action dispose = () => disposed = true;

            using (DisposableContext dc = new DisposableContext())
            {
                using (DisposableContext dc2 = new DisposableContext())
                {
                    dc2.Enlist(dispose.ToDisposable());
                }

                Assert.IsFalse(disposed);
            }

            Assert.IsTrue(disposed);
        }

        [Test]
        public void IsEnlistedDisposable()
        {
            Action dispose = () => { };

            IDisposable first = dispose.ToDisposable();
            IDisposable second = dispose.ToDisposable();

            using (DisposableContext dc = new DisposableContext())
            {
                dc.Enlist(first);
                Assert.IsTrue(dc.IsEnlisted(first));

                using (DisposableContext dc2 = new DisposableContext())
                {
                    dc2.Enlist(second);

                    Assert.IsFalse(dc2.IsEnlisted(first));
                    Assert.IsTrue(dc2.IsEnlisted(second));
                }

                Assert.IsTrue(dc.IsEnlisted(first));
                Assert.IsTrue(dc.IsEnlisted(second));
                Assert.IsFalse(dc.IsEnlisted(dispose.ToDisposable()));
            }
        }

        [Test]
        public void DisposeHandlesExceptionsGracefully()
        {
            bool disposed = false;
            Action dispose = () => disposed = true;
            Action throwOnDispose = () => { Assert.IsFalse(disposed); throw new InvalidOperationException(); };

            try
            {
                using (DisposableContext dc = new DisposableContext())
                {
                    dc.Enlist(throwOnDispose.ToDisposable());
                    dc.Enlist(dispose.ToDisposable());
                }
            }
            catch (AggregateException ex)
            {
                Assert.IsTrue(disposed);
                Assert.AreEqual(1, ex.InnerExceptions.Count);

                Assert.IsAssignableFrom<InvalidOperationException>(ex.InnerExceptions[0]);
            }
        }

        [Test]
        public void Current_WithoutNested_ReturnsCorrectInstance()
        {
            using(DisposableContext dc = new DisposableContext())
            {
                Assert.IsTrue(ReferenceEquals(dc, DisposableContext.Current));
            }
        }

        [Test]
        public void Current_WithNested_ReturnsCorrectInstance()
        {
            using (DisposableContext dc = new DisposableContext())
            {
                using(DisposableContext dc2 = new DisposableContext())
                    Assert.IsTrue(ReferenceEquals(dc2, DisposableContext.Current));

                Assert.IsTrue(ReferenceEquals(dc, DisposableContext.Current));
            }
        }
    }
}
