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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using iSynaptic.Commons.Collections;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> SelectMaybe<T, TResult>(this IEnumerable<T> @this, Func<T, Maybe<TResult>> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");

            return @this.Select(selector)
                        .Where(x => x.HasValue)
                        .Select(x => x.Value);
        }

        public static IEnumerable<TResult> SelectMaybe<T, TResult>(this IEnumerable<T> @this, Func<T, int, Maybe<TResult>> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");

            return @this.Select(selector)
                        .Where(x => x.HasValue)
                        .Select(x => x.Value);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> SelectMany<T, TResult>(this IEnumerable<T> @this, Func<T, Maybe<TResult>> selector)
        {
            return @this.SelectMaybe(selector);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> SelectMany<T, TResult>(this IEnumerable<T> @this, Func<T, int, Maybe<TResult>> selector)
        {
            return @this.SelectMaybe(selector);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static IEnumerable<TResult> SelectMany<T, TIntermediate, TResult>(this IEnumerable<T> @this, Func<T, Maybe<TIntermediate>> selector, Func<T, TIntermediate, TResult> combiner)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");
            Guard.NotNull(combiner, "combiner");

            return @this.Select(x => selector(x).Select(y => combiner(x, y))).Squash();
        }

        public static bool None<T>(this IEnumerable<T> @this)
        {
            return !Guard.NotNull(@this, "this").Any();
        }

        public static bool None<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return !Guard.NotNull(@this, "this").Any(predicate);
        }

        public static bool AllTrue(this IEnumerable<bool> @this)
        {
            return Guard.NotNull(@this, "this").All(x => x);
        }

        public static bool AllFalse(this IEnumerable<bool> @this)
        {
            return Guard.NotNull(@this, "this").All(x => !x);
        }

        public static bool Any(this IEnumerable<bool> @this)
        {
            return Guard.NotNull(@this, "this").Any(x => x);
        }

        public static IEnumerable<TResult> Let<T, TResult>(this IEnumerable<T> @this, Func<IEnumerable<T>, IEnumerable<TResult>> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");

            return LetCore(@this, selector);
        }

        private static IEnumerable<TResult> LetCore<T, TResult>(IEnumerable<T> @this, Func<IEnumerable<T>, IEnumerable<TResult>> selector)
        {
            foreach (var item in selector(@this))
                yield return item;
        }

        public static IEnumerable<T> Unless<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(predicate, "predicate");

            return @this.Where(x => !predicate(x));
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> @this)
            where T : class
        {
            Guard.NotNull(@this, "this");
            return @this.Where(x => x != null);
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> @this)
            where T : struct
        {
            Guard.NotNull(@this, "this");
            return @this.Where(x => x.HasValue).Select(x => x.Value);
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> @this, Func<TSource, TSource, int> comparer)
        {
            return OrderBy(@this, x => x, comparer);
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> @this, Func<TSource, TKey> keySelector, Func<TKey, TKey, int> comparer)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(keySelector, "keySelector");
            Guard.NotNull(comparer, "comparer");

            return @this.OrderBy(keySelector, comparer.ToComparer());
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> @this, Func<TSource, TSource, int> comparer)
        {
            return OrderByDescending(@this, x => x, comparer);
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> @this, Func<TSource, TKey> keySelector, Func<TKey, TKey, int> comparer)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(keySelector, "keySelector");
            Guard.NotNull(comparer, "comparer");

            return @this.OrderByDescending(keySelector, comparer.ToComparer());
        }

        public static IOrderedEnumerable<TSource> OrderByPriorities<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> higherPrioritySelector, params Func<TSource, bool>[] additionalPrioritySelectors)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(higherPrioritySelector, "higherPrioritySelector");

            return @this.OrderByPriorities((l, r) => higherPrioritySelector(l), additionalPrioritySelectors != null
                                                                                    ? additionalPrioritySelectors.Select(s => (Func<TSource, TSource, bool>)((l, r) => s(l))).ToArray()
                                                                                    : null);
        }

        public static IOrderedEnumerable<TSource> OrderByPriorities<TSource>(this IEnumerable<TSource> @this, Func<TSource, TSource, bool> higherPrioritySelector, params Func<TSource, TSource, bool>[] additionalPrioritySelectors)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(higherPrioritySelector, "higherPrioritySelector");

            IEnumerable<Func<TSource, TSource, bool>> selectors = new[] { higherPrioritySelector };

            if (additionalPrioritySelectors != null)
                selectors = selectors.Concat(additionalPrioritySelectors);

            return @this.OrderBy(x => x, (l, r) => (from selector in selectors
                                                     let leftHasPriority = selector(l, r)
                                                     let rightHasPriority = selector(r, l)
                                                     where leftHasPriority ^ rightHasPriority
                                                     select leftHasPriority ? -1 : 1).FirstOrDefault());
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> @this, Func<T, TKey> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");

            return @this.Distinct(selector.ToEqualityComparer());
        }

        public static void CopyTo<T>(this IEnumerable<T> @this, T[] array, int index)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(array, "array");

            var buffer = new List<T>();
            
            foreach (var item in @this)
                buffer.Add(item);

            buffer.CopyTo(array, index);
        }

        public static IEnumerable<IndexedValue<T>> WithIndex<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "this");

            int index = 0;
            return @this.Select(x => new IndexedValue<T>(index++, x));
        }

        public static IEnumerable<LookAheadableValue<T>> AsLookAheadable<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "this");
            return new LookAheadEnumerable<T>(@this);
        }

        public static IEnumerable<T> Buffer<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "this");
            return @this.ToArray();
        }

        public static IEnumerable<Batch<T>> Batch<T>(this IEnumerable<T> @this, int size)
        {
            Guard.NotNull(@this, "this");

            if (size < 0) throw new ArgumentOutOfRangeException("size", "Size must not be negative.");

            return BatchCore(@this, (x, i, info) => info.Count < size, x => x);
        }

        public static IEnumerable<Batch<T>> Batch<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(predicate, "predicate");

            return BatchCore(@this, (x, i, info) => predicate(x), x => x);
        }

        public static IEnumerable<Batch<T>> Batch<T>(this IEnumerable<T> @this, Func<T, int, bool> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(predicate, "predicate");

            return BatchCore(@this, (x, i, info) => predicate(x, i), x => x);
        }

        public static IEnumerable<Batch<T>> Batch<T>(this IEnumerable<T> @this, Func<T, BatchInfo, bool> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(predicate, "predicate");

            return BatchCore(@this, (x, i, info) => predicate(x, info), x => x);
        }

        public static IEnumerable<Batch<T>> Batch<T>(this IEnumerable<T> @this, Func<T, int, BatchInfo, bool> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(predicate, "predicate");

            return BatchCore(@this, predicate, x => x);
        }

        public static IEnumerable<Batch<TResult>> Batch<T, TResult>(this IEnumerable<T> @this, Func<T, int, BatchInfo, bool> predicate, Func<T, TResult> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(selector, "selector");

            return BatchCore(@this, predicate, selector);
        }

        private static IEnumerable<Batch<TResult>> BatchCore<T, TResult>(IEnumerable<T> @this, Func<T, int, BatchInfo, bool> predicate, Func<T, TResult> selector)
        {
            using (var enumerator = @this.GetEnumerator())
            {
                int index = -1;

                int itemIndex = 0;

                int batchIndex = 0;
                var buffer = new List<T>();

                while (enumerator.MoveNext())
                {
                    index++;

                    if (!predicate(enumerator.Current, index, new BatchInfo(batchIndex, buffer.Count)) && buffer.Count > 0)
                    {
                        yield return new Batch<TResult>(buffer.Select(selector).ToArray(), batchIndex, itemIndex);
                        batchIndex++;
                        itemIndex = index;

                        buffer.Clear();
                    }

                    buffer.Add(enumerator.Current);
                }

                if (buffer.Count != 0)
                    yield return new Batch<TResult>(buffer.Select(selector).ToArray(), batchIndex, itemIndex);
            }
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this)
        {
            Guard.NotNull(@this, "this");
            return @this.ToDictionary(x => x.Key, x => x.Value);
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this)
        {
            Guard.NotNull(@this, "this");

            return DictionaryExtensions.ToReadOnlyDictionary(@this.ToDictionary());
        }

        public static string Delimit<T>(this IEnumerable<T> @this, string delimiter)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(delimiter, "delimiter");

            return Delimit(@this, delimiter, x => x.ToString());
        }

        public static string Delimit<T>(this IEnumerable<T> @this, string delimiter, string formatString)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(delimiter, "delimiter");
            Guard.NotNull(formatString, "formatString");

            return @this.Delimit(delimiter, x => string.Format(formatString, x));
        }

        public static string Delimit<T>(this IEnumerable<T> @this, string delimiter, Func<T, string> formatter)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(delimiter, "delimiter");
            Guard.NotNull(formatter, "formatter");

            var builder = new StringBuilder();

            @this.SmartLoop()
                .Between((x, y) => builder.Append(delimiter))
                .Each(x => builder.Append(formatter(x)))
                .Execute();

            return builder.ToString();
        }

        public static IEnumerable<TResult> ZipAll<TSource, TOther, TResult>(this IEnumerable<TSource> @this, IEnumerable<TOther> other, Func<Maybe<TSource>, Maybe<TOther>, TResult> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(other, "other");
            Guard.NotNull(selector, "selector");

            return ZipAllCore(@this, other, selector);
        }

        public static IEnumerable<Maybe<T>[]> ZipAll<T>(this IEnumerable<T> @this, params IEnumerable<T>[] sources)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(sources, "sources");

            return ZipAll(new[] { @this }.Concat(sources));
        }

        public static IEnumerable<Maybe<T>[]> ZipAll<T>(this IEnumerable<T>[] @this)
        {
            Guard.NotNull(@this, "this");
            return ZipAll((IEnumerable<IEnumerable<T>>)@this);
        }

        public static IEnumerable<Maybe<T>[]> ZipAll<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            Guard.NotNull(@this, "this");
            return ZipAllCore(@this);
        }

        private static IEnumerable<TResult> ZipAllCore<TSource, TOther, TResult>(this IEnumerable<TSource> @this, IEnumerable<TOther> other, Func<Maybe<TSource>, Maybe<TOther>, TResult> selector)
        {
            using (var left = @this.ToZipableEnumerable().GetEnumerator())
            using (var right = other.ToZipableEnumerable().GetEnumerator())
            {
                while (left.MoveNext() && right.MoveNext())
                {
                    var leftCurrent = left.Current;
                    var rightCurrent = right.Current;

                    if (leftCurrent.HasValue || rightCurrent.HasValue)
                        yield return selector(leftCurrent, rightCurrent);
                    else 
                        yield break;
                }
            }
        }

        private static IEnumerable<Maybe<T>[]> ZipAllCore<T>(IEnumerable<IEnumerable<T>> sources)
        {
            using (var compositeDisposable = new CompositeDisposable())
            {
                var enumerators = sources
                    .Where(x => x != null)
                    .Select(x => x.ToZipableEnumerable())
                    .Select(x => compositeDisposable.Add(x.GetEnumerator()))
                    .ToArray();

                while (true)
                {
                    var items = enumerators
                        .Select(x => { x.MoveNext(); return x.Current; })
                        .ToArray();

                    if (items.Any(x => x.HasValue))
                        yield return items;
                    else
                        yield break;
                }
            }
        }

        public static IEnumerable<Maybe<T>> ToZipableEnumerable<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "this");

            foreach (var item in @this)
                yield return item.ToMaybe();

            while (true)
                yield return Maybe<T>.NoValue;
        }

        public static IEnumerable<T> Run<T>(this IEnumerable<T> @this, Action<T> action = null)
        {
            var source = Guard.NotNull(@this, "this");

            if (action != null)
                source = source.OnValue(action);

            return source.ToArray();
        }

        public static SmartLoop<T> SmartLoop<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "this");
            return new SmartLoop<T>(@this);
        }

        public static IEnumerable<T> Recurse<T>(this T @this, Func<T, T> recurseSelector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(recurseSelector, "recurseSelector");

            return RecurseCore(new[] { @this }, x => new[] { recurseSelector(x) }, x => x, null);
        }

        public static IEnumerable<T> RecurseWhile<T>(this T @this, Func<T, T> selector, Func<T, Boolean> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");
            Guard.NotNull(predicate, "predicate");

            return RecurseCore(new[] { @this }, x => new[] { selector(x) }, x => x, predicate);
        }

        public static IEnumerable<T> Recurse<T>(this T @this, Func<T, Maybe<T>> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");

            return RecurseCore(new[] { @this }, x => selector(x).ToEnumerable(), x => x, null);
        }

        public static IEnumerable<T> RecurseWhile<T>(this T @this, Func<T, Maybe<T>> selector, Func<T, Boolean> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");
            Guard.NotNull(predicate, "predicate");

            return RecurseCore(new[] { @this }, x => selector(x).ToEnumerable(), x => x, predicate);
        }

        public static IEnumerable<T> Recurse<T>(this T @this, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");

            return RecurseCore(new[] { @this }, selector, x => x, null);
        }

        public static IEnumerable<T> RecurseWhile<T>(this T @this, Func<T, IEnumerable<T>> selector, Func<T, Boolean> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");
            Guard.NotNull(predicate, "predicate");

            return RecurseCore(new[] { @this }, selector, x => x, predicate);
        }

        public static IEnumerable<T> Recurse<T>(this Maybe<T> @this, Func<T, T> selector)
        {
            Guard.NotNull(selector, "selector");

            return RecurseCore(@this.ToEnumerable(), x => new[]{ selector(x) }, x => x, null);
        }

        public static IEnumerable<T> RecurseWhile<T>(this Maybe<T> @this, Func<T, T> selector, Func<T, Boolean> predicate)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(predicate, "predicate");

            return RecurseCore(@this.ToEnumerable(), x => new[]{ selector(x) }, x => x, predicate);
        }

        public static IEnumerable<T> Recurse<T>(this Maybe<T> @this, Func<T, Maybe<T>> selector)
        {
            Guard.NotNull(selector, "selector");

            return RecurseCore(@this.ToEnumerable(), x => selector(x).ToEnumerable(), x => x, null);
        }

        public static IEnumerable<T> RecurseWhile<T>(this Maybe<T> @this, Func<T, Maybe<T>> selector, Func<T, Boolean> predicate)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(predicate, "predicate");

            return RecurseCore(@this.ToEnumerable(), x => selector(x).ToEnumerable(), x => x, predicate);
        }

        public static IEnumerable<T> Recurse<T>(this Maybe<T> @this, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(selector, "selector");

            return RecurseCore(@this.ToEnumerable(), selector, x => x, null);
        }

        public static IEnumerable<T> RecurseWhile<T>(this Maybe<T> @this, Func<T, IEnumerable<T>> selector, Func<T, Boolean> predicate)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(predicate, "predicate");

            return RecurseCore(@this.ToEnumerable(), selector, x => x, predicate);
        }

        public static IEnumerable<T> Recurse<T>(this IEnumerable<T> @this, Func<T, T> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");

            return RecurseCore(@this, x => new[]{ selector(x) }, x => x, null);
        }

        public static IEnumerable<T> RecurseWhile<T>(this IEnumerable<T> @this, Func<T, T> selector, Func<T, Boolean> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");
            Guard.NotNull(predicate, "predicate");

            return RecurseCore(@this, x => new[]{ selector(x) }, x => x, predicate);
        }

        public static IEnumerable<T> Recurse<T>(this IEnumerable<T> @this, Func<T, Maybe<T>> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");

            return RecurseCore(@this, x => selector(x).ToEnumerable(), x => x, null);
        }

        public static IEnumerable<T> RecurseWhile<T>(this IEnumerable<T> @this, Func<T, Maybe<T>> selector, Func<T, Boolean> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");
            Guard.NotNull(predicate, "predicate");

            return RecurseCore(@this, x => selector(x).ToEnumerable(), x => x, predicate);
        }

        public static IEnumerable<T> Recurse<T>(this IEnumerable<T> @this, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");

            return RecurseCore(@this, selector, x => x, null);
        }

        public static IEnumerable<T> RecurseWhile<T>(this IEnumerable<T> @this, Func<T, IEnumerable<T>> selector, Func<T, Boolean> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(selector, "selector");
            Guard.NotNull(predicate, "predicate");

            return RecurseCore(@this, selector, x => x, predicate);
        }

        private static IEnumerable<TResult> RecurseCore<T, TResult>(this IEnumerable<T> @this, Func<T, IEnumerable<T>> recurseSelector, Func<T, TResult> resultSelector, Func<T, Boolean> predicate)
        {
            var stack = new Stack<IEnumerator<T>>();
            stack.Push(@this.GetEnumerator());

            while (true)
            {
                if (stack.Count == 0)
                    yield break;

                var top = stack.Peek();
                if (!top.MoveNext())
                {
                    stack.Pop();
                    continue;
                }

                var item = top.Current;

                if (item == null || (predicate != null && !predicate(item)))
                    continue;

                yield return resultSelector(item);

                var children = recurseSelector(item);
                if (children != null)
                {
                    var enumerator = children.GetEnumerator();
                    if(enumerator != null)
                        stack.Push(enumerator);
                }
            }
        }

        public static IEnumerable<T> Squash<T>(this IEnumerable<Maybe<T>> @this)
        {
            Guard.NotNull(@this, "this");

            return @this.Where(x => x.HasValue)
                .Select(x => x.Value);
        }

        public static IEnumerable<T> Squash<T>(this IEnumerable<IMaybe<T>> @this)
        {
            Guard.NotNull(@this, "this");

            return @this
                .Where(x => x != null && x.HasValue && x.Value != null)
                .Select(x => x.Value);
        }

        public static IEnumerable<T> OnFirst<T>(this IEnumerable<T> @this, Action<T> action)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(action, "action");

            return OnFirstCore(@this, action);
        }

        private static IEnumerable<T> OnFirstCore<T>(IEnumerable<T> @this, Action<T> action)
        {
            bool isFirst = true;
            foreach (var item in @this)
            {
                if(isFirst)
                {
                    isFirst = false;
                    action(item);
                }

                yield return item;
            }
        }

        public static IEnumerable<T> OnLast<T>(this IEnumerable<T> @this, Action<T> action)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(action, "action");

            return OnLastCore(@this, action);
        }

        private static IEnumerable<T> OnLastCore<T>(IEnumerable<T> @this, Action<T> action)
        {
            bool itemRetreived = false;
            var lastItem = default(T);

            try
            {
                foreach (var item in @this)
                {
                    itemRetreived = true;
                    lastItem = item;
                    yield return item;
                }
            }
            finally
            {
                if(itemRetreived)
                    action(lastItem);    
            }
        }

        public static IEnumerable<T> OnValue<T>(this IEnumerable<T> @this, Action<T> action)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(action, "action");

            return OnValueCore(@this, action);
        }

        private static IEnumerable<T> OnValueCore<T>(IEnumerable<T> @this, Action<T> action)
        {
            foreach(var item in @this)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<T> OnNoValue<T>(this IEnumerable<T> @this, Action action)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(action, "action");

            return OnNoValueCore(@this, action);
        }

        private static IEnumerable<T> OnNoValueCore<T>(IEnumerable<T> @this, Action action)
        {
            bool itemRetreived = false;

            foreach(var item in @this)
            {
                itemRetreived = true;
                yield return item;
            }

            if (!itemRetreived)
                action();
        }

        public static IEnumerable<T> OnException<T>(this IEnumerable<T> @this, Action<Exception> action)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(action, "action");

            return OnExceptionCore(@this, action);
        }

        private static IEnumerable<T> OnExceptionCore<T>(IEnumerable<T> @this, Action<Exception> action)
        {
            using (var enumerator = @this.GetEnumerator())
            {
                while (true)
                {
                    bool didMove;
                    try
                    {
                        didMove = enumerator.MoveNext();
                    }
                    catch (Exception ex)
                    {
                        action(ex);
                        throw;
                    }

                    if (!didMove)
                        yield break;

                    yield return enumerator.Current;
                }
            }
        }

        public static IEnumerable<T> Leading<T>(this IEnumerable<T> @this, Action action)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(action, "action");

            return LeadingCore(@this, action);
        }

        private static IEnumerable<T> LeadingCore<T>(IEnumerable<T> @this, Action action)
        {
            action();

            foreach (var item in @this)
                yield return item;
        }

        public static IEnumerable<T> Finally<T>(this IEnumerable<T> @this, Action action)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(action, "action");

            return FinallyCore(@this, action);
        }

        private static IEnumerable<T> FinallyCore<T>(IEnumerable<T> @this, Action action)
        {
            try
            {
                using (var enumerator = @this.GetEnumerator())
                {
                    while (true)
                    {
                        if (enumerator.MoveNext() != true)
                            yield break;

                        yield return enumerator.Current;
                    }
                }
            }
            finally
            {
                action();
            }
        }

        public static IEnumerable<Neighbors<T>> WithNeighbors<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "this");
            return WithNeighborsCore(@this);
        }

        private static IEnumerable<Neighbors<T>> WithNeighborsCore<T>(this IEnumerable<T> @this)
        {
            var previous = Maybe<T>.NoValue;
            var current = Maybe<T>.NoValue;
            var next = Maybe<T>.NoValue;

            foreach (var item in @this)
            {
                previous = current;
                current = next;
                next = item.ToMaybe();

                if (current.HasValue)
                    yield return new Neighbors<T>(current.Value, previous, next);
            }

            if (next.HasValue)
                yield return new Neighbors<T>(next.Value, current, Maybe<T>.NoValue);
        }

        public static IEnumerable<T> Or<T>(this IEnumerable<T> @this, IEnumerable<T> other)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(other, "other");

            return OrCore(@this, other);
        }

        private static IEnumerable<T> OrCore<T>(this IEnumerable<T> @this, IEnumerable<T> other)
        {
            bool yielded = false;
            using(var thisEnumerator = @this.GetEnumerator())
            {
                bool hasItem = thisEnumerator.MoveNext();
                if(hasItem)
                {
                    yielded = true;
                    do
                    {
                        yield return thisEnumerator.Current;
                    } while (thisEnumerator.MoveNext());
                }
            }

            if(yielded)
                yield break;

            foreach(var item in other)
                yield return item;
        }

        public static Maybe<T> TryElementAt<T>(this IEnumerable<T> @this, int index)
        {
            Guard.NotNull(@this, "this");
            
            if(index < 0)
                throw new ArgumentOutOfRangeException("index");

            using(var thisEnumerator = @this.GetEnumerator())
            {
                while(thisEnumerator.MoveNext())
                {
                    if(index == 0)
                        return new Maybe<T>(thisEnumerator.Current);

                    --index;
                }
            }

            return Maybe.NoValue;
        }

        public static Maybe<T> TryFirst<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "this");

            return TryFirstCore(@this, null);
        }

        public static Maybe<T> TryFirst<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(predicate, "predicate");

            return TryFirstCore(@this, predicate);
        }

        private static Maybe<T> TryFirstCore<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return Maybe.Defer(() =>
            {
                using (var thisEnumerator = @this.GetEnumerator())
                {
                    while (thisEnumerator.MoveNext())
                    {
                        if (predicate == null || predicate(thisEnumerator.Current))
                            return new Maybe<T>(thisEnumerator.Current);
                    }
                }

                return Maybe<T>.NoValue;
            });
        }

        public static Maybe<T> TryLast<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "this");
            return TryLastCore(@this, null);
        }

        public static Maybe<T> TryLast<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(predicate, "predicate");

            return TryLastCore(@this, predicate);
        }

        private static Maybe<T> TryLastCore<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return Maybe.Defer(() =>
            {
                var lastItem = default(T);
                bool itemFound = false;

                using (var thisEnumerator = @this.GetEnumerator())
                {
                    while (thisEnumerator.MoveNext())
                    {
                        if (predicate == null || predicate(thisEnumerator.Current))
                        {
                            itemFound = true;
                            lastItem = thisEnumerator.Current;
                        }
                    }
                }

                return itemFound
                           ? new Maybe<T>(lastItem)
                           : Maybe<T>.NoValue;
            });
        }

        public static Maybe<T> TrySingle<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "this");
            return TrySingleCore(@this, null);
        }

        public static Maybe<T> TrySingle<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(predicate, "predicate");

            return TrySingleCore(@this, predicate);
        }

        private static Maybe<T> TrySingleCore<T>(this IEnumerable<T> @this, Func<T, bool> predicate)
        {
            return Maybe.Defer(() =>
            {
                var lastItem = default(T);
                bool itemFound = false;

                using (var thisEnumerator = @this.GetEnumerator())
                {
                    while (thisEnumerator.MoveNext())
                    {
                        if (predicate == null || predicate(thisEnumerator.Current))
                        {
                            if (itemFound)
                                throw new InvalidOperationException("Sequence contains more than one element");

                            itemFound = true;
                            lastItem = thisEnumerator.Current;
                        }
                    }
                }

                return itemFound
                           ? new Maybe<T>(lastItem)
                           : Maybe<T>.NoValue;
            });
        }

        public static IWithHasCurrentEnumerator<T> WithHasCurrent<T>(this IEnumerator<T> @this)
        {
            Guard.NotNull(@this, "this");
            return new WithHasCurrentEnumerator<T>(@this);
        }

        public static IWithHasCurrentEnumerator WithHasCurrent(this IEnumerator @this)
        {
            Guard.NotNull(@this, "this");
            return new WithHasCurrentEnumerator(@this);
        }
    }
}
