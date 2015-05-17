using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Kore.Collections;
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

        public SubscriberApiController(
            IMembershipService membershipService,
            Lazy<MembershipSettings> membershipSettings)
        {
            this.membershipService = membershipService;
            this.membershipSettings = membershipSettings;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<Subscriber> Get()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<Subscriber>().AsQueryable();
            }

            var userIds = membershipService
                .GetProfileEntriesByKeyAndValue(NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, "true")
                .Select(x => x.UserId);

            return membershipService.GetUsers(x => userIds.Contains(x.Id))
                .ToHashSet()
                .Select(x => new Subscriber
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = membershipService.GetUserDisplayName(x)
                })
                .OrderBy(x => x.Name)
                .AsQueryable();
        }

        [EnableQuery]
        public SingleResult<Subscriber> Get([FromODataUri] string key)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return SingleResult.Create(Enumerable.Empty<Subscriber>().AsQueryable());
            }

            var entity = membershipService.GetUserById(key);
            var subscriber = new Subscriber
            {
                Id = entity.Id,
                Email = entity.Email,
                Name = entity.UserName
            };
            return SingleResult.Create(new[] { subscriber }.AsQueryable());
        }

        public IHttpActionResult Delete([FromODataUri] string key)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            var entity = membershipService.GetUserById(key);
            if (entity == null)
            {
                return NotFound();
            }

            membershipService.SaveProfileEntry(key, NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, "false");

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