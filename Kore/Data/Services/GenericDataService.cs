using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;

namespace Kore.Data.Services
{
    public class GenericDataService<TEntity> : IGenericDataService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> repository;

        protected GenericDataService(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        #region IGenericDataService<TEntity> Members

        public IRepository<TEntity> Repository
        {
            get { return repository; }
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return repository.Find(keyValues);
        }

        public virtual int Count()
        {
            return repository.Count();
        }

        public virtual int Count(Expression<Func<TEntity, bool>> countExpression)
        {
            return repository.Count(countExpression);
        }

        public virtual int DeleteAll()
        {
            return repository.DeleteAll();
        }

        public virtual int Delete(TEntity entity)
        {
            return repository.Delete(entity);
        }

        public virtual int Delete(IEnumerable<TEntity> entities)
        {
            return repository.Delete(entities);
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            return repository.Delete(filterExpression);
        }

        public virtual int Delete(IQueryable<TEntity> query)
        {
            return repository.Delete(query);
        }

        //public virtual Task<int> DeleteAllAsync()
        //{
        //    return repository.DeleteAllAsync();
        //}

        //public virtual Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
        //{
        //    return repository.DeleteAsync(filterExpression);
        //}

        public virtual int Insert(TEntity entity)
        {
            return repository.Insert(entity);
        }

        public virtual int Insert(IEnumerable<TEntity> entities)
        {
            return repository.Insert(entities);
        }

        public virtual IEnumerable<TEntity> Translate(string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            return repository.Translate(storedProcedure, parameters);
        }

        public virtual int Update(TEntity entity)
        {
            return repository.Update(entity);
        }

        public virtual int Update(IEnumerable<TEntity> entities)
        {
            return repository.Update(entities);
        }

        public virtual int Update(Expression<Func<TEntity, TEntity>> updateExpression)
        {
            return repository.Update(updateExpression);
        }

        public virtual int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            return repository.Update(filterExpression, updateExpression);
        }

        public virtual int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            return repository.Update(query, updateExpression);
        }

        //public virtual Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> updateExpression)
        //{
        //    return repository.UpdateAsync(updateExpression);
        //}

        //public virtual Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
        //{
        //    return repository.UpdateAsync(filterExpression, updateExpression);
        //}

        #endregion IGenericDataService<TEntity> Members
    }
}