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

        public static void RemoveAll<T>(this ICollection<T> self, Func<T, bool> match)
        {
            if(self == null)
                throw new ArgumentNullException("self");

            if(match == null)
                throw new ArgumentNullException("match");

            var itemsToRemove = self
                .Where(match)
                .ToArray();

            foreach (var itemToRemove in itemsToRemove)
                self.Remove(itemToRemove);
        }
    }
}
