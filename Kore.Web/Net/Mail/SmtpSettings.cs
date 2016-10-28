using System.ComponentModel.DataAnnotations;
using Kore.ComponentModel;
using Kore.Web;
using Kore.Web.Configuration;

namespace Kore.Net.Mail
{
    public class SmtpSettings : ISettings
    {
        public SmtpSettings()
        {
            MaxTries = 3;
            MessagesPerBatch = 50;
        }

        #region ISettings Members

        public string Name
        {
            get { return "SMTP Settings"; }
        }

        public bool IsTenantRestricted
        {
            get { return false; }
        }

        public string EditorTemplatePath
        {
            // Using an Embedded View in this case, since this assembly is not a plugin or the main app
            //TODO: Need to find a way to separate this: since it relies on the CMS project
            get { return "Kore.Web.Views.Shared.EditorTemplates.SmtpSettings"; }
        }

        #endregion ISettings Members

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Smtp.DisplayName)]
        public string DisplayName { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Smtp.Host)]
        public string Host { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Smtp.Port)]
        public int Port { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Smtp.EnableSsl)]
        public bool EnableSsl { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Smtp.UseDefaultCredentials)]
        public bool UseDefaultCredentials { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Smtp.Username)]
        public string Username { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Smtp.Password)]
        public string Password { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Smtp.MaxTries)]
        public int MaxTries { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.Smtp.MessagesPerBatch)]
        public int MessagesPerBatch { get; set; }
    }
}