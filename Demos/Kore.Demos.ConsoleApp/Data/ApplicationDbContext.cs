using System;
using System.Data.Entity;
using Kore.Data.EntityFramework;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;

namespace Kore.Demos.ConsoleApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            Database.SetInitializer(new CreateTablesIfNotExist<ApplicationDbContext>());
        }

        public ApplicationDbContext() : base("DefaultConnection")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            this.Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            var configurations = EngineContext.Current.ResolveAll<IEntityTypeConfiguration>();

            foreach (dynamic configuration in configurations)
            {
                modelBuilder.Configurations.Add(configuration);
            }
        }
    }
}