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

            if ((index < 0) || (index > destination.Length))
                throw new ArgumentOutOfRangeException(
                    "index", "Number must be either non-negative and less than or equal to Int32.MaxValue or -1.");

            if ((destination.Length - index) < source.Count())
                throw new ArgumentException("Destination array is not long enough to copy all the items in the collection. Check array index and length.", "index");

            foreach (var item in source)
                destination[index++] = item;
        }

        public static IEnumerable<IndexedValue<T>> WithIndex<T>(this IEnumerable<T> self)
        {
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

        public static IEnumerable<T[]> Zip<T>(this IEnumerable<IEnumerable<T>> iterables)
        {
            return ZipCore(iterables);
        }

        public static IEnumerable<T[]> Zip<T>(this IEnumerable<T>[] iterables)
        {
            return ZipCore(iterables);
        }
        
        public static IEnumerable<T[]> Zip<T>(this IEnumerable<T> first, params IEnumerable<T>[] iterables)
        {
            Guard.NotNull(first, "first");
            Guard.NotNull(iterables, "iterables");

            return ZipCore(new[] { first }.Concat(iterables));
        }

        private static IEnumerable<T[]> ZipCore<T>(IEnumerable<IEnumerable<T>> iterables)
        {
            IEnumerator<T>[] enumerators = iterables
                .Select(x => x != null ? x.GetEnumerator() : null)
                .ToArray();

            while (enumerators.Where(x => x != null).Count() > 0)
            {
                int index = 0;
                T[] values = new T[enumerators.Length];

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
            return candidates.Where(specification.IsSatisfiedBy);
        }

        public static IEnumerable<T> FailsSpecification<T>(this IEnumerable<T> candidates, Specification<T> specification)
        {
            return candidates.Where(x => specification.IsSatisfiedBy(x) != true);
        }

        public static bool AllSatisfy<T>(this IEnumerable<T> candidates, Specification<T> specification)
        {
            return candidates.All(specification.IsSatisfiedBy);
        }

        public static SmartLoop<T> SmartLoop<T>(this IEnumerable<T> items)
        {
            Guard.NotNull(items, "items");
            return new SmartLoop<T>(items);
        }

        public static IEnumerable<T> Flatten<T>(this T root, Func<T, IEnumerable<T>> selector)
        {
            Guard.NotNull(root, "root");
            Guard.NotNull(selector, "selector");
            
            return new [] {root}.Concat((selector(root) ?? new T[0]).SelectMany(x => Flatten(x, selector)));
        }
    }
}
