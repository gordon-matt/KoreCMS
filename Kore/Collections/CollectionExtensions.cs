using System;
using System.Collections.Generic;

namespace Kore.Collections
{
    public static class CollectionExtensions
    {
        public static void AddIf<T>(this ICollection<T> collection, T item, Func<T, bool> predicate)
        {
            if (predicate(item))
            {
                collection.Add(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                collection.Add(item);
            }
        }
    }
}