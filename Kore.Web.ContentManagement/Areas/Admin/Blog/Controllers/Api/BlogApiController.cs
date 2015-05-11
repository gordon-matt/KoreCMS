using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Data;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class BlogApiController : GenericODataController<BlogEntry, Guid>
    {
        private readonly Lazy<IMembershipService> membershipService;

        public BlogApiController(
            IRepository<BlogEntry> repository,
            Lazy<IMembershipService> membershipService)
            : base(repository)
        {
            this.membershipService = membershipService;
        }

        [AllowAnonymous]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<BlogEntry> Get()
        {
            return base.Get();
        }

        [AllowAnonymous]
        [EnableQuery]
        public override SingleResult<BlogEntry> Get([FromODataUri] Guid key)
        {
            return base.Get(key);
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