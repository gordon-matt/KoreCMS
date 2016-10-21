﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Kore.EntityFramework.Data;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Security.Membership;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters.Controllers.Api
{
    public class SubscriberApiController : ODataController
    {
        private readonly IMembershipService membershipService;
        private readonly Lazy<MembershipSettings> membershipSettings;
        private readonly IWorkContext workContext;

        public SubscriberApiController(
            IMembershipService membershipService,
            Lazy<MembershipSettings> membershipSettings,
            IWorkContext workContext)
        {
            this.membershipService = membershipService;
            this.membershipSettings = membershipSettings;
            this.workContext = workContext;
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public async Task<IEnumerable<Subscriber>> Get(ODataQueryOptions<Subscriber> options)
        {
            if (!CheckPermission(CmsPermissions.NewsletterRead))
            {
                return Enumerable.Empty<Subscriber>().AsQueryable();
            }

            var userIds = (await membershipService
                .GetProfileEntriesByKeyAndValue(workContext.CurrentTenant.Id, NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, "true"))
                .Select(x => x.UserId);

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            var users = await membershipService.GetUsers(workContext.CurrentTenant.Id, x => userIds.Contains(x.Id));

            var tasks = users
                .Select(async x => new Subscriber
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = await membershipService.GetUserDisplayName(x)
                });

            var subscribers = await Task.WhenAll(tasks);

            var query = subscribers
                .OrderBy(x => x.Name)
                .AsQueryable();

            var results = options.ApplyTo(query);
            return await (results as IQueryable<Subscriber>).ToHashSetAsync();
        }

        [EnableQuery]
        public async Task<SingleResult<Subscriber>> Get([FromODataUri] string key)
        {
            if (!CheckPermission(CmsPermissions.NewsletterRead))
            {
                return SingleResult.Create(Enumerable.Empty<Subscriber>().AsQueryable());
            }

            var entity = await membershipService.GetUserById(key);
            var subscriber = new Subscriber
            {
                Id = entity.Id,
                Email = entity.Email,
                Name = entity.UserName
            };
            return SingleResult.Create(new[] { subscriber }.AsQueryable());
        }

        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            if (!CheckPermission(CmsPermissions.NewsletterWrite))
            {
                return Unauthorized();
            }

            var entity = await membershipService.GetUserById(key);
            if (entity == null)
            {
                return NotFound();
            }

            await membershipService.SaveProfileEntry(key, NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, "false");

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }

    public class Subscriber
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}