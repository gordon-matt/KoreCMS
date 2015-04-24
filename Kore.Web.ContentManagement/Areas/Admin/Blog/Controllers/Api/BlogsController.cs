using System;
using System.Web.Http;
using System.Web.Http.OData;
using Kore.Data;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class BlogsController : GenericODataController<BlogEntry, Guid>
    {
        private readonly Lazy<IMembershipService> membershipService;

        public BlogsController(
            IRepository<BlogEntry> repository,
            Lazy<IMembershipService> membershipService)
            : base(repository)
        {
            this.membershipService = membershipService;
        }

        public override IHttpActionResult Post(BlogEntry entity)
        {
            entity.DateCreated = DateTime.UtcNow;
            entity.UserId = membershipService.Value.GetUserByName(User.Identity.Name).Id;
            return base.Post(entity);
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, BlogEntry entity)
        {
            var currentEntry = Repository.Find(entity.Id);
            entity.UserId = currentEntry.UserId;
            entity.DateCreated = currentEntry.DateCreated;
            return base.Put(key, entity);
        }

        protected override Guid GetId(BlogEntry entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(BlogEntry entity)
        {
            entity.Id = Guid.NewGuid();
        }
    }
}