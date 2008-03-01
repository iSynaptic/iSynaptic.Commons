using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

namespace iSynaptic.Commons.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IndexedValue<T>> WithIndex<T>(this IEnumerable<T> self)
        {
            int index = 0;

            foreach(T item in self)
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

        public static bool TrueForAll<T>(this IEnumerable<T> self, Predicate<T> predicate)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            foreach (T item in self)
            {
                if (predicate(item) != true)
                    return false;
            }

            return true;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            if (self == null)
                yield break;

            if (action == null)
                throw new ArgumentNullException("action");

            foreach (T item in self)
            {
                action(item);
                yield return item;
            }
        }

        public static IEnumerable<T> Pipeline<T>(this IEnumerable<T> self, Func<IEnumerable<T>, IEnumerable<T>> processor)
        {
            if (self == null)
                return null;

            return new PipelinedEnumerable<T>(self, processor);
        }

        public static IEnumerable<T> Pipeline<T>(this IEnumerable<T> self, Func<T, T> processor)
        {
            if (self == null)
                return null;

            return new PipelinedEnumerable<T>(self, processor);
        }

        public static void ForceEnumeration<T>(this IEnumerable<T> self)
        {
            if (self == null)
                return;

            foreach (T item in self)
                continue;
        }
    }
}
