using System.Data.Entity;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;

namespace KoreCMS.Data
{
    public class ApplicationDbContextFactory : IDbContextFactory
    {
        private readonly DataSettings dataSettings;

        public ApplicationDbContextFactory(DataSettings dataSettings)
        {
            this.dataSettings = dataSettings;
        }

        public DbContext GetContext()
        {
            return new ApplicationDbContext(dataSettings.ConnectionString);
        }
    }
}