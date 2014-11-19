//using System.Data.Common;
//using System.Data.Entity;
//using System.Data.Entity.Core.Objects;
//using System.Data.Entity.Infrastructure;
//using Kore.Infrastructure;
//using Kore.Localization.Domain;

//namespace Kore.Data.EntityFramework
//{
//    public class KoreDbContext : DbContext
//    {
//        #region Constructors

//        public KoreDbContext()
//            : base()
//        {
//            Database.SetInitializer<KoreDbContext>(null);
//        }

//        public KoreDbContext(string nameOrConnectionString)
//            : base(nameOrConnectionString)
//        {
//            Database.SetInitializer<KoreDbContext>(null);
//        }

//        public KoreDbContext(DbCompiledModel model)
//            : base(model)
//        {
//            Database.SetInitializer<KoreDbContext>(null);
//        }

//        public KoreDbContext(string nameOrConnectionString, DbCompiledModel model)
//            : base(nameOrConnectionString, model)
//        {
//            Database.SetInitializer<KoreDbContext>(null);
//        }

//        public KoreDbContext(DbConnection existingConnection, bool contextOwnsConnection)
//            : base(existingConnection, contextOwnsConnection)
//        {
//            Database.SetInitializer<KoreDbContext>(null);
//        }

//        public KoreDbContext(ObjectContext objectContext, bool dbContextOwnsConnection)
//            : base(objectContext, dbContextOwnsConnection)
//        {
//            Database.SetInitializer<KoreDbContext>(null);
//        }

//        public KoreDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection)
//            : base(existingConnection, model, contextOwnsConnection)
//        {
//            Database.SetInitializer<KoreDbContext>(null);
//        }

//        #endregion Constructors

//        public DbSet<Language> Languages { get; set; }

//        public DbSet<LocalizableString> LocalizableStrings { get; set; }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            var configurations = EngineContext.Current.ResolveAll<IEntityTypeConfiguration>();

//            foreach (dynamic configuration in configurations)
//            {
//                modelBuilder.Configurations.Add(configuration);
//            }
//        }
//    }
//}