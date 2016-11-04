﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.EntityFramework;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Localization.Domain;
using Kore.Logging.Domain;
using Kore.Tasks.Domain;
using Kore.Tenants.Domain;
using Kore.Web.Identity.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using LanguageEntity = Kore.Localization.Domain.Language;

namespace Kore.Web.Identity
{
    public abstract class KoreIdentityDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>,
        ISupportSeed, IKoreDbContext
        where TUser : KoreIdentityUser
        where TRole : KoreIdentityRole
    {
        #region Constructors

        public KoreIdentityDbContext()
            : base()
        {
            var settings = DataSettingsManager.LoadSettings();
            this.Database.Connection.ConnectionString = settings.ConnectionString;
            this.Init();
        }

        public KoreIdentityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Init();
        }

        protected virtual void Init()
        {
            // Warning: Setting LazyLoadingEnabled to false causes problems with Identity 2.0:
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
                var seeders = EngineContext.Current.ResolveAll<IDbSeeder>().OrderBy(x => x.Order);

                foreach (var seeder in seeders)
                {
                    seeder.Seed(this);
                }
            }
        }

        #endregion ISupportSeed Members

        #region Additional Tables

        // Users and Roles are added automatically in IdentityDbContext base class, but not these ones:
        // In general, don't use these tables below, but they are needed in some occassions. Maybe make these "internal"?

        public DbSet<IdentityUserRole> UserRoles { get; set; }

        public DbSet<IdentityUserLogin> UserLogins { get; set; }

        public DbSet<IdentityUserClaim> UserClaims { get; set; }

        #endregion Additional Tables

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            var usersTable = modelBuilder.Entity<TUser>().ToTable("AspNetUsers");
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

            var rolesTable = modelBuilder.Entity<TRole>().ToTable("AspNetRoles");

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
    }
}