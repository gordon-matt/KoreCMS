using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Membership.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class RoleApiController : ODataController
    {
        protected IMembershipService Service { get; private set; }

        public RoleApiController(IMembershipService service)
        {
            this.Service = service;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IQueryable<KoreRole> Get()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<KoreRole>().AsQueryable();
            }
            return Service.GetAllRoles().AsQueryable();
        }

        [EnableQuery]
        public virtual SingleResult<KoreRole> Get([FromODataUri] string key)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return SingleResult.Create(Enumerable.Empty<KoreRole>().AsQueryable());
            }
            var entity = Service.GetRoleById(key);
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        public virtual IHttpActionResult Put([FromODataUri] string key, KoreRole entity)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
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
                Service.UpdateRole(entity);
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

        public virtual IHttpActionResult Post(KoreRole entity)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Service.InsertRole(entity);

            return Created(entity);
        }

        [AcceptVerbs("PATCH", "MERGE")]
        public virtual IHttpActionResult Patch([FromODataUri] string key, Delta<KoreRole> patch)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            KoreRole entity = Service.GetRoleById(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                Service.UpdateRole(entity);
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

        public virtual IHttpActionResult Delete([FromODataUri] string key)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            KoreRole entity = Service.GetRoleById(key);
            if (entity == null)
            {
                return NotFound();
            }

            Service.DeleteRole(key);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public virtual IEnumerable<EdmKoreRole> GetRolesForUser(ODataActionParameters parameters)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<EdmKoreRole>().AsQueryable();
            }
            string userId = (string)parameters["userId"];
            return Service.GetRolesForUser(userId).Select(x => new EdmKoreRole
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        [HttpPost]
        public virtual IHttpActionResult AssignPermissionsToRole(ODataActionParameters parameters)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            string roleId = (string)parameters["roleId"];
            var permissionIds = (IEnumerable<string>)parameters["permissions"];

            Service.AssignPermissionsToRole(roleId, permissionIds);

            return Ok();
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

    public struct EdmKoreRole
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}