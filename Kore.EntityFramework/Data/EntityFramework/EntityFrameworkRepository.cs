﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Castle.Core.Logging;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;
using Kore.Collections;
using Kore.Exceptions;
using Kore.Logging;

namespace Kore.Data.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly DbContext context;
        private IDbSet<TEntity> set;

        private ILogger logger;

        public DbContext Context
        {
            get { return context; }
        }

        protected IDbSet<TEntity> Set
        {
            get { return set ?? (set = context.Set<TEntity>()); }
        }

        public EntityFrameworkRepository(DbContext context)
        {
            this.context = context;
            this.logger = LoggingUtilities.Resolve();
        }

        #region IRepository<TEntity> Members

        public IQueryable<TEntity> Table
        {
            get { return Set.AsNoTracking(); }
        }

        public TEntity Find(params object[] keyValues)
        {
            return Set.Find(keyValues);
        }

        public int Count()
        {
            return Set.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> countExpression)
        {
            return Set.Count(countExpression);
        }

        public int DeleteAll()
        {
            int rowsAffected = Table.Delete();
            RefreshAll();
            return rowsAffected;
        }

        public int Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            int rowsAffected = Table.Delete(filterExpression);
            RefreshMany(Table.Where(filterExpression).ToHashSet());
            return rowsAffected;
        }

        public int Delete(IQueryable<TEntity> query)
        {
            int rowsAffected = Table.Delete(query);
            RefreshAll();
            return rowsAffected;
        }

        public int Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                if (localEntity != null)
                {
                    Context.Entry(localEntity).State = EntityState.Deleted;
                }
                else
                {
                    Set.Attach(entity);
                    Context.Entry(entity).State = EntityState.Deleted;
                }
            }
            else
            {
                Set.Remove(entity);
            }
            return Context.SaveChanges();
        }

        public int Delete(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                    if (localEntity != null)
                    {
                        Context.Entry(localEntity).State = EntityState.Deleted;
                    }
                    else
                    {
                        Set.Attach(entity);
                        Context.Entry(entity).State = EntityState.Deleted;
                    }
                }
                else
                {
                    Set.Remove(entity);
                }
            }
            return Context.SaveChanges();
        }

        //public Task<int> DeleteAllAsync()
        //{
        //    return Table.DeleteAsync();
        //}

        //public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
        //{
        //    return Table.DeleteAsync(filterExpression);
        //}

        public int Insert(TEntity entity)
        {
            Set.Add(entity);
            return Context.SaveChanges();
        }

        public int Insert(IEnumerable<TEntity> entities)
        {
            if (entities.Count() > 20)
            {
                Context.BulkInsert(entities);
                return entities.Count();
            }
            else
            {
                foreach (var entity in entities)
                {
                    Set.Add(entity);
                }
                return Context.SaveChanges();
            }
        }

        public IEnumerable<TEntity> Translate(string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            try
            {
                var command = this.Context.Database.Connection.CreateCommand();
                command.CommandText = storedProcedure;

                this.Context.Database.Connection.Open();

                var reader = command.ExecuteReader();

                return ObjectContext.Translate<TEntity>(
                    reader,
                    Context.GetEntitySetName(typeof(TEntity)),
                    MergeOption.AppendOnly);
            }
            finally
            {
                this.Context.Database.Connection.Close();
            }
        }

        public int Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                if (Context.Entry(entity).State == EntityState.Detached)
                {
                    var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                    if (localEntity != null)
                    {
                        Context.Entry(localEntity).CurrentValues.SetValues(entity);
                    }
                    else
                    {
                        entity = Set.Attach(entity);
                        Context.Entry(entity).State = EntityState.Modified;
                    }
                }
                else
                {
                    Context.Entry(entity).State = EntityState.Modified;
                }

                return Context.SaveChanges();
            }
            catch (DbEntityValidationException x)
            {
                var msg = x.EntityValidationErrors
                    .SelectMany(validationErrors => validationErrors.ValidationErrors)
                    .Aggregate(string.Empty, (current, validationError) => current + (System.Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));

                logger.Error(msg, x);

                throw new KoreException(msg, x);
            }
        }

        public int Update(IEnumerable<TEntity> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("items");
                }

                foreach (var entity in entities)
                {
                    if (Context.Entry(entity).State == EntityState.Detached)
                    {
                        var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                        if (localEntity != null)
                        {
                            Context.Entry(localEntity).CurrentValues.SetValues(entity);
                        }
                        else
                        {
                            Set.Attach(entity);
                            Context.Entry(entity).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        Context.Entry(entity).State = EntityState.Modified;
                    }
                }
                return Context.SaveChanges();
            }
            catch (DbEntityValidationException x)
            {
                var msg = x.EntityValidationErrors
                    .SelectMany(validationErrors => validationErrors.ValidationErrors)
                    .Aggregate(string.Empty, (current, validationError) => current + (System.Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));

                logger.Error(msg, x);

                throw new KoreException(msg, x);
            }
        }

        public int Update(Expression<Func<TEntity, TEntity>> updateExpression)
        {
            int rowsAffected = Table.Update(updateExpression);
            RefreshAll();
            return rowsAffected;
        }

        public int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            int rowsAffected = Table.Update(filterExpression, updateExpression);
            RefreshMany(Table.Where(filterExpression).ToHashSet());
            return rowsAffected;
        }

        public int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            int rowsAffected = Table.Update(query, updateExpression);
            RefreshAll();
            return rowsAffected;
        }

        //public Task<int> UpdateAsync(Expression<Func<TEntity, TEntity>> updateExpression)
        //{
        //    return Table.UpdateAsync(updateExpression);
        //}

        //public Task<int> UpdateAsync(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
        //{
        //    return Table.UpdateAsync(filterExpression, updateExpression);
        //}

        #endregion IRepository<TEntity> Members

        public ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)Context).ObjectContext; }
        }

        public virtual void RefreshAll()
        {
            var refreshableObjects = ObjectContext.ObjectStateManager
                .GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged)
                .Where(x => x.EntityKey != null)
                .Select(x => x.Entity).ToList();

            ObjectContext.Refresh(RefreshMode.StoreWins, refreshableObjects);
        }

        public virtual void RefreshMany(IEnumerable collection)
        {
            ObjectContext.Refresh(RefreshMode.StoreWins, collection);
        }

        public virtual void Refresh(object entity)
        {
            ObjectContext.Refresh(RefreshMode.StoreWins, entity);
        }
    }
}