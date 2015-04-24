using System.Collections.Generic;
using Kore.Web.Mvc.Themes;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public class LocationFormatProvider : ILocationFormatProvider
    {
        #region ILocationFormatProvider Members

        public IEnumerable<string> AreaViewLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{3}/Kore.Web.ContentManagement/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Themes/{3}/Kore.Web.ContentManagement/Areas/{2}/Views/Shared/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.ContentManagement/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.ContentManagement/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> AreaMasterLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{3}/Kore.Web.ContentManagement/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Themes/{3}/Kore.Web.ContentManagement/Areas/{2}/Views/Shared/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.ContentManagement/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.ContentManagement/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> AreaPartialViewLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{3}/Kore.Web.ContentManagement/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Themes/{3}/Kore.Web.ContentManagement/Areas/{2}/Views/Shared/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.ContentManagement/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.ContentManagement/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> ViewLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{2}/Kore.Web.ContentManagement/Views/{1}/{0}.cshtml",
                    "~/Themes/{2}/Kore.Web.ContentManagement/Views/Shared/{0}.cshtml",
                    "~/Views/Kore.Web.ContentManagement/{1}/{0}.cshtml",
                    "~/Views/Kore.Web.ContentManagement/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> MasterLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{2}/Kore.Web.ContentManagement/Views/{1}/{0}.cshtml",
                    "~/Themes/{2}/Kore.Web.ContentManagement/Views/Shared/{0}.cshtml",
                    "~/Views/Kore.Web.ContentManagement/{1}/{0}.cshtml",
                    "~/Views/Kore.Web.ContentManagement/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> PartialViewLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{2}/Kore.Web.ContentManagement/Views/{1}/{0}.cshtml",
                    "~/Themes/{2}/Kore.Web.ContentManagement/Views/Shared/{0}.cshtml",
                    "~/Views/Kore.Web.ContentManagement/{1}/{0}.cshtml",
                    "~/Views/Kore.Web.ContentManagement/Shared/{0}.cshtml",
                };
            }
        }

        #endregion ILocationFormatProvider Members
    }
}