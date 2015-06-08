using System.Data.Entity;
using Kore.Infrastructure;

namespace Kore.EntityFramework
{
    public static class DatabaseHelper
    {
        public static bool IsDatabaseInstalled()
        {
            var dbContext = EngineContext.Current.Resolve<DbContext>();

            if (dbContext == null || !dbContext.Database.Exists())
            {
                return false;
            }

            return true;
        }
    }
}