

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
		public void SynchronizeOfT1_PreventsConcurrentAccess()
		{
			int count = 0;
            Func<int, int> func = (t1) => { count++; return count; };
			func = func.Synchronize((t1) => true);

            var random = new Random(DateTime.UtcNow.Second);
            int start = random.Next(10, 30);
            int end = random.Next(50, 100);

            Parallel.For(start, end, x => func(1));

            Assert.AreEqual(end - start, count);
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
		public void SynchronizeOfT1T2_PreventsConcurrentAccess()
		{
			int count = 0;
            Func<int, int, int> func = (t1, t2) => { count++; return count; };
			func = func.Synchronize((t1, t2) => true);

            var random = new Random(DateTime.UtcNow.Second);
            int start = random.Next(10, 30);
            int end = random.Next(50, 100);

            Parallel.For(start, end, x => func(1, 2));

            Assert.AreEqual(end - start, count);
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
		public void SynchronizeOfT1T2T3_PreventsConcurrentAccess()
		{
			int count = 0;
            Func<int, int, int, int> func = (t1, t2, t3) => { count++; return count; };
			func = func.Synchronize((t1, t2, t3) => true);

            var random = new Random(DateTime.UtcNow.Second);
            int start = random.Next(10, 30);
            int end = random.Next(50, 100);

            Parallel.For(start, end, x => func(1, 2, 3));

            Assert.AreEqual(end - start, count);
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
		public void SynchronizeOfT1T2T3T4_PreventsConcurrentAccess()
		{
			int count = 0;
            Func<int, int, int, int, int> func = (t1, t2, t3, t4) => { count++; return count; };
			func = func.Synchronize((t1, t2, t3, t4) => true);

            var random = new Random(DateTime.UtcNow.Second);
            int start = random.Next(10, 30);
            int end = random.Next(50, 100);

            Parallel.For(start, end, x => func(1, 2, 3, 4));

            Assert.AreEqual(end - start, count);
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
		public void SynchronizeOfT1T2T3T4T5_PreventsConcurrentAccess()
		{
			int count = 0;
            Func<int, int, int, int, int, int> func = (t1, t2, t3, t4, t5) => { count++; return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5) => true);

            var random = new Random(DateTime.UtcNow.Second);
            int start = random.Next(10, 30);
            int end = random.Next(50, 100);

            Parallel.For(start, end, x => func(1, 2, 3, 4, 5));

            Assert.AreEqual(end - start, count);
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
		public void SynchronizeOfT1T2T3T4T5T6_PreventsConcurrentAccess()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6) => { count++; return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6) => true);

            var random = new Random(DateTime.UtcNow.Second);
            int start = random.Next(10, 30);
            int end = random.Next(50, 100);

            Parallel.For(start, end, x => func(1, 2, 3, 4, 5, 6));

            Assert.AreEqual(end - start, count);
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
		public void SynchronizeOfT1T2T3T4T5T6T7_PreventsConcurrentAccess()
		{
			int count = 0;
            Func<int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7) => { count++; return count; };
			func = func.Synchronize((t1, t2, t3, t4, t5, t6, t7) => true);

            var random = new Random(DateTime.UtcNow.Second);
            int start = random.Next(10, 30);
            int end = random.Next(50, 100);

            Parallel.For(start, end, x => func(1, 2, 3, 4, 5, 6, 7));

            Assert.AreEqual(end - start, count);
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

			}
}