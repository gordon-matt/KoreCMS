using System.Globalization;
using System.Web.Mvc;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUIWizardValueProvider : IValueProvider
    {
        private readonly object model;
        private readonly IValueProvider valueProvider;

        public RoboUIWizardValueProvider(IValueProvider valueProvider, object model)
        {
            this.valueProvider = valueProvider;
            this.model = model;
        }

        public bool ContainsPrefix(string prefix)
        {
            return prefix == "model" || valueProvider.ContainsPrefix(prefix);
        }

        public ValueProviderResult GetValue(string key)
        {
            return key == "model"
                ? new ValueProviderResult(model, null, CultureInfo.InvariantCulture)
                : valueProvider.GetValue(key);
        }
    }
}