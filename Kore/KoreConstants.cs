namespace Kore
{
    public static class KoreConstants
    {
        public static class CacheKeys
        {
            /// <summary>
            /// {0}: Tenant ID, {1}: Culture Code
            /// </summary>
            public const string LocalizableStringsFormat = "Kore_LocalizableStrings_{0}_{1}";

            /// <summary>
            /// {0}: Tenant ID
            /// </summary>
            public const string LocalizableStringsPatternFormat = "Kore_LocalizableStrings_{0}_.*";
        }

        public static class Roles
        {
            public const string Administrators = "Administrators";
        }

        //public static class StateProviderNames
        //{
        //    public const string CurrentCultureCode = "CurrentCultureCode";
        //}
    }
}