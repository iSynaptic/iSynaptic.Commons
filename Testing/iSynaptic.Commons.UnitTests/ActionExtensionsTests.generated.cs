

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    public partial class ActionExtensionsTests
    {
		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1()
        {
            Action<int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1()
        {
            Action<int> action = (t1) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int> action = (t1) => actionExecuted = true;
            action = action.MakeConditional((t1) => conditionResult);

            action(1);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int> action = (t1) => actionExecuted = true;
            action = action.MakeConditional((t1) => conditionResult, (t1) => falseCount++);

            action(1);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1()
        {
            Action<int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1()
        {
            var exceptions = new List<Exception>();

            Action<int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1()
        {
            Action<int> action = (t1) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1()
        {
            var exceptions = new List<Exception>();

            Action<int> action = (t1) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int> action = (t1) => count++;
            action = action.MakeIdempotent();

            action(1);
            action(1);
            action(1);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1_WithNullArgument_ThrowsException()
        {
            Action<int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int> originalAction = (t1) => executed = true;
            Action<int> action = originalAction.FollowedBy(null);

            action(1);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int> originalAction = (t1) => executed = true;
            Action<int> action = ((Action<int>)null).FollowedBy(originalAction);

            action(1);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int> left = (t1) => leftExecuted = true;
            Action<int> right = (t1) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2()
        {
            Action<int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2()
        {
            Action<int, int> action = (t1, t2) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int> action = (t1, t2) => actionExecuted = true;
            action = action.MakeConditional((t1, t2) => conditionResult);

            action(1, 2);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int> action = (t1, t2) => actionExecuted = true;
            action = action.MakeConditional((t1, t2) => conditionResult, (t1, t2) => falseCount++);

            action(1, 2);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2()
        {
            Action<int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2()
        {
            var exceptions = new List<Exception>();

            Action<int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2()
        {
            Action<int, int> action = (t1, t2) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2()
        {
            var exceptions = new List<Exception>();

            Action<int, int> action = (t1, t2) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int> action = (t1, t2) => count++;
            action = action.MakeIdempotent();

            action(1, 2);
            action(1, 2);
            action(1, 2);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2_WithNullArgument_ThrowsException()
        {
            Action<int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int> originalAction = (t1, t2) => executed = true;
            Action<int, int> action = originalAction.FollowedBy(null);

            action(1, 2);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int> originalAction = (t1, t2) => executed = true;
            Action<int, int> action = ((Action<int, int>)null).FollowedBy(originalAction);

            action(1, 2);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int> left = (t1, t2) => leftExecuted = true;
            Action<int, int> right = (t1, t2) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3()
        {
            Action<int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3()
        {
            Action<int, int, int> action = (t1, t2, t3) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int> action = (t1, t2, t3) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3) => conditionResult);

            action(1, 2, 3);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int> action = (t1, t2, t3) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3) => conditionResult, (t1, t2, t3) => falseCount++);

            action(1, 2, 3);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3()
        {
            Action<int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3()
        {
            Action<int, int, int> action = (t1, t2, t3) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int> action = (t1, t2, t3) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int> action = (t1, t2, t3) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3);
            action(1, 2, 3);
            action(1, 2, 3);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3_WithNullArgument_ThrowsException()
        {
            Action<int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int> originalAction = (t1, t2, t3) => executed = true;
            Action<int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int> originalAction = (t1, t2, t3) => executed = true;
            Action<int, int, int> action = ((Action<int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int> left = (t1, t2, t3) => leftExecuted = true;
            Action<int, int, int> right = (t1, t2, t3) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4()
        {
            Action<int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4()
        {
            Action<int, int, int, int> action = (t1, t2, t3, t4) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int> action = (t1, t2, t3, t4) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4) => conditionResult);

            action(1, 2, 3, 4);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int> action = (t1, t2, t3, t4) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4) => conditionResult, (t1, t2, t3, t4) => falseCount++);

            action(1, 2, 3, 4);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4()
        {
            Action<int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4()
        {
            Action<int, int, int, int> action = (t1, t2, t3, t4) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int> action = (t1, t2, t3, t4) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int> action = (t1, t2, t3, t4) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4);
            action(1, 2, 3, 4);
            action(1, 2, 3, 4);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int> originalAction = (t1, t2, t3, t4) => executed = true;
            Action<int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int> originalAction = (t1, t2, t3, t4) => executed = true;
            Action<int, int, int, int> action = ((Action<int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int> left = (t1, t2, t3, t4) => leftExecuted = true;
            Action<int, int, int, int> right = (t1, t2, t3, t4) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5()
        {
            Action<int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5()
        {
            Action<int, int, int, int, int> action = (t1, t2, t3, t4, t5) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int> action = (t1, t2, t3, t4, t5) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5) => conditionResult);

            action(1, 2, 3, 4, 5);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int> action = (t1, t2, t3, t4, t5) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5) => conditionResult, (t1, t2, t3, t4, t5) => falseCount++);

            action(1, 2, 3, 4, 5);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5()
        {
            Action<int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5()
        {
            Action<int, int, int, int, int> action = (t1, t2, t3, t4, t5) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int> action = (t1, t2, t3, t4, t5) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int> action = (t1, t2, t3, t4, t5) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5);
            action(1, 2, 3, 4, 5);
            action(1, 2, 3, 4, 5);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5) => executed = true;
            Action<int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5) => executed = true;
            Action<int, int, int, int, int> action = ((Action<int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int> left = (t1, t2, t3, t4, t5) => leftExecuted = true;
            Action<int, int, int, int, int> right = (t1, t2, t3, t4, t5) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6()
        {
            Action<int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6()
        {
            Action<int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6) => conditionResult);

            action(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6) => conditionResult, (t1, t2, t3, t4, t5, t6) => falseCount++);

            action(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6()
        {
            Action<int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6()
        {
            Action<int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6);
            action(1, 2, 3, 4, 5, 6);
            action(1, 2, 3, 4, 5, 6);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6) => executed = true;
            Action<int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6) => executed = true;
            Action<int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6) => leftExecuted = true;
            Action<int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7()
        {
            Action<int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7()
        {
            Action<int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7) => conditionResult, (t1, t2, t3, t4, t5, t6, t7) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7()
        {
            Action<int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7()
        {
            Action<int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7);
            action(1, 2, 3, 4, 5, 6, 7);
            action(1, 2, 3, 4, 5, 6, 7);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7) => executed = true;
            Action<int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7) => executed = true;
            Action<int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7) => leftExecuted = true;
            Action<int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8()
        {
            Action<int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7T8()
        {
            Action<int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7T8()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7, 8);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7T8()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8) => conditionResult, (t1, t2, t3, t4, t5, t6, t7, t8) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7, 8);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8()
        {
            Action<int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7T8()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7T8()
        {
            Action<int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7, 8);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7T8()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7, 8);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7, 8);
            action(1, 2, 3, 4, 5, 6, 7, 8);
            action(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8) => executed = true;
            Action<int, int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8) => executed = true;
            Action<int, int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7, t8) => leftExecuted = true;
            Action<int, int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7, t8) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7, 8);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9()
        {
            Action<int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7T8T9()
        {
            Action<int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7T8T9()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7T8T9()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9) => conditionResult, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9()
        {
            Action<int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7T8T9()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7T8T9()
        {
            Action<int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7T8T9()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => executed = true;
            Action<int, int, int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => executed = true;
            Action<int, int, int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => leftExecuted = true;
            Action<int, int, int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10()
        {
            Action<int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7T8T9T10()
        {
            Action<int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7T8T9T10()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7T8T9T10()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => conditionResult, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10()
        {
            Action<int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7T8T9T10()
        {
            Action<int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7T8T9T10()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => leftExecuted = true;
            Action<int, int, int, int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7T8T9T10T11()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7T8T9T10T11()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => conditionResult, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7T8T9T10T11()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7T8T9T10T11()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => leftExecuted = true;
            Action<int, int, int, int, int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => conditionResult, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => leftExecuted = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => conditionResult, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12T13_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => leftExecuted = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => conditionResult, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => leftExecuted = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => conditionResult, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => leftExecuted = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

		
		[Test]
        public void MakeConditional_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => false); });
        }

        [Test]
        public void MakeConditional_WithNullCondition_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { };
            Assert.Throws<ArgumentNullException>(() => { action = action.MakeConditional(null); });
        }

        [Test]
        public void MakeConditional_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
			bool conditionResult = false;
            bool actionExecuted = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => conditionResult);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            Assert.IsFalse(actionExecuted);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            Assert.IsTrue(actionExecuted);
        }

        [Test]
        public void MakeConditionalWithFalseAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
            int falseCount = 0;
			bool conditionResult = false;
            bool actionExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => actionExecuted = true;
            action = action.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => conditionResult, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => falseCount++);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            Assert.IsFalse(actionExecuted);
			Assert.AreEqual(1, falseCount);

			conditionResult = true;
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            Assert.IsTrue(actionExecuted);
			Assert.AreEqual(1, falseCount);
        }

		[Test]
        public void CatchExceptions_WithNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.CatchExceptions());
        }

		[Test]
        public void CatchExceptions_WithCollectionAndNullAction_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> actionT1 = null;
            Assert.Throws<ArgumentNullException>(() => actionT1.CatchExceptions(exceptions));
        }

		[Test]
        public void CatchExceptions_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        }

		[Test]
        public void CatchExceptions_WithCollection_ForActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
            var exceptions = new List<Exception>();

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { throw new InvalidOperationException(); };
            action = action.CatchExceptions(exceptions);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            
            Assert.AreEqual(1, exceptions.Count);
            Assert.IsTrue(exceptions[0].GetType() == typeof(InvalidOperationException));
        }

		[Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_EnsuresActionsExecuteOnlyOnce()
        {
            int count = 0;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => count++;
            action = action.MakeIdempotent();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MakeIdempotentOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithNullArgument_ThrowsException()
        {
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = null;
            Assert.Throws<ArgumentNullException>(() => action.MakeIdempotent());
        }

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithNullArgument_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = originalAction.FollowedBy(null);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_ExtendingNullAction_ReturnsOriginal()
        {
            bool executed = false;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> originalAction = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => executed = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> action = ((Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int>)null).FollowedBy(originalAction);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.IsTrue(ReferenceEquals(originalAction, action));
            Assert.IsTrue(executed);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_CallsBothActionsInSuccession()
        {
            bool leftExecuted = false;
            bool rightExecuted = false;

            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => leftExecuted = true;
            Action<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => rightExecuted = true;

            var action = left.FollowedBy(right);

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            Assert.IsTrue(leftExecuted && rightExecuted);
        }

			}
}