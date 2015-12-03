using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kore.Data.Services
{
    public interface IGenericDataService<TEntity> where TEntity : class
    {
        //IRepository<TEntity> Repository { get; }

        #region Find

        /// <summary>
        /// Retrieve all entities
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> Find();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterExpression);

        Task<IEnumerable<TEntity>> FindAsync();

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression);

        TEntity FindOne(params object[] keyValues);

        TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression);

        Task<TEntity> FindOneAsync(params object[] keyValues);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression);

        #endregion

        #region Query

        IQueryable<TEntity> Query();

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filterExpression);

        #endregion Find

        #region Count

        int Count();

        int Count(Expression<Func<TEntity, bool>> countExpression);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<TEntity, bool>> countExpression);

        #endregion Count

        #region Delete

        int DeleteAll();

        int Delete(TEntity entity);

        int Delete(IEnumerable<TEntity> entities);

        int Delete(Expression<Func<TEntity, bool>> filterExpression);

        int Delete(IQueryable<TEntity> query);

        Task<int> DeleteAllAsync();

        Task<int> DeleteAsync(TEntity entity);

        Task<int> DeleteAsync(IEnumerable<TEntity> entities);

        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filterExpression);

        Task<int> DeleteAsync(IQueryable<TEntity> query);

        #endregion Delete

        #region Insert

        int Insert(TEntity entity);

        int Insert(IEnumerable<TEntity> entities);

        Task<int> InsertAsync(TEntity entity);

        Task<int> InsertAsync(IEnumerable<TEntity> entities);

        #endregion Insert

        #region Update

        int Update(TEntity entity);

        int Update(IEnumerable<TEntity> entities);

        Task<int> UpdateAsync(TEntity entity);

        Task<int> UpdateAsync(IEnumerable<TEntity> entities);

        //int Update(Expression<Func<TEntity, TEntity>> updateExpression);

        //int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression);

        //int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression);

        #endregion Update

        IEnumerable<TEntity> Translate(string storedProcedure, IEnumerable<DbParameter> parameters);
    }
}