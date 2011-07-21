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
using System.Text;

using NUnit.Framework;

namespace iSynaptic.Commons.AOP
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [Test]
        public void CurrentReturnsCorrectly()
        {
            Assert.IsNull(UnitOfWorkStub.Current);

            using (UnitOfWorkStub uow = new UnitOfWorkStub())
            {
                Assert.IsNotNull(UnitOfWorkStub.Current);
                Assert.IsTrue(object.ReferenceEquals(uow, UnitOfWorkStub.Current));
            }

            Assert.IsNull(UnitOfWorkStub.Current);
        }

        [Test]
        public void CurrentReturnsCorrectlyWhenNested()
        {
            Assert.IsNull(UnitOfWorkStub.Current);

            using (UnitOfWorkStub uow = new UnitOfWorkStub())
            {
                Assert.IsTrue(object.ReferenceEquals(uow, UnitOfWorkStub.Current));

                using (UnitOfWorkStub uow2 = new UnitOfWorkStub())
                {
                    Assert.IsTrue(object.ReferenceEquals(uow2, UnitOfWorkStub.Current));
                }

                Assert.IsTrue(object.ReferenceEquals(uow, UnitOfWorkStub.Current));
            }

            Assert.IsNull(UnitOfWorkStub.Current);
        }

        [Test]
        public void EnlistItem()
        {
            object obj = new object();

            using (UnitOfWorkStub uow = new UnitOfWorkStub())
            {
                uow.Enlist(obj);
                Assert.IsTrue(uow.IsEnlisted(obj));
            }
        }

        [Test]
        public void IsEnlistedItem()
        {
            object obj = new object();

            using (UnitOfWorkStub uow = new UnitOfWorkStub())
            {
                uow.Enlist(obj);

                Assert.IsTrue(uow.IsEnlisted(obj));
                Assert.IsFalse(uow.IsEnlisted(new object()));

                using (UnitOfWorkStub uow2 = new UnitOfWorkStub())
                {
                    Assert.IsFalse(uow2.IsEnlisted(obj));
                    Assert.IsFalse(uow2.IsEnlisted(new object()));
                }
            }
        }

        [Test]
        public void EnlistItemWhenNested()
        {
            object obj = new object();

            using (UnitOfWorkStub uow = new UnitOfWorkStub())
            {
                using (UnitOfWorkStub uow2 = new UnitOfWorkStub())
                {
                    uow2.Enlist(obj);
                    Assert.IsFalse(uow.IsEnlisted(obj));
                    Assert.IsTrue(uow2.IsEnlisted(obj));
                }

                Assert.IsFalse(uow.IsEnlisted(obj));
            }
        }

        [Test]
        public void ProcessCalled()
        {
            bool processCalled = false;

            object obj = new object();
            Action<object> process = item =>
            {
                Assert.IsTrue(object.ReferenceEquals(obj, item));
                processCalled = true;
            };

            using (UnitOfWorkStub uow = new UnitOfWorkStub(process))
            {
                uow.Enlist(obj);
                uow.Complete();
            }

            Assert.IsTrue(processCalled);
        }

        [Test]
        public void CompletingTwiceIsNotPermitted()
        {
            using (UnitOfWorkStub uow = new UnitOfWorkStub())
            {
                uow.Complete();
                Assert.Throws<InvalidOperationException>(uow.Complete);
            }
        }

        [Test]
        public void Complete_WhenAlreadyDisposed_ThrowsObjectDisposedException()
        {
            UnitOfWorkStub uow = new UnitOfWorkStub();
            uow.Dispose();

            Assert.Throws<ObjectDisposedException>(uow.Complete);
        }

        [Test]
        public void IsEnlisted_WhenAlreadyDisposed_ThrowsObjectDisposedException()
        {
            UnitOfWorkStub uow = new UnitOfWorkStub();
            uow.Dispose();

            Assert.Throws<ObjectDisposedException>(() => uow.IsEnlisted(null));
        }

        [Test]
        public void EnlistParams_WhenAlreadyDisposed_ThrowsObjectDisposedException()
        {
            UnitOfWorkStub uow = new UnitOfWorkStub();
            uow.Dispose();

            Assert.Throws<ObjectDisposedException>(() => uow.Enlist(new object()));
        }

        [Test]
        public void EnlistIEnumerable_WhenAlreadyDisposed_ThrowsObjectDisposedException()
        {
            UnitOfWorkStub uow = new UnitOfWorkStub();
            uow.Dispose();

            IEnumerable<object> items = new[] {new object()};

            Assert.Throws<ObjectDisposedException>(() => uow.Enlist(items));
        }
    }
}
