using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static void Remove<T>(this ICollection<T> self, params T[] itemsToRemove)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            if (itemsToRemove == null || itemsToRemove.Length <= 0)
                return;

            foreach (T item in itemsToRemove)
                self.Remove(item);
        }

        public static void RemoveAll<T>(this ICollection<T> self, Func<T, bool> predicate)
        {
            if(self == null)
                throw new ArgumentNullException("self");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            var itemsToRemove = self
                .Where(predicate)
                .ToArray();

            foreach (var itemToRemove in itemsToRemove)
                self.Remove(itemToRemove);
        }

        public static ProjectionCollection<TSourceItem, TProjectedItem> ToProjectedCollection<TSourceItem, TProjectedItem>(this ICollection<TSourceItem> self, Func<TSourceItem, TProjectedItem> selector)
        {
            return new ProjectionCollection<TSourceItem, TProjectedItem>(self, selector);
        }
    }
}
