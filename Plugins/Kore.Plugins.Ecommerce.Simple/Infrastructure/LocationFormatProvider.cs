using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.Web.Mvc.Themes;

namespace Kore.Plugins.Ecommerce.Simple.Infrastructure
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
                    //themes
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //shared
                    "~/Areas/{2}/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Plugins.Ecommerce.Simple/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Plugins/Plugins.Ecommerce.Simple/Areas/{2}/Views/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> AreaMasterLocationFormats
        {
            get
            {
                return new[]
                {
                    //themes
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //shared
                    "~/Areas/{2}/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Plugins.Ecommerce.Simple/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Plugins/Plugins.Ecommerce.Simple/Areas/{2}/Views/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> AreaPartialViewLocationFormats
        {
            get
            {
                return new[]
                {
                    //themes
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //shared
                    "~/Areas/{2}/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Plugins.Ecommerce.Simple/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Plugins/Plugins.Ecommerce.Simple/Areas/{2}/Views/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> ViewLocationFormats
        {
            get
            {
                return new[]
                {
                    //themes
                    "~/Themes/{2}/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //shared
                    "~/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Plugins.Ecommerce.Simple/Views/{1}/{0}.cshtml",
                    "~/Plugins/Plugins.Ecommerce.Simple/Views/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> MasterLocationFormats
        {
            get
            {
                return new[]
                {
                    //themes
                    "~/Themes/{2}/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //shared
                    "~/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Plugins.Ecommerce.Simple/Views/{1}/{0}.cshtml",
                    "~/Plugins/Plugins.Ecommerce.Simple/Views/Shared/{0}.cshtml",
                };
            }
        }

        public IEnumerable<string> PartialViewLocationFormats
        {
            get
            {
                return new[]
                {
                    //themes
                    "~/Themes/{2}/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //shared
                    "~/Views/Plugins/Plugins.Ecommerce.Simple/{1}/{0}.cshtml",
                    "~/Views/Plugins/Plugins.Ecommerce.Simple/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Plugins.Ecommerce.Simple/Views/{1}/{0}.cshtml",
                    "~/Plugins/Plugins.Ecommerce.Simple/Views/Shared/{0}.cshtml",
                };
            }
        }

        #endregion ILocationFormatProvider Members
    }
}
