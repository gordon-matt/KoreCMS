using System;
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
    //public class EntityFrameworkRepository<TEntity> : IRepository<TEntity>, IDisposable
    //    where TEntity : class, IEntity
    //{
    //    #region Private Members

    //    private bool disposed;
    //    private DbContext context;
    //    private readonly IDbContextFactory contextFactory;
    //    private readonly ILogger logger;
    //    private readonly Lazy<IKoreEntityFrameworkHelper> efHelper;

    //    #endregion Private Members

    //    #region Properties

    //    public DbContext Context
    //    {
    //        get
    //        {
    //            if (context == null)
    //            {
    //                context = contextFactory.GetContext();
    //            }
    //            return context;
    //        }
    //    }

    //    protected DbSet<TEntity> Set
    //    {
    //        get { return Context.Set<TEntity>(); }
    //    }

    //    #endregion Properties

    //    #region Constructor / Destructor

    //    public EntityFrameworkRepository(IDbContextFactory contextFactory, Lazy<IKoreEntityFrameworkHelper> efHelper)
    //    {
    //        this.contextFactory = contextFactory;
    //        this.efHelper = efHelper;
    //        this.logger = LoggingUtilities.Resolve();
    //    }

    //    ~EntityFrameworkRepository()
    //    {
    //        Dispose(false);
    //    }

    //    #endregion Constructor / Destructor

    //    #region IRepository<TEntity> Members

    //    public IQueryable<TEntity> Table
    //    {
    //        get { return Set.AsNoTracking(); }
    //    }

    //    #region Find

    //    public IEnumerable<TEntity> Find()
    //    {
    //        return Table.ToHashSet();
    //    }

    //    public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        return Table.Where(filterExpression).ToHashSet();
    //    }

    //    public async Task<IEnumerable<TEntity>> FindAsync()
    //    {
    //        return await Table.ToHashSetAsync();
    //    }

    //    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        return await Table.Where(filterExpression).ToHashSetAsync();
    //    }

    //    public TEntity FindOne(params object[] keyValues)
    //    {
    //        return Set.Find(keyValues);
    //    }

    //    public TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        return Table.FirstOrDefault(filterExpression);
    //    }

    //    public async Task<TEntity> FindOneAsync(params object[] keyValues)
    //    {
    //        return await Set.FindAsync(keyValues);
    //    }

    //    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
    //    {
    //        return await Table.FirstOrDefaultAsync(filterExpression);
    //    }

    //    #endregion Find

    //    #region Count

    //    public int Count()
    //    {
    //        return Table.Count();
    //    }

    //    public int Count(Expression<Func<TEntity, bool>> countExpression)
    //    {
    //        return Table.Count(countExpression);
    //    }

    //    public async Task<int> CountAsync()
    //    {
    //        return await Table.CountAsync();
    //    }

    //    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> countExpression)
    //    {
    //        return await Table.CountAsync(countExpression);
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
    //        if (Context.Entry(entity).State == EntityState.Detached)
    //        {
    //            var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //            if (localEntity != null)
    //            {
    //                Context.Entry(localEntity).State = EntityState.Deleted;
    //            }
    //            else
    //            {
    //                Set.Attach(entity);
    //                Context.Entry(entity).State = EntityState.Deleted;
    //            }
    //        }
    //        else
    //        {
    //            Set.Remove(entity);
    //        }
    //        return Context.SaveChanges();
    //    }

    //    public int Delete(IEnumerable<TEntity> entities)
    //    {
    //        foreach (var entity in entities)
    //        {
    //            if (Context.Entry(entity).State == EntityState.Detached)
    //            {
    //                var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                if (localEntity != null)
    //                {
    //                    Context.Entry(localEntity).State = EntityState.Deleted;
    //                }
    //                else
    //                {
    //                    Set.Attach(entity);
    //                    Context.Entry(entity).State = EntityState.Deleted;
    //                }
    //            }
    //            else
    //            {
    //                Set.Remove(entity);
    //            }
    //        }
    //        return Context.SaveChanges();
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
    //        if (Context.Entry(entity).State == EntityState.Detached)
    //        {
    //            var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //            if (localEntity != null)
    //            {
    //                Context.Entry(localEntity).State = EntityState.Deleted;
    //            }
    //            else
    //            {
    //                Set.Attach(entity);
    //                Context.Entry(entity).State = EntityState.Deleted;
    //            }
    //        }
    //        else
    //        {
    //            Set.Remove(entity);
    //        }
    //        return await Context.SaveChangesAsync();
    //    }

    //    public async Task<int> DeleteAsync(IEnumerable<TEntity> entities)
    //    {
    //        foreach (var entity in entities)
    //        {
    //            if (Context.Entry(entity).State == EntityState.Detached)
    //            {
    //                var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                if (localEntity != null)
    //                {
    //                    Context.Entry(localEntity).State = EntityState.Deleted;
    //                }
    //                else
    //                {
    //                    Set.Attach(entity);
    //                    Context.Entry(entity).State = EntityState.Deleted;
    //                }
    //            }
    //            else
    //            {
    //                Set.Remove(entity);
    //            }
    //        }
    //        return await Context.SaveChangesAsync();
    //    }

    //    #endregion Delete

    //    #region Insert

    //    public int Insert(TEntity entity)
    //    {
    //        Set.Add(entity);
    //        return Context.SaveChanges();
    //    }

    //    public int Insert(IEnumerable<TEntity> entities)
    //    {
    //        if (efHelper.Value.SupportsBulkInsert && entities.Count() > 20)
    //        {
    //            Context.BulkInsert(entities);
    //            return entities.Count();
    //        }
    //        else
    //        {
    //            foreach (var entity in entities)
    //            {
    //                Set.Add(entity);
    //            }
    //            return Context.SaveChanges();
    //        }
    //    }

    //    public async Task<int> InsertAsync(TEntity entity)
    //    {
    //        Set.Add(entity);
    //        return await Context.SaveChangesAsync();
    //    }

    //    public async Task<int> InsertAsync(IEnumerable<TEntity> entities)
    //    {
    //        int count = entities.Count();
    //        if (efHelper.Value.SupportsBulkInsert && count > 20)
    //        {
    //            Context.BulkInsert(entities);
    //            return await Task.FromResult(count);
    //        }
    //        else
    //        {
    //            foreach (var entity in entities)
    //            {
    //                Set.Add(entity);
    //            }
    //            return await Context.SaveChangesAsync();
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

    //            if (Context.Entry(entity).State == EntityState.Detached)
    //            {
    //                var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                if (localEntity != null)
    //                {
    //                    Context.Entry(localEntity).CurrentValues.SetValues(entity);
    //                }
    //                else
    //                {
    //                    entity = Set.Attach(entity);
    //                    Context.Entry(entity).State = EntityState.Modified;
    //                }
    //            }
    //            else
    //            {
    //                Context.Entry(entity).State = EntityState.Modified;
    //            }

    //            return Context.SaveChanges();
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

    //            foreach (var entity in entities)
    //            {
    //                if (Context.Entry(entity).State == EntityState.Detached)
    //                {
    //                    var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                    if (localEntity != null)
    //                    {
    //                        Context.Entry(localEntity).CurrentValues.SetValues(entity);
    //                    }
    //                    else
    //                    {
    //                        Set.Attach(entity);
    //                        Context.Entry(entity).State = EntityState.Modified;
    //                    }
    //                }
    //                else
    //                {
    //                    Context.Entry(entity).State = EntityState.Modified;
    //                }
    //            }
    //            return Context.SaveChanges();
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

    //            if (Context.Entry(entity).State == EntityState.Detached)
    //            {
    //                var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                if (localEntity != null)
    //                {
    //                    Context.Entry(localEntity).CurrentValues.SetValues(entity);
    //                }
    //                else
    //                {
    //                    entity = Set.Attach(entity);
    //                    Context.Entry(entity).State = EntityState.Modified;
    //                }
    //            }
    //            else
    //            {
    //                Context.Entry(entity).State = EntityState.Modified;
    //            }

    //            return await Context.SaveChangesAsync();
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

    //            foreach (var entity in entities)
    //            {
    //                if (Context.Entry(entity).State == EntityState.Detached)
    //                {
    //                    var localEntity = Set.Local.FirstOrDefault(x => x.KeyValues.ArrayEquals(entity.KeyValues));

    //                    if (localEntity != null)
    //                    {
    //                        Context.Entry(localEntity).CurrentValues.SetValues(entity);
    //                    }
    //                    else
    //                    {
    //                        Set.Attach(entity);
    //                        Context.Entry(entity).State = EntityState.Modified;
    //                    }
    //                }
    //                else
    //                {
    //                    Context.Entry(entity).State = EntityState.Modified;
    //                }
    //            }
    //            return await Context.SaveChangesAsync();
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
    //        try
    //        {
    //            var command = this.Context.Database.Connection.CreateCommand();
    //            command.CommandText = storedProcedure;

    //            this.Context.Database.Connection.Open();

    //            var reader = command.ExecuteReader();

    //            return ObjectContext.Translate<TEntity>(
    //                reader,
    //                Context.GetEntitySetName(typeof(TEntity)),
    //                MergeOption.AppendOnly);
    //        }
    //        finally
    //        {
    //            this.Context.Database.Connection.Close();
    //        }
    //    }

    //    #endregion IRepository<TEntity> Members

    //    #region IDisposable Members

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (disposed)
    //        {
    //            return;
    //        }

    //        if (disposing)
    //        {
    //            if (context != null)
    //            {
    //                context.Dispose();
    //                context = null;
    //            }
    //        }

    //        disposed = true;
    //    }

    //    #endregion IDisposable Members

    //    #region Non-Public Members

    //    protected ObjectContext ObjectContext
    //    {
    //        get { return ((IObjectContextAdapter)Context).ObjectContext; }
    //    }

    //    protected virtual void RefreshAll()
    //    {
    //        var refreshableObjects = ObjectContext.ObjectStateManager
    //            .GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged)
    //            .Where(x => x.EntityKey != null)
    //            .Select(x => x.Entity).ToList();

    //        ObjectContext.Refresh(RefreshMode.StoreWins, refreshableObjects);
    //    }

    //    protected virtual void RefreshMany(IEnumerable collection)
    //    {
    //        ObjectContext.Refresh(RefreshMode.StoreWins, collection);
    //    }

    //    protected virtual void Refresh(object entity)
    //    {
    //        ObjectContext.Refresh(RefreshMode.StoreWins, entity);
    //    }

    //    #endregion Non-Public Members
    //}

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

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> filterExpression)
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().AsNoTracking().Where(filterExpression).ToHashSet();
            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync()
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().ToHashSetAsync();
            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().Where(filterExpression).ToHashSetAsync();
            }
        }

        public TEntity FindOne(params object[] keyValues)
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().Find(keyValues);
            }
        }

        public TEntity FindOne(Expression<Func<TEntity, bool>> filterExpression)
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().AsNoTracking().FirstOrDefault(filterExpression);
            }
        }

        public async Task<TEntity> FindOneAsync(params object[] keyValues)
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().FindAsync(keyValues);
            }
        }

        public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filterExpression);
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

        public int Count(Expression<Func<TEntity, bool>> countExpression)
        {
            using (var context = contextFactory.GetContext())
            {
                return context.Set<TEntity>().AsNoTracking().Count(countExpression);
            }
        }

        public async Task<int> CountAsync()
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().CountAsync();
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> countExpression)
        {
            using (var context = contextFactory.GetContext())
            {
                return await context.Set<TEntity>().AsNoTracking().CountAsync(countExpression);
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

        public int Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>().AsNoTracking();

                if (efHelper.Value.SupportsEFExtended)
                {
                    int rowsAffected = set.Where(filterExpression).Delete();
                    //RefreshMany(set.Where(filterExpression).ToHashSet());
                    return rowsAffected;
                }
                else
                {
                    // TODO: This will cause out-of-memory exceptions with tables that have too many records. We need a better solution!
                    //  Change this to use a while loop and use Skip() and Take() to get paged results to delete.
                    var entities = set.Where(filterExpression).ToHashSet();
                    return Delete(entities);
                }
            }
        }

        public int Delete(IQueryable<TEntity> query)
        {
            if (efHelper.Value.SupportsEFExtended)
            {
                int rowsAffected = query.Delete();
                //RefreshAll();
                return rowsAffected;
            }
            else
            {
                // TODO: This will cause out-of-memory exceptions with tables that have too many records. We need a better solution!
                //  Change this to use a while loop and use Skip() and Take() to get paged results to delete.
                var entities = query.ToHashSet();
                return Delete(entities);
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

        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            using (var context = contextFactory.GetContext())
            {
                var set = context.Set<TEntity>().AsNoTracking();

                if (efHelper.Value.SupportsEFExtended)
                {
                    int rowsAffected = set.Where(filterExpression).Delete();
                    //RefreshMany(Table.Where(filterExpression).ToHashSet());
                    return await Task.FromResult(rowsAffected);
                }
                else
                {
                    // TODO: This will cause out-of-memory exceptions with tables that have too many records. We need a better solution!
                    //  Change this to use a while loop and use Skip() and Take() to get paged results to delete.
                    var entities = set.Where(filterExpression).ToHashSet();
                    return await DeleteAsync(entities);
                }
            }
        }

        public async Task<int> DeleteAsync(IQueryable<TEntity> query)
        {
            if (efHelper.Value.SupportsEFExtended)
            {
                int rowsAffected = query.Delete();
                //RefreshAll();
                return await Task.FromResult(rowsAffected);
            }
            else
            {
                // TODO: This will cause out-of-memory exceptions with tables that have too many records. We need a better solution!
                //  Change this to use a while loop and use Skip() and Take() to get paged results to delete.
                var entities = query.ToHashSet();
                return await DeleteAsync(entities);
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
                if (efHelper.Value.SupportsBulkInsert && entities.Count() > 20)
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

        public IEnumerable<TEntity> Translate(string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            using (var context = contextFactory.GetContext())
            {
                try
                {
                    var command = context.Database.Connection.CreateCommand();
                    command.CommandText = storedProcedure;

                    context.Database.Connection.Open();

                    var reader = command.ExecuteReader();

                    return ((IObjectContextAdapter)context).ObjectContext.Translate<TEntity>(
                        reader,
                        context.GetEntitySetName(typeof(TEntity)),
                        MergeOption.AppendOnly);
                }
                finally
                {
                    context.Database.Connection.Close();
                }
            }
        }

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