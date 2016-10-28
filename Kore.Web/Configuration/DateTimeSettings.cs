using System.ComponentModel.DataAnnotations;
using Kore.ComponentModel;
using Kore.Web.Configuration;

namespace Kore.Web.Configuration
{
    public class DateTimeSettings : ISettings
    {
        [Required]
        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.DateTime.DefaultTimeZoneId)]
        public string DefaultTimeZoneId { get; set; }

        [LocalizedDisplayName(KoreWebLocalizableStrings.Settings.DateTime.AllowUsersToSetTimeZone)]
        public bool AllowUsersToSetTimeZone { get; set; }

        #region ISettings Members

        public string Name
        {
            get { return "Date/Time Settings"; }
        }

        public bool IsTenantRestricted
        {
            get { return false; }
        }

        public string EditorTemplatePath
        {
            get { return "Kore.Web.Views.Shared.EditorTemplates.DateTimeSettings"; }
        }

        #endregion ISettings Members
    }
}