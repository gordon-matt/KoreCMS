using System.Data.Entity;
using Kore.EntityFramework.Data.EntityFramework;

namespace Kore.Demos.ConsoleApp.Data
{
    public class ApplicationDbContextFactory : IDbContextFactory
    {
        public DbContext GetContext()
        {
            return new ApplicationDbContext();
        }
    }
}