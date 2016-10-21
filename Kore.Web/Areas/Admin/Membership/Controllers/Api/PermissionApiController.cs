﻿using System;
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
    public class PermissionApiController : ODataController
    {
        private readonly Lazy<ILogger> logger;
        private readonly IWorkContext workContext;

        protected IMembershipService Service { get; private set; }

        public PermissionApiController(IMembershipService service, Lazy<ILogger> logger, IWorkContext workContext)
        {
            this.Service = service;
            this.logger = logger;
            this.workContext = workContext;
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual async Task<IEnumerable<KorePermission>> Get(ODataQueryOptions<KorePermission> options)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<KorePermission>().AsQueryable();
            }

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            var results = options.ApplyTo((await Service.GetAllPermissions(workContext.CurrentTenant.Id)).AsQueryable());
            return (results as IQueryable<KorePermission>).ToHashSet();
        }

        [EnableQuery]
        public virtual async Task<SingleResult<KorePermission>> Get([FromODataUri] string key)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return SingleResult.Create(Enumerable.Empty<KorePermission>().AsQueryable());
            }
            var entity = await Service.GetPermissionById(key);
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        public virtual async Task<IHttpActionResult> Put([FromODataUri] string key, KorePermission entity)
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
                await Service.UpdatePermission(entity);
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

        public virtual async Task<IHttpActionResult> Post(KorePermission entity)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await Service.InsertPermission(workContext.CurrentTenant.Id, entity);

            return Created(entity);
        }

        [AcceptVerbs("PATCH", "MERGE")]
        public virtual async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<KorePermission> patch)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            KorePermission entity = await Service.GetPermissionById(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                await Service.UpdatePermission(entity);
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
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            KorePermission entity = await Service.GetPermissionById(key);
            if (entity == null)
            {
                return NotFound();
            }

            await Service.DeletePermission(key);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public virtual async Task<IEnumerable<EdmKorePermission>> GetPermissionsForRole(ODataActionParameters parameters)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<EdmKorePermission>().AsQueryable();
            }
            string roleId = (string)parameters["roleId"];
            var role = await Service.GetRoleById(roleId);
            return (await Service.GetPermissionsForRole(workContext.CurrentTenant.Id, role.Name)).Select(x => new EdmKorePermission
            {
                Id = x.Id,
                Name = x.Name
            });
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

    public struct EdmKorePermission
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}