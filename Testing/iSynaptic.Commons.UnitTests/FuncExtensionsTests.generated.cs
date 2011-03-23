
using System;
using System.Collections.Generic;
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
		public void SynchronizeOfT1()
		{
			int count = 0;
            Func<int, int> func = (t1) => { count++; Thread.Sleep(150); return count-- + t1;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1));
            var task2 = Task.Factory.StartNew(() => func(1));

			int expected = 1 + 1;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2()
		{
			int count = 0;
            Func<int, int, int> func = (t1, t2) => { count++; Thread.Sleep(150); return count-- + t1 + t2;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2));
            var task2 = Task.Factory.StartNew(() => func(1, 2));

			int expected = 1 + 1 + 2;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3()
		{
			int count = 0;
            Func<int, int, int, int> func = (t1, t2, t3) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3));

			int expected = 1 + 1 + 2 + 3;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4()
		{
			int count = 0;
            Func<int, int, int, int, int> func = (t1, t2, t3, t4) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4));

			int expected = 1 + 1 + 2 + 3 + 4;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5()
		{
			int count = 0;
            Func<int, int, int, int, int, int> func = (t1, t2, t3, t4, t5) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5));

			int expected = 1 + 1 + 2 + 3 + 4 + 5;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7T8()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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
		public void SynchronizeOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { count++; Thread.Sleep(150); return count-- + t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16;};
			func = func.Synchronize();

            var task1 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));
            var task2 = Task.Factory.StartNew(() => func(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16));

			int expected = 1 + 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16;

			Assert.AreEqual(expected, task1.Result);
			Assert.AreEqual(expected, task2.Result);
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

			}
}