using System.Configuration;

namespace Kore.Configuration
{
    public class DataConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("defaultConnectionString", IsRequired = true)]
        public string DefaultConnectionString
        {
            get { return (string)base["defaultConnectionString"]; }
            set { base["defaultConnectionString"] = value; }
        }

        [ConfigurationProperty("autoCreateTables", IsRequired = false, DefaultValue = false)]
        public bool AutoCreateTables
        {
            get { return (bool)base["autoCreateTables"]; }
            set { base["autoCreateTables"] = value; }
        }
    }
}