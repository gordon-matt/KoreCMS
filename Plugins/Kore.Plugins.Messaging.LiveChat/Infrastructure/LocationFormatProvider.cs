using System.Collections.Generic;
using Kore.Web.Mvc.Themes;

namespace Kore.Plugins.Messaging.LiveChat.Infrastructure
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
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //shared
                    "~/Areas/{2}/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Messaging.LiveChat/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Plugins/Messaging.LiveChat/Areas/{2}/Views/Shared/{0}.cshtml",
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
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //shared
                    "~/Areas/{2}/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Messaging.LiveChat/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Plugins/Messaging.LiveChat/Areas/{2}/Views/Shared/{0}.cshtml",
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
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //shared
                    "~/Areas/{2}/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Messaging.LiveChat/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Plugins/Messaging.LiveChat/Areas/{2}/Views/Shared/{0}.cshtml",
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
                    "~/Themes/{2}/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //shared
                    "~/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Messaging.LiveChat/Views/{1}/{0}.cshtml",
                    "~/Plugins/Messaging.LiveChat/Views/Shared/{0}.cshtml",
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
                    "~/Themes/{2}/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //shared
                    "~/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Messaging.LiveChat/Views/{1}/{0}.cshtml",
                    "~/Plugins/Messaging.LiveChat/Views/Shared/{0}.cshtml",
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
                    "~/Themes/{2}/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //shared
                    "~/Views/Plugins/Messaging.LiveChat/{1}/{0}.cshtml",
                    "~/Views/Plugins/Messaging.LiveChat/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Messaging.LiveChat/Views/{1}/{0}.cshtml",
                    "~/Plugins/Messaging.LiveChat/Views/Shared/{0}.cshtml",
                };
            }
        }

        #endregion ILocationFormatProvider Members
    }
}