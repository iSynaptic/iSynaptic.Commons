using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

using MbUnit.Framework;

using iSynaptic.Commons.Transactions;

namespace iSynaptic.Commons.UnitTests.Transactions
{
    [TestFixture]
    public class TransactionalTests
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
        [ExpectedException(typeof(TransactionalConcurrencyException))]
        public void ConcurrencyCollision()
        {
            SimpleObject so = new SimpleObject();
            Transactional<SimpleObject> tso = new Transactional<SimpleObject>(so);

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
        }
    }
}
