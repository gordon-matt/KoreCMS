using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Services;
using Kore.Web.Identity;
using Kore.Web.Identity.Models;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership;
using KoreCMS;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Kore.Controllers
{
    // NOTE: Even though we're not changing the implementation, we still have to override each action.
    //  This is because the MVC route attributes are not inherited and you will get a 404 error

    [Authorize]
    [RoutePrefix("account")]
    public class AccountController : KoreAccountController<ApplicationUser>
    {
        private UserManager<ApplicationUser, string> userManager;

        public AccountController(
            IMembershipService membershipService,
            IMessageService messageService,
            Lazy<IEnumerable<IUserProfileProvider>> userProfileProviders)
            : base(membershipService, messageService, userProfileProviders)
        {
        }

        public override UserManager<ApplicationUser, string> UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            protected set
            {
                userManager = value;
            }
        }

        [AllowAnonymous]
        [Compress]
        [Route("login")]
        public override ActionResult Login(string returnUrl)
        {
            return base.Login(returnUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        [Compress]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public override async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            return await base.Login(model, returnUrl);
        }

        [AllowAnonymous]
        [Compress]
        [Route("register")]
        public override ActionResult Register()
        {
            return base.Register();
        }

        [HttpPost]
        [AllowAnonymous]
        [Compress]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Register(RegisterViewModel model)
        {
            return await base.Register(model);
        }

        [AllowAnonymous]
        [Compress]
        [Route("confirm-email")]
        public override async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            return await base.ConfirmEmail(userId, code);
        }

        [AllowAnonymous]
        [Compress]
        [Route("forgot-password")]
        public override ActionResult ForgotPassword()
        {
            return base.ForgotPassword();
        }

        [HttpPost]
        [AllowAnonymous]
        [Compress]
        [Route("forgot-password")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            return await base.ForgotPassword(model);
        }

        [AllowAnonymous]
        [Compress]
        [Route("forgot-password-confirmation")]
        public override ActionResult ForgotPasswordConfirmation()
        {
            return base.ForgotPasswordConfirmation();
        }

        [AllowAnonymous]
        [Compress]
        [Route("reset-password")]
        public override ActionResult ResetPassword(string code)
        {
            return base.ResetPassword(code);
        }

        [HttpPost]
        [AllowAnonymous]
        [Compress]
        [ValidateAntiForgeryToken]
        [Route("reset-password")]
        public override async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            return await base.ResetPassword(model);
        }

        [AllowAnonymous]
        [Compress]
        [Route("reset-password-confirmation")]
        public override ActionResult ResetPasswordConfirmation()
        {
            return base.ResetPasswordConfirmation();
        }

        [HttpPost]
        [Route("disassociate")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            return await base.Disassociate(loginProvider, providerKey);
        }

        [Compress]
        [Route("manage")]
        public override ActionResult Manage(KoreAccountController<ApplicationUser>.ManageMessageId? message)
        {
            return base.Manage(message);
        }

        [HttpPost]
        [Compress]
        [Route("manage")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            return await base.Manage(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Compress]
        [Route("external-login")]
        [ValidateAntiForgeryToken]
        public override ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return base.ExternalLogin(provider, returnUrl);
        }

        [AllowAnonymous]
        [Compress]
        [Route("external-login-callback")]
        public override async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            return await base.ExternalLoginCallback(returnUrl);
        }

        [HttpPost]
        [AllowAnonymous]
        [Compress]
        [Route("external-login-confirmation")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            return await base.ExternalLoginConfirmation(model, returnUrl);
        }

        [AllowAnonymous]
        [Compress]
        [Route("external-login-failure")]
        public override ActionResult ExternalLoginFailure()
        {
            return base.ExternalLoginFailure();
        }

        [Compress]
        [HttpPost]
        [Route("link-login")]
        [ValidateAntiForgeryToken]
        public override ActionResult LinkLogin(string provider)
        {
            return base.LinkLogin(provider);
        }

        [Compress]
        [Route("link-login-callback")]
        public override async Task<ActionResult> LinkLoginCallback()
        {
            return await base.LinkLoginCallback();
        }

        [HttpPost]
        [Compress]
        [Route("log-off")]
        [ValidateAntiForgeryToken]
        public override ActionResult LogOff()
        {
            return base.LogOff();
        }

        [ChildActionOnly]
        [Route("remove-account-list")]
        public override ActionResult RemoveAccountList()
        {
            return base.RemoveAccountList();
        }

        [Compress]
        [Route("profile/{userId}")]
        public override async Task<ActionResult> ViewProfile(string userId)
        {
            return await base.ViewProfile(userId);
        }

        [Compress]
        [Route("my-profile")]
        public override async Task<ActionResult> ViewMyProfile()
        {
            return await base.ViewMyProfile();
        }

        [Compress]
        [Route("profile/edit/{userId}/")]
        public override async Task<ActionResult> EditProfile(string userId)
        {
            return await base.EditProfile(userId);
        }

        [Compress]
        [Route("my-profile/edit")]
        public override async Task<ActionResult> EditMyProfile()
        {
            return await base.EditMyProfile();
        }

        [HttpPost]
        [Compress]
        [Route("update-profile")]
        [ValidateInput(false)]
        public override async Task<ActionResult> UpdateProfile()
        {
            return await base.UpdateProfile();
        }
    }
}