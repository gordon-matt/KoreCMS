using System.Data.Entity;

namespace Kore.EntityFramework
{
    public interface IDbSeeder
    {
        void Seed(DbContext context);

        int Order { get; }
    }
}