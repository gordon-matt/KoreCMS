using System.Configuration;

namespace Kore.Web.Configuration
{
    public class KoreWebConfigurationSection : ConfigurationSection
    {
        private static KoreWebConfigurationSection instance;

        public static KoreWebConfigurationSection Instance
        {
            get { return instance ?? (instance = (KoreWebConfigurationSection)ConfigurationManager.GetSection("kore.web")); }
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

        [ConfigurationProperty("httpCompression", IsRequired = false)]
        public HttpCompressionConfigurationElement HttpCompression
        {
            get { return (HttpCompressionConfigurationElement)base["httpCompression"]; }
        }

        [ConfigurationProperty("resources", IsRequired = false)]
        public ResourcesConfigurationElement Resources
        {
            get { return (ResourcesConfigurationElement)base["resources"]; }
        }
    }
}