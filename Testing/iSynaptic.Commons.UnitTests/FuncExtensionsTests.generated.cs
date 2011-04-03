

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

namespace iSynaptic.Commons
{
    public partial class FuncExtensionsTests
    {
		
		[Test]
        public void ToActionOfT1()
        {
            int val = 0;

            Func<int, int> func = (t1) => { val = t1; return val; };
            var action = func.ToAction();

            action(1);

			int expected = 1;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, bool> nullFunc = null;
            Func<int, bool> notNullFunc = (t1) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, bool> left = (t1) => leftResult;
            Func<int, bool> right = (t1) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1));

			leftResult = false;
            Assert.IsFalse(andFunc(1));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1));
        }

        [Test]
        public void OrOfT1_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, bool> nullFunc = null;
            Func<int, bool> notNullFunc = (t1) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, bool> left = (t1) => leftResult;
            Func<int, bool> right = (t1) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1));

			leftResult = false;
            Assert.IsTrue(orFunc(1));

			rightResult = false;
            Assert.IsFalse(orFunc(1));
        }

        [Test]
        public void XOrOfT1_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, bool> nullFunc = null;
            Func<int, bool> notNullFunc = (t1) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, bool> left = (t1) => leftResult;
            Func<int, bool> right = (t1) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1));

			leftResult = false;
            Assert.IsTrue(orFunc(1));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1));

			leftResult = false;
            Assert.IsFalse(orFunc(1));
        }

		[Test]
		public void SynchronizeOfT1_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int> func = (t1) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1));
            var task2 = Task.Factory.StartNew(() => func(1));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1));
            task2 = Task.Factory.StartNew(() => func(1));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1_UsesCorrectArguments()
		{
            Func<int, int> func = (t1) => t1;
			func = func.Synchronize();

			int expected = 1;
			
			Assert.AreEqual(expected, func(1));
		}

		[Test]
		public void MakeConditionalOfT1()
		{
			
			Func<int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1) => true); });

            func = (t1) => t1;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1;

            var simpleConditionalFunc = func.MakeConditional((t1) => t1 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0));
            Assert.AreEqual(expected, simpleConditionalFunc(1));

            var withDefaultValueFunc = func.MakeConditional((t1) => t1 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0));
            Assert.AreEqual(expected, withDefaultValueFunc(1));

            var withFalseFunc = func.MakeConditional((t1) => t1 == expected, (t1) => 42);
            Assert.AreEqual(42, withFalseFunc(0));
            Assert.AreEqual(expected, withFalseFunc(1));
		}

		[Test]
		public void MemoizeOfT1()
		{
			Func<int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1;
			int secondExpected = 2;

            func = (t1) => { count++; return t1; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1));
			Assert.AreEqual(firstExpected, func(1));
			Assert.AreEqual(firstExpected, func(1));

			
			Assert.AreEqual(secondExpected, func(2));
			Assert.AreEqual(secondExpected, func(2));
			Assert.AreEqual(secondExpected, func(2));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1_WithNullArgument_ReturnsOriginal()
        {
            Func<int, Maybe<int>> originalFunc = (t1) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, Maybe<int>> originalFunc = (t1) => 42;
            var func = ((Func<int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1_CallsFirstFunc()
        {
            Func<int, Maybe<int>> left = (t1) => 42;
            Func<int, Maybe<int>> right = (t1) => 7;

            var func = left.FollowedBy(right);

            var results = func(1);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1_CallsSecondFunc()
        {
            Func<int, Maybe<int>> left = (t1) => Maybe<int>.NoValue;
            Func<int, Maybe<int>> right = (t1) => 42;

            var func = left.FollowedBy(right);

            var results = func(1);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1_WithNullArgument_ReturnsOriginal()
        {
            Func<int, Maybe<int>> originalFunc = (t1) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, Maybe<int>> originalFunc = (t1) => 42;
            var func = ((Func<int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1_CallsFirstFunc()
        {
            Func<int, Maybe<int>> left = (t1) => 42;
            Func<int, Maybe<int>> right = (t1) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1_CallsSecondFunc()
        {
            Func<int, Maybe<int>> left = (t1) => 7;
            Func<int, Maybe<int>> right = (t1) => 42;

            var func = left.PrecededBy(right);

            var results = func(1);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2()
        {
            int val = 0;

            Func<int, int, int> func = (t1, t2) => { val = t1 + t2; return val; };
            var action = func.ToAction();

            action(1, 2);

			int expected = 1 + 2;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, bool> nullFunc = null;
            Func<int, int, bool> notNullFunc = (t1, t2) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, bool> left = (t1, t2) => leftResult;
            Func<int, int, bool> right = (t1, t2) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2));
        }

        [Test]
        public void OrOfT1T2_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, bool> nullFunc = null;
            Func<int, int, bool> notNullFunc = (t1, t2) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, bool> left = (t1, t2) => leftResult;
            Func<int, int, bool> right = (t1, t2) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2));
        }

        [Test]
        public void XOrOfT1T2_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, bool> nullFunc = null;
            Func<int, int, bool> notNullFunc = (t1, t2) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, bool> left = (t1, t2) => leftResult;
            Func<int, int, bool> right = (t1, t2) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2));
        }

		[Test]
		public void SynchronizeOfT1T2_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int> func = (t1, t2) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2));
            var task2 = Task.Factory.StartNew(() => func(1, 2));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2));
            task2 = Task.Factory.StartNew(() => func(1, 2));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2_UsesCorrectArguments()
		{
            Func<int, int, int> func = (t1, t2) => t1 + t2;
			func = func.Synchronize();

			int expected = 3;
			
			Assert.AreEqual(expected, func(1, 2));
		}

		[Test]
		public void MakeConditionalOfT1T2()
		{
			
			Func<int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2) => true); });

            func = (t1, t2) => t1 + t2;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2;

            var simpleConditionalFunc = func.MakeConditional((t1, t2) => t1 + t2 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2));

            var withDefaultValueFunc = func.MakeConditional((t1, t2) => t1 + t2 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2));

            var withFalseFunc = func.MakeConditional((t1, t2) => t1 + t2 == expected, (t1, t2) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1));
            Assert.AreEqual(expected, withFalseFunc(1, 2));
		}

		[Test]
		public void MemoizeOfT1T2()
		{
			Func<int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2;
			int secondExpected = 2 + 4;

            func = (t1, t2) => { count++; return t1 + t2; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2));
			Assert.AreEqual(firstExpected, func(1, 2));
			Assert.AreEqual(firstExpected, func(1, 2));

			
			Assert.AreEqual(secondExpected, func(2, 4));
			Assert.AreEqual(secondExpected, func(2, 4));
			Assert.AreEqual(secondExpected, func(2, 4));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, Maybe<int>> originalFunc = (t1, t2) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, Maybe<int>> originalFunc = (t1, t2) => 42;
            var func = ((Func<int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2_CallsFirstFunc()
        {
            Func<int, int, Maybe<int>> left = (t1, t2) => 42;
            Func<int, int, Maybe<int>> right = (t1, t2) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2_CallsSecondFunc()
        {
            Func<int, int, Maybe<int>> left = (t1, t2) => Maybe<int>.NoValue;
            Func<int, int, Maybe<int>> right = (t1, t2) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, Maybe<int>> originalFunc = (t1, t2) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, Maybe<int>> originalFunc = (t1, t2) => 42;
            var func = ((Func<int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2_CallsFirstFunc()
        {
            Func<int, int, Maybe<int>> left = (t1, t2) => 42;
            Func<int, int, Maybe<int>> right = (t1, t2) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2_CallsSecondFunc()
        {
            Func<int, int, Maybe<int>> left = (t1, t2) => 7;
            Func<int, int, Maybe<int>> right = (t1, t2) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3()
        {
            int val = 0;

            Func<int, int, int, int> func = (t1, t2, t3) => { val = t1 + t2 + t3; return val; };
            var action = func.ToAction();

            action(1, 2, 3);

			int expected = 1 + 2 + 3;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, bool> nullFunc = null;
            Func<int, int, int, bool> notNullFunc = (t1, t2, t3) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, bool> left = (t1, t2, t3) => leftResult;
            Func<int, int, int, bool> right = (t1, t2, t3) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3));
        }

        [Test]
        public void OrOfT1T2T3_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, bool> nullFunc = null;
            Func<int, int, int, bool> notNullFunc = (t1, t2, t3) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, bool> left = (t1, t2, t3) => leftResult;
            Func<int, int, int, bool> right = (t1, t2, t3) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3));
        }

        [Test]
        public void XOrOfT1T2T3_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, bool> nullFunc = null;
            Func<int, int, int, bool> notNullFunc = (t1, t2, t3) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, bool> left = (t1, t2, t3) => leftResult;
            Func<int, int, int, bool> right = (t1, t2, t3) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3));
        }

		[Test]
		public void SynchronizeOfT1T2T3_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int> func = (t1, t2, t3) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3_UsesCorrectArguments()
		{
            Func<int, int, int, int> func = (t1, t2, t3) => t1 + t2 + t3;
			func = func.Synchronize();

			int expected = 6;
			
			Assert.AreEqual(expected, func(1, 2, 3));
		}

		[Test]
		public void MakeConditionalOfT1T2T3()
		{
			
			Func<int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3) => true); });

            func = (t1, t2, t3) => t1 + t2 + t3;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3) => t1 + t2 + t3 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3) => t1 + t2 + t3 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3));

            var withFalseFunc = func.MakeConditional((t1, t2, t3) => t1 + t2 + t3 == expected, (t1, t2, t3) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3));
		}

		[Test]
		public void MemoizeOfT1T2T3()
		{
			Func<int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3;
			int secondExpected = 2 + 4 + 6;

            func = (t1, t2, t3) => { count++; return t1 + t2 + t3; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3));
			Assert.AreEqual(firstExpected, func(1, 2, 3));
			Assert.AreEqual(firstExpected, func(1, 2, 3));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6));
			Assert.AreEqual(secondExpected, func(2, 4, 6));
			Assert.AreEqual(secondExpected, func(2, 4, 6));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, Maybe<int>> originalFunc = (t1, t2, t3) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, Maybe<int>> originalFunc = (t1, t2, t3) => 42;
            var func = ((Func<int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3_CallsFirstFunc()
        {
            Func<int, int, int, Maybe<int>> left = (t1, t2, t3) => 42;
            Func<int, int, int, Maybe<int>> right = (t1, t2, t3) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3_CallsSecondFunc()
        {
            Func<int, int, int, Maybe<int>> left = (t1, t2, t3) => Maybe<int>.NoValue;
            Func<int, int, int, Maybe<int>> right = (t1, t2, t3) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, Maybe<int>> originalFunc = (t1, t2, t3) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, Maybe<int>> originalFunc = (t1, t2, t3) => 42;
            var func = ((Func<int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3_CallsFirstFunc()
        {
            Func<int, int, int, Maybe<int>> left = (t1, t2, t3) => 42;
            Func<int, int, int, Maybe<int>> right = (t1, t2, t3) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3_CallsSecondFunc()
        {
            Func<int, int, int, Maybe<int>> left = (t1, t2, t3) => 7;
            Func<int, int, int, Maybe<int>> right = (t1, t2, t3) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4()
        {
            int val = 0;

            Func<int, int, int, int, int> func = (t1, t2, t3, t4) => { val = t1 + t2 + t3 + t4; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4);

			int expected = 1 + 2 + 3 + 4;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, bool> left = (t1, t2, t3, t4) => leftResult;
            Func<int, int, int, int, bool> right = (t1, t2, t3, t4) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4));
        }

        [Test]
        public void OrOfT1T2T3T4_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, bool> left = (t1, t2, t3, t4) => leftResult;
            Func<int, int, int, int, bool> right = (t1, t2, t3, t4) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4));
        }

        [Test]
        public void XOrOfT1T2T3T4_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, bool> left = (t1, t2, t3, t4) => leftResult;
            Func<int, int, int, int, bool> right = (t1, t2, t3, t4) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int> func = (t1, t2, t3, t4) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4_UsesCorrectArguments()
		{
            Func<int, int, int, int, int> func = (t1, t2, t3, t4) => t1 + t2 + t3 + t4;
			func = func.Synchronize();

			int expected = 10;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4()
		{
			
			Func<int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4) => true); });

            func = (t1, t2, t3, t4) => t1 + t2 + t3 + t4;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4) => t1 + t2 + t3 + t4 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4) => t1 + t2 + t3 + t4 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4) => t1 + t2 + t3 + t4 == expected, (t1, t2, t3, t4) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4));
		}

		[Test]
		public void MemoizeOfT1T2T3T4()
		{
			Func<int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4;
			int secondExpected = 2 + 4 + 6 + 8;

            func = (t1, t2, t3, t4) => { count++; return t1 + t2 + t3 + t4; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4) => 42;
            var func = ((Func<int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4_CallsFirstFunc()
        {
            Func<int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4) => 42;
            Func<int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4_CallsSecondFunc()
        {
            Func<int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4) => Maybe<int>.NoValue;
            Func<int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4) => 42;
            var func = ((Func<int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4_CallsFirstFunc()
        {
            Func<int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4) => 42;
            Func<int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4_CallsSecondFunc()
        {
            Func<int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4) => 7;
            Func<int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5()
        {
            int val = 0;

            Func<int, int, int, int, int, int> func = (t1, t2, t3, t4, t5) => { val = t1 + t2 + t3 + t4 + t5; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5);

			int expected = 1 + 2 + 3 + 4 + 5;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5) => leftResult;
            Func<int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5));
        }

        [Test]
        public void OrOfT1T2T3T4T5_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5) => leftResult;
            Func<int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5));
        }

        [Test]
        public void XOrOfT1T2T3T4T5_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5) => leftResult;
            Func<int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int> func = (t1, t2, t3, t4, t5) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int> func = (t1, t2, t3, t4, t5) => t1 + t2 + t3 + t4 + t5;
			func = func.Synchronize();

			int expected = 15;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5()
		{
			
			Func<int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5) => true); });

            func = (t1, t2, t3, t4, t5) => t1 + t2 + t3 + t4 + t5;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5) => t1 + t2 + t3 + t4 + t5 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5) => t1 + t2 + t3 + t4 + t5 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5) => t1 + t2 + t3 + t4 + t5 == expected, (t1, t2, t3, t4, t5) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5()
		{
			Func<int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5;
			int secondExpected = 2 + 4 + 6 + 8 + 10;

            func = (t1, t2, t3, t4, t5) => { count++; return t1 + t2 + t3 + t4 + t5; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5) => 42;
            var func = ((Func<int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5_CallsFirstFunc()
        {
            Func<int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5) => 42;
            Func<int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5_CallsSecondFunc()
        {
            Func<int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5) => 42;
            var func = ((Func<int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5_CallsFirstFunc()
        {
            Func<int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5) => 42;
            Func<int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5_CallsSecondFunc()
        {
            Func<int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5) => 7;
            Func<int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6) => { val = t1 + t2 + t3 + t4 + t5 + t6; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6);

			int expected = 1 + 2 + 3 + 4 + 5 + 6;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6) => leftResult;
            Func<int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6) => leftResult;
            Func<int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6) => leftResult;
            Func<int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6) => t1 + t2 + t3 + t4 + t5 + t6;
			func = func.Synchronize();

			int expected = 21;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6()
		{
			
			Func<int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6) => true); });

            func = (t1, t2, t3, t4, t5, t6) => t1 + t2 + t3 + t4 + t5 + t6;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6) => t1 + t2 + t3 + t4 + t5 + t6 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6) => t1 + t2 + t3 + t4 + t5 + t6 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6) => t1 + t2 + t3 + t4 + t5 + t6 == expected, (t1, t2, t3, t4, t5, t6) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6()
		{
			Func<int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12;

            func = (t1, t2, t3, t4, t5, t6) => { count++; return t1 + t2 + t3 + t4 + t5 + t6; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6) => 42;
            var func = ((Func<int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6) => 42;
            Func<int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6) => 42;
            var func = ((Func<int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6) => 42;
            Func<int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6) => 7;
            Func<int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7) => leftResult;
            Func<int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7) => leftResult;
            Func<int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7) => leftResult;
            Func<int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7) => t1 + t2 + t3 + t4 + t5 + t6 + t7;
			func = func.Synchronize();

			int expected = 28;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7()
		{
			
			Func<int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7) => t1 + t2 + t3 + t4 + t5 + t6 + t7;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7) => t1 + t2 + t3 + t4 + t5 + t6 + t7 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7) => t1 + t2 + t3 + t4 + t5 + t6 + t7 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7) => t1 + t2 + t3 + t4 + t5 + t6 + t7 == expected, (t1, t2, t3, t4, t5, t6, t7) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7()
		{
			Func<int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14;

            func = (t1, t2, t3, t4, t5, t6, t7) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7) => 42;
            var func = ((Func<int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7) => 42;
            Func<int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7) => 42;
            var func = ((Func<int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7) => 42;
            Func<int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7) => 7;
            Func<int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7T8()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7T8_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7T8_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8) => leftResult;
            Func<int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7, 8));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8) => leftResult;
            Func<int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8) => leftResult;
            Func<int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8;
			func = func.Synchronize();

			int expected = 36;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7, 8));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7T8()
		{
			
			Func<int, int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7, t8) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3, 4));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7, 8));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3, 4));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7, 8));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 == expected, (t1, t2, t3, t4, t5, t6, t7, t8) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3, 4));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7, 8));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7T8()
		{
			Func<int, int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14 + 16;

            func = (t1, t2, t3, t4, t5, t6, t7, t8) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8) => 42;
            Func<int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8) => 42;
            Func<int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8) => 7;
            Func<int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7T8T9()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7T8T9_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7T8T9_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9;
			func = func.Synchronize();

			int expected = 45;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7, 8, 9));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7T8T9()
		{
			
			Func<int, int, int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3, 4, 4));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3, 4, 4));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 == expected, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3, 4, 4));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7, 8, 9));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7T8T9()
		{
			Func<int, int, int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14 + 16 + 18;

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 42;
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 42;
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 7;
            Func<int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7T8T9T10()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10;
			func = func.Synchronize();

			int expected = 55;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7T8T9T10()
		{
			
			Func<int, int, int, int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 == expected, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7T8T9T10()
		{
			Func<int, int, int, int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14 + 16 + 18 + 20;

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 7;
            Func<int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7T8T9T10T11()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11;
			func = func.Synchronize();

			int expected = 66;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7T8T9T10T11()
		{
			
			Func<int, int, int, int, int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 == expected, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7T8T9T10T11()
		{
			Func<int, int, int, int, int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14 + 16 + 18 + 20 + 22;

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 7;
            Func<int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7T8T9T10T11T12()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12;
			func = func.Synchronize();

			int expected = 78;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7T8T9T10T11T12()
		{
			
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 == expected, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7T8T9T10T11T12()
		{
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14 + 16 + 18 + 20 + 22 + 24;

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 7;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13;
			func = func.Synchronize();

			int expected = 91;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
		{
			
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 == expected, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
		{
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14 + 16 + 18 + 20 + 22 + 24 + 26;

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 7;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14;
			func = func.Synchronize();

			int expected = 105;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
		{
			
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 == expected, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
		{
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14 + 16 + 18 + 20 + 22 + 24 + 26 + 28;

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 7;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15;
			func = func.Synchronize();

			int expected = 120;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
		{
			
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 == expected, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
		{
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14 + 16 + 18 + 20 + 22 + 24 + 26 + 28 + 30;

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 7;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

            Assert.AreEqual(42, results.Value);
        }

		
		[Test]
        public void ToActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16;
            Assert.AreEqual(expected, val);
        }

		[Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void AndOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithValidFuncs_ComposesCorrectly()
        {
			bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => rightResult;

            var andFunc = left.And(right);

            Assert.IsTrue(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			leftResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			leftResult = true;
			rightResult = false;
            Assert.IsFalse(andFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void OrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => rightResult;

            var orFunc = left.Or(right);

            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			rightResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> nullFunc = null;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> notNullFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOrOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithValidFuncs_ComposesCorrectly()
        {
            bool leftResult = true;
			bool rightResult = true;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => leftResult;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, bool> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => rightResult;

            var orFunc = left.XOr(right);

            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			leftResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			leftResult = true;
			rightResult = false;
            Assert.IsTrue(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			leftResult = false;
            Assert.IsFalse(orFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
        }

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_PreventsConcurrentAccess()
		{
			bool shouldSync = true;

			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { count++; if(count == 1) { Thread.Sleep(100); } return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => shouldSync);

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			var results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{1, 2}));

			shouldSync = false;
			count = 0;

			task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
            task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			results = new[]{task1, task2}.Select(x => x.Result).OrderBy(x => x);
			Assert.IsTrue(results.SequenceEqual(new[]{2, 2}));
		}

		[Test]
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_UsesCorrectArguments()
		{
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16;
			func = func.Synchronize();

			int expected = 136;
			
			Assert.AreEqual(expected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
		}

		[Test]
		public void MakeConditionalOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
		{
			
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => true); });

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16;

            var simpleConditionalFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16 == expected);
            Assert.AreEqual(0, simpleConditionalFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8));
            Assert.AreEqual(expected, simpleConditionalFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

            var withDefaultValueFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16 == expected, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8));
            Assert.AreEqual(expected, withDefaultValueFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

            var withFalseFunc = func.MakeConditional((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16 == expected, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 42);
            Assert.AreEqual(42, withFalseFunc(0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8));
            Assert.AreEqual(expected, withFalseFunc(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
		}

		[Test]
		public void MemoizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
		{
			Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = null;

			Assert.Throws<ArgumentNullException>(() => { func.Memoize(); });

			int count = 0;

			int firstExpected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16;
			int secondExpected = 2 + 4 + 6 + 8 + 10 + 12 + 14 + 16 + 18 + 20 + 22 + 24 + 26 + 28 + 30 + 32;

            func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { count++; return t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16; };
			func = func.Memoize();

			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
			Assert.AreEqual(firstExpected, func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32));
			Assert.AreEqual(secondExpected, func(2, 4, 6, 8, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32));


			Assert.AreEqual(2, count);

		}

		[Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 42;
            var func = originalFunc.FollowedBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_ExtendingNullFunc_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).FollowedBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 7;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void FollowedByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => Maybe<int>.NoValue;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 42;

            var func = left.FollowedBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_WithNullArgument_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 42;
            var func = originalFunc.PrecededBy(null);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_ExtendingNullAction_ReturnsOriginal()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> originalFunc = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 42;
            var func = ((Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>>)null).PrecededBy(originalFunc);

            var result = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.IsTrue(ReferenceEquals(originalFunc, func));
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_CallsFirstFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 42;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => Maybe<int>.NoValue;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.AreEqual(42, results.Value);
        }

        [Test]
        public void PrecededByOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16_CallsSecondFunc()
        {
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> left = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 7;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Maybe<int>> right = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => 42;

            var func = left.PrecededBy(right);

            var results = func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

            Assert.AreEqual(42, results.Value);
        }

			}
}