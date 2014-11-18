using System;
using System.Collections.Generic;
using System.Data.Entity;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.EntityFramework;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Localization.Domain;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using LanguageEntity = Kore.Localization.Domain.Language;

namespace KoreCMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, ISupportSeed, IKoreDbContext, IKoreSecurityDbContext
    {
        #region Constructors

        static ApplicationDbContext()
        {
#if DEBUG
            Database.SetInitializer<ApplicationDbContext>(new DropCreateSeedDatabaseIfModelChanges<ApplicationDbContext>());
#else
            Database.SetInitializer<ApplicationDbContext>(new CreateSeedDatabaseIfNotExists<ApplicationDbContext>());
#endif
        }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        #endregion

        #region IKoreDbContext Members

        public DbSet<LanguageEntity> Languages { get; set; }

        public DbSet<LocalizableString> LocalizableStrings { get; set; }

        #endregion IKoreDbContext Members

        public DbSet<Permission> Permissions { get; set; }

        #region ISupportSeed Members

        public virtual void Seed()
        {
            InitializeLocalizableStrings();
        }

        #endregion ISupportSeed Members

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

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
            var localizedStringsProviders = EngineContext.Current.ResolveAll<IDefaultLocalizableStringsProvider>();

            var toInsert = new HashSet<LocalizableString>();
            foreach (var provider in localizedStringsProviders)
            {
                foreach (var translation in provider.GetTranslations())
                {
                    foreach (var localizedString in translation.LocalizedStrings)
                    {
                        toInsert.Add(new LocalizableString
                        {
                            Id = Guid.NewGuid(),
                            CultureCode = translation.CultureCode,
                            TextKey = localizedString.Key,
                            TextValue = localizedString.Value
                        });
                    }
                }
            }
            LocalizableStrings.AddRange(toInsert);
            SaveChanges();
        }
    }
}