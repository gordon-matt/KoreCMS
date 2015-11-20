using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Castle.Core.Logging;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;
using Kore.Collections;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Exceptions;
using Kore.Logging;
using Kore.EntityFramework.Data;

namespace Kore.Data.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        #region Private Members

        private readonly DbContext context;
        private readonly ILogger logger;
        private readonly Lazy<IKoreEntityFrameworkHelper> efHelper;
        private DbSet<TEntity> set;

        #endregion Private Members

        #region Properties

        public DbContext Context
        {
            get { return context; }
        }

        protected DbSet<TEntity> Set
        {
            get { return set ?? (set = context.Set<TEntity>()); }
        }

        #endregion Properties

        #region Constructor

        public EntityFrameworkRepository(DbContext context, Lazy<IKoreEntityFrameworkHelper> efHelper)
        {
            this.context = context;
            this.logger = LoggingUtilities.Resolve();
            this.efHelper = efHelper;
        }

        #endregion Constructor

        #region IRepository<TEntity> Members

        public IQueryable<TEntity> Table
        {
            get { return Set.AsNoTracking(); }
        }

        #region Find

        public IEnumerable<TEntity> Find()
        {
            return Set.ToHashSet();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Set.Where(filterExpression).ToHashSet();
        }

        public async Task<IEnumerable<TEntity>> FindAsync()
        {
            return await Set.ToHashSetAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return await Set.Where(filterExpression).ToHashSetAsync();
        }

        public TEntity FindOne(params object[] keyValues)
        {
            return Set.Find(keyValues);
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Set.FirstOrDefault(filterExpression);
        }

        public async Task<TEntity> FindOneAsync(params object[] keyValues)
        {
            return await Set.FindAsync(keyValues);
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return await Set.FirstOrDefaultAsync(filterExpression);
        }

        #endregion

        #region Count

        public int Count()
        {
            return Set.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> countExpression)
        {
            return Set.Count(countExpression);
        }

        public async Task<int> CountAsync()
        {
            return await Set.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> countExpression)
        {
            return await Set.CountAsync(countExpression);
        }

        #endregion

        #region Delete

        public int DeleteAll()
        {
            if (efHelper.Value.SupportsEFExtended)
            {
                int rowsAffected = Table.Delete();
                RefreshAll();
                return rowsAffected;
            }
            else
            {
                var entities = Table.ToHashSet();
                return Delete(entities);
            }
        }

        public int Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            if (efHelper.Value.SupportsEFExtended)
            {
                int rowsAffected = Table.Where(filterExpression).Delete();
                RefreshMany(Table.Where(filterExpression).ToHashSet());
                return rowsAffected;
            }
            else
            {
                var entities = Table.Where(filterExpression).ToHashSet();
                return Delete(entities);
            }
        }

        public int Delete(IQueryable<TEntity> query)
        {
            if (efHelper.Value.SupportsEFExtended)
            {
                int rowsAffected = query.Delete();
                RefreshAll();
                return rowsAffected;
            }
            else
            {
                var entities = query.ToHashSet();
                return Delete(entities);
            }
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

        public async Task<int> DeleteAllAsync()
        {
            if (efHelper.Value.SupportsEFExtended)
            {
                int rowsAffected = Table.Delete();
                RefreshAll();
                return await Task.FromResult(rowsAffected);
            }
            else
            {
                var entities = Table.ToHashSet();
                return await DeleteAsync(entities);
            }
        }

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            if (efHelper.Value.SupportsEFExtended)
            {
                int rowsAffected = Table.Where(filterExpression).Delete();
                RefreshMany(Table.Where(filterExpression).ToHashSet());
                return await Task.FromResult(rowsAffected);
            }
            else
            {
                var entities = Table.Where(filterExpression).ToHashSet();
                return await DeleteAsync(entities);
            }
        }

        public async Task<int> DeleteAsync(IQueryable<TEntity> query)
        {
            if (efHelper.Value.SupportsEFExtended)
            {
                int rowsAffected = query.Delete();
                RefreshAll();
                return await Task.FromResult(rowsAffected);
            }
            else
            {
                var entities = query.ToHashSet();
                return await DeleteAsync(entities);
            }
        }

        public async Task<int> DeleteAsync(TEntity entity)
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
            return await Context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
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
            return await Context.SaveChangesAsync();
        }

        #endregion

        #region Insert

        public int Insert(TEntity entity)
        {
            Set.Add(entity);
            return Context.SaveChanges();
        }

        public int Insert(IEnumerable<TEntity> entities)
        {
            if (efHelper.Value.SupportsBulkInsert && entities.Count() > 20)
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

        public async Task<int> InsertAsync(TEntity entity)
        {
            Set.Add(entity);
            return await Context.SaveChangesAsync();
        }

        public async Task<int> InsertAsync(IEnumerable<TEntity> entities)
        {
            int count = entities.Count();
            if (efHelper.Value.SupportsBulkInsert && count > 20)
            {
                Context.BulkInsert(entities);
                return await Task.FromResult(count);
            }
            else
            {
                foreach (var entity in entities)
                {
                    Set.Add(entity);
                }
                return await Context.SaveChangesAsync();
            }
        }

        #endregion

        #region Update

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

        public async Task<int> UpdateAsync(TEntity entity)
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

                return await Context.SaveChangesAsync();
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

        public async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
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
                return await Context.SaveChangesAsync();
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

        //public int Update(Expression<Func<TEntity, TEntity>> updateExpression)
        //{
        //    if (efHelper.Value.SupportsEFExtended)
        //    {
        //        int rowsAffected = Table.Update(updateExpression);
        //        RefreshAll();
        //        return rowsAffected;
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
        //{
        //    if (efHelper.Value.SupportsEFExtended)
        //    {
        //        int rowsAffected = Table.Update(filterExpression, updateExpression);
        //        RefreshMany(Table.Where(filterExpression).ToHashSet());
        //        return rowsAffected;
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression)
        //{
        //    if (efHelper.Value.SupportsEFExtended)
        //    {
        //        int rowsAffected = Table.Update(query, updateExpression);
        //        RefreshAll();
        //        return rowsAffected;
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        #endregion

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

        #endregion IRepository<TEntity> Members

        #region Non-Public Members

        protected ObjectContext ObjectContext
        {
            get { return ((IObjectContextAdapter)Context).ObjectContext; }
        }

        protected virtual void RefreshAll()
        {
            var refreshableObjects = ObjectContext.ObjectStateManager
                .GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged)
                .Where(x => x.EntityKey != null)
                .Select(x => x.Entity).ToList();

            ObjectContext.Refresh(RefreshMode.StoreWins, refreshableObjects);
        }

        protected virtual void RefreshMany(IEnumerable collection)
        {
            ObjectContext.Refresh(RefreshMode.StoreWins, collection);
        }

        protected virtual void Refresh(object entity)
        {
            ObjectContext.Refresh(RefreshMode.StoreWins, entity);
        }

        #endregion Non-Public Members
    }
}