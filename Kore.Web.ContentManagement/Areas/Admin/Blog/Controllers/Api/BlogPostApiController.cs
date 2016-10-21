using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IWorkContext workContext;

        public BlogPostApiController(
            IBlogPostService service,
            Lazy<IMembershipService> membershipService,
            Lazy<IBlogPostTagService> postTagService,
            IWorkContext workContext)
            : base(service)
        {
            this.membershipService = membershipService;
            this.postTagService = postTagService;
            this.workContext = workContext;
        }

        [AllowAnonymous]
        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override async Task<IEnumerable<BlogPost>> Get(ODataQueryOptions<BlogPost> options)
        {
            return await base.Get(options);
        }

        [AllowAnonymous]
        [EnableQuery]
        public override async Task<SingleResult<BlogPost>> Get([FromODataUri] Guid key)
        {
            return await base.Get(key);
        }

        public override async Task<IHttpActionResult> Post(BlogPost entity)
        {
            entity.DateCreatedUtc = DateTime.UtcNow;
            entity.UserId = (await membershipService.Value.GetUserByName(workContext.CurrentTenant.Id, User.Identity.Name)).Id;

            var tags = entity.Tags;
            entity.Tags = null;

            var result = await base.Post(entity);

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

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, BlogPost entity)
        {
            var currentEntry = await Service.FindOneAsync(entity.Id);
            entity.UserId = currentEntry.UserId;
            entity.DateCreatedUtc = currentEntry.DateCreatedUtc;
            var result = await base.Put(key, entity);

            if (!entity.Tags.IsNullOrEmpty())
            {
                var chosenTagIds = entity.Tags.Select(x => x.TagId);
                var existingTags = await postTagService.Value.FindAsync(x => x.PostId == entity.Id);
                var existingTagIds = existingTags.Select(x => x.TagId);

                var toDelete = existingTags.Where(x => !chosenTagIds.Contains(x.TagId));
                var toInsert = chosenTagIds.Where(x => !existingTagIds.Contains(x)).Select(x => new BlogPostTag
                {
                    PostId = entity.Id,
                    TagId = x
                });

                await postTagService.Value.DeleteAsync(toDelete);
                await postTagService.Value.InsertAsync(toInsert);
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