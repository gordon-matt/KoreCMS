using System.Collections.Generic;

namespace Kore.Collections
{
    public static class QueueExtensions
    {
        public static IEnumerable<T> DequeueMany<T>(this Queue<T> queue, int count)
        {
            if (queue.Count < count)
            {
                count = queue.Count;
            }

            ICollection<T> results = null;

            if (count > 20)
            {
                results = new HashSet<T>();
            }
            else
            {
                results = new List<T>();
            }

            for (int i = 0; i < count; i++)
            {
                results.Add(queue.Dequeue());
            }

            return results;
        }
    }
}