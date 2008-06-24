﻿using System;
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

        public static IPipelinedEnumerable<T> Pipeline<T>(this IEnumerable<T> self, Action<T> processor)
        {
            if (processor == null)
                throw new ArgumentNullException("processor");

            return Pipeline(self, o => { processor(o); return o; });
        }

        public static IPipelinedEnumerable<T> Pipeline<T>(this IEnumerable<T> self, PipelineAction<T> processor)
        {
            if (processor == null)
                throw new ArgumentNullException("processor");

            return Pipeline(self, o => { var result = o; processor(ref o); return result; });
        }

        public static IPipelinedEnumerable<T> Pipeline<T>(this IEnumerable<T> self, Func<IEnumerable<T>, IEnumerable<T>> processor)
        {
            if (self == null)
                return null;

            return new PipelinedEnumerable<T>(self, processor);
        }

        public static IPipelinedEnumerable<T> Pipeline<T>(this IEnumerable<T> self, Func<T, T> processor)
        {
            if (self == null)
                return null;

            if (processor == null)
                throw new ArgumentNullException("processor");

            return new PipelinedEnumerable<T>(self, processor);
        }

        public static void ProcessPipeline<T>(this IPipelinedEnumerable<T> self)
        {
            self.ForceEnumeration();
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
