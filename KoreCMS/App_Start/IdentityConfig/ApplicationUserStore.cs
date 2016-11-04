using System.Data.Entity;
using Kore.Web.Identity;
using KoreCMS.Data.Domain;

namespace KoreCMS
{
    public class ApplicationUserStore : KoreUserStore<ApplicationUser, ApplicationRole>
    {
        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }
}