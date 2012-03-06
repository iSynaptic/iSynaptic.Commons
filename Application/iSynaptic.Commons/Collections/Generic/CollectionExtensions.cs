// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
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
using System.Linq;

namespace iSynaptic.Commons.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> @this, IEnumerable<T> items)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(items, "items");

            foreach(var item in items)
                @this.Add(item);
        }

        public static void Remove<T>(this ICollection<T> @this, params T[] itemsToRemove)
        {
            Guard.NotNull(@this, "@this");

            if (itemsToRemove == null || itemsToRemove.Length <= 0)
                return;

            foreach (T item in itemsToRemove)
                @this.Remove(item);
        }

        public static void RemoveAll<T>(this ICollection<T> @this, Func<T, bool> predicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(predicate, "predicate");

            var itemsToRemove = @this
                .Where(predicate)
                .ToArray();

            foreach (var itemToRemove in itemsToRemove)
                @this.Remove(itemToRemove);
        }

        public static ProjectionCollection<TSourceItem, TProjectedItem> ToProjectedCollection<TSourceItem, TProjectedItem>(this ICollection<TSourceItem> @this, Func<TSourceItem, TProjectedItem> selector)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(selector, "selector");

            return new ProjectionCollection<TSourceItem, TProjectedItem>(@this, selector);
        }
    }
}
