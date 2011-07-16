using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TKey, TKey, int> comparer)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(keySelector, "keySelector");
            Guard.NotNull(comparer, "comparer");

            return source.OrderBy(keySelector, comparer.ToComparer());
        }

        public static IEnumerable<TSource> OrderByPriorities<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> higherPrioritySelector, params Func<TSource, bool>[] additionalPrioritySelectors)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(higherPrioritySelector, "higherPrioritySelector");

            return source.OrderByPriorities((l, r) => higherPrioritySelector(l), additionalPrioritySelectors != null
                                                                                    ? additionalPrioritySelectors.Select(s => (Func<TSource, TSource, bool>)((l, r) => s(l))).ToArray()
                                                                                    : null);
        }

        public static IEnumerable<TSource> OrderByPriorities<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, bool> higherPrioritySelector, params Func<TSource, TSource, bool>[] additionalPrioritySelectors)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(higherPrioritySelector, "higherPrioritySelector");

            IEnumerable<Func<TSource, TSource, bool>> selectors = new[] { higherPrioritySelector };

            if (additionalPrioritySelectors != null)
                selectors = selectors.Concat(additionalPrioritySelectors);

            return source.OrderBy(x => x, (l, r) => (from selector in selectors
                                                     let leftHasPriority = selector(l, r)
                                                     let rightHasPriority = selector(r, l)
                                                     where leftHasPriority ^ rightHasPriority
                                                     select leftHasPriority ? -1 : 1).FirstOrDefault());
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(selector, "selector");

            return source.Distinct(selector.ToEqualityComparer());
        }

        public static void CopyTo<T>(this IEnumerable<T> source, T[] destination, int index)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(destination, "destination");

            Guard.MustBeGreaterThanOrEqual(index, 0, "index");
            Guard.MustBeLessThan(index, destination.Length, "index", "The destination is not large enough for the given index.");

            if ((destination.Length - index) < source.Count())
                throw new ArgumentException("Destination array is not large enough to copy all the items in the collection at the given index. Check array index and length.", "index");

            foreach (var item in source)
                destination[index++] = item;
        }

        public static IEnumerable<IndexedValue<T>> WithIndex<T>(this IEnumerable<T> self)
        {
            Guard.NotNull(self, "self");

            int index = 0;
            return self.Select(x => new IndexedValue<T>(index++, x));
        }

        public static IEnumerable<LookAheadableValue<T>> AsLookAheadable<T>(this IEnumerable<T> self)
        {
            Guard.NotNull(self, "self");
            return new LookAheadEnumerable<T>(self);
        }

        public static IEnumerable<T> Buffer<T>(this IEnumerable<T> self)
        {
            Guard.NotNull(self, "self");
            return self.ToArray();
        }

        public static IEnumerable<Batch<T>> Batch<T>(this IEnumerable<T> self, int batchSize)
        {
            Guard.NotNull(self, "self");
            Guard.MustBeGreaterThan(batchSize, 0, "batchSize");

            return BatchCore(self, batchSize);
        }

        private static IEnumerable<Batch<T>> BatchCore<T>(IEnumerable<T> self, int batchSize)
        {
            using (var enumerator = self.GetEnumerator())
            {
                int batchIndex = 0;
                int count = 0;
                T[] buffer = new T[batchSize];

                while (enumerator.MoveNext())
                {
                    buffer[count++] = enumerator.Current;
                    if (count == batchSize)
                    {
                        yield return new Batch<T>(batchIndex, batchSize, buffer);
                        count = 0;
                        batchIndex++;
                        buffer = new T[batchSize];
                    }
                }

                if (count != 0)
                    yield return new Batch<T>(batchIndex, count, buffer.Take(count));
            }
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> self)
        {
            Guard.NotNull(self, "self");
            return self.ToDictionary(x => x.Key, x => x.Value);
        }

        public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> self)
        {
            Guard.NotNull(self, "self");

            return DictionaryExtensions.ToReadOnlyDictionary(self
                                          .ToDictionary());
        }

        public static string Delimit<T>(this IEnumerable<T> self, string delimiter)
        {
            return Delimit(self, delimiter, item => item.ToString());
        }

        public static string Delimit<T>(this IEnumerable<T> self, string delimiter, Func<T, string> selector)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(delimiter, "delimiter");
            Guard.NotNull(selector, "selector");

            var builder = new StringBuilder();

            self.SmartLoop()
                .Between((x, y) => builder.Append(delimiter))
                .Each(x => builder.Append(selector(x)))
                .Execute();

            return builder.ToString();
        }

        public static IEnumerable<Maybe<T>[]> ZipAll<T>(this IEnumerable<T> first, params IEnumerable<T>[] enumerables)
        {
            Guard.NotNull(first, "first");
            Guard.NotNull(enumerables, "enumerables");

            return ZipAll(new[] { first }.Concat(enumerables));
        }

        public static IEnumerable<Maybe<T>[]> ZipAll<T>(this IEnumerable<T>[] enumerables)
        {
            Guard.NotNull(enumerables, "enumerables");
            return ZipAll((IEnumerable<IEnumerable<T>>)enumerables);
        }

        public static IEnumerable<Maybe<T>[]> ZipAll<T>(this IEnumerable<IEnumerable<T>> enumerables)
        {
            Guard.NotNull(enumerables, "enumerables");
            return ZipAllCore(enumerables);
        }

        private static IEnumerable<Maybe<T>[]> ZipAllCore<T>(IEnumerable<IEnumerable<T>> enumerables)
        {
            using (var compositeDisposable = new CompositeDisposable())
            {
                var enumerators = enumerables
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

        private static IEnumerable<Maybe<T>> ToZipableEnumerable<T>(this IEnumerable<T> source)
        {
            Guard.NotNull(source, "source");

            foreach (var item in source)
                yield return item.ToMaybe();

            while (true)
                yield return Maybe<T>.NoValue;
        }

        public static void ForceEnumeration<T>(this IEnumerable<T> self)
        {
            Guard.NotNull(self, "self");
            self.All(x => true);
        }

        public static SmartLoop<T> SmartLoop<T>(this IEnumerable<T> items)
        {
            Guard.NotNull(items, "items");
            return new SmartLoop<T>(items);
        }

        public static IEnumerable<T> Recurse<T>(this T self, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(selector, "selector");

            return new[] { self }.Recurse(selector);
        }

        public static IEnumerable<T> Recurse<T>(this IEnumerable<T> self, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(selector, "selector");

            return self.SelectMany(x => new[] { x }.Concat((selector(x) ?? Enumerable.Empty<T>()).Recurse(selector)));
        }

        public static IEnumerable<T> Recurse<T>(this T self, Func<T, Maybe<T>> selector)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(selector, "selector");

            return new[] { self }.Concat(selector(self).Select(x => Recurse(x, selector)).ValueOrDefault(Enumerable.Empty<T>()));
        }

        public static IEnumerable<T> Squash<T>(this IEnumerable<Maybe<T>> self)
        {
            Guard.NotNull(self, "self");

            return self.Select(x => x.ThrowOnException())
                .Where(x => x.HasValue)
                .Select(x => x.Value);
        }
    }
}
