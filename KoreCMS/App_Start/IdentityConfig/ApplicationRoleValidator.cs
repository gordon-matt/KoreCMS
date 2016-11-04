using Kore.Web.Identity;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;

namespace KoreCMS
{
    public class ApplicationRoleValidator : KoreRoleValidator<ApplicationRole>
    {
        public ApplicationRoleValidator(RoleManager<ApplicationRole, string> manager)
            : base(manager)
        {
        }
    }
}