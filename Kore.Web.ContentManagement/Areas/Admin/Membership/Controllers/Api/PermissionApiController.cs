using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Security.Membership;

namespace Kore.Web.ContentManagement.Areas.Admin.Membership.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class PermissionApiController : ODataController
    {
        protected IMembershipService Service { get; private set; }

        public PermissionApiController(IMembershipService service)
        {
            this.Service = service;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IQueryable<KorePermission> Get()
        {
            return Service.GetAllPermissions().AsQueryable();
        }

        [EnableQuery]
        public virtual SingleResult<KorePermission> Get([FromODataUri] string key)
        {
            var entity = Service.GetPermissionById(key);
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        public virtual IHttpActionResult Put([FromODataUri] string key, KorePermission entity)
        {
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

        public virtual IHttpActionResult Post(KorePermission entity)
        {
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
    }

    public struct EdmKorePermission
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}