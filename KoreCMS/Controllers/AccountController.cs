using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web;
using Kore.Web.ContentManagement.Messaging;
using Kore.Web.ContentManagement.Messaging.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Routing;
using Kore.Web.Security.Membership;
using Kore.Web.Security.Membership.Permissions;
using KoreCMS;
using KoreCMS.Data;
using KoreCMS.Data.Domain;
using KoreCMS.Messaging;
using KoreCMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Kore.Controllers
{
    [Authorize]
    [RoutePrefix("account")]
    public class AccountController : KoreController
    {
        private ApplicationUserManager _userManager;
        private readonly IMembershipService membershipService;
        private readonly IMessageService messageService;
        private readonly Lazy<IEnumerable<IUserProfileProvider>> userProfileProviders;

        public AccountController(IMembershipService membershipService, IMessageService messageService, Lazy<IEnumerable<IUserProfileProvider>> userProfileProviders)
        {
            this.membershipService = membershipService;
            this.messageService = messageService;
            this.userProfileProviders = userProfileProviders;
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            this.membershipService = EngineContext.Current.Resolve<IMembershipService>();
            this.messageService = EngineContext.Current.Resolve<IMessageService>();
            this.userProfileProviders = new Lazy<IEnumerable<IUserProfileProvider>>(() =>
            {
                return EngineContext.Current.ResolveAll<IUserProfileProvider>();
            });
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [Route("login")]
        public virtual ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //var user = await UserManager.FindAsync(model.Email, model.Password);

                //if (user == null)
                //{
                //    var temp = await UserManager.FindByEmailAsync(model.Email);
                //    user = await UserManager.FindAsync(temp.UserName, model.Password);
                //}

                var user = await UserManager.FindByEmailAsync(model.Email);
                var authorizedUser = await UserManager.FindAsync(user.UserName, model.Password);

                if (authorizedUser != null)
                {
                    await SignInAsync(authorizedUser, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", T(LocalizableStrings.Account.InvalidUserNameOrPassword));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        [Route("register")]
        public virtual ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                    var tokens = new List<Token>
                    {
                        new Token("[UserName]", user.UserName),
                        new Token("[Email]", user.Email),
                        new Token("[ConfirmationToken]", callbackUrl)
                    };

                    messageService.SendEmailMessage(AccountMessageTemplates.Account_Registered, tokens, user.Email);

                    // TODO: Make this a message template
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        [Route("confirm-email")]
        public virtual async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                var user = membershipService.GetUserById(userId);
                var tokens = new List<Token>
                {
                    new Token("[UserName]", user.UserName),
                    new Token("[Email]", user.Email)
                };
                messageService.SendEmailMessage(AccountMessageTemplates.Account_Confirmed, tokens, user.Email);

                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        [Route("forgot-password")]
        public virtual ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", T(LocalizableStrings.Account.UserDoesNotExistOrIsNotConfirmed));
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                // TODO: Make this a message template
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        [Route("forgot-password-confirmation")]
        public virtual ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        [Route("reset-password")]
        public virtual ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("reset-password")]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", T(LocalizableStrings.Account.NoUserFound));
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    var tokens = new List<Token>
                    {
                        new Token("[UserName]", user.UserName),
                        new Token("[Email]", user.Email)
                    };
                    messageService.SendEmailMessage(AccountMessageTemplates.Account_PasswordReset, tokens, user.Email);

                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        [Route("reset-password-confirmation")]
        public virtual ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [Route("disassociate")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        [Route("manage")]
        public virtual ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? T(LocalizableStrings.Account.ManageMessages.ChangePasswordSuccess)
                : message == ManageMessageId.SetPasswordSuccess ? T(LocalizableStrings.Account.ManageMessages.SetPasswordSuccess)
                : message == ManageMessageId.RemoveLoginSuccess ? T(LocalizableStrings.Account.ManageMessages.RemoveLoginSuccess)
                : message == ManageMessageId.Error ? T(LocalizableStrings.Account.ManageMessages.Error)
                : string.Empty;
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [Route("manage")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [Route("external-login")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        [Route("external-login-callback")]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [Route("link-login")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        [Route("link-login-callback")]
        public virtual async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [Route("external-login-confirmation")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        
                        // TODO: Make this a message template
                        SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [Route("log-off")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        [Route("external-login-failure")]
        public virtual ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        [Route("remove-account-list")]
        public virtual ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region User Profile

        [Route("profile/{userId}")]
        public virtual ActionResult ViewProfile(string userId)
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Account.Title));

            if (userId == WorkContext.CurrentUser.Id)
            {
                ViewBag.Title = T(LocalizableStrings.Account.MyProfile);
                WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Account.MyProfile));
                ViewBag.CanEdit = true;
            }
            else if (CheckPermission(StandardPermissions.FullAccess))
            {
                var user = membershipService.GetUserById(userId);
                ViewBag.Title = string.Format(T(LocalizableStrings.Account.ProfileForUser), user.UserName);
                WorkContext.Breadcrumbs.Add(string.Format(T(LocalizableStrings.Account.ProfileForUser), user.UserName));
                ViewBag.CanEdit = true;
            }
            else
            {
                var user = membershipService.GetUserById(userId);
                ViewBag.Title = string.Format(T(LocalizableStrings.Account.ProfileForUser), user.UserName);
                WorkContext.Breadcrumbs.Add(string.Format(T(LocalizableStrings.Account.ProfileForUser), user.UserName));
                ViewBag.CanEdit = false;
            }

            return View("Profile", model: userId);
        }

        [Route("my-profile")]
        public virtual ActionResult ViewMyProfile()
        {
            return ViewProfile(WorkContext.CurrentUser.Id);
        }

        [Route("profile/edit/{userId}/")]
        public virtual ActionResult EditProfile(string userId)
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Account.Title));

            if (userId == WorkContext.CurrentUser.Id)
            {
                ViewBag.Title = T(LocalizableStrings.Account.EditMyProfile);
                WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Account.MyProfile), Url.Action("ViewMyProfile"));
                WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Edit));
            }
            else if (CheckPermission(StandardPermissions.FullAccess))
            {
                ViewBag.Title = T(LocalizableStrings.Account.EditProfile);
                var user = membershipService.GetUserById(userId);
                WorkContext.Breadcrumbs.Add(string.Format(T(LocalizableStrings.Account.ProfileForUser), user.UserName), Url.Action("ViewProfile", new { userId = userId }));
                WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Edit));
            }
            else
            {
                return new HttpUnauthorizedResult();
            }

            return View("ProfileEdit", model: userId);
        }

        [Route("my-profile/edit")]
        public virtual ActionResult EditMyProfile()
        {
            return EditProfile(WorkContext.CurrentUser.Id);
        }

        [HttpPost]
        [Route("update-profile")]
        [ValidateInput(false)]
        public virtual ActionResult UpdateProfile()
        {
            var userId = Request.Form["UserId"];

            var newProfile = new Dictionary<string, string>();

            foreach (var provider in userProfileProviders.Value)
            {
                foreach (var fieldName in provider.GetFieldNames())
                {
                    string value = Request.Form[fieldName];

                    if (value == "true,false")
                    {
                        value = "true";
                    }

                    newProfile.Add(fieldName, value);
                }
            }

            membershipService.UpdateProfile(userId, newProfile);

            //eventBus.Notify<IMembershipEventHandler>(x => x.ProfileChanged(userId, newProfile));

            if (userId == WorkContext.CurrentUser.Id)
            {
                return RedirectToAction("ViewMyProfile");
            }
            return RedirectToAction("ViewMyProfile", RouteData.Values.Merge(new { userId }));
        }

        #endregion User Profile

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771

            string link = new FluentTagBuilder("a")
                .MergeAttribute("href", callbackUrl)
                .SetInnerText(callbackUrl)
                .ToString();

            string body = string.Format("{0}<br/>{1}", message, link);

            messageService.SendEmailMessage(subject, body, email);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion Helpers
    }
}