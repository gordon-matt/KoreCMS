using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Castle.Core.Logging;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;
using Kore.Collections;
using Kore.EntityFramework.Data;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Exceptions;
using Kore.Logging;

namespace Kore.Data.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        #region Private Members

        private readonly IDbContextFactory contextFactory;
        private readonly ILogger logger;
        private readonly Lazy<IKoreEntityFrameworkHelper> efHelper;

        #endregion Private Members

        #region Constructor

        public EntityFrameworkRepository(IDbContextFactory contextFactory, Lazy<IKoreEntityFrameworkHelper> efHelper)
        {
            this.contextFactory = contextFactory;
            this.efHelper = efHelper;
            this.logger = LoggingUtilities.Resolve();
        }

        #endregion Constructor

        #region IRepository<TEntity> Members

        public IRepositoryConnection<TEntity> OpenConnection()
        {
            var context = contextFactory.GetContext();
            return new EntityFrameworkRepositoryConnection<TEntity>(context, true);
        }

        public IRepositoryConnection<TEntity> UseConnection<TOther>(IRepositoryConnection<TOther> connection)
            where TOther : class
        {
            if (!(connection is EntityFrameworkRepositoryConnection<TOther>))
            {
                throw new NotSupportedException("The other connection must be of type EntityFrameworkRepositoryConnection<T>");
            }

            var otherConnection = (connection as EntityFrameworkRepositoryConnection<TOther>);
            return new EntityFrameworkRepositoryConnection<TEntity>(otherConnection.context, false);
        }

        #region Find

        public IEnumerable<TEntity> Find()
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().AsNoTracking().ToHashSet();
            }
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().AsNoTracking().Where(predicate).ToHashSet();
            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync()
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().ToHashSetAsync();
            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().Where(predicate).ToHashSetAsync();
            }
        }

        public TEntity FindOne(params object[] keyValues)
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().Find(keyValues);
            }
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate);
            }
        }

        public async Task<TEntity> FindOneAsync(params object[] keyValues)
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().FindAsync(keyValues);
            }
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
            }
        }

        #endregion Find

        #region Count

        public int Count()
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().AsNoTracking().Count();
            }
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().AsNoTracking().Count(predicate);
            }
        }

        public async Task<int> CountAsync()
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().CountAsync();
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().CountAsync(predicate);
            }
        }

        #endregion Count

        #region Delete

        public int DeleteAll()
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>().AsNoTracking();

                if (efHelper.Value.SupportsEFExtended)
                {
                    int rowsAffected = set.Delete();
                    //RefreshAll();
                    return rowsAffected;
                }
                else
                {
                    // TODO: This will cause out-of-memory exceptions with tables that have too many records. We need a better solution!
                    //  Change this to use a while loop and use Skip() and Take() to get paged results to delete.
                    var entities = set.ToHashSet();
                    return Delete(entities);
                }
            }
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>().AsNoTracking();

                if (efHelper.Value.SupportsEFExtended)
                {
                    int rowsAffected = set.Where(predicate).Delete();
                    //RefreshMany(set.Where(predicate).ToHashSet());
                    return rowsAffected;
                }
                else
                {
                    // TODO: This will cause out-of-memory exceptions with tables that have too many records. We need a better solution!
                    //  Change this to use a while loop and use Skip() and Take() to get paged results to delete.
                    var entities = set.Where(predicate).ToHashSet();
                    return Delete(entities);
                }
            }
        }

        public int Delete(TEntity entity)
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>();

                if (context.Entry(entity).State == EntityState.Detached)
                {
                    var localEntity = set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                    if (localEntity != null)
                    {
                        context.Entry(localEntity).State = EntityState.Deleted;
                    }
                    else
                    {
                        set.Attach(entity);
                        context.Entry(entity).State = EntityState.Deleted;
                    }
                }
                else
                {
                    set.Remove(entity);
                }
                return context.SaveChanges();
            }
        }

        public int Delete(IEnumerable<TEntity> entities)
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>();

                foreach (var entity in entities)
                {
                    if (context.Entry(entity).State == EntityState.Detached)
                    {
                        var localEntity = set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                        if (localEntity != null)
                        {
                            context.Entry(localEntity).State = EntityState.Deleted;
                        }
                        else
                        {
                            set.Attach(entity);
                            context.Entry(entity).State = EntityState.Deleted;
                        }
                    }
                    else
                    {
                        set.Remove(entity);
                    }
                }
                return context.SaveChanges();
            }
        }

        public async Task<int> DeleteAllAsync()
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>().AsNoTracking();

                if (efHelper.Value.SupportsEFExtended)
                {
                    int rowsAffected = set.Delete();
                    //RefreshAll();
                    return await Task.FromResult(rowsAffected);
                }
                else
                {
                    // TODO: This will cause out-of-memory exceptions with tables that have too many records. We need a better solution!
                    //  Change this to use a while loop and use Skip() and Take() to get paged results to delete.
                    var entities = set.ToHashSet();
                    return await DeleteAsync(entities);
                }
            }
        }

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>().AsNoTracking();

                if (efHelper.Value.SupportsEFExtended)
                {
                    int rowsAffected = set.Where(predicate).Delete();
                    //RefreshMany(Table.Where(predicate).ToHashSet());
                    return await Task.FromResult(rowsAffected);
                }
                else
                {
                    // TODO: This will cause out-of-memory exceptions with tables that have too many records. We need a better solution!
                    //  Change this to use a while loop and use Skip() and Take() to get paged results to delete.
                    var entities = set.Where(predicate).ToHashSet();
                    return await DeleteAsync(entities);
                }
            }
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>();

                if (context.Entry(entity).State == EntityState.Detached)
                {
                    var localEntity = set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                    if (localEntity != null)
                    {
                        context.Entry(localEntity).State = EntityState.Deleted;
                    }
                    else
                    {
                        set.Attach(entity);
                        context.Entry(entity).State = EntityState.Deleted;
                    }
                }
                else
                {
                    set.Remove(entity);
                }
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>();

                foreach (var entity in entities)
                {
                    if (context.Entry(entity).State == EntityState.Detached)
                    {
                        var localEntity = set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                        if (localEntity != null)
                        {
                            context.Entry(localEntity).State = EntityState.Deleted;
                        }
                        else
                        {
                            set.Attach(entity);
                            context.Entry(entity).State = EntityState.Deleted;
                        }
                    }
                    else
                    {
                        set.Remove(entity);
                    }
                }
                return await context.SaveChangesAsync();
            }
        }

        #endregion Delete

        #region Insert

        public int Insert(TEntity entity)
        {
            using (var context = contextFactory.GetContext())
            {
                context.Set<TEntity>().Add(entity);
                return context.SaveChanges();
            }
        }

        public int Insert(IEnumerable<TEntity> entities)
        {
            using (var context = contextFactory.GetContext())
            {
                if (efHelper.Value.SupportsBulkInsert && entities.HasMoreThan(20))
                {
                    context.BulkInsert(entities);
                    return entities.Count();
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        context.Set<TEntity>().Add(entity);
                    }
                    return context.SaveChanges();
                }
            }
        }

        public async Task<int> InsertAsync(TEntity entity)
        {
            using (var context = contextFactory.GetContext())
            {
                context.Set<TEntity>().Add(entity);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> InsertAsync(IEnumerable<TEntity> entities)
        {
            using (var context = contextFactory.GetContext())
            {
                int count = entities.Count();
                if (efHelper.Value.SupportsBulkInsert && count > 20)
                {
                    context.BulkInsert(entities);
                    return await Task.FromResult(count);
                }
                else
                {
                    foreach (var entity in entities)
                    {
                        context.Set<TEntity>().Add(entity);
                    }
                    return await context.SaveChangesAsync();
                }
            }
        }

        #endregion Insert

        #region Update

        public int Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                using (var context = contextFactory.GetContext())
                {
                    var set = context.Set<TEntity>();

                    if (context.Entry(entity).State == EntityState.Detached)
                    {
                        var localEntity = set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                        if (localEntity != null)
                        {
                            context.Entry(localEntity).CurrentValues.SetValues(entity);
                        }
                        else
                        {
                            entity = set.Attach(entity);
                            context.Entry(entity).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        context.Entry(entity).State = EntityState.Modified;
                    }

                    return context.SaveChanges();
                }
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

                using (var context = contextFactory.GetContext())
                {
                    var set = context.Set<TEntity>();

                    foreach (var entity in entities)
                    {
                        if (context.Entry(entity).State == EntityState.Detached)
                        {
                            var localEntity = set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                            if (localEntity != null)
                            {
                                context.Entry(localEntity).CurrentValues.SetValues(entity);
                            }
                            else
                            {
                                set.Attach(entity);
                                context.Entry(entity).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            context.Entry(entity).State = EntityState.Modified;
                        }
                    }
                    return context.SaveChanges();
                }
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

                using (var context = contextFactory.GetContext())
                {
                    var set = context.Set<TEntity>();

                    if (context.Entry(entity).State == EntityState.Detached)
                    {
                        var localEntity = set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                        if (localEntity != null)
                        {
                            context.Entry(localEntity).CurrentValues.SetValues(entity);
                        }
                        else
                        {
                            entity = set.Attach(entity);
                            context.Entry(entity).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        context.Entry(entity).State = EntityState.Modified;
                    }

                    return await context.SaveChangesAsync();
                }
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

                using (var context = contextFactory.GetContext())
                {
                    var set = context.Set<TEntity>();

                    foreach (var entity in entities)
                    {
                        if (context.Entry(entity).State == EntityState.Detached)
                        {
                            var localEntity = set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

                            if (localEntity != null)
                            {
                                context.Entry(localEntity).CurrentValues.SetValues(entity);
                            }
                            else
                            {
                                set.Attach(entity);
                                context.Entry(entity).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            context.Entry(entity).State = EntityState.Modified;
                        }
                    }
                    return await context.SaveChangesAsync();
                }
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

        #endregion Update

        //public IEnumerable<TEntity> Translate(string storedProcedure, IEnumerable<DbParameter> parameters)
        //{
        //    using (var context = contextFactory.GetContext())
        //    {
        //        try
        //        {
        //            var command = context.Database.Connection.CreateCommand();
        //            command.CommandText = storedProcedure;

        //            context.Database.Connection.Open();

        //            var reader = command.ExecuteReader();

        //            return ((IObjectContextAdapter)context).ObjectContext.Translate<TEntity>(
        //                reader,
        //                context.GetEntitySetName(typeof(TEntity)),
        //                MergeOption.AppendOnly);
        //        }
        //        finally
        //        {
        //            context.Database.Connection.Close();
        //        }
        //    }
        //}

        #endregion IRepository<TEntity> Members

        #region Non-Public Members

        //protected ObjectContext ObjectContext
        //{
        //    get { return ((IObjectContextAdapter)Context).ObjectContext; }
        //}

        //protected virtual void RefreshAll()
        //{
        //    var refreshableObjects = ObjectContext.ObjectStateManager
        //        .GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged)
        //        .Where(x => x.EntityKey != null)
        //        .Select(x => x.Entity).ToList();

        //    ObjectContext.Refresh(RefreshMode.StoreWins, refreshableObjects);
        //}

        //protected virtual void RefreshMany(IEnumerable collection)
        //{
        //    ObjectContext.Refresh(RefreshMode.StoreWins, collection);
        //}

        //protected virtual void Refresh(object entity)
        //{
        //    ObjectContext.Refresh(RefreshMode.StoreWins, entity);
        //}

        #endregion Non-Public Members
    }
}