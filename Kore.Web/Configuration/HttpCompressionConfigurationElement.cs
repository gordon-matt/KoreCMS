using System.Configuration;

namespace Kore.Web.Configuration
{
    public class HttpCompressionConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = false)]
        public bool Enabled
        {
            get
            {
                bool result;
                if (bool.TryParse(base["enabled"].ToString(), out result))
                {
                    return result;
                }
                return false;
            }
            set { base["enabled"] = value; }
        }
    }
}