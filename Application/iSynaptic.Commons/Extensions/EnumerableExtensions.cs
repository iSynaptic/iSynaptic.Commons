using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
