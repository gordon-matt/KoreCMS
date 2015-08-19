using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Kore.Collections;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class BlogPostApiController : GenericODataController<BlogPost, Guid>
    {
        private readonly Lazy<IMembershipService> membershipService;
        private readonly Lazy<IBlogPostTagService> postTagService;

        public BlogPostApiController(
            IBlogPostService service,
            Lazy<IMembershipService> membershipService,
            Lazy<IBlogPostTagService> postTagService)
            : base(service)
        {
            this.membershipService = membershipService;
            this.postTagService = postTagService;
        }

        [AllowAnonymous]
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<BlogPost> Get()
        {
            return base.Get();
        }

        [AllowAnonymous]
        [EnableQuery]
        public override SingleResult<BlogPost> Get([FromODataUri] Guid key)
        {
            return base.Get(key);
        }

        public override IHttpActionResult Post(BlogPost entity)
        {
            entity.DateCreatedUtc = DateTime.UtcNow;
            entity.UserId = membershipService.Value.GetUserByName(User.Identity.Name).Id;

            var tags = entity.Tags;
            entity.Tags = null;

            var result = base.Post(entity);

            if (!tags.IsNullOrEmpty())
            {
                var toInsert = tags.Select(x => new BlogPostTag
                {
                    PostId = entity.Id,
                    TagId = x.TagId
                });
                postTagService.Value.Insert(toInsert);
            }

            return result;
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, BlogPost entity)
        {
            var currentEntry = Service.FindOne(entity.Id);
            entity.UserId = currentEntry.UserId;
            entity.DateCreatedUtc = currentEntry.DateCreatedUtc;
            var result = base.Put(key, entity);

            if (!entity.Tags.IsNullOrEmpty())
            {
                var chosenTagIds = entity.Tags.Select(x => x.TagId);
                var existingTags = postTagService.Value.Find(x => x.PostId == entity.Id);
                var existingTagIds = existingTags.Select(x => x.TagId);

                var toDelete = existingTags.Where(x => !chosenTagIds.Contains(x.TagId));
                var toInsert = chosenTagIds.Where(x => !existingTagIds.Contains(x)).Select(x => new BlogPostTag
                {
                    PostId = entity.Id,
                    TagId = x
                });

                postTagService.Value.Delete(toDelete);
                postTagService.Value.Insert(toInsert);
            }

            return result;
        }

        protected override Guid GetId(BlogPost entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(BlogPost entity)
        {
            entity.Id = Guid.NewGuid();
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.BlogRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.BlogWrite; }
        }
    }
}