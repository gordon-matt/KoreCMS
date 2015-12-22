using System;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;
using Kore.Web.Security;

namespace Kore.Web.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminAntiForgeryAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly bool _ignore;

        /// <summary>
        /// Anti-forgery security attribute
        /// </summary>
        /// <param name="ignore">Pass false in order to ignore this security validation</param>
        public AdminAntiForgeryAttribute(bool ignore = false)
        {
            this._ignore = ignore;
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (_ignore)
            {
                return;
            }

            // Don't apply filter to child methods
            if (filterContext.IsChildAction)
            {
                return;
            }

            // Only POST requests
            if (!string.Equals(filterContext.HttpContext.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (!DataSettingsHelper.IsDatabaseInstalled)
            {
                return;
            }

            var securitySettings = EngineContext.Current.Resolve<SecuritySettings>();

            if (!securitySettings.EnableXsrfProtectionForAdmin)
            {
                return;
            }

            var validator = new ValidateAntiForgeryTokenAttribute();
            validator.OnAuthorization(filterContext);
        }
    }
}