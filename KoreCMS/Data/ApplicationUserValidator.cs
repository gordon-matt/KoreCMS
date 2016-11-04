using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KoreCMS.Data.Domain;
using Microsoft.AspNet.Identity;

namespace KoreCMS.Data
{
    public class ApplicationUserValidator : UserValidator<ApplicationUser>
    {
        private UserManager<ApplicationUser, string> Manager { get; set; }

        public ApplicationUserValidator(UserManager<ApplicationUser, string> manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser item)
        {
            IdentityResult identityResult;
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var errors = new List<string>();
            await this.ValidateUserName(item, errors);

            if (this.RequireUniqueEmail)
            {
                await this.ValidateEmail(item, errors);
            }

            identityResult = (errors.Count <= 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
            return identityResult;
        }

        private async Task ValidateEmail(ApplicationUser user, List<string> errors)
        {
            string email = user.Email;
            if (!string.IsNullOrWhiteSpace(email))
            {
                try
                {
                    MailAddress mailAddress = new MailAddress(email);
                }
                catch (FormatException)
                {
                    errors.Add(string.Format(CultureInfo.CurrentCulture, "Email '{0}' is invalid.", email));
                    return;
                }
                var existingUser = await this.Manager.FindByEmailAsync(email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    errors.Add(string.Format(CultureInfo.CurrentCulture, "Email '{0}' is already taken.", email));
                }
            }
            else
            {
                errors.Add("Email cannot be null or empty.");
            }
        }

        private async Task ValidateUserName(ApplicationUser user, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
                errors.Add("Name cannot be null or empty.");
            }
            else if (!this.AllowOnlyAlphanumericUserNames || Regex.IsMatch(user.UserName, "^[A-Za-z0-9@_\\.]+$"))
            {
                var existingUser = await this.Manager.FindByNameAsync(user.UserName);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    errors.Add(string.Format(CultureInfo.CurrentCulture, "Name {0} is already taken.", user.UserName));
                }
            }
            else
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, "User name {0} is invalid, can only contain letters or digits.", user.UserName));
            }
        }
    }
}