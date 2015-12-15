using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.Http.Results;
using Castle.Core.Logging;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Membership.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class PermissionApiController : ODataController
    {
        private readonly Lazy<ILogger> logger;

        protected IMembershipService Service { get; private set; }

        public PermissionApiController(IMembershipService service, Lazy<ILogger> logger)
        {
            this.Service = service;
            this.logger = logger;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IQueryable<KorePermission> Get()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<KorePermission>().AsQueryable();
            }
            return Service.GetAllPermissions().AsQueryable();
        }

        [EnableQuery]
        public virtual SingleResult<KorePermission> Get([FromODataUri] string key)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return SingleResult.Create(Enumerable.Empty<KorePermission>().AsQueryable());
            }
            var entity = Service.GetPermissionById(key);
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        public virtual IHttpActionResult Put([FromODataUri] string key, KorePermission entity)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(entity.Id))
            {
                return BadRequest();
            }

            try
            {
                Service.UpdatePermission(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                logger.Value.Error(x.Message, x);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        public virtual IHttpActionResult Post(KorePermission entity)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Service.InsertPermission(entity);

            return Created(entity);
        }

        [AcceptVerbs("PATCH", "MERGE")]
        public virtual IHttpActionResult Patch([FromODataUri] string key, Delta<KorePermission> patch)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            KorePermission entity = Service.GetPermissionById(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                Service.UpdatePermission(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                logger.Value.Error(x.Message, x);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        public virtual IHttpActionResult Delete([FromODataUri] string key)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            KorePermission entity = Service.GetPermissionById(key);
            if (entity == null)
            {
                return NotFound();
            }

            Service.DeletePermission(key);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public virtual IEnumerable<EdmKorePermission> GetPermissionsForRole(ODataActionParameters parameters)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<EdmKorePermission>().AsQueryable();
            }
            string roleId = (string)parameters["roleId"];
            var role = Service.GetRoleById(roleId);
            return Service.GetPermissionsForRole(role.Name).Select(x => new EdmKorePermission
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        protected virtual bool EntityExists(string key)
        {
            return Service.GetUserById(key) != null;
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }

    public struct EdmKorePermission
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}