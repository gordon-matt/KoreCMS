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
        [ConfigurationProperty("virtualBasePath", IsRequired = false, DefaultValue = "~/Scripts")]
        public string VirtualBasePath
        {
            get { return (string)base["virtualBasePath"]; }
            set { base["virtualBasePath"] = value; }
        }
    }

    public class StylesConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("virtualBasePath", IsRequired = false, DefaultValue = "~/Content")]
        public string VirtualBasePath
        {
            get { return (string)base["virtualBasePath"]; }
            set { base["virtualBasePath"] = value; }
        }
    }
}