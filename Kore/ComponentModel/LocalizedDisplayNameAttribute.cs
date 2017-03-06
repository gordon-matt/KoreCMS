using System;
using System.ComponentModel;
using Kore.Localization;

namespace Kore.ComponentModel
{
    //TODO: Implement this with a custom DataAnnotationsModelMetadataProvider
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute//, IModelAttribute
    {
        private static Localizer localizer;

        private static Localizer T
        {
            get
            {
                if (localizer == null)
                {
                    localizer = LocalizationUtilities.Resolve();
                }
                return localizer;
            }
        }

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
                return T(ResourceKey);
            }
        }

        //public string Name
        //{
        //    get { return "LocalizedDisplayNameAttribute"; }
        //}
    }
}