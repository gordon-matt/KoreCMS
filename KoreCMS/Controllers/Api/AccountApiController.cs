using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Kore.Web.Identity;
using Kore.Web.Identity.Models.Api;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace KoreCMS.Controllers.Api
{
    // NOTE: Even though we're not changing the implementation, we still have to override each action.
    //  This is because the route attributes are not inherited and you will get a 404 error

    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountApiController : KoreAccountApiController<ApplicationUser>
    {
        private UserManager<ApplicationUser, string> userManager;

        #region Constructors

        public AccountApiController()
        {
        }

        public AccountApiController(
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
            : base(accessTokenFormat)
        {
        }

        #endregion Constructors

        public override string PublicClientId
        {
            get { return Startup.PublicClientId; }
        }

        public override UserManager<ApplicationUser, string> UserManager
        {
            get { return userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            protected set { userManager = value; }
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("user-info")]
        public override UserInfoViewModel GetUserInfo()
        {
            return base.GetUserInfo();
        }

        [Route("logout")]
        public override IHttpActionResult Logout()
        {
            return base.Logout();
        }

        [Route("manage-info")]
        public override Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            return base.GetManageInfo(returnUrl, generateState);
        }

        [Route("change-password")]
        public override Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            return base.ChangePassword(model);
        }

        [Route("set-password")]
        public override Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            return base.SetPassword(model);
        }

        [Route("add-external-login")]
        public override Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            return base.AddExternalLogin(model);
        }

        [Route("remove-login")]
        public override Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            return base.RemoveLogin(model);
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("external-login", Name = "ExternalLogin")]
        public override Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            return base.GetExternalLogin(provider, error);
        }

        [AllowAnonymous]
        [Route("external-logins")]
        public override IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            return base.GetExternalLogins(returnUrl, generateState);
        }

        [AllowAnonymous]
        [Route("register")]
        public override Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            return base.Register(model);
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("register-external")]
        public override Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            return base.RegisterExternal(model);
        }
    }
}