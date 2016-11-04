using Kore.Web.Identity;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;

namespace KoreCMS
{
    public class ApplicationUserValidator : KoreUserValidator<ApplicationUser>
    {
        public ApplicationUserValidator(UserManager<ApplicationUser, string> manager)
            : base(manager)
        {
        }
    }
}