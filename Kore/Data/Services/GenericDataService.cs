using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using Castle.Core.Logging;
using Kore.Caching;
using Kore.Collections;
using Kore.Logging;

namespace Kore.Data.Services
{
    public class GenericDataService<TEntity> : IGenericDataService<TEntity> where TEntity : class
    {
        #region Private Members

        private static string cacheKey;
        private static string cacheKeyFiltered;
        private readonly ICacheManager cacheManager;
        private readonly IRepository<TEntity> repository;

        #endregion Private Members

        #region Properties

        protected virtual string CacheKey
        {
            get
            {
                if (string.IsNullOrEmpty(cacheKey))
                {
                    cacheKey = string.Format("Repository_{0}", typeof(TEntity).Name);
                }
                return cacheKey;
            }
        }

        protected virtual string CacheKeyFiltered
        {
            get
            {
                if (string.IsNullOrEmpty(cacheKeyFiltered))
                {
                    cacheKeyFiltered = string.Format("Repository_{0}_{{0}}", typeof(TEntity).Name);
                }
                return cacheKeyFiltered;
            }
        }

        public ICacheManager CacheManager
        {
            get { return cacheManager; }
        }

        public ILogger Logger { get; private set; }

        public IRepository<TEntity> Repository
        {
            get { return repository; }
        }

        #endregion Properties

        #region Constructor

        protected GenericDataService(ICacheManager cacheManager, IRepository<TEntity> repository)
        {
            this.cacheManager = cacheManager;
            this.repository = repository;
            this.Logger = LoggingUtilities.Resolve();
        }

        #endregion Constructor

        #region IGenericDataService<TEntity> Members

        public virtual IEnumerable<TEntity> Find()
        {
            return CacheManager.Get(CacheKey, () =>
            {
                return repository.Table.ToHashSet();
            });
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterExpression)
        {
            return repository.Table.Where(filterExpression).ToHashSet();

            // Ignore caching for now, since converting and Expression<T> to a string does not always result in something unique
            //  Example: when there are variables. So, we need to find a way to get the variables translated to their values and use that
            //  in a custom ToString() implementation.. maybe...
            // Maybe this can help:
            //  http://referencesource.microsoft.com/#System.Core/Microsoft/Scripting/Ast/ExpressionStringBuilder.cs#240c0ae863272266
            //string key = string.Format(CacheKeyFiltered, filterExpression);
            //return CacheManager.Get(key, () =>
            //{
            //    return repository.Table.Where(filterExpression).ToHashSet();
            //});
        }

        public virtual TEntity FindOne(params object[] keyValues)
        {
            return repository.Find(keyValues);
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            return repository.Table.FirstOrDefault(filterExpression);
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
            int rowsAffected = repository.DeleteAll();
            ClearCache();
            return rowsAffected;
        }

        public virtual int Delete(TEntity entity)
        {
            int rowsAffected = repository.Delete(entity);
            ClearCache();
            return rowsAffected;
        }

        public virtual int Delete(IEnumerable<TEntity> entities)
        {
            int rowsAffected = repository.Delete(entities);
            ClearCache();
            return rowsAffected;
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            int rowsAffected = repository.Delete(filterExpression);
            ClearCache();
            return rowsAffected;
        }

        public virtual int Delete(IQueryable<TEntity> query)
        {
            int rowsAffected = repository.Delete(query);
            ClearCache();
            return rowsAffected;
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
            int rowsAffected = repository.Insert(entity);
            ClearCache();
            return rowsAffected;
        }

        public virtual int Insert(IEnumerable<TEntity> entities)
        {
            int rowsAffected = repository.Insert(entities);
            ClearCache();
            return rowsAffected;
        }

        public virtual IEnumerable<TEntity> Translate(string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            return repository.Translate(storedProcedure, parameters);
        }

        public virtual int Update(TEntity entity)
        {
            int rowsAffected = repository.Update(entity);
            ClearCache();
            return rowsAffected;
        }

        public virtual int Update(IEnumerable<TEntity> entities)
        {
            int rowsAffected = repository.Update(entities);
            ClearCache();
            return rowsAffected;
        }

        public virtual int Update(Expression<Func<TEntity, TEntity>> updateExpression)
        {
            int rowsAffected = repository.Update(updateExpression);
            ClearCache();
            return rowsAffected;
        }

        public virtual int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            int rowsAffected = repository.Update(filterExpression, updateExpression);
            ClearCache();
            return rowsAffected;
        }

        public virtual int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            int rowsAffected = repository.Update(query, updateExpression);
            ClearCache();
            return rowsAffected;
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

        protected virtual void ClearCache()
        {
            CacheManager.Remove(CacheKey);
            CacheManager.RemoveByPattern(string.Format(CacheKeyFiltered, ".*"));
        }
    }
}