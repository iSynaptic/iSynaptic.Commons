using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace iSynaptic.Commons.Collections.Generic
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
            using(var enumerator = self.GetEnumerator())
            {
                int batchIndex = 0;
                int count = 0;
                T[] buffer = new T[batchSize];

                while(enumerator.MoveNext())
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

                if(count != 0)
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

            return self
                .ToDictionary()
                .ToReadOnlyDictionary();
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

        public static IEnumerable<Maybe<T>[]> Zip<T>(this IEnumerable<IEnumerable<T>> iterables)
        {
            return ZipCore(iterables);
        }

        public static IEnumerable<Maybe<T>[]> Zip<T>(this IEnumerable<T>[] iterables)
        {
            return ZipCore(iterables);
        }

        public static IEnumerable<Maybe<T>[]> Zip<T>(this IEnumerable<T> first, params IEnumerable<T>[] iterables)
        {
            Guard.NotNull(first, "first");
            Guard.NotNull(iterables, "iterables");

            return ZipCore(new[] { first }.Concat(iterables));
        }

        private static IEnumerable<Maybe<T>[]> ZipCore<T>(IEnumerable<IEnumerable<T>> iterables)
        {
            var enumerators = iterables
                .Where(x => x != null)
                .Select(x => x.GetEnumerator())
                .ToArray();

            while (enumerators.Where(x => x != null).Count() > 0)
            {
                int index = 0;
                var values = new Maybe<T>[enumerators.Length];

                bool anyIsAvailable = false;
                foreach (IEnumerator<T> enumerator in enumerators)
                {
                    if (enumerator == null)
                        continue;

                    bool isAvailable = enumerator.MoveNext();

                    if (isAvailable != true)
                    {
                        enumerators[index] = null;
                        index++;

                        continue;
                    }

                    anyIsAvailable = true;
                    values[index++] = enumerator.Current;
                }

                if (anyIsAvailable)
                    yield return values;
            }
        }

        public static void ForceEnumeration<T>(this IEnumerable<T> self)
        {
            Guard.NotNull(self, "self");
            self.All(x => true);
        }

        public static IEnumerable<T> MeetsSpecifcation<T>(this IEnumerable<T> candidates, Specification<T> specification)
        {
            Guard.NotNull(candidates, "candidates");
            Guard.NotNull(specification, "specification");

            return candidates.Where(specification.IsSatisfiedBy);
        }

        public static IEnumerable<T> FailsSpecification<T>(this IEnumerable<T> candidates, Specification<T> specification)
        {
            Guard.NotNull(candidates, "candidates");
            Guard.NotNull(specification, "specification");

            return candidates.Where(x => specification.IsSatisfiedBy(x) != true);
        }

        public static bool AllSatisfy<T>(this IEnumerable<T> candidates, Specification<T> specification)
        {
            Guard.NotNull(candidates, "candidates");
            Guard.NotNull(specification, "specification");

            return candidates.All(specification.IsSatisfiedBy);
        }

        public static SmartLoop<T> SmartLoop<T>(this IEnumerable<T> items)
        {
            Guard.NotNull(items, "items");
            return new SmartLoop<T>(items);
        }

        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> self, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(selector, "selector");

            return self.SelectMany(x => new[]{x}.Concat((selector(x) ?? Enumerable.Empty<T>()).Flatten(selector)));
        }

        public static IEnumerable<T> Flatten<T>(this T self, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(selector, "selector");

            return new[] {self}.Flatten(selector);
        }

        public static IEnumerable<T> Flatten<T>(this T self, Func<T, Maybe<T>> selector)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(selector, "selector");

            return new[] {self}.Concat(selector(self).Select(x => Flatten(x, selector)).Return(Enumerable.Empty<T>()));
        }
    }
}
