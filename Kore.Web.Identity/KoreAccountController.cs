using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Messaging;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Services;
using Kore.Web.Identity.Domain;
using Kore.Web.Identity.Messaging;
using Kore.Web.Identity.Models;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Mvc.Routing;
using Kore.Web.Security.Membership;
using Kore.Web.Security.Membership.Permissions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Kore.Web.Identity
{
    [Authorize]
    public abstract class KoreAccountController<TUser> : KoreController
        where TUser : KoreIdentityUser, new()
    {
        private readonly IMembershipService membershipService;
        private readonly IMessageService messageService;
        private readonly Lazy<IEnumerable<IUserProfileProvider>> userProfileProviders;

        public KoreAccountController(
            IMembershipService membershipService,
            IMessageService messageService,
            Lazy<IEnumerable<IUserProfileProvider>> userProfileProviders)
        {
            this.membershipService = membershipService;
            this.messageService = messageService;
            this.userProfileProviders = userProfileProviders;
        }

        public abstract UserManager<TUser, string> UserManager { get; protected set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        [Compress]
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
        [Compress]
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

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, T(LocalizableStrings.InvalidUserNameOrPassword));
                }
                else
                {
                    var authorizedUser = await UserManager.FindAsync(user.UserName, model.Password);

                    if (authorizedUser != null)
                    {
                        await SignInAsync(authorizedUser, model.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, T(LocalizableStrings.InvalidUserNameOrPassword));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        [Compress]
        [Route("register")]
        public virtual ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [Compress]
        [Route("register")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new TUser() { UserName = model.Email, Email = model.Email };
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

                    messageService.SendEmailMessage(WorkContext.CurrentTenant.Id, AccountMessageTemplates.Account_Registered, tokens, user.Email);

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
        [Compress]
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
                var user = await membershipService.GetUserById(userId);
                var tokens = new List<Token>
                {
                    new Token("[UserName]", user.UserName),
                    new Token("[Email]", user.Email)
                };
                messageService.SendEmailMessage(WorkContext.CurrentTenant.Id, AccountMessageTemplates.Account_Confirmed, tokens, user.Email);

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
        [Compress]
        [Route("forgot-password")]
        public virtual ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [Compress]
        [Route("forgot-password")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", T(LocalizableStrings.UserDoesNotExistOrIsNotConfirmed));
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
        [Compress]
        [Route("forgot-password-confirmation")]
        public virtual ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        [Compress]
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
        [Compress]
        [ValidateAntiForgeryToken]
        [Route("reset-password")]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", T(LocalizableStrings.NoUserFound));
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
                    messageService.SendEmailMessage(WorkContext.CurrentTenant.Id, AccountMessageTemplates.Account_PasswordReset, tokens, user.Email);

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
        [Compress]
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
        [Compress]
        [Route("manage")]
        public virtual ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? T(LocalizableStrings.ManageMessages.ChangePasswordSuccess)
                : message == ManageMessageId.SetPasswordSuccess ? T(LocalizableStrings.ManageMessages.SetPasswordSuccess)
                : message == ManageMessageId.RemoveLoginSuccess ? T(LocalizableStrings.ManageMessages.RemoveLoginSuccess)
                : message == ManageMessageId.Error ? T(LocalizableStrings.ManageMessages.Error)
                : string.Empty;
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [Compress]
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
        [Compress]
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
        [Compress]
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
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [Compress]
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
                var user = new TUser() { UserName = model.Email, Email = model.Email };
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
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        [Compress]
        [Route("external-login-failure")]
        public virtual ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //
        // POST: /Account/LinkLogin
        [Compress]
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
        [Compress]
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
        // POST: /Account/LogOff
        [HttpPost]
        [Compress]
        [Route("log-off")]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
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

        [Compress]
        [Route("profile/{userId}")]
        public virtual async Task<ActionResult> ViewProfile(string userId)
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Title));

            if (userId == WorkContext.CurrentUser.Id)
            {
                ViewBag.Title = T(LocalizableStrings.MyProfile);
                WorkContext.Breadcrumbs.Add(T(LocalizableStrings.MyProfile));
                ViewBag.CanEdit = true;
            }
            else if (CheckPermission(StandardPermissions.FullAccess))
            {
                var user = await membershipService.GetUserById(userId);
                ViewBag.Title = string.Format(T(LocalizableStrings.ProfileForUser), user.UserName);
                WorkContext.Breadcrumbs.Add(string.Format(T(LocalizableStrings.ProfileForUser), user.UserName));
                ViewBag.CanEdit = true;
            }
            else
            {
                var user = await membershipService.GetUserById(userId);
                ViewBag.Title = string.Format(T(LocalizableStrings.ProfileForUser), user.UserName);
                WorkContext.Breadcrumbs.Add(string.Format(T(LocalizableStrings.ProfileForUser), user.UserName));
                ViewBag.CanEdit = false;
            }

            return View("Profile", model: userId);
        }

        [Compress]
        [Route("my-profile")]
        public virtual async Task<ActionResult> ViewMyProfile()
        {
            return await ViewProfile(WorkContext.CurrentUser.Id);
        }

        [Compress]
        [Route("profile/edit/{userId}/")]
        public virtual async Task<ActionResult> EditProfile(string userId)
        {
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Title));

            if (userId == WorkContext.CurrentUser.Id)
            {
                ViewBag.Title = T(LocalizableStrings.EditMyProfile);
                WorkContext.Breadcrumbs.Add(T(LocalizableStrings.MyProfile), Url.Action("ViewMyProfile"));
                WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Edit));
            }
            else if (CheckPermission(StandardPermissions.FullAccess))
            {
                ViewBag.Title = T(LocalizableStrings.EditProfile);
                var user = await membershipService.GetUserById(userId);
                WorkContext.Breadcrumbs.Add(string.Format(T(LocalizableStrings.ProfileForUser), user.UserName), Url.Action("ViewProfile", new { userId = userId }));
                WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Edit));
            }
            else
            {
                return new HttpUnauthorizedResult();
            }

            return View("ProfileEdit", model: userId);
        }

        [Compress]
        [Route("my-profile/edit")]
        public virtual async Task<ActionResult> EditMyProfile()
        {
            return await EditProfile(WorkContext.CurrentUser.Id);
        }

        [HttpPost]
        [Compress]
        [Route("update-profile")]
        [ValidateInput(false)]
        public virtual async Task<ActionResult> UpdateProfile()
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

            await membershipService.UpdateProfile(userId, newProfile);

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
        protected const string XsrfKey = "XsrfId";

        protected virtual IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        protected virtual async Task SignInAsync(TUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        protected virtual void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected virtual bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        protected virtual void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771

            string link = new FluentTagBuilder("a")
                .MergeAttribute("href", callbackUrl)
                .SetInnerText(callbackUrl)
                .ToString();

            string body = string.Format("{0}<br/>{1}", message, link);

            messageService.SendEmailMessage(WorkContext.CurrentTenant.Id, subject, body, email);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        protected virtual ActionResult RedirectToLocal(string returnUrl)
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

        protected class ChallengeResult : HttpUnauthorizedResult
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