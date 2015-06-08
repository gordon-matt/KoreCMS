using System;
using Kore.Web.Common.Areas.Admin.Regions;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class StoreRegionSettings : IRegionSettings
    {
        internal const string SettingsName = "Simple Commerce";

        public bool IsEnabled { get; set; }

        #region IRegionData Members

        public string Name
        {
            get { return SettingsName; }
        }

        public string EditorTemplatePath
        {
            get { return "/Plugins/Ecommerce.Simple/Views/Shared/EditorTemplates/StoreRegionSettings.cshtml"; }
        }

        #endregion IRegionData Members
    }
}