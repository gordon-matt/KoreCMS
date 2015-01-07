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
    }
}