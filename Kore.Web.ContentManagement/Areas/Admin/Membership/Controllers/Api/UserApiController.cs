﻿using System;
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
using Kore.Web.Security.Membership;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Membership.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class UserApiController : ODataController
    {
        protected IMembershipService Service { get; private set; }
        private readonly Lazy<MembershipSettings> membershipSettings;

        public UserApiController(
            IMembershipService service,
            Lazy<MembershipSettings> membershipSettings)
        {
            this.Service = service;
            this.membershipSettings = membershipSettings;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IQueryable<KoreUser> Get()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<KoreUser>().AsQueryable();
            }
            return Service.GetAllUsersAsQueryable();
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
                Service.UpdateUser(entity);
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

        public virtual IHttpActionResult Post(KoreUser entity)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
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
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
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

        [EnableQuery]
        [HttpPost]
        public virtual IQueryable<KoreUser> GetUsersInRole(ODataActionParameters parameters)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<KoreUser>().AsQueryable();
            }
            string roleId = (string)parameters["roleId"];
            return Service.GetUsersByRoleId(roleId).AsQueryable();
        }

        [HttpPost]
        public virtual IHttpActionResult AssignUserToRoles(ODataActionParameters parameters)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
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
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
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