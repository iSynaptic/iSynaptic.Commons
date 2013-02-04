// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using iSynaptic.Commons.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using System.Collections;
using Rhino.Mocks;

namespace iSynaptic.Commons.Linq
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

            Assert.Throws<ArgumentException>(() => source.CopyTo(destination, 42));
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
        public void Run()
        {
            bool enumerationComplete = false;
            IEnumerable<int> numbers = GetRange(1, 10, () => { enumerationComplete = true; });

            Assert.IsFalse(enumerationComplete);

            numbers.Run();
            Assert.IsTrue(enumerationComplete);

            IEnumerable<int> nullEnumerable = null;
            Assert.Throws<ArgumentNullException>(() => nullEnumerable.Run());
        }

        [Test]
        public void Delimit()
        {
            IEnumerable<int> range = Enumerable.Range(1, 9);
            Assert.AreEqual("1, 2, 3, 4, 5, 6, 7, 8, 9", range.Delimit(", "));

            IEnumerable<int> nullEnumerable = null;

            Assert.Throws<ArgumentNullException>(() => nullEnumerable.Delimit(""));
            Assert.Throws<ArgumentNullException>(() => range.Delimit(null));
            Assert.Throws<ArgumentNullException>(() => range.Delimit("", (string)null));
            Assert.Throws<ArgumentNullException>(() => range.Delimit("", (Func<int, string>)null));
        }

        [Test]
        public void ZipAll_WithNullIterables_ThrowsArgumentNullException()
        {
            IEnumerable<int> first = null;
            IEnumerable<int> other = Enumerable.Range(1, 10);

            Assert.Throws<ArgumentNullException>(() => first.ZipAll(other));
            Assert.Throws<ArgumentNullException>(() => other.ZipAll(null));
        }

        [Test]
        public void ZipAll()
        {
            var rangeOne = Enumerable.Range(1, 10);
            var rangeTwo = Enumerable.Range(10, 10);

            var zipped = rangeOne.ZipAll(rangeTwo);

            var expected = new[] { 1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19 };

            Assert.IsTrue(zipped.SelectMany(x => x).Select(x => x.ValueOrDefault()).SequenceEqual(expected));
        }

        [Test]
        public void ZipAll_ArrayOfEnumerables()
        {
            var array = new[] { Enumerable.Range(1, 10), Enumerable.Range(10, 10) };
            var zipped = array.ZipAll();

            var expected = new[] { 1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19 };

            Assert.IsTrue(zipped.SelectMany(x => x).Select(x => x.ValueOrDefault()).SequenceEqual(expected));
        }

        [Test]
        public void ZipAll_EnumerableOfEnumerables()
        {
            var array = new[] { Enumerable.Range(1, 10), Enumerable.Range(10, 10) };
            var zipped = array.AsEnumerable().ZipAll();

            var expected = new[] { 1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19 };

            Assert.IsTrue(zipped.SelectMany(x => x).Select(x => x.ValueOrDefault()).SequenceEqual(expected));
        }

        [Test]
        public void ZipAll_WithDifferentItemCounts()
        {
            var left = Enumerable.Range(1, 4);
            var right = Enumerable.Range(1, 3);

            var zipped = left.ZipAll(right).SelectMany(x => x);

            Assert.IsTrue(zipped.SequenceEqual(new[] { 1, 1, 2, 2, 3, 3, 4 }.Select(x => x.ToMaybe()).Concat(new[] { Maybe<int>.NoValue })));
        }

        [Test]
        public void ZipAll_WithSelector()
        {
            var rangeOne = Enumerable.Range(1, 10);
            var rangeTwo = Enumerable.Range(10, 10);

            var zipped = rangeOne.ZipAll(rangeTwo, (l, r) => new[]{l.Value, r.Value});

            var expected = new[] { 1, 10, 2, 11, 3, 12, 4, 13, 5, 14, 6, 15, 7, 16, 8, 17, 9, 18, 10, 19 };

            Assert.IsTrue(zipped.SelectMany(x => x).Select(x => x).SequenceEqual(expected));

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
        public void Recurse()
        {
            var r1 = new Recursive(1,
                        new Recursive(2,
                            new Recursive(4)),
                        new Recursive(3));

            var recursives = r1.Recurse(r => r.Recursives).ToArray();

            Assert.IsTrue(recursives.Select(x => x.Value).SequenceEqual(new[] { 1, 2, 4, 3 }));
        }

        [Test]
        public void RecurseWhile()
        {
            var r1 = new Recursive(1,
                        new Recursive(2,
                            new Recursive(4),
                            new Recursive(5)),
                        new Recursive(3,
                            new Recursive(6),
                            new Recursive(7)));

            var recursives = r1.RecurseWhile(r => r.Recursives, r => r.Value % 2 == 1).ToArray();
            Assert.IsTrue(recursives.Select(x => x.Value).SequenceEqual(new[] { 1, 3, 7 }));
        }

        [Test]
        public void Distinct_WithSelector()
        {
            var items = new[]
                            {
                                new TestSubject { Number =1, Text = "Foo"},
                                new TestSubject { Number =1, Text = "Bar"},
                                new TestSubject { Number =2, Text = "Foo"},
                                new TestSubject { Number =3, Text = "Baz"}
                            };

            Assert.IsTrue(items.Distinct(x => x.Number).Select(x => x.Text).SequenceEqual(new[] { "Foo", "Foo", "Baz" }));
            Assert.IsTrue(items.Distinct(x => x.Text).Select(x => x.Text).SequenceEqual(new[] { "Foo", "Bar", "Baz" }));
        }

        [Test]
        public void Or_PicksFirstStream_IfItHasItems()
        {
            var first = new List<int> { 1, 2, 3, 4, 5 };
            var second = new List<int> { 6, 7, 8, 9, 10 };

            var results = first.Or(second);

            Assert.IsTrue(results.SequenceEqual(new[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void Or_PicksSecondStream_IfFirstHasNoItems()
        {
            var first = new List<int>();
            var second = new List<int> { 6, 7, 8, 9, 10 };

            var results = first.Or(second);

            Assert.IsTrue(results.SequenceEqual(new[] { 6,7,8,9,10 }));
        }

        [Test]
        public void TryFirst_WithNullSource_ThrowsException()
        {
            IEnumerable<int> source = null;

            Assert.Throws<ArgumentNullException>(() => source.TryFirst());
            Assert.Throws<ArgumentNullException>(() => source.TryFirst(x => true));
        }

        [Test]
        public void TryFirst_WithNullPredicate_ThrowsException()
        {
            IEnumerable<int> source = new int[0];
            Assert.Throws<ArgumentNullException>(() => source.TryFirst(null));
        }

        [Test]
        public void TryFirst_WithEmptySource_ReturnsNoValue()
        {
            IEnumerable<int> source = new int[0];

            var result = source.TryFirst();
            Assert.IsFalse(result.HasValue);

            result = source.TryFirst(x => true);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void TryFirst_WithOneItem_ReturnsItem()
        {
            IEnumerable<int> source = new []{42};

            var result = source.TryFirst();
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(42, result.Value);

            result = source.TryFirst(x => true);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void TryFirst_WithManyItems_ReturnsFirstItem()
        {
            IEnumerable<int> source = new[] { 42, 84, 168 };

            var result = source.TryFirst();
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(42, result.Value);

            result = source.TryFirst(x => true);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void TryFirst_WithFirstItemNull_ReturnsNoValue()
        {
            IEnumerable<string> source = new[] { null, "Foo", "Baz" };

            var result = source.TryFirst();
            Assert.IsFalse(result.HasValue);

            result = source.TryFirst(x => true);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void TryFirst_WherePredicateMatchesItem_ReturnsFirstMatchingItem()
        {
            IEnumerable<string> source = new[] { "Foo", "Baz", "Boo", "Quix" };

            var result = source.TryFirst(x => x.StartsWith("B"));
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual("Baz", result.Value);
        }

        [Test]
        public void TryFirst_WherePredicateMatchesNoItems_ReturnsNoValue()
        {
            IEnumerable<string> source = new[] { "Foo", "Baz", "Boo", "Quix" };

            var result = source.TryFirst(x => false);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void TryLast_WithNullSource_ThrowsException()
        {
            IEnumerable<int> source = null;

            Assert.Throws<ArgumentNullException>(() => source.TryLast());
            Assert.Throws<ArgumentNullException>(() => source.TryLast(x => true));
        }

        [Test]
        public void TryLast_WithNullPredicate_ThrowsException()
        {
            IEnumerable<int> source = new int[0];
            Assert.Throws<ArgumentNullException>(() => source.TryLast(null));
        }

        [Test]
        public void TryLast_WithEmptySource_ReturnsNoValue()
        {
            IEnumerable<int> source = new int[0];

            var result = source.TryLast();
            Assert.IsFalse(result.HasValue);

            result = source.TryLast(x => true);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void TryLast_WithOneItem_ReturnsItem()
        {
            IEnumerable<int> source = new[] { 42 };

            var result = source.TryLast();
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(42, result.Value);

            result = source.TryLast(x => true);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void TryLast_WithManyItems_ReturnsLastItem()
        {
            IEnumerable<int> source = new[] { 42, 84, 168 };

            var result = source.TryLast();
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(168, result.Value);

            result = source.TryLast(x => true);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(168, result.Value);
        }

        [Test]
        public void TryLast_WithLastItemNull_ReturnsNoValue()
        {
            IEnumerable<string> source = new[] { "Foo", "Baz", null };

            var result = source.TryLast();
            Assert.IsFalse(result.HasValue);

            result = source.TryLast(x => true);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void TryLast_WherePredicateMatchesItem_ReturnsLastMatchingItem()
        {
            IEnumerable<string> source = new[] { "Foo", "Baz", "Boo", "Quix" };

            var result = source.TryLast(x => x.StartsWith("B"));
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual("Boo", result.Value);
        }

        [Test]
        public void TryLast_WherePredicateMatchesNoItems_ReturnsNoValue()
        {
            IEnumerable<string> source = new[] { "Foo", "Baz", "Boo", "Quix" };

            var result = source.TryLast(x => false);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void TrySingle_WithNullSource_ThrowsException()
        {
            IEnumerable<int> source = null;

            Assert.Throws<ArgumentNullException>(() => source.TrySingle());
            Assert.Throws<ArgumentNullException>(() => source.TrySingle(x => true));
        }

        [Test]
        public void TrySingle_WithNullPredicate_ThrowsException()
        {
            IEnumerable<int> source = new int[0];
            Assert.Throws<ArgumentNullException>(() => source.TrySingle(null));
        }

        [Test]
        public void TrySingle_WithEmptySource_ReturnsNoValue()
        {
            IEnumerable<int> source = new int[0];

            var result = source.TrySingle();
            Assert.IsFalse(result.HasValue);

            result = source.TrySingle(x => true);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void TrySingle_WithOneItem_ReturnsItem()
        {
            IEnumerable<int> source = new[] { 42 };

            var result = source.TrySingle();
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(42, result.Value);

            result = source.TrySingle(x => true);
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void TrySingle_WithManyItems_ThrowsExceptionImmediately()
        {
            IEnumerable<int> source = new[] { 42, 84, 168 };

            var results = source.TrySingle();
            Assert.Throws<InvalidOperationException>(() => results.Run());

            results = source.TrySingle(x => true);
            Assert.Throws<InvalidOperationException>(() => results.Run());
        }

        [Test]
        public void TrySingle_WithOnlyItemNull_ReturnsNoValue()
        {
            IEnumerable<string> source = new[] { (string)null };

            var result = source.TrySingle();
            Assert.IsFalse(result.HasValue);

            result = source.TrySingle(x => true);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void TrySingle_WherePredicateMatchesOneItem_ReturnsMatchingItem()
        {
            IEnumerable<string> source = new[] { "Foo", "Baz", "Boo", "Quix" };

            var result = source.TrySingle(x => x.StartsWith("Bo"));
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual("Boo", result.Value);
        }

        [Test]
        public void TrySingle_WherePredicateMatchesMultipleItems_ThrowsException()
        {
            IEnumerable<string> source = new[] { "Foo", "Baz", "Boo", "Quix" };
            var results = source.TrySingle(x => x.StartsWith("B"));

            Assert.Throws<InvalidOperationException>(() => results.Run());
        }

        [Test]
        public void TrySingle_WherePredicateMatchesNoItems_ReturnsNoValue()
        {
            IEnumerable<string> source = new[] { "Foo", "Baz", "Boo", "Quix" };

            var result = source.TrySingle(x => false);
            Assert.IsFalse(result.HasValue);
        }

        [Test]
        public void Any_WithEnumerableBooleans_ReturnsTrueForAny()
        {
            Assert.IsFalse(new[]{ false, false, false }.Any());
            Assert.IsTrue(new[] { false, true, false }.Any());
            Assert.IsTrue(new[] { true, true, true }.Any());
        }


        [Test]
        public void AllTrue_WithEnumerableBooleans_ReturnsTrueForAll()
        {
            Assert.IsFalse(new[] { false, false, false }.AllTrue());
            Assert.IsFalse(new[] { false, true, false }.AllTrue());
            Assert.IsTrue(new[] { true, true, true }.AllTrue());
        }

        [Test]
        public void AllFalse_WithEnumerableBooleans_ReturnsTrueForNone()
        {
            Assert.IsTrue(new[] { false, false, false }.AllFalse());
            Assert.IsFalse(new[] { false, true, false }.AllFalse());
            Assert.IsFalse(new[] { true, true, true }.AllFalse());
        }

        [Test]
        public void None_WithPredicate_ReturnsTrueForNone()
        {
            Assert.IsTrue(new[] { "false", "false", "false" }.None(bool.Parse));
            Assert.IsFalse(new[] { "false", "true", "false" }.None(bool.Parse));
            Assert.IsFalse(new[] { "true", "true", "true" }.None(bool.Parse));
        }

        [Test]
        public void None_WithNoPredicate_ReturnsTrueForEmptyEnumerables()
        {
            Assert.IsTrue(new String[]{}.None());
            Assert.IsFalse(new [] { "Hello, World!" }.None());
        }

        [Test]
        public void SelectMaybe_OnlyYieldsValuesWhereMaybeHasValue()
        {
            Assert.IsTrue(Enumerable.Range(1, 10)
                .SelectMaybe(x => x.ToMaybe().Where(y => y % 2 == 0))
                .SequenceEqual(new[]{2,4,6,8,10}));
        }

        [Test]
        public void SelectMaybe_WithIndexedSelector_OnlyYieldsValuesWhereMaybeHasValue()
        {
            Assert.IsTrue(Enumerable.Range(1, 10)
                .SelectMaybe((i, x) => x.ToMaybe().Where(y => i % 2 == 0))
                .SequenceEqual(new[] { 1, 3, 5, 7, 9 }));
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

        public class TestSubject
        {
            public int Number { get; set; }
            public string Text { get; set; }
        }
    }
}
