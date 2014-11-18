using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Kore.Data.EntityFramework
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> source, Expression<Func<T, TProperty>> path, bool condition) where T : class
        {
            return condition ? source.Include(path) : source;
        }

        public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
    }
}