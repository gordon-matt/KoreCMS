using System;
using Kore.Web.Configuration;

namespace Kore.Plugins.Watchdog
{
    public class WatchdogSettings : ISettings
    {
        public WatchdogSettings()
        {
            OnlyShowWatched = false;
            AllowAddRemove = true;
        }

        public bool OnlyShowWatched { get; set; }

        public bool AllowAddRemove { get; set; }

        #region ISettings Members

        public string Name
        {
            get { return "Watchdog Settings"; }
        }

        public string EditorTemplatePath
        {
            get { throw new NotImplementedException(); }
        }

        #endregion ISettings Members
    }
}