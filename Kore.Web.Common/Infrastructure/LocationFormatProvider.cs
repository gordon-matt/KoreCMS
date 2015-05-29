using System.Collections.Generic;
using Kore.Web.Mvc.Themes;

namespace Kore.Web.Common.Infrastructure
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
                    "~/Themes/{3}/Areas/{2}/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Themes/{3}/Areas/{2}/Views/Kore.Web.Common/Shared/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.Common/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> AreaMasterLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{3}/Areas/{2}/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Themes/{3}/Areas/{2}/Views/Kore.Web.Common/Shared/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.Common/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> AreaPartialViewLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{3}/Areas/{2}/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Themes/{3}/Areas/{2}/Views/Kore.Web.Common/Shared/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Kore.Web.Common/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> ViewLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{2}/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Kore.Web.Common/Shared/{0}.cshtml",
                    "~/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Views/Kore.Web.Common/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> MasterLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{2}/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Kore.Web.Common/Shared/{0}.cshtml",
                    "~/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Views/Kore.Web.Common/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> PartialViewLocationFormats
        {
            get
            {
                return new[]
                {
                    "~/Themes/{2}/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Kore.Web.Common/Shared/{0}.cshtml",
                    "~/Views/Kore.Web.Common/{1}/{0}.cshtml",
                    "~/Views/Kore.Web.Common/Shared/{0}.cshtml",
                };
            }
        }

        #endregion ILocationFormatProvider Members
    }
}