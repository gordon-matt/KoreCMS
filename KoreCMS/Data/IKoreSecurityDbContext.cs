using System.Data.Entity;
using KoreCMS.Data.Domain;

namespace KoreCMS.Data
{
    public interface IKoreSecurityDbContext
    {
        DbSet<Permission> Permissions { get; set; }
    }
}