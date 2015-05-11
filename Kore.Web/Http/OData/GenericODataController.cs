using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Http.OData
{
    public abstract class GenericODataController<TEntity, TKey> : ODataController where TEntity : class
    {
        protected IRepository<TEntity> Repository { get; private set; }

        public GenericODataController(IRepository<TEntity> repository)
        {
            this.Repository = repository;
        }

        // GET: odata/<Entity>
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IQueryable<TEntity> Get()
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<TEntity>().AsQueryable();
            }
            return Repository.Table;
        }

        // GET: odata/<Entity>(5)
        [EnableQuery]
        public virtual SingleResult<TEntity> Get([FromODataUri] TKey key)
        {
            if (!CheckPermission(ReadPermission))
            {
                return SingleResult.Create(Enumerable.Empty<TEntity>().AsQueryable());
            }
            var entity = Repository.Find(key);
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        // PUT: odata/<Entity>(5)
        public virtual IHttpActionResult Put([FromODataUri] TKey key, TEntity entity)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
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
                Repository.Update(entity);
                OnAfterSave(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
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
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SetNewId(entity);

            OnBeforeSave(entity);
            Repository.Insert(entity);
            OnAfterSave(entity);

            return Created(entity);
        }

        // PATCH: odata/<Entity>(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public virtual IHttpActionResult Patch([FromODataUri] TKey key, Delta<TEntity> patch)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TEntity entity = Repository.Find(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                Repository.Update(entity);
                //db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
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
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            TEntity entity = Repository.Find(key);
            if (entity == null)
            {
                return NotFound();
            }

            Repository.Delete(entity);

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected virtual bool EntityExists(TKey key)
        {
            return Repository.Find(key) != null;
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
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }

        protected abstract Permission ReadPermission { get; }

        protected abstract Permission WritePermission { get; }
    }
}