using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace iSynaptic.Commons.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IndexedValue<T>> WithIndex<T>(this IEnumerable<T> self)
        {
            int index = 0;

            foreach (T item in self)
                yield return new IndexedValue<T>(index++, item);
        }

        public static IEnumerable<LookAheadableValue<T>> AsLookAheadable<T>(this IEnumerable<T> self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            return new LookAheadEnumerable<T>(self);
        }

        public static IEnumerable<T> Buffer<T>(this IEnumerable<T> self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            return self.ToArray();
        }

        public static string Delimit<T>(this IEnumerable<T> self, string delimiter)
        {
            return Delimit(self, delimiter, item => item.ToString());
        }

        public static string Delimit<T>(this IEnumerable<T> self, string delimiter, Func<T, string> selector)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            if (delimiter == null)
                throw new ArgumentNullException("delimeter");

            if (selector == null)
                throw new ArgumentNullException("selector");

            StringBuilder builder = new StringBuilder();
            bool isFirst = true;

            foreach (T item in self)
            {
                if (isFirst)
                    isFirst = false;
                else
                    builder.Append(delimiter);

                builder.Append(selector(item));
            }

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
            if (first == null)
                throw new ArgumentNullException("first");

            if (iterables == null)
                throw new ArgumentNullException("iterables");

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
            if (self == null)
                return;

            foreach (T item in self)
                continue;
        }

        public static IEnumerable<T> MeetsSpecifcation<T>(this IEnumerable<T> candidates, ISpecification<T> specification)
        {
            return candidates.Where(specification.IsSatisfiedBy);
        }

        public static IEnumerable<T> FailsSpecification<T>(this IEnumerable<T> candidates, ISpecification<T> specification)
        {
            return candidates.Where(x => specification.IsSatisfiedBy(x) != true);
        }

        public static bool Satisfies<T>(this T[] candidates, ISpecification<T> specification)
        {
            return candidates.All(specification.IsSatisfiedBy);
        }

        public static bool Satisfies<T>(this IEnumerable<T> candidates, ISpecification<T> specification)
        {
            return candidates.All(specification.IsSatisfiedBy);
        }
    }
}
