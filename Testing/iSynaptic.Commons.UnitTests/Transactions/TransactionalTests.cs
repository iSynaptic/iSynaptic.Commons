using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

using NUnit.Framework;

using iSynaptic.Commons.Transactions;
using Rhino.Mocks;
using iSynaptic.Commons.Testing.NUnit;

namespace iSynaptic.Commons.Transactions
{
    [TestFixture]
    public class TransactionalTests : NUnitBaseTestFixture
    {
        [Test]
        public void NullOnCreation()
        {
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>();
            Assert.IsNull(tso.Value);
        }

        [Test]
        public void InitialValue()
        {
            SimpleObject so = new SimpleObject();
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(so);

            Assert.IsNotNull(tso.Value);
            Assert.AreEqual(so, tso.Value);
        }

        [Test]
        public void CopiesNullOnNewTransaction()
        {
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>();

            using (TransactionScope scope = new TransactionScope())
            {
                Assert.IsNull(tso.Value);
                scope.Complete();
            }
        }

        [Test]
        public void CopiesInitalValueOnNewTransaction()
        {
            SimpleObject so = new SimpleObject();
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(so);

            using (TransactionScope scope = new TransactionScope())
            {
                Assert.IsFalse(object.ReferenceEquals(so, tso.Value));

                Assert.AreEqual(so.TestInt, tso.Value.TestInt);
                Assert.AreEqual(so.TestGuid, tso.Value.TestGuid);
                Assert.AreEqual(so.TestString, tso.Value.TestString);

                scope.Complete();
            }
        }

        [Test]
        public void ChangesNotVisibleOutsideOfTransaction()
        {
            SimpleObject so = new SimpleObject();
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(so);

            Guid testGuid = Guid.NewGuid();

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 7;
                tso.Value.TestGuid = testGuid;
                tso.Value.TestString = "Testing";

                using (TransactionScope suppressScope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    Assert.AreNotEqual(7, tso.Value.TestInt);
                    Assert.AreNotEqual(testGuid, tso.Value.TestGuid);
                    Assert.AreNotEqual("Testing", tso.Value.TestString);

                    suppressScope.Complete();
                }

                scope.Complete();
            }
        }

        [Test]
        public void ChangesVisibleWithinTransaction()
        {
            SimpleObject so = new SimpleObject();
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(so);

            Guid testGuid = Guid.NewGuid();

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 7;
                tso.Value.TestGuid = testGuid;
                tso.Value.TestString = "Testing";

                Assert.AreEqual(7, tso.Value.TestInt);
                Assert.AreEqual(testGuid, tso.Value.TestGuid);
                Assert.AreEqual("Testing", tso.Value.TestString);

                scope.Complete();
            }
        }

        [Test]
        public void ChangesRolledBack()
        {
            SimpleObject so = new SimpleObject();
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(so);

            Guid testGuid = Guid.NewGuid();

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 7;
                tso.Value.TestGuid = testGuid;
                tso.Value.TestString = "Testing";
            }

            Assert.AreNotEqual(7, tso.Value.TestInt);
            Assert.AreNotEqual(testGuid, tso.Value.TestGuid);
            Assert.AreNotEqual("Testing", tso.Value.TestString);
        }

        [Test]
        public void ChangesCommited()
        {
            SimpleObject so = new SimpleObject();
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(so);

            Guid testGuid = Guid.NewGuid();

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 7;
                tso.Value.TestGuid = testGuid;
                tso.Value.TestString = "Testing";

                scope.Complete();
            }

            Assert.AreEqual(7, tso.Value.TestInt);
            Assert.AreEqual(testGuid, tso.Value.TestGuid);
            Assert.AreEqual("Testing", tso.Value.TestString);
        }

        [Test]
        public void ConcurrencyCollision()
        {
            SimpleObject so = new SimpleObject();
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(so);

            Assert.Throws<TransactionAbortedException>(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    tso.Value.TestInt = 7;
                    tso.Value.TestGuid = Guid.NewGuid();
                    tso.Value.TestString = "Testing";

                    using (TransactionScope scopeTwo = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        tso.Value.TestInt = 9;
                        tso.Value.TestGuid = Guid.NewGuid();
                        tso.Value.TestString = "Testing Two";

                        scopeTwo.Complete();
                    }

                    scope.Complete();
                }
            });
        }

        [Test]
        public void DependentTransactions()
        {
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(new SimpleObject());
            Guid id = Guid.NewGuid();

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 7;
                tso.Value.TestGuid = id;
                tso.Value.TestString = "Testing";

                DependentTransaction dt = Transaction.Current.DependentClone(DependentCloneOption.RollbackIfNotComplete);

                Action assertAction = delegate()
                {
                    using (dt)
                    using (TransactionScope scopeTwo = new TransactionScope(dt))
                    {
                        Assert.AreEqual(7, tso.Value.TestInt);
                        Assert.AreEqual(id, tso.Value.TestGuid);
                        Assert.AreEqual("Testing", tso.Value.TestString);

                        scopeTwo.Complete();
                        dt.Complete();
                    }
                };

                IAsyncResult ar = assertAction.BeginInvoke(null, null);

                ar.AsyncWaitHandle.WaitOne();
                assertAction.EndInvoke(ar);

                scope.Complete();
            }
        }

        [Test]
        public void NotVisibleOnOtherThread()
        {
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(new SimpleObject());
            Guid id = Guid.NewGuid();

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 7;
                tso.Value.TestGuid = id;
                tso.Value.TestString = "Testing";

                Action assertAction = delegate()
                {
                    Assert.AreNotEqual(7, tso.Value.TestInt);
                    Assert.AreNotEqual(id, tso.Value.TestGuid);
                    Assert.AreNotEqual("Testing", tso.Value.TestString);
                };

                IAsyncResult ar = assertAction.BeginInvoke(null, null);

                ar.AsyncWaitHandle.WaitOne();
                assertAction.EndInvoke(ar);

                scope.Complete();
            }
        }

        [Test]
        public void ConcurrencyCollisionAcrossThreads()
        {
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(new SimpleObject());
            Guid id = Guid.NewGuid();

            Assert.Throws<TransactionAbortedException>(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    tso.Value.TestInt = 7;
                    tso.Value.TestGuid = id;
                    tso.Value.TestString = "Testing";

                    Action assertAction = delegate()
                    {
                        using (TransactionScope scopeTwo = new TransactionScope())
                        {
                            tso.Value.TestInt = 9;
                            tso.Value.TestGuid = Guid.NewGuid();
                            tso.Value.TestString = "Testing Two";

                            scopeTwo.Complete();
                        }
                    };

                    IAsyncResult ar = assertAction.BeginInvoke(null, null);

                    ar.AsyncWaitHandle.WaitOne();
                    assertAction.EndInvoke(ar);

                    scope.Complete();
                }
            });
        }

        [Test]
        public void MultipleSerializedTransactions()
        {
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(new SimpleObject());
            Guid id = Guid.NewGuid();
            Guid idTwo = Guid.NewGuid();

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 7;
                tso.Value.TestGuid = id;
                tso.Value.TestString = "Testing";

                scope.Complete();
            }

            Assert.AreEqual(7, tso.Value.TestInt);
            Assert.AreEqual(id, tso.Value.TestGuid);
            Assert.AreEqual("Testing", tso.Value.TestString);

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 9;
                tso.Value.TestGuid = idTwo;
                tso.Value.TestString = "Testing Two";

                scope.Complete();
            }

            Assert.AreEqual(9, tso.Value.TestInt);
            Assert.AreEqual(idTwo, tso.Value.TestGuid);
            Assert.AreEqual("Testing Two", tso.Value.TestString);
        }

        [Test]
        public void MultipleSerializedTransactionsAcrossThreads()
        {
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(new SimpleObject());
            Guid id = Guid.NewGuid();
            Guid idTwo = Guid.NewGuid();

            Action firstChange = delegate()
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    tso.Value.TestInt = 7;
                    tso.Value.TestGuid = id;
                    tso.Value.TestString = "Testing";

                    scope.Complete();
                }
            };

            IAsyncResult ar = firstChange.BeginInvoke(null, null);
            ar.AsyncWaitHandle.WaitOne();
            firstChange.EndInvoke(ar);

            Assert.AreEqual(7, tso.Value.TestInt);
            Assert.AreEqual(id, tso.Value.TestGuid);
            Assert.AreEqual("Testing", tso.Value.TestString);

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 9;
                tso.Value.TestGuid = idTwo;
                tso.Value.TestString = "Testing Two";

                scope.Complete();
            }

            Assert.AreEqual(9, tso.Value.TestInt);
            Assert.AreEqual(idTwo, tso.Value.TestGuid);
            Assert.AreEqual("Testing Two", tso.Value.TestString);
        }

        public class ExposedValuesTransactional<T> : Transactional<T>
        {
            public ExposedValuesTransactional(T value) : base(value)
            {
            }

            public Dictionary<string, KeyValuePair<Guid, T>> GetValues()
            {
                return Values;
            }
        }

        [Test]
        public void NoMemoryLeakOnCommit()
        {
            ExposedValuesTransactional<SimpleObject> tso = new ExposedValuesTransactional<SimpleObject>(new SimpleObject());

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 7;
                tso.Value.TestGuid = Guid.NewGuid();
                tso.Value.TestString = "Testing";

                Assert.AreEqual(1, tso.GetValues().Count);

                scope.Complete();
            }

            Assert.AreEqual(0, tso.GetValues().Count);
        }

        [Test]
        public void NoMemoryLeakOnRollback()
        {
            ExposedValuesTransactional<SimpleObject> tso = new ExposedValuesTransactional<SimpleObject>(new SimpleObject());

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value.TestInt = 7;
                tso.Value.TestGuid = Guid.NewGuid();
                tso.Value.TestString = "Testing";

                Assert.AreEqual(1, tso.GetValues().Count);
            }

            Assert.AreEqual(0, tso.GetValues().Count);
        }

        [Test]
        public void ExceptionUponCompletionRollback()
        {
            MockRepository mocks = new MockRepository();

            IEnlistmentNotification concurrencyConflictResource = mocks.StrictMock<IEnlistmentNotification>();
            concurrencyConflictResource.Prepare(null);
            LastCall.IgnoreArguments().Throw(new TransactionalConcurrencyException());

            mocks.ReplayAll();

            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(new SimpleObject());
            tso.Value.TestInt = 0;
            tso.Value.TestGuid = Guid.Empty;
            tso.Value.TestString = null;

            Assert.Throws<TransactionalConcurrencyException>(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    tso.Value.TestInt = 7;
                    tso.Value.TestGuid = Guid.NewGuid();
                    tso.Value.TestString = "Testing";

                    Transaction.Current.EnlistVolatile(concurrencyConflictResource, EnlistmentOptions.None);

                    scope.Complete();
                }
            });

            Assert.AreEqual(0, tso.Value.TestInt);
            Assert.AreEqual(Guid.Empty, tso.Value.TestGuid);
            Assert.AreEqual(null, tso.Value.TestString);
        }

        [Test]
        public void SetValueWithoutTransaction()
        {
            var tso = new Transactional<SimpleObject>();

            SimpleObject state = new SimpleObject();

            state.TestInt = 7;
            state.TestGuid = Guid.NewGuid();
            state.TestString = "Testing";

            Assert.IsNull(tso.Value);

            tso.Value = state;

            Assert.IsNotNull(state);
            Assert.IsTrue(object.ReferenceEquals(state, tso.Value));
        }

        [Test]
        public void SetValue()
        {
            var tso = new Transactional<SimpleObject>();

            SimpleObject state = new SimpleObject();

            state.TestInt = 7;
            state.TestGuid = Guid.NewGuid();
            state.TestString = "Testing";

            Assert.IsNull(tso.Value);

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value = state;

                using (TransactionScope scopeTwo = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    Assert.IsNull(tso.Value);
                }

                scope.Complete();
            }

            Assert.IsNotNull(tso.Value);
            Assert.IsTrue(object.ReferenceEquals(state, tso.Value));
        }

        [Test]
        public void SetValueTwice()
        {
            var tso = new Transactional<SimpleObject>();

            SimpleObject state = new SimpleObject();

            state.TestInt = 7;
            state.TestGuid = Guid.NewGuid();
            state.TestString = "Testing";

            SimpleObject state2 = new SimpleObject();

            state2.TestInt = 9;
            state2.TestGuid = Guid.NewGuid();
            state2.TestString = "Testing2";

            Assert.IsNull(tso.Value);

            using (TransactionScope scope = new TransactionScope())
            {
                tso.Value = state;
                tso.Value = state2;

                using (TransactionScope scopeTwo = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    Assert.IsNull(tso.Value);
                }

                scope.Complete();
            }

            Assert.IsNotNull(tso.Value);
            Assert.IsTrue(object.ReferenceEquals(state2, tso.Value));
        }

        [Test]
        public void NonCloneableType()
        {
            Assert.Throws<InvalidOperationException>(() => new Transactional<IntPtr>());
        }

        [Test]
        public void InDoubt()
        {
            var tso = new Transactional<SimpleObject>();

            SimpleObject obj = new SimpleObject
            {
                TestInt = 7,
                TestGuid = Guid.NewGuid(),
                TestString = "Testing"
            };

            MockRepository mocks = new MockRepository();

            ISinglePhaseNotification badResource = mocks.StrictMock<ISinglePhaseNotification>();
            badResource.SinglePhaseCommit(null);

            Action<SinglePhaseEnlistment> onCommit = (SinglePhaseEnlistment spe) => spe.InDoubt();
            LastCall.IgnoreArguments().Do(onCommit);

            mocks.ReplayAll();

            Assert.Throws<TransactionInDoubtException>(() =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    tso.Value = obj;
                    Transaction.Current.EnlistDurable(Guid.NewGuid(), badResource, EnlistmentOptions.None);

                    scope.Complete();
                }
            });

            Assert.IsNull(tso.Value);
        }

        //[Test]
        //public void CommitingOverlappingTransactionsWithNoValueChange()
        //{
        //    var tso = new Transactional<SimpleObject>();

        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        var x = tso.Value;

        //        using (TransactionScope scope2 = new TransactionScope(TransactionScopeOption.RequiresNew))
        //        {
        //            var y = tso.Value;
        //            scope2.Complete();
        //        }

        //        scope.Complete();
        //    }
        //}
    }
}
