using System;
using Kore.Localization;

namespace Kore.ComponentModel
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LocalizedHelpTextAttribute : Attribute
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

        public LocalizedHelpTextAttribute(string resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public string HelpText
        {
            get { return T(ResourceKey); }
        }
    }
}