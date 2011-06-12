using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> items)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(items, "items");

            foreach(var item in items)
                self.Add(item);
        }

        public static void Remove<T>(this ICollection<T> self, params T[] itemsToRemove)
        {
            Guard.NotNull(self, "self");

            if (itemsToRemove == null || itemsToRemove.Length <= 0)
                return;

            foreach (T item in itemsToRemove)
                self.Remove(item);
        }

        public static void RemoveAll<T>(this ICollection<T> self, Func<T, bool> predicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(predicate, "predicate");

            var itemsToRemove = self
                .Where(predicate)
                .ToArray();

            foreach (var itemToRemove in itemsToRemove)
                self.Remove(itemToRemove);
        }

        public static ProjectionCollection<TSourceItem, TProjectedItem> ToProjectedCollection<TSourceItem, TProjectedItem>(this ICollection<TSourceItem> self, Func<TSourceItem, TProjectedItem> selector)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(selector, "selector");

            return new ProjectionCollection<TSourceItem, TProjectedItem>(self, selector);
        }
    }
}
