using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using Kore.Web.ContentManagement;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Infrastructure;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using LanguageEntity = Kore.Localization.Domain.Language;
using PermissionEntity = KoreCMS.Data.Domain.Permission;

namespace KoreCMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>,
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
        }

        public ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        #endregion Constructors

        #region IKoreDbContext Members

        public DbSet<LanguageEntity> Languages { get; set; }

        public DbSet<LocalizableString> LocalizableStrings { get; set; }

        public DbSet<ScheduledTask> ScheduledTasks { get; set; }

        public DbSet<Setting> Settings { get; set; }

        #endregion IKoreDbContext Members

        #region IKoreCmsDbContext Members

        public DbSet<BlogPost> Blog { get; set; }

        public DbSet<ContentBlock> ContentBlocks { get; set; }

        public DbSet<LogEntry> Log { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<MessageTemplate> MessageTemplates { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<PageVersion> PageVersions { get; set; }

        public DbSet<QueuedEmail> QueuedEmails { get; set; }

        public DbSet<Zone> Zones { get; set; }

        #endregion IKoreCmsDbContext Members

        #region IKoreSecurityDbContext Members

        public DbSet<PermissionEntity> Permissions { get; set; }

        #endregion IKoreSecurityDbContext Members

        #region Others

        public DbSet<UserProfileEntry> UserProfiles { get; set; }

        #endregion Others

        #region ISupportSeed Members

        public virtual void Seed()
        {
            InitializeLocalizableStrings();
        }

        #endregion ISupportSeed Members

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var configurations = EngineContext.Current.ResolveAll<IEntityTypeConfiguration>();

            foreach (dynamic configuration in configurations)
            {
                modelBuilder.Configurations.Add(configuration);
            }
        }

        private void InitializeLocalizableStrings()
        {
            var languagePacks = EngineContext.Current.ResolveAll<ILanguagePack>();

            var toInsert = new HashSet<LocalizableString>();
            foreach (var languagePack in languagePacks)
            {
                foreach (var localizedString in languagePack.LocalizedStrings)
                {
                    toInsert.Add(new LocalizableString
                    {
                        Id = Guid.NewGuid(),
                        CultureCode = languagePack.CultureCode,
                        TextKey = localizedString.Key,
                        TextValue = localizedString.Value
                    });
                }
            }
            LocalizableStrings.AddRange(toInsert);
            SaveChanges();
        }
    }
}