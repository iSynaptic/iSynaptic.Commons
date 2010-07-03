using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using iSynaptic.Commons.Extensions;

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
                Assert.IsTrue(uow.GetItems().Contains(obj));
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
                    Assert.IsTrue(uow2.IsEnlisted(obj));
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
                    Assert.IsFalse(uow.GetItems().Contains(obj));
                    Assert.IsTrue(uow2.GetItems().Contains(obj));
                }

                Assert.IsFalse(uow.GetItems().Contains(obj));
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
