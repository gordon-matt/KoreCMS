﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Query;
using Castle.Core.Logging;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.EntityFramework.Data;
using Kore.Infrastructure;
using Kore.Logging;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Http.OData
{
    public abstract class GenericODataController<TEntity, TKey> : ODataController 
        where TEntity : class
    {
        #region Non-Public Properties

        protected IGenericDataService<TEntity> Service { get; private set; }

        protected ILogger Logger { get; private set; }

        #endregion Non-Public Properties

        #region Constructors

        public GenericODataController(IGenericDataService<TEntity> service)
        {
            this.Service = service;
            this.Logger = LoggingUtilities.Resolve();
        }

        public GenericODataController(IRepository<TEntity> repository)
        {
            var cacheManager = EngineContext.Current.Resolve<ICacheManager>();
            this.Service = new GenericDataService<TEntity>(cacheManager, repository);
            this.Logger = LoggingUtilities.Resolve();
        }

        #endregion Constructors

        #region Public Methods

        // GET: odata/<Entity>
        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual async Task<IEnumerable<TEntity>> Get(ODataQueryOptions<TEntity> options)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<TEntity>().AsQueryable();
            }

            options.Validate(new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            });

            using (var connection = Service.OpenConnection())
            {
                var query = connection.Query();
                query = ApplyMandatoryFilter(query);
                var results = options.ApplyTo(query);
                return await (results as IQueryable<TEntity>).ToHashSetAsync();
            }
        }

        // GET: odata/<Entity>(5)
        [EnableQuery]
        public virtual async Task<SingleResult<TEntity>> Get([FromODataUri] TKey key)
        {
            if (!CheckPermission(ReadPermission))
            {
                return SingleResult.Create(Enumerable.Empty<TEntity>().AsQueryable());
            }
            var entity = await Service.FindOneAsync(key);

            // TODO: CheckPermission(ReadPermission) is getting done twice.. once above, and once in CanViewEntity(). Unnecessary... see if this can be modified
            if (!CanViewEntity(entity))
            {
                return SingleResult.Create(Enumerable.Empty<TEntity>().AsQueryable());
            }

            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        // PUT: odata/<Entity>(5)
        public virtual async Task<IHttpActionResult> Put([FromODataUri] TKey key, TEntity entity)
        {
            if (!CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(GetId(entity)))
            {
                return BadRequest();
            }

            try
            {
                OnBeforeSave(entity);
                await Service.UpdateAsync(entity);
                OnAfterSave(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                Logger.Error(x.Message, x);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        // POST: odata/<Entity>
        public virtual async Task<IHttpActionResult> Post(TEntity entity)
        {
            if (!CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SetNewId(entity);

            OnBeforeSave(entity);
            await Service.InsertAsync(entity);
            OnAfterSave(entity);

            return Created(entity);
        }

        // PATCH: odata/<Entity>(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public virtual async Task<IHttpActionResult> Patch([FromODataUri] TKey key, Delta<TEntity> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TEntity entity = await Service.FindOneAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            if (!CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            patch.Patch(entity);

            try
            {
                await Service.UpdateAsync(entity);
                //db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException x)
            {
                Logger.Error(x.Message, x);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        // DELETE: odata/<Entity>(5)
        public virtual async Task<IHttpActionResult> Delete([FromODataUri] TKey key)
        {
            TEntity entity = await Service.FindOneAsync(key);

            if (entity == null)
            {
                return NotFound();
            }

            if (!CanModifyEntity(entity))
            {
                return Unauthorized();
            }

            await Service.DeleteAsync(entity);

            return StatusCode(HttpStatusCode.NoContent);
        }

        #endregion Public Methods

        #region Non-Public Methods

        protected virtual bool EntityExists(TKey key)
        {
            return Service.FindOne(key) != null;
        }

        protected abstract TKey GetId(TEntity entity);

        /// <summary>
        /// Should only be necessary for Guid types
        /// </summary>
        /// <param name="entity"></param>
        protected abstract void SetNewId(TEntity entity);

        /// <summary>
        /// Add any filters which must be applied for the client. Mostly used for fields such as "TenantId", where you don't want
        /// the user viewing data for a different site (tenant)
        /// </summary>
        /// <param name="entity"></param>
        protected virtual IQueryable<TEntity> ApplyMandatoryFilter(IQueryable<TEntity> query)
        {
            // Do nothing, by default
            return query;
        }

        protected virtual bool CanViewEntity(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            return CheckPermission(ReadPermission);
        }

        protected virtual bool CanModifyEntity(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            return CheckPermission(WritePermission);
        }

        protected virtual void OnBeforeSave(TEntity entity)
        {
        }

        protected virtual void OnAfterSave(TEntity entity)
        {
        }

        protected static bool CheckPermission(Permission permission)
        {
            if (permission == null)
            {
                return true;
            }

            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }

        protected abstract Permission ReadPermission { get; }

        protected abstract Permission WritePermission { get; }

        protected virtual IHttpActionResult Ok(object content, Type type)
        {
            var resultType = typeof(OkNegotiatedContentResult<>).MakeGenericType(type);
            return Activator.CreateInstance(resultType, content, this) as IHttpActionResult;
        }

        #endregion Non-Public Methods
    }
}