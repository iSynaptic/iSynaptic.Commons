using System;
using System.Collections.Generic;
using System.Text;
using iSynaptic.Commons.Extensions;
using System.Collections;


namespace System.Linq
{
    public static class Enumerable
    {
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
        {
            return Distinct<TSource>(source, EqualityComparer<TSource>.Default);
        }

        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (comparer == null)
                throw new ArgumentNullException("comparer");

            List<TSource> list = new List<TSource>();
            
            foreach (TSource item in source)
            {
                if (list.Exists(val => comparer.Equals(val, item)) != true)
                {
                    list.Add(item);
                    yield return item;
                }
            }
        }

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

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (selector == null)
                throw new ArgumentNullException("selector");

            foreach (TSource item in source)
                foreach (TResult child in selector(item))
                    yield return child;
        }

        public static bool SequenceEqual<TSource>(this IEnumerable<TSource> self, IEnumerable<TSource> second)
        {
            return self.SequenceEqual(second, null);
        }

        public static bool SequenceEqual<TSource>(this IEnumerable<TSource> self, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            if (second == null)
                throw new ArgumentNullException("second");

            if (comparer == null)
                comparer = EqualityComparer<TSource>.Default;

            using (IEnumerator<TSource> enumeratorOne = self.GetEnumerator())
            {
                using (IEnumerator<TSource> enumeratorTwo = second.GetEnumerator())
                {
                    while (enumeratorOne.MoveNext())
                    {
                        if (!enumeratorTwo.MoveNext() || !comparer.Equals(enumeratorOne.Current, enumeratorTwo.Current))
                            return false;
                    }

                    if (enumeratorTwo.MoveNext())
                        return false;
                }
            }

            return true;
        }

        public static TSource[] ToArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if(source is TSource[])
                return source as TSource[];

            int count = 0;
            TSource[] items = null;

            ICollection<TSource> col = source as ICollection<TSource>;
            if (col != null)
            {
                count = col.Count;
                if (count > 0)
                {
                    items = new TSource[count];
                    col.CopyTo(items, 0);
                }
            }
            else
            {
                foreach (TSource item in source)
                {
                    if (items == null)
                    {
                        items = new TSource[4];
                    }
                    else if (items.Length == count)
                    {
                        TSource[] newItems = new TSource[count * 2];
                        Array.Copy(items, 0, newItems, 0, count);
                        items = newItems;
                    }

                    items[count] = item;
                    count++;
                }
            }

            if (count == 0)
                return new TSource[0];

            if (items.Length == count)
                return items;

            TSource[] results = new TSource[count];
            Array.Copy(items, 0, results, 0, count);

            return results;
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
