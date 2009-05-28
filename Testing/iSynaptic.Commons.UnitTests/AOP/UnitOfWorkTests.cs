using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.UnitTests.AOP
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
        public void EnlistDisposable()
        {
            bool disposed = false;
            Action dispose = () => disposed = true;

            using (UnitOfWorkStub uow = new UnitOfWorkStub())
            {
                uow.Enlist(dispose.ToDisposable());
            }

            Assert.IsTrue(disposed);
        }

        [Test]
        public void EnlistDisposableWhenNested()
        {
            bool disposed = false;
            Action dispose = () => disposed = true;

            using (UnitOfWorkStub uow = new UnitOfWorkStub())
            {
                using (UnitOfWorkStub uow2 = new UnitOfWorkStub())
                {
                    uow2.Enlist(dispose.ToDisposable());
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

            using (UnitOfWorkStub uow = new UnitOfWorkStub())
            {
                uow.Enlist(first);
                Assert.IsTrue(uow.IsEnlisted(first));
                
                using (UnitOfWorkStub uow2 = new UnitOfWorkStub())
                {
                    uow2.Enlist(second);

                    Assert.IsTrue(uow2.IsEnlisted(first));
                    Assert.IsTrue(uow2.IsEnlisted(second));
                }

                Assert.IsTrue(uow.IsEnlisted(first));
                Assert.IsTrue(uow.IsEnlisted(second));
                Assert.IsFalse(uow.IsEnlisted(dispose.ToDisposable()));
            }
        }

        [Test]
        public void DisposeHandlesExceptionsGracefully()
        {
            bool disposed = false;
            Action dispose = () => disposed = true;
            Action throwOnDispose = () => { Assert.IsFalse(disposed);  throw new InvalidOperationException(); };

            try
            {
                using (UnitOfWorkStub uow = new UnitOfWorkStub())
                {
                    uow.Enlist(throwOnDispose.ToDisposable());
                    uow.Enlist(dispose.ToDisposable());
                }
            }
            catch (CompoundException ex)
            {
                Assert.IsTrue(disposed);
                Assert.AreEqual(1, ex.Exceptions.Count);

                Assert.IsAssignableFrom<InvalidOperationException>(ex.Exceptions[0]);
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
    }
}
