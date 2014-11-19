using System.Data.Entity;

namespace Kore.EntityFramework
{
    public class DropCreateSeedDatabaseIfModelChanges<TDbContext> : DropCreateDatabaseIfModelChanges<TDbContext>
        where TDbContext : DbContext, ISupportSeed
    {
        protected override void Seed(TDbContext context)
        {
            base.Seed(context);
            context.Seed();
        }
    }
}