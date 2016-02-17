using System.Data.Entity;

namespace Kore.EntityFramework.Data.EntityFramework
{
    public interface IDbContextFactory
    {
        DbContext GetContext();
    }
}