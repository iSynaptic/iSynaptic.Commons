using System;
using System.Collections.Generic;
using System.Text;
using iSynaptic.Commons.Extensions;
using System.Collections;


namespace System.Linq
{
    public static class Enumerable
    {
        public static IEnumerable<int> Range(int start, int count)
        {
            long num = (start + count) - 1L;
            if ((count < 0) || (num > 0x7fffffffL))
                throw new ArgumentOutOfRangeException("count");

            for (int index = start; index < start + count; index++)
                yield return index;
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (selector == null)
                throw new ArgumentNullException("selector");

            foreach (TSource item in source)
                yield return selector(item);
        }

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            SimpleArray<TSource> array = new SimpleArray<TSource>(source);
            return array.ToArray();
        }

        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new List<TSource>(source);
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            foreach (TSource item in source)
            {
                if (predicate(item))
                    yield return item;
            }
        }

        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            foreach (IndexedValue<TSource> item in source.WithIndex())
            {
                if (predicate(item.Value, item.Index))
                    yield return item.Value;
            }
        }

        public static IEnumerable<TResult> OfType<TResult>(this IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            foreach (object item in source)
            {
                if (item is TResult)
                    yield return (TResult)item;
            }
        }
    }
}
