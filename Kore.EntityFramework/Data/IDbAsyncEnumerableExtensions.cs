using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Kore.EntityFramework.Data
{
    internal static class IDbAsyncEnumerableExtensions
    {
        internal static async Task ForEachAsync(this IDbAsyncEnumerable source, Action<object> action, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IDbAsyncEnumerator asyncEnumerator = source.GetAsyncEnumerator())
            {
                if (await TaskExtensions.WithCurrentCulture<bool>(asyncEnumerator.MoveNextAsync(cancellationToken)))
                {
                    Task<bool> moveNextTask;
                    do
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        object current = asyncEnumerator.Current;
                        moveNextTask = asyncEnumerator.MoveNextAsync(cancellationToken);
                        action(current);
                    }
                    while (await TaskExtensions.WithCurrentCulture<bool>(moveNextTask));
                }
            }
        }

        internal static Task<HashSet<T>> ToHashSetAsync<T>(this IDbAsyncEnumerable source)
        {
            return IDbAsyncEnumerableExtensions.ToHashSetAsync<T>(source, CancellationToken.None);
        }

        internal static async Task<HashSet<T>> ToHashSetAsync<T>(this IDbAsyncEnumerable source, CancellationToken cancellationToken)
        {
            var hashSet = new HashSet<T>();
            await TaskExtensions.WithCurrentCulture(IDbAsyncEnumerableExtensions.ForEachAsync(source, (Action<object>)(e => hashSet.Add((T)e)), cancellationToken));
            return hashSet;
        }
    }
}