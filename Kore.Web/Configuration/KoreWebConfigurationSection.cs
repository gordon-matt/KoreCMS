using System.Configuration;

namespace Kore.Web.Configuration
{
    public class KoreWebConfigurationSection : ConfigurationSection
    {
        private static KoreWebConfigurationSection webInstance;

        public static KoreWebConfigurationSection WebInstance
        {
            get
            {
                return webInstance ??
                       (webInstance = (KoreWebConfigurationSection)ConfigurationManager.GetSection("kore.web"));
            }
        }

        [ConfigurationProperty("themes", IsRequired = false)]
        public ThemesConfigurationElement Themes
        {
            get { return (ThemesConfigurationElement)base["themes"]; }
        }

        [ConfigurationProperty("mobile", IsRequired = false)]
        public MobileConfigurationElement Mobile
        {
            get { return (MobileConfigurationElement)base["mobile"]; }
        }

        [ConfigurationProperty("resources", IsRequired = false)]
        public ResourcesConfigurationElement Resources
        {
            get { return (ResourcesConfigurationElement)base["resources"]; }
        }
    }
}