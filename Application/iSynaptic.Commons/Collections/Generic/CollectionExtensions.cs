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
