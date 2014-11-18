using System.ComponentModel.DataAnnotations;
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

        #endregion

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required]
        public string Host { get; set; }

        [Required]
        public int Port { get; set; }

        [Display(Name = "Enable SSL")]
        public bool EnableSsl { get; set; }

        [Display(Name = "Use Default Credentials")]
        public bool UseDefaultCredentials { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        [Required]
        public int MaxTries { get; set; }

        [Display(Name = "Messages Per Batch")]
        [Required]
        public int MessagesPerBatch { get; set; }
    }
}