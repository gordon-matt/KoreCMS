using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Kore.EntityFramework.Data
{
    public static class QueryableExtensions
    {
        public static Task<HashSet<TSource>> ToHashSetAsync<TSource>(this IQueryable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return IDbAsyncEnumerableExtensions.ToHashSetAsync<TSource>(QueryableExtensions.AsDbAsyncEnumerable<TSource>(source));
        }

        private static IDbAsyncEnumerable<T> AsDbAsyncEnumerable<T>(this IQueryable<T> source)
        {
            IDbAsyncEnumerable<T> dbAsyncEnumerable = source as IDbAsyncEnumerable<T>;
            if (dbAsyncEnumerable != null)
            {
                return dbAsyncEnumerable;
            }

            throw new InvalidOperationException("IQueryable not async!");
        }
    }
}