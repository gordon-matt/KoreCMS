using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Castle.Core.Logging;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Security.Membership;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Membership.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class UserApiController : ODataController
    {
        private readonly Lazy<ILogger> logger;

        protected IMembershipService Service { get; private set; }

        private readonly Lazy<MembershipSettings> membershipSettings;

        public UserApiController(
            IMembershipService service,
            Lazy<MembershipSettings> membershipSettings,
            Lazy<ILogger> logger)
        {
            this.Service = service;
            this.membershipSettings = membershipSettings;
            this.logger = logger;
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IEnumerable<KoreUser> Get(ODataQueryOptions<KoreUser> options)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<KoreUser>().AsQueryable();
            }

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            var results = options.ApplyTo(Service.GetAllUsersAsQueryable());
            return (results as IQueryable<KoreUser>).ToHashSet();
        }

        [EnableQuery]
        public virtual SingleResult<KoreUser> Get([FromODataUri] string key)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return SingleResult.Create(Enumerable.Empty<KoreUser>().AsQueryable());
            }
            var entity = Service.GetUserById(key);
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        public virtual IHttpActionResult Put([FromODataUri] string key, KoreUser entity)
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
                Service.UpdateUser(entity);
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

        public virtual IHttpActionResult Post(KoreUser entity)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string password = System.Web.Security.Membership.GeneratePassword(
                membershipSettings.Value.GeneratedPasswordLength,
                membershipSettings.Value.GeneratedPasswordNumberOfNonAlphanumericChars);

            Service.InsertUser(entity, password);

            return Created(entity);
        }

        [AcceptVerbs("PATCH", "MERGE")]
        public virtual IHttpActionResult Patch([FromODataUri] string key, Delta<KoreUser> patch)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            KoreUser entity = Service.GetUserById(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                Service.UpdateUser(entity);
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

            KoreUser entity = Service.GetUserById(key);
            if (entity == null)
            {
                return NotFound();
            }

            Service.DeleteUser(key);

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected virtual bool EntityExists(string key)
        {
            return Service.GetUserById(key) != null;
        }

        //[EnableQuery]
        [HttpPost]
        public virtual IEnumerable<KoreUser> GetUsersInRole(ODataQueryOptions<KoreUser> options, ODataActionParameters parameters)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<KoreUser>().AsQueryable();
            }
            string roleId = (string)parameters["roleId"];

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            var results = options.ApplyTo(Service.GetUsersByRoleId(roleId).AsQueryable());
            return (results as IQueryable<KoreUser>).ToHashSet();
        }

        [HttpPost]
        public virtual IHttpActionResult AssignUserToRoles(ODataActionParameters parameters)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            string userId = (string)parameters["userId"];
            var roleIds = (IEnumerable<string>)parameters["roles"];

            Service.AssignUserToRoles(userId, roleIds);

            return Ok();
        }

        [HttpPost]
        public virtual IHttpActionResult ChangePassword(ODataActionParameters parameters)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            string userId = (string)parameters["userId"];
            string password = (string)parameters["password"];

            Service.ChangePassword(userId, password);

            return Ok();
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }
}