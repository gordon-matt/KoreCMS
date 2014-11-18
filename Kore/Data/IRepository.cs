using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace Kore.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Table { get; }

        TEntity Find(params object[] keyValues);

        int Count();

        int Count(Expression<Func<TEntity, bool>> countExpression);

        int DeleteAll();

        int Delete(TEntity entity);

        int Delete(IEnumerable<TEntity> entities);

        int Delete(Expression<Func<TEntity, bool>> filterExpression);

        int Delete(IQueryable<TEntity> query);

        //Task<int> DeleteAllAsync();

        //Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filterExpression);

        int Insert(TEntity entity);

        int Insert(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Translate(string storedProcedure, IEnumerable<DbParameter> parameters);

        int Update(TEntity entity);

        int Update(IEnumerable<TEntity> entities);

        int Update(Expression<Func<TEntity, TEntity>> updateExpression);

        int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression);

        int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression);

        //Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> updateExpression);

        //Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression);
    }
}