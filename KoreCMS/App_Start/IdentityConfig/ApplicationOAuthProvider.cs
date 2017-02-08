using Kore.Web.Identity.Providers;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;

namespace KoreCMS
{
    public class ApplicationOAuthProvider : KoreIdentityOAuthProvider<ApplicationUser>
    {
        public ApplicationOAuthProvider(string publicClientId)
            : base(publicClientId)
        {
        }

        public override UserManager<ApplicationUser, string> GetUserManager(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return context.OwinContext.GetUserManager<ApplicationUserManager>();
        }
    }
}