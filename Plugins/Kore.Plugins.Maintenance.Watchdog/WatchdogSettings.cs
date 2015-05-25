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

        public string Name
        {
            get { return "Maintenance: Watchdog Settings"; }
        }

        public string EditorTemplatePath
        {
            get { return "/Plugins/Maintenance.Watchdog/Views/Shared/EditorTemplates/WatchdogSettings.cshtml"; }
        }

        #endregion ISettings Members
    }
}