using System.Data.Entity;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Web.ContentManagement;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Domain;
using Kore.Web.Identity;
using KoreCMS.Data.Domain;
using PermissionEntity = KoreCMS.Data.Domain.Permission;

namespace KoreCMS.Data
{
    public class ApplicationDbContext : KoreIdentityDbContext<ApplicationUser, ApplicationRole>,
        IKoreCmsDbContext,
        IKoreSecurityDbContext
    {
        #region Constructors

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new CreateTablesIfNotExist<ApplicationDbContext>());
            //Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            //Database.SetInitializer<ApplicationDbContext>(null);
        }

        public ApplicationDbContext()
            : base()
        {
        }

        public ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        #endregion Constructors

        #region IKoreCmsDbContext Members

        public DbSet<BlogCategory> BlogCategories { get; set; }

        public DbSet<BlogPost> BlogPosts { get; set; }

        public DbSet<BlogPostTag> BlogPostTags { get; set; }

        public DbSet<BlogTag> BlogTags { get; set; }

        public DbSet<ContentBlock> ContentBlocks { get; set; }

        public DbSet<EntityTypeContentBlock> EntityTypeContentBlocks { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<MessageTemplate> MessageTemplates { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<PageType> PageTypes { get; set; }

        public DbSet<PageVersion> PageVersions { get; set; }

        public DbSet<QueuedEmail> QueuedEmails { get; set; }

        public DbSet<SitemapConfig> SitemapConfig { get; set; }

        public DbSet<Zone> Zones { get; set; }

        #endregion IKoreCmsDbContext Members

        #region IKoreSecurityDbContext Members

        public DbSet<PermissionEntity> Permissions { get; set; }

        public DbSet<UserProfileEntry> UserProfiles { get; set; }

        #endregion IKoreSecurityDbContext Members
    }
}