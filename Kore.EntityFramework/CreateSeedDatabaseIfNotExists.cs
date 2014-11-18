using System.Data.Entity;

namespace Kore.EntityFramework
{
    public class CreateSeedDatabaseIfNotExists<TDbContext> : CreateDatabaseIfNotExists<TDbContext>
        where TDbContext : DbContext, ISupportSeed
    {
        protected override void Seed(TDbContext context)
        {
            base.Seed(context);
            context.Seed();
        }
    }
}