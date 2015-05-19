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

        [ScaffoldColumn(false)]
        public string Name
        {
            get { return "SMTP Settings"; }
        }

        public string EditorTemplatePath
        {
            // Using an Embedded View in this case, since this assembly is not a plugin or the main app
            //TODO: Need to find a way to separate this: since it relies on the CMS project
            get { return "Kore.Web.Views.Shared.EditorTemplates.SmtpSettings"; }
        }

        #endregion ISettings Members

        [LocalizedDisplayName(KoreWebLocalizableStrings.SmtpSettings.DisplayName)]
        public string DisplayName { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.SmtpSettings.Host)]
        public string Host { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.SmtpSettings.Port)]
        public int Port { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.SmtpSettings.EnableSsl)]
        public bool EnableSsl { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.SmtpSettings.UseDefaultCredentials)]
        public bool UseDefaultCredentials { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.SmtpSettings.Username)]
        public string Username { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.SmtpSettings.Password)]
        public string Password { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.SmtpSettings.MaxTries)]
        public int MaxTries { get; set; }

        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.SmtpSettings.MessagesPerBatch)]
        public int MessagesPerBatch { get; set; }
    }
}