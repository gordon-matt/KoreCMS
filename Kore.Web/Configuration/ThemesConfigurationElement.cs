using System.Configuration;

namespace Kore.Web.Configuration
{
    public class ThemesConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("basePath", IsRequired = false, DefaultValue = "~/Themes/")]
        public string BasePath
        {
            get { return (string)base["basePath"]; }
            set { base["basePath"] = value; }
        }

        [ConfigurationProperty("allowUserToSelectTheme", IsRequired = false, DefaultValue = false)]
        public bool AllowUserToSelectTheme
        {
            get { return (bool)base["allowUserToSelectTheme"]; }
            set { base["allowUserToSelectTheme"] = value; }
        }

        [ConfigurationProperty("defaultDesktopTheme", IsRequired = false, DefaultValue = "Default")]
        public string DefaultDesktopTheme
        {
            get { return (string)base["defaultDesktopTheme"]; }
            set { base["defaultDesktopTheme"] = value; }
        }

        [ConfigurationProperty("defaultMobileTheme", IsRequired = false, DefaultValue = "Mobile")]
        public string DefaultMobileTheme
        {
            get { return (string)base["defaultMobileTheme"]; }
            set { base["defaultMobileTheme"] = value; }
        }
    }
}