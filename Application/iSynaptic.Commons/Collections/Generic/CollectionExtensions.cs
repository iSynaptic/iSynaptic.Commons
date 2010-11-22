using System;
using System.Collections.Generic;

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
    }
}
