using System.Configuration;

namespace Kore.Web.Configuration
{
    public class IndexingConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("exactMatch", IsRequired = false, DefaultValue = false)]
        public bool ExactMatch
        {
            get
            {
                bool result;
                if (bool.TryParse(base["exactMatch"].ToString(), out result))
                {
                    return result;
                }
                return false;
            }
            set { base["exactMatch"] = value; }
        }
    }
}