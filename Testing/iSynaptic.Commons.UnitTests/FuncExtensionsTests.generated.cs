
using System;
using System.Collections.Generic;
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
        public void ToActionOfT1T2T3T4T5T6T7T8T9T10T11T12T13T14T15T16()
        {
            int val = 0;

            Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int> func = (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => { val = t1 + t2 + t3 + t4 + t5 + t6 + t7 + t8 + t9 + t10 + t11 + t12 + t13 + t14 + t15 + t16; return val; };
            var action = func.ToAction();

            action(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);

			int expected = 1 + 2 + 3 + 4 + 5 + 6 + 7 + 8 + 9 + 10 + 11 + 12 + 13 + 14 + 15 + 16;
            Assert.AreEqual(expected, val);
        }

			}
}