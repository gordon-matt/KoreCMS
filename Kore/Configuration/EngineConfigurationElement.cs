using System.Configuration;

namespace Kore.Configuration
{
    public class EngineConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = false, DefaultValue = "")]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}