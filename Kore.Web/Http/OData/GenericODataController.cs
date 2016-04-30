using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Query;
using Castle.Core.Logging;
using Kore.Caching;
using Kore.Collections;
using Kore.Data;
using Kore.Data.Services;
using Kore.Infrastructure;
using Kore.Logging;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Http.OData
{
    public abstract class GenericODataController<TEntity, TKey> : ODataController where TEntity : class
    {
        protected IGenericDataService<TEntity> Service { get; private set; }

        protected ILogger Logger { get; private set; }

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

        // GET: odata/<Entity>
        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IEnumerable<TEntity> Get(ODataQueryOptions<TEntity> options)
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
                var results = options.ApplyTo(connection.Query());
                return (results as IQueryable<TEntity>).ToHashSet();
            }
        }

        // GET: odata/<Entity>(5)
        [EnableQuery]
        public virtual SingleResult<TEntity> Get([FromODataUri] TKey key)
        {
            if (!CheckPermission(ReadPermission))
            {
                return SingleResult.Create(Enumerable.Empty<TEntity>().AsQueryable());
            }
            var entity = Service.FindOne(key);
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        // PUT: odata/<Entity>(5)
        public virtual IHttpActionResult Put([FromODataUri] TKey key, TEntity entity)
        {
            if (!CheckPermission(WritePermission))
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
                Service.Update(entity);
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
        public virtual IHttpActionResult Post(TEntity entity)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SetNewId(entity);

            OnBeforeSave(entity);
            Service.Insert(entity);
            OnAfterSave(entity);

            return Created(entity);
        }

        // PATCH: odata/<Entity>(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public virtual IHttpActionResult Patch([FromODataUri] TKey key, Delta<TEntity> patch)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TEntity entity = Service.FindOne(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                Service.Update(entity);
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
        public virtual IHttpActionResult Delete([FromODataUri] TKey key)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            TEntity entity = Service.FindOne(key);
            if (entity == null)
            {
                return NotFound();
            }

            Service.Delete(entity);

            return StatusCode(HttpStatusCode.NoContent);
        }

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
    }
}