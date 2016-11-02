using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Linq.Expressions;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.EntityFramework;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Localization.Domain;
using Kore.Logging.Domain;
using Kore.Tasks.Domain;
using Kore.Tenants.Domain;
using Kore.Web.ContentManagement;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Localization;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Domain;
using Kore.Web.Infrastructure;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using LanguageEntity = Kore.Localization.Domain.Language;
using PermissionEntity = KoreCMS.Data.Domain.Permission;

namespace KoreCMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>,
        ISupportSeed,
        IKoreDbContext,
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
            var settings = DataSettingsManager.LoadSettings();
            this.Database.Connection.ConnectionString = settings.ConnectionString;
            this.Init();
        }

        public ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Init();
        }

        private void Init()
        {// Warning: Setting LazyLoadingEnabled to false causes problems with Identity 2.0:
            //  See: http://stackoverflow.com/questions/22816478/using-usermanager-and-rolemanager-in-microsoft-aspnet-identity
            //this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
        }

        #endregion Constructors

        #region IKoreDbContext Members

        public DbSet<LanguageEntity> Languages { get; set; }

        public DbSet<LocalizableProperty> LocalizableProperties { get; set; }

        public DbSet<LocalizableString> LocalizableStrings { get; set; }

        public DbSet<LogEntry> Log { get; set; }

        public DbSet<ScheduledTask> ScheduledTasks { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Tenant> Tenants { get; set; }

        #endregion IKoreDbContext Members

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

        #region ISupportSeed Members

        public virtual void Seed()
        {
            // Create default tenant
            Tenants.Add(new Tenant
            {
                Name = "Default",
                Url = "my-domain.com",
                Hosts = "my-domain.com"
            });
            SaveChanges();

            InitializeLocalizableStrings();

            var dataSettings = EngineContext.Current.Resolve<DataSettings>();

            if (dataSettings.CreateSampleData)
            {
                InstallContentBlocks();
            }
        }

        #endregion ISupportSeed Members

        //public DbSet<ApplicationUser> Users { get; set; }

        //public DbSet<ApplicationRole> Roles { get; set; }

        //public DbSet<IdentityUserRole> UserRoles { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    var configurations = EngineContext.Current.ResolveAll<IEntityTypeConfiguration>();

        //    foreach (dynamic configuration in configurations)
        //    {
        //        modelBuilder.Configurations.Add(configuration);
        //    }
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            var usersTable = modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            usersTable.HasMany(x => x.Roles).WithRequired().HasForeignKey(x => x.UserId);
            usersTable.HasMany(x => x.Claims).WithRequired().HasForeignKey(x => x.UserId);
            usersTable.HasMany(x => x.Logins).WithRequired().HasForeignKey(x => x.UserId);

            usersTable.Property(x => x.TenantId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true, Order = 1 }));

            usersTable.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true, Order = 2 }));

            usersTable.Property(x => x.Email).HasMaxLength(256);

            modelBuilder.Entity<IdentityUserRole>().HasKey(x => new
            {
                UserId = x.UserId,
                RoleId = x.RoleId
            }).ToTable("AspNetUserRoles");

            modelBuilder.Entity<IdentityUserLogin>().HasKey(x => new
            {
                LoginProvider = x.LoginProvider,
                ProviderKey = x.ProviderKey,
                UserId = x.UserId
            }).ToTable("AspNetUserLogins");

            modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

            var rolesTable = modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");

            rolesTable.Property(x => x.TenantId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true, Order = 1 }));

            rolesTable.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true, Order = 2 }));

            rolesTable.HasMany(x => x.Users).WithRequired().HasForeignKey(x => x.RoleId);

            var configurations = EngineContext.Current.ResolveAll<IEntityTypeConfiguration>();

            foreach (dynamic configuration in configurations)
            {
                modelBuilder.Configurations.Add(configuration);
            }
        }

        private void InitializeLocalizableStrings()
        {
            // We need to create localizable strings for all tenants,
            //  but at this point there will only be 1 tenant, because this is initialization for the DB.
            //  TODO: When admin user creates a new tenant, we need to insert localized strings for it. Probably in TenantApiController somewhere...
            int tenantId = Tenants.First().Id;
            var languagePacks = EngineContext.Current.ResolveAll<ILanguagePack>();

            var toInsert = new HashSet<LocalizableString>();
            foreach (var languagePack in languagePacks)
            {
                foreach (var localizedString in languagePack.LocalizedStrings)
                {
                    toInsert.Add(new LocalizableString
                    {
                        Id = Guid.NewGuid(),
                        TenantId = tenantId,
                        CultureCode = languagePack.CultureCode,
                        TextKey = localizedString.Key,
                        TextValue = localizedString.Value
                    });
                }
            }
            LocalizableStrings.AddRange(toInsert);
            SaveChanges();
        }

        private void InstallContentBlocks()
        {
            var languageSwitchZone = EnsureZone("LanguageSwitch");
            var adminLanguageSwitchZone = EnsureZone("AdminLanguageSwitch");

            var templateLanguageSwitchBlock = new LanguageSwitchBlock();

            #region Language Switch

            var block = ContentBlocks.FirstOrDefault(x =>
               x.ZoneId == languageSwitchZone.Id &&
               x.Title == "Language Switch");

            if (block == null)
            {
                ContentBlocks.Add(new ContentBlock
                {
                    Id = Guid.NewGuid(),
                    ZoneId = languageSwitchZone.Id,
                    Title = "Language Switch",
                    BlockType = GetTypeFullName(templateLanguageSwitchBlock.GetType()),
                    BlockName = templateLanguageSwitchBlock.Name,
                    IsEnabled = true,
                    BlockValues = @"{""Style"":""0"",""IncludeInvariant"":false,""InvariantText"":""[ Invariant ]""}"
                });
            }

            #endregion Language Switch

            #region Admin Language Switch

            block = ContentBlocks.FirstOrDefault(x =>
                x.ZoneId == adminLanguageSwitchZone.Id &&
                x.Title == "Admin Language Switch");

            if (block == null)
            {
                ContentBlocks.Add(new ContentBlock
                {
                    Id = Guid.NewGuid(),
                    ZoneId = adminLanguageSwitchZone.Id,
                    Title = "Admin Language Switch",
                    BlockType = GetTypeFullName(templateLanguageSwitchBlock.GetType()),
                    BlockName = templateLanguageSwitchBlock.Name,
                    IsEnabled = true,
                    BlockValues = @"{""Style"":""0"",""IncludeInvariant"":false,""InvariantText"":""[ Invariant ]""}"
                });
            }

            #endregion Admin Language Switch

            SaveChanges();
        }

        private Zone EnsureZone(string zoneName)
        {
            var zone = Zones.FirstOrDefault(x => x.Name == zoneName);

            if (zone == null)
            {
                Zones.Add(new Zone
                {
                    Id = Guid.NewGuid(),
                    Name = zoneName
                });
                SaveChanges();
                zone = Zones.FirstOrDefault(x => x.Name == zoneName);
            }

            return zone;
        }

        private static string GetTypeFullName(Type type)
        {
            return string.Concat(type.FullName, ", ", type.Assembly.GetName().Name);
        }
    }
}