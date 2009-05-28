using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using iSynaptic.Commons.Extensions;
using System.Linq;
using System.Collections;
using Rhino.Mocks;

namespace iSynaptic.Commons.UnitTests.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTests : BaseTestFixture
    {
        [Test]
        public void WithIndex()
        {
            int[] items = { 1, 3, 5, 7, 9 };

            int index = 0;
            foreach (var item in items.WithIndex())
            {
                Assert.AreEqual(index, item.Index);
                Assert.AreEqual(items[index], item.Value);

                index++;
            }
        }

        [Test]
        public void LookAheadEnumerable()
        {
            int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            using (var enumerator = items.AsLookAheadable().GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(1, enumerator.Current.Value);
                Assert.AreEqual(2, enumerator.Current.LookAhead(0));

                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(2, enumerator.Current.Value);
                Assert.AreEqual(3, enumerator.Current.LookAhead(0));
                Assert.AreEqual(5, enumerator.Current.LookAhead(2));
                Assert.AreEqual(4, enumerator.Current.LookAhead(1));

                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(3, enumerator.Current.Value);
                Assert.AreEqual(4, enumerator.Current.LookAhead(0));
            }

            IEnumerable<int> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(() => { nullEnumerable.AsLookAheadable(); });
        }

        [Test]
        public void LookAheadEnumerableViaNonGenericGetEnumerator()
        {
            int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var enumerable = items.AsLookAheadable();
            IEnumerator nonGenericEnumerator = null;

            using ((IDisposable)(nonGenericEnumerator = ((IEnumerable)enumerable).GetEnumerator()))
            {
                Assert.IsTrue(nonGenericEnumerator.MoveNext());
                Assert.AreEqual(1, ((LookAheadableValue<int>)nonGenericEnumerator.Current).Value);
                Assert.AreEqual(2, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(0));

                Assert.IsTrue(nonGenericEnumerator.MoveNext());
                Assert.AreEqual(2, ((LookAheadableValue<int>)nonGenericEnumerator.Current).Value);
                Assert.AreEqual(3, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(0));
                Assert.AreEqual(5, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(2));
                Assert.AreEqual(4, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(1));

                Assert.IsTrue(nonGenericEnumerator.MoveNext());
                Assert.AreEqual(3, ((LookAheadableValue<int>)nonGenericEnumerator.Current).Value);
                Assert.AreEqual(4, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(0));
            }
        }

        [Test]
        public void LookAheadWithNullEnumerator()
        {
            MockRepository mocks = new MockRepository();

            IEnumerable<int> enumerable = mocks.CreateMock<IEnumerable<int>>();
            Expect.Call(enumerable.GetEnumerator()).Return(null);

            mocks.ReplayAll();

            var lookAheadable = enumerable.AsLookAheadable();
            Assert.IsNull(lookAheadable.GetEnumerator());
        }

        [Test]
        public void Buffer()
        {
            bool enumerationComplete = false;
            IEnumerable<int> numbers = GetRange(1, 10, () => { enumerationComplete = true; });

            Assert.IsFalse(enumerationComplete);

            numbers = numbers.Buffer();
            Assert.IsTrue(enumerationComplete);

            Assert.IsTrue(numbers.SequenceEqual(Enumerable.Range(1, 10)));

            IEnumerable<int> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(() => { nullEnumerable.Buffer(); });
        }

        [Test]
        public void ForceEnumeration()
        {
            bool enumerationComplete = false;
            IEnumerable<int> numbers = GetRange(1, 10, () => { enumerationComplete = true; });

            Assert.IsFalse(enumerationComplete);

            numbers.ForceEnumeration();
            Assert.IsTrue(enumerationComplete);

            IEnumerable<int> nullEnumerable = null;
            nullEnumerable.ForceEnumeration();
        }

        [Test]
        public void Delimit()
        {
            IEnumerable<int> range = Enumerable.Range(1, 9);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9", range.Delimit(", "));

            IEnumerable<int> nullEnumerable = null;

            Assert.Throws<ArgumentNullException>(() => { nullEnumerable.Delimit(""); });
            Assert.Throws<ArgumentNullException>(() => { range.Delimit(null); });
            Assert.Throws<ArgumentNullException>(() => { range.Delimit("", null); });
        }

        [Test]
        public void TrueForAll()
        {
            IEnumerable<int> range = Enumerable.Range(1, 9);

            Assert.IsTrue(range.TrueForAll(i => i < 10));
            Assert.IsFalse(range.TrueForAll(i => i < 5));

            IEnumerable<int> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(() => { nullEnumerable.TrueForAll(i => i < 10); });
            Assert.Throws<ArgumentNullException>(() => { range.TrueForAll(null); });
        }

        [Test]
        public void MakeConditional()
        {
            Func<int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(x => x < 3); });

            func = x => x;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

            var simpleConditionalFunc = func.MakeConditional(x => x > 5);
            Assert.AreEqual(0, simpleConditionalFunc(1));
            Assert.AreEqual(6, simpleConditionalFunc(6));

            var withDefaultValueFunc = func.MakeConditional(x => x > 5, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(1));
            Assert.AreEqual(6, withDefaultValueFunc(6));

            var withFalseFunc = func.MakeConditional(x => x > 5, x => x * 2);
            Assert.AreEqual(2, withFalseFunc(1));
            Assert.AreEqual(6, withFalseFunc(6));
        }

        [Test]
        public void Pipeline()
        {
            var multiplyBy5 = ((Func<int, IEnumerable<int>, IEnumerable<int>>)Multiply).Curry(5);

            List<int> items = new List<int>();

            bool enumerationComplete = false;
            IEnumerable<int> numbers = GetRange(1, 9, () => { enumerationComplete = true; });

            var pipeline = numbers
                .Pipeline(i => i * 2)
                .Pipeline(i => items.Add(i))
                .Pipeline(multiplyBy5);

            Assert.IsFalse(enumerationComplete);

            Assert.IsTrue(pipeline.SequenceEqual(new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 }));
            Assert.IsTrue(enumerationComplete);

            Assert.IsTrue(items.SequenceEqual(new int[] { 2, 4, 6, 8, 10, 12, 14, 16, 18 }));

            items.Clear();
            enumerationComplete = false;

            Assert.IsFalse(enumerationComplete);

            Assert.IsTrue(((IEnumerable)pipeline).OfType<int>().SequenceEqual(new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90 }));
            Assert.IsTrue(enumerationComplete);

            Assert.IsTrue(items.SequenceEqual(new int[] { 2, 4, 6, 8, 10, 12, 14, 16, 18 }));


            IEnumerable<int> nullEnumerable = null;

            nullEnumerable.Pipeline(i => i * 2);
            Assert.Throws<ArgumentNullException>(() => { items.Pipeline((Func<int, int>)null); });

            nullEnumerable.Pipeline(multiplyBy5);
            Assert.Throws<ArgumentNullException>(() => { items.Pipeline((Func<IEnumerable<int>, IEnumerable<int>>)null); });

            nullEnumerable.Pipeline(i => Console.WriteLine(i));
            Assert.Throws<ArgumentNullException>(() => { Enumerable.Range(1, 10).Pipeline((Action<int>)null); });

            Assert.Throws<ArgumentNullException>(() => { items.Pipeline((PipelineAction<int>)null); });
        }

        private IEnumerable<int> Multiply(int multiplier, IEnumerable<int> source)
        {
            foreach (int i in source)
                yield return i * multiplier;
        }

        private IEnumerable<int> GetRange(int start, int end, Action after)
        {
            foreach (int i in Enumerable.Range(start, end))
                yield return i;

            if (after != null)
                after();
        }
    }
}
