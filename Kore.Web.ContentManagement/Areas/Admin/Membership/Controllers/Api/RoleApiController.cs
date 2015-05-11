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
            return Service.GetAllRoles().AsQueryable();
        }

        [EnableQuery]
        public virtual SingleResult<KoreRole> Get([FromODataUri] string key)
        {
            var entity = Service.GetRoleById(key);
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        public virtual IHttpActionResult Put([FromODataUri] string key, KoreRole entity)
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
            string roleId = (string)parameters["roleId"];
            var permissionIds = (IEnumerable<string>)parameters["permissions"];

            Service.AssignPermissionsToRole(roleId, permissionIds);

            return Ok();
        }

        protected virtual bool EntityExists(string key)
        {
            return Service.GetUserById(key) != null;
        }
    }

    public struct EdmKoreRole
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}