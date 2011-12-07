// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
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
using System.Text;
using System.Linq;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> Let<T, TResult>(this IEnumerable<T> @this, Func<IEnumerable<T>, IEnumerable<TResult>> selector)
        {
            Guard.NotNull(@this, "@this");
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
            Guard.NotNull(@this, "@this");
            Guard.NotNull(predicate, "predicate");

            return @this.Where(x => !predicate(x));
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> @this)
            where T : class
        {
            Guard.NotNull(@this, "@this");
            return @this.Where(x => x != null);
        }

        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> @this)
            where T : struct
        {
            Guard.NotNull(@this, "@this");
            return @this.Where(x => x.HasValue).Select(x => x.Value);
        }

        public static IEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> @this, Func<TSource, TKey> keySelector, Func<TKey, TKey, int> comparer)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(keySelector, "keySelector");
            Guard.NotNull(comparer, "comparer");

            return @this.OrderBy(keySelector, comparer.ToComparer());
        }

        public static IEnumerable<TSource> OrderByPriorities<TSource>(this IEnumerable<TSource> @this, Func<TSource, bool> higherPrioritySelector, params Func<TSource, bool>[] additionalPrioritySelectors)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(higherPrioritySelector, "higherPrioritySelector");

            return @this.OrderByPriorities((l, r) => higherPrioritySelector(l), additionalPrioritySelectors != null
                                                                                    ? additionalPrioritySelectors.Select(s => (Func<TSource, TSource, bool>)((l, r) => s(l))).ToArray()
                                                                                    : null);
        }

        public static IEnumerable<TSource> OrderByPriorities<TSource>(this IEnumerable<TSource> @this, Func<TSource, TSource, bool> higherPrioritySelector, params Func<TSource, TSource, bool>[] additionalPrioritySelectors)
        {
            Guard.NotNull(@this, "@this");
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
            Guard.NotNull(@this, "@this");
            Guard.NotNull(selector, "selector");

            return @this.Distinct(selector.ToEqualityComparer());
        }

        public static void CopyTo<T>(this IEnumerable<T> @this, T[] destination, int index)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(destination, "destination");

            if(index < 0 || index >= destination.Length)
                throw new ArgumentOutOfRangeException("index", "Index must be between zero and one less than the destinations length.");

            var buffer = new List<T>();
            
            int count = 0;
            int maxCount = destination.Length - index;

            foreach (var item in @this)
            {
                buffer.Add(item);
                count++;

                if (maxCount < count)
                    throw new ArgumentException("Destination array is not large enough to copy all the items in the collection at the given index. Check array index and length.", "index");
            }

            buffer.CopyTo(destination, index);
        }

        public static IEnumerable<IndexedValue<T>> WithIndex<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "@this");

            int index = 0;
            return @this.Select(x => new IndexedValue<T>(index++, x));
        }

        public static IEnumerable<LookAheadableValue<T>> AsLookAheadable<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "@this");
            return new LookAheadEnumerable<T>(@this);
        }

        public static IEnumerable<T> Buffer<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.ToArray();
        }

        public static IEnumerable<Batch<T>> Batch<T>(this IEnumerable<T> @this, int batchSize)
        {
            Guard.NotNull(@this, "@this");

            if (batchSize < 0)
                throw new ArgumentOutOfRangeException("batchSize", "Batch size must not be negative.");

            return BatchCore(@this, batchSize);
        }

        private static IEnumerable<Batch<T>> BatchCore<T>(IEnumerable<T> @this, int batchSize)
        {
            using (var enumerator = @this.GetEnumerator())
            {
                int batchIndex = 0;
                int count = 0;
                T[] buffer = new T[batchSize];

                while (enumerator.MoveNext())
                {
                    buffer[count++] = enumerator.Current;
                    if (count == batchSize)
                    {
                        yield return new Batch<T>(buffer, batchIndex, batchSize);
                        count = 0;
                        batchIndex++;
                        buffer = new T[batchSize];
                    }
                }

                if (count != 0)
                    yield return new Batch<T>(buffer.Take(count), batchIndex, count);
            }
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.ToDictionary(x => x.Key, x => x.Value);
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this)
        {
            Guard.NotNull(@this, "@this");

            return DictionaryExtensions.ToReadOnlyDictionary(@this.ToDictionary());
        }

        public static string Delimit<T>(this IEnumerable<T> @this, string delimiter)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(delimiter, "delimiter");

            return Delimit(@this, delimiter, x => x.ToString());
        }

        public static string Delimit<T>(this IEnumerable<T> @this, string delimiter, string formatString)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(delimiter, "delimiter");
            Guard.NotNull(formatString, "formatString");

            return @this.Delimit(delimiter, x => string.Format(formatString, x));
        }

        public static string Delimit<T>(this IEnumerable<T> @this, string delimiter, Func<T, string> formatter)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(delimiter, "delimiter");
            Guard.NotNull(formatter, "formatter");

            var builder = new StringBuilder();

            @this.SmartLoop()
                .Between((x, y) => builder.Append(delimiter))
                .Each(x => builder.Append(formatter(x)))
                .Execute();

            return builder.ToString();
        }

        public static IEnumerable<Maybe<T>[]> ZipAll<T>(this IEnumerable<T> @this, params IEnumerable<T>[] sources)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(sources, "sources");

            return ZipAll(new[] { @this }.Concat(sources));
        }

        public static IEnumerable<Maybe<T>[]> ZipAll<T>(this IEnumerable<T>[] @this)
        {
            Guard.NotNull(@this, "@this");
            return ZipAll((IEnumerable<IEnumerable<T>>)@this);
        }

        public static IEnumerable<Maybe<T>[]> ZipAll<T>(this IEnumerable<IEnumerable<T>> @this)
        {
            Guard.NotNull(@this, "@this");
            return ZipAllCore(@this);
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

        private static IEnumerable<Maybe<T>> ToZipableEnumerable<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "@this");

            foreach (var item in @this)
                yield return item.ToMaybe();

            while (true)
                yield return Maybe<T>.NoValue;
        }

        public static IEnumerable<T> Run<T>(this IEnumerable<T> @this, Action<T> action = null)
        {
            var source = Guard.NotNull(@this, "@this");

            if (action != null)
                source = source.OnValue(action);

            return source.ToArray();
        }

        public static SmartLoop<T> SmartLoop<T>(this IEnumerable<T> @this)
        {
            Guard.NotNull(@this, "@this");
            return new SmartLoop<T>(@this);
        }

        public static IEnumerable<T> Recurse<T>(this T @this, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(selector, "selector");

            return new[] { @this }.Recurse(selector);
        }

        public static IEnumerable<T> Recurse<T>(this IEnumerable<T> @this, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(selector, "selector");

            return @this.SelectMany(x => new[] { x }.Concat((selector(x) ?? Enumerable.Empty<T>()).Recurse(selector)));
        }

        public static IEnumerable<T> Recurse<T>(this T @this, Func<T, T> selector)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(selector, "selector");

            return @this.Recurse(x => selector(x).ToMaybe());
        }

        public static IEnumerable<T> Recurse<T>(this T @this, Func<T, Maybe<T>> selector)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(selector, "selector");

            return new[] { @this }.Concat(selector(@this).Select(x => Recurse(x, selector)).ValueOrDefault(Enumerable.Empty<T>()));
        }

        public static IEnumerable<T> Recurse<T>(this Maybe<T> @this, Func<T, Maybe<T>> selector)
        {
            Guard.NotNull(selector, "selector");

            return @this.Select(x => x.Recurse(selector))
                .ValueOrDefault(Enumerable.Empty<T>());
        }

        public static IEnumerable<T> Squash<T>(this IEnumerable<Maybe<T>> @this)
        {
            Guard.NotNull(@this, "@this");

            return @this.Where(x => x.HasValue)
                .Select(x => x.Value);
        }

        public static IEnumerable<T> Squash<T>(this IEnumerable<IMaybe<T>> @this)
        {
            Guard.NotNull(@this, "@this");

            return @this
                .Where(x => x != null && x.HasValue && x.Value != null)
                .Select(x => x.Value);
        }

        public static IEnumerable<T> OnFirst<T>(this IEnumerable<T> @this, Action<T> action)
        {
            Guard.NotNull(@this, "@this");
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
            Guard.NotNull(@this, "@this");
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
            Guard.NotNull(@this, "@this");
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
            Guard.NotNull(@this, "@this");
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
            Guard.NotNull(@this, "@this");
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
            Guard.NotNull(@this, "@this");
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
            Guard.NotNull(@this, "@this");
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
            Guard.NotNull(@this, "@this");
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
    }
}
