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
using Kore.EntityFramework.Data;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Exceptions;
using Kore.Logging;

namespace Kore.Data.EntityFramework
{
    public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class, IEntity
    {
        #region Private Members

        private bool disposed;
        private DbContext context;
        private readonly IDbContextFactory contextFactory;
        private readonly ILogger logger;
        private readonly Lazy<IKoreEntityFrameworkHelper> efHelper;

        #endregion Private Members

        #region Properties

        public DbContext Context
        {
            get
            {
                if (context == null)
                {
                    context = contextFactory.GetContext();
                }
                return context;
            }
        }

        protected DbSet<TEntity> Set
        {
            get { return Context.Set<TEntity>(); }
        }

        #endregion Properties

        #region Constructor / Destructor

        public EntityFrameworkRepository(IDbContextFactory contextFactory, Lazy<IKoreEntityFrameworkHelper> efHelper)
        {
            this.contextFactory = contextFactory;
            this.efHelper = efHelper;
            this.logger = LoggingUtilities.Resolve();
        }

        ~EntityFrameworkRepository()
        {
            Dispose(false);
        }

        #endregion Constructor / Destructor

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

        #endregion Find

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

        #endregion Count

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

        #endregion Delete

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

        #endregion Update

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

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (context != null)
                {
                    context.Dispose();
                    context = null;
                }
            }

            disposed = true;
        }

        #endregion IDisposable Members

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

    //public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>
    //    where TEntity : class, IEntity
    //{
    //    #region Private Members

    //    private readonly ILogger logger;
    //    private readonly Lazy<IKoreEntityFrameworkHelper> efHelper;
    //    private DbSet<TEntity> set;
    //    private readonly IDbContextFactory contextFactory;

    //    #endregion Private Members

    //    #region Properties

    //    public DbContext CreateContext()
    //    {
    //        return contextFactory.GetContext();
    //    }

    //    #endregion Properties

    //    #region Constructor

    //    public EntityFrameworkRepository(IDbContextFactory contextFactory, Lazy<IKoreEntityFrameworkHelper> efHelper)
    //    {
    //        this.contextFactory = contextFactory;
    //        this.logger = LoggingUtilities.Resolve();
    //        this.efHelper = efHelper;
    //    }

    //    #endregion Constructor

    //    #region IRepository<TEntity> Members

    //    public IQueryable<TEntity> Table
    //    {
    //        get
    //        {
    //            using (var context = CreateContext())
    //            {
    //                return context.Set<TEntity>().AsNoTracking();
    //            }
    //        }
    //    }

    //    #region Find

    //    public IEnumerable<TEntity> Find()
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return context.Set<TEntity>().ToHashSet();
    //        }
    //    }

    //    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return context.Set<TEntity>().Where(filterExpression).ToHashSet();
    //        }
    //    }

    //    public async Task<IEnumerable<TEntity>> FindAsync()
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return await context.Set<TEntity>().ToHashSetAsync();
    //        }
    //    }

    //    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return await context.Set<TEntity>().Where(filterExpression).ToHashSetAsync();
    //        }
    //    }

    //    public TEntity FindOne(params object[] keyValues)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return context.Set<TEntity>().Find(keyValues);
    //        }
    //    }

    //    public TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return context.Set<TEntity>().FirstOrDefault(filterExpression);
    //        }
    //    }

    //    public async Task<TEntity> FindOneAsync(params object[] keyValues)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return await context.Set<TEntity>().FindAsync(keyValues);
    //        }
    //    }

    //    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return await context.Set<TEntity>().FirstOrDefaultAsync(filterExpression);
    //        }
    //    }

    //    #endregion Find

    //    #region Count

    //    public int Count()
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return context.Set<TEntity>().Count();
    //        }
    //    }

    //    public int Count(Expression<Func<TEntity, bool>> countExpression)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return context.Set<TEntity>().Count(countExpression);
    //        }
    //    }

    //    public async Task<int> CountAsync()
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return await context.Set<TEntity>().CountAsync();
    //        }
    //    }

    //    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> countExpression)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            return await context.Set<TEntity>().CountAsync(countExpression);
    //        }
    //    }

    //    #endregion Count

    //    #region Delete

    //    public int DeleteAll()
    //    {
    //        if (efHelper.Value.SupportsEFExtended)
    //        {
    //            int rowsAffected = Table.Delete();
    //            RefreshAll();
    //            return rowsAffected;
    //        }
    //        else
    //        {
    //            var entities = Table.ToHashSet();
    //            return Delete(entities);
    //        }
    //    }

    //    public int Delete(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        if (efHelper.Value.SupportsEFExtended)
    //        {
    //            int rowsAffected = Table.Where(filterExpression).Delete();
    //            RefreshMany(Table.Where(filterExpression).ToHashSet());
    //            return rowsAffected;
    //        }
    //        else
    //        {
    //            var entities = Table.Where(filterExpression).ToHashSet();
    //            return Delete(entities);
    //        }
    //    }

    //    public int Delete(IQueryable<TEntity> query)
    //    {
    //        if (efHelper.Value.SupportsEFExtended)
    //        {
    //            int rowsAffected = query.Delete();
    //            RefreshAll();
    //            return rowsAffected;
    //        }
    //        else
    //        {
    //            var entities = query.ToHashSet();
    //            return Delete(entities);
    //        }
    //    }

    //    public int Delete(TEntity entity)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            if (context.Entry(entity).State == EntityState.Detached)
    //            {
    //                var localEntity = context.Set<TEntity>().Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                if (localEntity != null)
    //                {
    //                    context.Entry(localEntity).State = EntityState.Deleted;
    //                }
    //                else
    //                {
    //                    context.Set<TEntity>().Attach(entity);
    //                    context.Entry(entity).State = EntityState.Deleted;
    //                }
    //            }
    //            else
    //            {
    //                context.Set<TEntity>().Remove(entity);
    //            }
    //            return context.SaveChanges();
    //        }
    //    }

    //    public int Delete(IEnumerable<TEntity> entities)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            foreach (var entity in entities)
    //            {
    //                if (context.Entry(entity).State == EntityState.Detached)
    //                {
    //                    var localEntity = context.Set<TEntity>().Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                    if (localEntity != null)
    //                    {
    //                        context.Entry(localEntity).State = EntityState.Deleted;
    //                    }
    //                    else
    //                    {
    //                        context.Set<TEntity>().Attach(entity);
    //                        context.Entry(entity).State = EntityState.Deleted;
    //                    }
    //                }
    //                else
    //                {
    //                    context.Set<TEntity>().Remove(entity);
    //                }
    //            }
    //            return context.SaveChanges();
    //        }
    //    }

    //    public async Task<int> DeleteAllAsync()
    //    {
    //        if (efHelper.Value.SupportsEFExtended)
    //        {
    //            int rowsAffected = Table.Delete();
    //            RefreshAll();
    //            return await Task.FromResult(rowsAffected);
    //        }
    //        else
    //        {
    //            var entities = Table.ToHashSet();
    //            return await DeleteAsync(entities);
    //        }
    //    }

    //    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        if (efHelper.Value.SupportsEFExtended)
    //        {
    //            int rowsAffected = Table.Where(filterExpression).Delete();
    //            RefreshMany(Table.Where(filterExpression).ToHashSet());
    //            return await Task.FromResult(rowsAffected);
    //        }
    //        else
    //        {
    //            var entities = Table.Where(filterExpression).ToHashSet();
    //            return await DeleteAsync(entities);
    //        }
    //    }

    //    public async Task<int> DeleteAsync(IQueryable<TEntity> query)
    //    {
    //        if (efHelper.Value.SupportsEFExtended)
    //        {
    //            int rowsAffected = query.Delete();
    //            RefreshAll();
    //            return await Task.FromResult(rowsAffected);
    //        }
    //        else
    //        {
    //            var entities = query.ToHashSet();
    //            return await DeleteAsync(entities);
    //        }
    //    }

    //    public async Task<int> DeleteAsync(TEntity entity)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            if (context.Entry(entity).State == EntityState.Detached)
    //            {
    //                var localEntity = context.Set<TEntity>().Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                if (localEntity != null)
    //                {
    //                    context.Entry(localEntity).State = EntityState.Deleted;
    //                }
    //                else
    //                {
    //                    context.Set<TEntity>().Attach(entity);
    //                    context.Entry(entity).State = EntityState.Deleted;
    //                }
    //            }
    //            else
    //            {
    //                context.Set<TEntity>().Remove(entity);
    //            }
    //            return await context.SaveChangesAsync();
    //        }
    //    }

    //    public async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            foreach (var entity in entities)
    //            {
    //                if (context.Entry(entity).State == EntityState.Detached)
    //                {
    //                    var localEntity = context.Set<TEntity>().Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                    if (localEntity != null)
    //                    {
    //                        context.Entry(localEntity).State = EntityState.Deleted;
    //                    }
    //                    else
    //                    {
    //                        context.Set<TEntity>().Attach(entity);
    //                        context.Entry(entity).State = EntityState.Deleted;
    //                    }
    //                }
    //                else
    //                {
    //                    context.Set<TEntity>().Remove(entity);
    //                }
    //            }
    //            return await context.SaveChangesAsync();
    //        }
    //    }

    //    #endregion Delete

    //    #region Insert

    //    public int Insert(TEntity entity)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            context.Set<TEntity>().Add(entity);
    //            return context.SaveChanges();
    //        }
    //    }

    //    public int Insert(IEnumerable<TEntity> entities)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            if (efHelper.Value.SupportsBulkInsert && entities.Count() > 20)
    //            {
    //                context.BulkInsert(entities);
    //                return entities.Count();
    //            }
    //            else
    //            {
    //                foreach (var entity in entities)
    //                {
    //                    context.Set<TEntity>().Add(entity);
    //                }
    //                return context.SaveChanges();
    //            }
    //        }
    //    }

    //    public async Task<int> InsertAsync(TEntity entity)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            context.Set<TEntity>().Add(entity);
    //            return await context.SaveChangesAsync();
    //        }
    //    }

    //    public async Task<int> InsertAsync(IEnumerable<TEntity> entities)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            int count = entities.Count();
    //            if (efHelper.Value.SupportsBulkInsert && count > 20)
    //            {
    //                context.BulkInsert(entities);
    //                return await Task.FromResult(count);
    //            }
    //            else
    //            {
    //                foreach (var entity in entities)
    //                {
    //                    context.Set<TEntity>().Add(entity);
    //                }
    //                return await context.SaveChangesAsync();
    //            }
    //        }
    //    }

    //    #endregion Insert

    //    #region Update

    //    public int Update(TEntity entity)
    //    {
    //        try
    //        {
    //            if (entity == null)
    //            {
    //                throw new ArgumentNullException("entity");
    //            }

    //            using (var context = CreateContext())
    //            {
    //                if (context.Entry(entity).State == EntityState.Detached)
    //                {
    //                    var localEntity = context.Set<TEntity>().Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                    if (localEntity != null)
    //                    {
    //                        context.Entry(localEntity).CurrentValues.SetValues(entity);
    //                    }
    //                    else
    //                    {
    //                        entity = context.Set<TEntity>().Attach(entity);
    //                        context.Entry(entity).State = EntityState.Modified;
    //                    }
    //                }
    //                else
    //                {
    //                    context.Entry(entity).State = EntityState.Modified;
    //                }

    //                return context.SaveChanges();
    //            }
    //        }
    //        catch (DbEntityValidationException x)
    //        {
    //            var msg = x.EntityValidationErrors
    //                .SelectMany(validationErrors => validationErrors.ValidationErrors)
    //                .Aggregate(string.Empty, (current, validationError) => current + (System.Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));

    //            logger.Error(msg, x);

    //            throw new KoreException(msg, x);
    //        }
    //    }

    //    public int Update(IEnumerable<TEntity> entities)
    //    {
    //        try
    //        {
    //            if (entities == null)
    //            {
    //                throw new ArgumentNullException("items");
    //            }

    //            using (var context = CreateContext())
    //            {
    //                foreach (var entity in entities)
    //                {
    //                    if (context.Entry(entity).State == EntityState.Detached)
    //                    {
    //                        var localEntity = context.Set<TEntity>().Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                        if (localEntity != null)
    //                        {
    //                            context.Entry(localEntity).CurrentValues.SetValues(entity);
    //                        }
    //                        else
    //                        {
    //                            context.Set<TEntity>().Attach(entity);
    //                            context.Entry(entity).State = EntityState.Modified;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        context.Entry(entity).State = EntityState.Modified;
    //                    }
    //                }
    //                return context.SaveChanges();
    //            }
    //        }
    //        catch (DbEntityValidationException x)
    //        {
    //            var msg = x.EntityValidationErrors
    //                .SelectMany(validationErrors => validationErrors.ValidationErrors)
    //                .Aggregate(string.Empty, (current, validationError) => current + (System.Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));

    //            logger.Error(msg, x);

    //            throw new KoreException(msg, x);
    //        }
    //    }

    //    public async Task<int> UpdateAsync(TEntity entity)
    //    {
    //        try
    //        {
    //            if (entity == null)
    //            {
    //                throw new ArgumentNullException("entity");
    //            }

    //            using (var context = CreateContext())
    //            {
    //                if (context.Entry(entity).State == EntityState.Detached)
    //                {
    //                    var localEntity = context.Set<TEntity>().Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                    if (localEntity != null)
    //                    {
    //                        context.Entry(localEntity).CurrentValues.SetValues(entity);
    //                    }
    //                    else
    //                    {
    //                        entity = context.Set<TEntity>().Attach(entity);
    //                        context.Entry(entity).State = EntityState.Modified;
    //                    }
    //                }
    //                else
    //                {
    //                    context.Entry(entity).State = EntityState.Modified;
    //                }

    //                return await context.SaveChangesAsync();
    //            }
    //        }
    //        catch (DbEntityValidationException x)
    //        {
    //            var msg = x.EntityValidationErrors
    //                .SelectMany(validationErrors => validationErrors.ValidationErrors)
    //                .Aggregate(string.Empty, (current, validationError) => current + (System.Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));

    //            logger.Error(msg, x);

    //            throw new KoreException(msg, x);
    //        }
    //    }

    //    public async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
    //    {
    //        try
    //        {
    //            if (entities == null)
    //            {
    //                throw new ArgumentNullException("items");
    //            }

    //            using (var context = CreateContext())
    //            {
    //                foreach (var entity in entities)
    //                {
    //                    if (context.Entry(entity).State == EntityState.Detached)
    //                    {
    //                        var localEntity = context.Set<TEntity>().Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                        if (localEntity != null)
    //                        {
    //                            context.Entry(localEntity).CurrentValues.SetValues(entity);
    //                        }
    //                        else
    //                        {
    //                            context.Set<TEntity>().Attach(entity);
    //                            context.Entry(entity).State = EntityState.Modified;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        context.Entry(entity).State = EntityState.Modified;
    //                    }
    //                }
    //                return await context.SaveChangesAsync();
    //            }
    //        }
    //        catch (DbEntityValidationException x)
    //        {
    //            var msg = x.EntityValidationErrors
    //                .SelectMany(validationErrors => validationErrors.ValidationErrors)
    //                .Aggregate(string.Empty, (current, validationError) => current + (System.Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage)));

    //            logger.Error(msg, x);

    //            throw new KoreException(msg, x);
    //        }
    //    }

    //    //public int Update(Expression<Func<TEntity, TEntity>> updateExpression)
    //    //{
    //    //    if (efHelper.Value.SupportsEFExtended)
    //    //    {
    //    //        int rowsAffected = Table.Update(updateExpression);
    //    //        RefreshAll();
    //    //        return rowsAffected;
    //    //    }
    //    //    else
    //    //    {
    //    //        throw new NotImplementedException();
    //    //    }
    //    //}

    //    //public int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
    //    //{
    //    //    if (efHelper.Value.SupportsEFExtended)
    //    //    {
    //    //        int rowsAffected = Table.Update(filterExpression, updateExpression);
    //    //        RefreshMany(Table.Where(filterExpression).ToHashSet());
    //    //        return rowsAffected;
    //    //    }
    //    //    else
    //    //    {
    //    //        throw new NotImplementedException();
    //    //    }
    //    //}

    //    //public int Update(IQueryable<TEntity> query, Expression<Func<TEntity, TEntity>> updateExpression)
    //    //{
    //    //    if (efHelper.Value.SupportsEFExtended)
    //    //    {
    //    //        int rowsAffected = Table.Update(query, updateExpression);
    //    //        RefreshAll();
    //    //        return rowsAffected;
    //    //    }
    //    //    else
    //    //    {
    //    //        throw new NotImplementedException();
    //    //    }
    //    //}

    //    #endregion Update

    //    public IEnumerable<TEntity> Translate(string storedProcedure, IEnumerable<DbParameter> parameters)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            try
    //            {
    //                var command = context.Database.Connection.CreateCommand();
    //                command.CommandText = storedProcedure;

    //                context.Database.Connection.Open();

    //                var reader = command.ExecuteReader();

    //                var objectContext = ((IObjectContextAdapter)context).ObjectContext;

    //                return objectContext.Translate<TEntity>(
    //                    reader,
    //                    context.GetEntitySetName(typeof(TEntity)),
    //                    MergeOption.AppendOnly);
    //            }
    //            finally
    //            {
    //                context.Database.Connection.Close();
    //            }
    //        }
    //    }

    //    #endregion IRepository<TEntity> Members

    //    #region Non-Public Members

    //    protected virtual void RefreshAll()
    //    {
    //        using (var context = CreateContext())
    //        {
    //            var objectContext = ((IObjectContextAdapter)context).ObjectContext;

    //            var refreshableObjects = objectContext.ObjectStateManager
    //                .GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged)
    //                .Where(x => x.EntityKey != null)
    //                .Select(x => x.Entity).ToList();

    //            objectContext.Refresh(RefreshMode.StoreWins, refreshableObjects);
    //        }
    //    }

    //    protected virtual void RefreshMany(IEnumerable collection)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
    //            objectContext.Refresh(RefreshMode.StoreWins, collection);
    //        }
    //    }

    //    protected virtual void Refresh(object entity)
    //    {
    //        using (var context = CreateContext())
    //        {
    //            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
    //            objectContext.Refresh(RefreshMode.StoreWins, entity);
    //        }
    //    }

    //    #endregion Non-Public Members
    //}
}