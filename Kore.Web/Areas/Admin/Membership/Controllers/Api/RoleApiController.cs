using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Castle.Core.Logging;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Threading;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Membership.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class RoleApiController : ODataController
    {
        private readonly Lazy<ILogger> logger;
        private readonly IWorkContext workContext;

        protected IMembershipService Service { get; private set; }

        public RoleApiController(IMembershipService service, Lazy<ILogger> logger, IWorkContext workContext)
        {
            this.Service = service;
            this.logger = logger;
            this.workContext = workContext;
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual async Task<IEnumerable<KoreRole>> Get(ODataQueryOptions<KoreRole> options)
        {
            if (!CheckPermission(MembershipPermissions.ReadRoles))
            {
                return Enumerable.Empty<KoreRole>().AsQueryable();
            }

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            var results = options.ApplyTo((await Service.GetAllRoles(workContext.CurrentTenant.Id)).AsQueryable());
            return (results as IQueryable<KoreRole>).ToHashSet();
        }

        [EnableQuery]
        public virtual async Task<SingleResult<KoreRole>> Get([FromODataUri] string key)
        {
            if (!CheckPermission(MembershipPermissions.ReadRoles))
            {
                return SingleResult.Create(Enumerable.Empty<KoreRole>().AsQueryable());
            }
            var entity = await Service.GetRoleById(key);
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        public virtual async Task<IHttpActionResult> Put([FromODataUri] string key, KoreRole entity)
        {
            if (!CheckPermission(MembershipPermissions.WriteRoles))
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
                await Service.UpdateRole(entity);
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

        public virtual async Task<IHttpActionResult> Post(KoreRole entity)
        {
            if (!CheckPermission(MembershipPermissions.WriteRoles))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Service.InsertRole(workContext.CurrentTenant.Id, entity);

            return Created(entity);
        }

        [AcceptVerbs("PATCH", "MERGE")]
        public virtual async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<KoreRole> patch)
        {
            if (!CheckPermission(MembershipPermissions.WriteRoles))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            KoreRole entity = await Service.GetRoleById(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                await Service.UpdateRole(entity);
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

        public virtual async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            if (!CheckPermission(MembershipPermissions.WriteRoles))
            {
                return Unauthorized();
            }

            KoreRole entity = await Service.GetRoleById(key);
            if (entity == null)
            {
                return NotFound();
            }

            await Service.DeleteRole(key);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public virtual async Task<IEnumerable<EdmKoreRole>> GetRolesForUser(ODataActionParameters parameters)
        {
            if (!CheckPermission(MembershipPermissions.ReadRoles))
            {
                return Enumerable.Empty<EdmKoreRole>().AsQueryable();
            }
            string userId = (string)parameters["userId"];
            return (await Service.GetRolesForUser(userId)).Select(x => new EdmKoreRole
            {
                Id = x.Id,
                Name = x.Name
            });
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> AssignPermissionsToRole(ODataActionParameters parameters)
        {
            if (!CheckPermission(MembershipPermissions.WriteRoles))
            {
                return Unauthorized();
            }

            string roleId = (string)parameters["roleId"];
            var permissionIds = (IEnumerable<string>)parameters["permissions"];

            await Service.AssignPermissionsToRole(roleId, permissionIds);

            return Ok();
        }

        protected virtual bool EntityExists(string key)
        {
            return AsyncHelper.RunSync(() => Service.GetUserById(key)) != null;
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