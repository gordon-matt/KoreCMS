using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Collections;
using Kore.Security.Membership;
using Kore.Web.Security.Membership;

namespace Kore.Web.ContentManagement.Areas.Admin.Newsletters.Controllers.Api
{
    public class SubscribersController : ODataController
    {
        private readonly IMembershipService membershipService;
        private readonly Lazy<MembershipSettings> membershipSettings;

        public SubscribersController(
            IMembershipService membershipService,
            Lazy<MembershipSettings> membershipSettings)
        {
            this.membershipService = membershipService;
            this.membershipSettings = membershipSettings;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public IQueryable<Subscriber> Get()
        {
            var userIds = membershipService
                .GetProfileEntriesByKeyAndValue(NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, "true")
                .Select(x => x.UserId);

            return membershipService.GetUsers(x => userIds.Contains(x.Id))
                .ToHashSet()
                .Select(x => new Subscriber
                {
                    UserId = x.Id,
                    Email = x.Email,
                    Name = membershipService.GetUserDisplayName(x)
                })
                .AsQueryable();
        }

        [EnableQuery]
        public SingleResult<Subscriber> Get([FromODataUri] string key)
        {
            var entity = membershipService.GetUserById(key);
            var subscriber = new Subscriber
            {
                UserId = entity.Id,
                Email = entity.Email,
                Name = entity.UserName
            };
            return SingleResult.Create(new[] { subscriber }.AsQueryable());
        }

        public IHttpActionResult Delete([FromODataUri] string key)
        {
            var entity = membershipService.GetUserById(key);
            if (entity == null)
            {
                return NotFound();
            }

            membershipService.SaveProfileEntry(key, NewsletterUserProfileProvider.Fields.SubscribeToNewsletters, "false");

            return StatusCode(HttpStatusCode.NoContent);
        }
    }

    public class Subscriber
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}