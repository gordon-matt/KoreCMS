using Kore.ComponentModel;
using Kore.Web.Configuration;

namespace Kore.Plugins.Maintenance.Watchdog
{
    public class WatchdogSettings : ISettings
    {
        public WatchdogSettings()
        {
            OnlyShowWatched = false;
            AllowAddRemove = true;
        }

        [LocalizedDisplayName(LocalizableStrings.Settings.OnlyShowWatched)]
        public bool OnlyShowWatched { get; set; }

        [LocalizedDisplayName(LocalizableStrings.Settings.AllowAddRemove)]
        public bool AllowAddRemove { get; set; }

        #region ISettings Members

        public string Name => "Maintenance: Watchdog Settings";

        public bool IsTenantRestricted => false;

        public string EditorTemplatePath => "/Plugins/Maintenance.Watchdog/Views/Shared/EditorTemplates/WatchdogSettings.cshtml";

        #endregion ISettings Members
    }
}