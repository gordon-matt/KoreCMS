using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kore.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        //IQueryable<TEntity> Table { get; }

        IRepositoryConnection<TEntity> OpenConnection();

        IRepositoryConnection<TEntity> UseConnection<TOther>(IRepositoryConnection<TOther> connection)
            where TOther : class;

        #region Find

        IEnumerable<TEntity> Find();

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> FindAsync();

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity FindOne(params object[] keyValues);

        TEntity FindOne(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FindOneAsync(params object[] keyValues);

        Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Find

        #region Count

        int Count();

        int Count(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion Count

        #region Delete

        int DeleteAll();

        int Delete(TEntity entity);

        int Delete(IEnumerable<TEntity> entities);

        int Delete(Expression<Func<TEntity, bool>> predicate);

        int Delete(IQueryable<TEntity> query);

        Task<int> DeleteAllAsync();

        Task<int> DeleteAsync(TEntity entity);

        Task<int> DeleteAsync(IEnumerable<TEntity> entities);

        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);

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

        #endregion Update
    }
}