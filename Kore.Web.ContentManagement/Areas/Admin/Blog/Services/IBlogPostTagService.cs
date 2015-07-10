﻿using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Services
{
    public interface IBlogPostTagService : IGenericDataService<BlogPostTag>
    {
    }

    public class BlogPostTagService : GenericDataService<BlogPostTag>, IBlogPostTagService
    {
        public BlogPostTagService(ICacheManager cacheManager, IRepository<BlogPostTag> repository)
            : base(cacheManager, repository)
        {
        }
    }
}