using System.ComponentModel;
using Kore.Localization;

namespace Kore.ComponentModel
{
    //TODO: Implement this with a custom DataAnnotationsModelMetadataProvider
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute, IModelAttribute
    {
        private string resourceValue = string.Empty;

        public LocalizedDisplayNameAttribute(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {
                var localizer = LocalizationUtilities.Resolve();
                resourceValue = localizer(ResourceKey);
                return resourceValue;
            }
        }

        public string Name
        {
            get { return "LocalizedDisplayNameAttribute"; }
        }
    }
}