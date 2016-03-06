﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Kore.Data;

namespace Kore.EntityFramework.Data.EntityFramework
{
    public class EntityFrameworkRepositoryConnection<TEntity> : IRepositoryConnection<TEntity>
        where TEntity : class
    {
        #region Private Members

        private bool isContextOwner;
        private bool disposed;
        internal readonly DbContext context;

        #endregion Private Members

        #region Constructor / Destructor

        public EntityFrameworkRepositoryConnection(DbContext context, bool isContextOwner)
        {
            this.context = context;
            this.isContextOwner = isContextOwner;
        }

        ~EntityFrameworkRepositoryConnection()
        {
            Dispose(false);
        }

        #endregion Constructor / Destructor

        #region IRepositoryConnection<TEntity> Members

        public virtual IQueryable<TEntity> Query()
        {
            return context.Set<TEntity>().AsNoTracking();
        }

        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filterExpression)
        {
            return context.Set<TEntity>().AsNoTracking().Where(filterExpression);
        }

        #endregion IRepositoryConnection<TEntity> Members

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isContextOwner)
            {
                return;
            }

            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                }
            }

            disposed = true;
        }

        #endregion IDisposable Members
    }
}