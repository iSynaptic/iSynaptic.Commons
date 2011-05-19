using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Collections;
using Rhino.Mocks;

using iSynaptic.Commons;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void CopyTo_WithNullSource_ThrowsException()
        {
            IEnumerable<int> source = null;
            int[] destination = new int[1];

            Assert.Throws<ArgumentNullException>(() => source.CopyTo(destination, 0));
        }

        [Test]
        public void CopyTo_WithNullDestination_ThrowsException()
        {
            IEnumerable<int> source = Enumerable.Range(1, 1);
            int[] destination = null;

            Assert.Throws<ArgumentNullException>(() => source.CopyTo(destination, 0));
        }

        [Test]
        public void CopyTo_WithNegativeIndex_ThrowsException()
        {
            IEnumerable<int> source = Enumerable.Range(1, 1);
            int[] destination = new int[1];

            Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(destination, -1));
        }

        [Test]
        public void CopyTo_WithIndexGreaterThanDestinationUpperBound_ThrowsException()
        {
            IEnumerable<int> source = Enumerable.Range(1, 1);
            int[] destination = new int[1];

            Assert.Throws<ArgumentOutOfRangeException>(() => source.CopyTo(destination, 42));
        }

        [Test]
        public void CopyTo_WithIndexToHighGivenSourceAndDestinationSize_ThrowsException()
        {
            IEnumerable<int> source = Enumerable.Range(1, 3);
            int[] destination = new int[3];

            Assert.Throws<ArgumentException>(() => source.CopyTo(destination, 1));
        }

        [Test]
        public void CopyTo_WithValidInput_ReturnsCorrectly()
        {
            IEnumerable<int> source = Enumerable.Range(1, 3);
            int[] destination = new int[3];

            source.CopyTo(destination, 0);

            Assert.IsTrue(destination.SequenceEqual(new[] { 1, 2, 3 }));
        }

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
                Assert.AreEqual(2, enumerator.Current.LookAhead(0).Value);

                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(2, enumerator.Current.Value);
                Assert.AreEqual(3, enumerator.Current.LookAhead(0).Value);
                Assert.AreEqual(5, enumerator.Current.LookAhead(2).Value);
                Assert.AreEqual(4, enumerator.Current.LookAhead(1).Value);

                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(3, enumerator.Current.Value);
                Assert.AreEqual(4, enumerator.Current.LookAhead(0).Value);
            }

            IEnumerable<int> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(() => { nullEnumerable.AsLookAheadable(); });
        }

        [Test]
        public void LookAheadEnumeratorPassesOnReset()
        {
            var enumerable = MockRepository.GenerateStub<IEnumerable<int>>();
            var enumerator = MockRepository.GenerateMock<IEnumerator<int>>();

            enumerable.Stub(x => x.GetEnumerator()).Return(enumerator);
            enumerator.Expect(x => x.Reset());

            var la = enumerable.AsLookAheadable();
            var lar = la.GetEnumerator();

            lar.Reset();

            enumerator.VerifyAllExpectations();
        }

        [Test]
        public void LookAheadEnumerator_WhenUsedAfterBeingDisposed_ThrowsException()
        {
            var lookaheadable = Enumerable.Range(1, 2).AsLookAheadable();

            var enumerator = lookaheadable.GetEnumerator();
            enumerator.MoveNext();

            var value = enumerator.Current;
            enumerator.Dispose();

            Assert.Throws<ObjectDisposedException>(() => { var x = enumerator.Current; });
            Assert.Throws<ObjectDisposedException>(() => enumerator.MoveNext());
            Assert.Throws<ObjectDisposedException>(() => value.LookAhead(0));
            Assert.Throws<ObjectDisposedException>(() => enumerator.Reset());
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
                Assert.AreEqual(2, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(0).Value);

                Assert.IsTrue(nonGenericEnumerator.MoveNext());
                Assert.AreEqual(2, ((LookAheadableValue<int>)nonGenericEnumerator.Current).Value);
                Assert.AreEqual(3, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(0).Value);
                Assert.AreEqual(5, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(2).Value);
                Assert.AreEqual(4, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(1).Value);

                Assert.IsTrue(nonGenericEnumerator.MoveNext());
                Assert.AreEqual(3, ((LookAheadableValue<int>)nonGenericEnumerator.Current).Value);
                Assert.AreEqual(4, ((LookAheadableValue<int>)nonGenericEnumerator.Current).LookAhead(0).Value);
            }
        }

        [Test]
        public void LookAheadWithNullEnumerator()
        {
            MockRepository mocks = new MockRepository();

            IEnumerable<int> enumerable = mocks.StrictMock<IEnumerable<int>>();
            Expect.Call(enumerable.GetEnumerator()).Return(null);

            mocks.ReplayAll();

            var lookAheadable = enumerable.AsLookAheadable();
            Assert.IsNull(lookAheadable.GetEnumerator());
        }

        [Test]
        public void LookAheadToFar_ReturnsNoValue()
        {
            var lookAheadable = Enumerable.Range(1, 1).AsLookAheadable();
            foreach (var i in lookAheadable)
                Assert.AreEqual(Maybe<int>.NoValue, i.LookAhead(0));
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
            Assert.Throws<ArgumentNullException>(() => nullEnumerable.ForceEnumeration());
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
        public void Zip_WithNullIterables_ThrowsArgumentNullException()
        {
            IEnumerable<int> first = null;
            IEnumerable<int> other = Enumerable.Range(1, 10);

            Assert.Throws<ArgumentNullException>(() => first.Zip(other));
            Assert.Throws<ArgumentNullException>(() => other.Zip(null));
        }

        [Test]
        public void Zip()
        {
            var rangeOne = Enumerable.Range(1, 10);
            var rangeTwo = Enumerable.Range(10, 10);

            var zipped = rangeOne.Zip(rangeTwo);

            var expected = new[] { 1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19 };

            Assert.IsTrue(zipped.SelectMany(x => x).Select(x => x.Return()).SequenceEqual(expected));
        }

        [Test]
        public void Zip_ArrayOfEnumerables()
        {
            var array = new[] { Enumerable.Range(1, 10), Enumerable.Range(10, 10) };
            var zipped = array.Zip();

            var expected = new[] { 1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19 };

            Assert.IsTrue(zipped.SelectMany(x => x).Select(x => x.Return()).SequenceEqual(expected));
        }

        [Test]
        public void Zip_EnumerableOfEnumerables()
        {
            var array = new[] { Enumerable.Range(1, 10), Enumerable.Range(10, 10) };
            var zipped = array.AsEnumerable().Zip();

            var expected = new[] { 1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19 };

            Assert.IsTrue(zipped.SelectMany(x => x).Select(x => x.Return()).SequenceEqual(expected));
        }

        [Test]
        public void Zip_WithDifferentItemCounts()
        {
            var left = Enumerable.Range(1, 4);
            var right = Enumerable.Range(1, 3);

            var zipped = left.Zip(right).SelectMany(x => x);

            Assert.IsTrue(zipped.SequenceEqual(new[] { 1, 1, 2, 2, 3, 3, 4, Maybe<int>.NoValue }));
        }

        [Test]
        public void AllSatisfy()
        {
            Func<int, bool> isEven = x => x % 2 == 0;
            var spec = isEven.ToSpecification();

            var numbers = new[] { 2, 4, 6, 8, 10 };
            Assert.IsTrue(numbers.AllSatisfy(spec));
        }

        [Test]
        public void ToDictionary_WithNull_ThrowsArgumentNullException()
        {
            IEnumerable<KeyValuePair<string, string>> pairs = null;
            Assert.Throws<ArgumentNullException>(() => pairs.ToDictionary());
        }

        [Test]
        public void Batch_WithBalancedBatches_ReturnsAllItems()
        {
            var batches = Enumerable.Range(0, 9).Batch(3).ToArray();

            Assert.AreEqual(3, batches.Length);

            Assert.AreEqual(0, batches[0].Index);
            Assert.AreEqual(1, batches[1].Index);
            Assert.AreEqual(2, batches[2].Index);

            Assert.AreEqual(3, batches[0].Size);
            Assert.AreEqual(3, batches[1].Size);
            Assert.AreEqual(3, batches[2].Size);

            Assert.IsTrue(batches[0].SequenceEqual(new[] { 0, 1, 2 }));
            Assert.IsTrue(batches[1].SequenceEqual(new[] { 3, 4, 5 }));
            Assert.IsTrue(batches[2].SequenceEqual(new[] { 6, 7, 8 }));
        }

        [Test]
        public void Batch_WithUnbalancedBatches_ReturnsAllItemsAndLastBatchContainsOnlyRemainingItems()
        {
            var batches = Enumerable.Range(0, 10).Batch(3).ToArray();

            Assert.AreEqual(4, batches.Length);

            Assert.AreEqual(0, batches[0].Index);
            Assert.AreEqual(1, batches[1].Index);
            Assert.AreEqual(2, batches[2].Index);
            Assert.AreEqual(3, batches[3].Index);

            Assert.AreEqual(3, batches[0].Size);
            Assert.AreEqual(3, batches[1].Size);
            Assert.AreEqual(3, batches[2].Size);
            Assert.AreEqual(1, batches[3].Size);

            Assert.IsTrue(batches[0].SequenceEqual(new[] { 0, 1, 2 }));
            Assert.IsTrue(batches[1].SequenceEqual(new[] { 3, 4, 5 }));
            Assert.IsTrue(batches[2].SequenceEqual(new[] { 6, 7, 8 }));
            Assert.IsTrue(batches[3].SequenceEqual(new[] { 9 }));
        }

        [Test]
        public void Batch_ViaNonGenericEnumerator_ReturnsAllItems()
        {
            var batches = Enumerable.Range(0, 9)
                .Batch(3)
                .Cast<IEnumerable>()
                .SelectMany(x => x.OfType<int>().ToArray())
                .ToArray();

            Assert.IsTrue(batches.SequenceEqual(Enumerable.Range(0, 9)));
        }

        [Test]
        public void TestRecursivePopulationThreeLevel()
        {
            var r1 = new Recursive(1,
                        new Recursive(2,
                            new Recursive(4)),
                        new Recursive(3));

            var flatten = r1.Flatten(r => r.Recursives).ToArray();

            Assert.AreEqual(4, flatten.Length);
            Assert.IsTrue(flatten.Select(x => x.Value).SequenceEqual(new[] { 1, 2, 4, 3 }));
        }

        private static IEnumerable<int> GetRange(int start, int end, Action after)
        {
            foreach (int i in Enumerable.Range(start, end))
                yield return i;

            if (after != null)
                after();
        }

        public class Recursive
        {
            public Recursive(int value, params Recursive[] recursives)
            {
                Value = value;
                Recursives = recursives;
            }

            public int Value { get; private set; }
            public Recursive[] Recursives { get; private set; }
        }
    }
}
