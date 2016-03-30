using System.Configuration;

namespace Kore.Web.Configuration
{
    public class ResourcesConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("scripts", IsRequired = false)]
        public ScriptsConfigurationElement Scripts
        {
            get { return (ScriptsConfigurationElement)base["scripts"]; }
        }

        [ConfigurationProperty("styles", IsRequired = false)]
        public StylesConfigurationElement Styles
        {
            get { return (StylesConfigurationElement)base["styles"]; }
        }
    }

    public class ScriptsConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("basePath", IsRequired = false, DefaultValue = "~/Scripts")]
        public string BasePath
        {
            get { return (string)base["basePath"]; }
            set { base["basePath"] = value; }
        }
    }

    public class StylesConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("basePath", IsRequired = false, DefaultValue = "~/Content")]
        public string BasePath
        {
            get { return (string)base["basePath"]; }
            set { base["basePath"] = value; }
        }
    }
}