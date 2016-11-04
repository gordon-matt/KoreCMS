using KoreCMS.Data;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace KoreCMS
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store)
            : base(store)
        {
            this.RoleValidator = new ApplicationRoleValidator(this);
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(
                new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));
        }
    }
}