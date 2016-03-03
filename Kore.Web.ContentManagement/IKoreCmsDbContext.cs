﻿using System.Data.Entity;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Domain;

namespace Kore.Web.ContentManagement
{
    public interface IKoreCmsDbContext
    {
        DbSet<BlogCategory> BlogCategories { get; set; }

        DbSet<BlogPost> BlogPosts { get; set; }

        DbSet<BlogPostTag> BlogPostTags { get; set; }

        DbSet<BlogTag> BlogTags { get; set; }

        DbSet<ContentBlock> ContentBlocks { get; set; }

        DbSet<EntityTypeContentBlock> EntityTypeContentBlocks { get; set; }

        DbSet<Menu> Menus { get; set; }

        DbSet<MenuItem> MenuItems { get; set; }

        DbSet<MessageTemplate> MessageTemplates { get; set; }

        DbSet<Page> Pages { get; set; }

        DbSet<PageType> PageTypes { get; set; }

        DbSet<PageVersion> PageVersions { get; set; }

        DbSet<QueuedEmail> QueuedEmails { get; set; }

        DbSet<SitemapConfig> SitemapConfig { get; set; }

        DbSet<Zone> Zones { get; set; }
    }
}