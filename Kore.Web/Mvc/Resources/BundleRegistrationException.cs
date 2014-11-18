using System;

namespace Kore.Web.Mvc.Resources
{
    public class BundleRegistrationException : Exception
    {
        private const string MessageFormat = "'{0}' could not be registered.";

        public BundleRegistrationException(string bundleUrl)
            : base(string.Format(MessageFormat, bundleUrl))
        {
        }
    }
}