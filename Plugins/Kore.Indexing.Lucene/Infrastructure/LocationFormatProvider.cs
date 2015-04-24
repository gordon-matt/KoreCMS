using System.Collections.Generic;
using Kore.Web.Mvc.Themes;

namespace Kore.Indexing.Lucene.Infrastructure
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
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //shared
                    "~/Areas/{2}/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Kore.Indexing.Lucene/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Plugins/Kore.Indexing.Lucene/Areas/{2}/Views/Shared/{0}.cshtml",
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
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //shared
                    "~/Areas/{2}/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Kore.Indexing.Lucene/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Plugins/Kore.Indexing.Lucene/Areas/{2}/Views/Shared/{0}.cshtml",
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
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Areas/{2}/Themes/{3}/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //shared
                    "~/Areas/{2}/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Areas/{2}/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Kore.Indexing.Lucene/Areas/{2}/Views/{1}/{0}.cshtml",
                    "~/Plugins/Kore.Indexing.Lucene/Areas/{2}/Views/Shared/{0}.cshtml",
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
                    "~/Themes/{2}/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //shared
                    "~/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Kore.Indexing.Lucene/Views/{1}/{0}.cshtml",
                    "~/Plugins/Kore.Indexing.Lucene/Views/Shared/{0}.cshtml",
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
                    "~/Themes/{2}/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //shared
                    "~/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Kore.Indexing.Lucene/Views/{1}/{0}.cshtml",
                    "~/Plugins/Kore.Indexing.Lucene/Views/Shared/{0}.cshtml",
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
                    "~/Themes/{2}/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Themes/{2}/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //shared
                    "~/Views/Plugins/Kore.Indexing.Lucene/{1}/{0}.cshtml",
                    "~/Views/Plugins/Kore.Indexing.Lucene/Shared/{0}.cshtml",

                    //default
                    "~/Plugins/Kore.Indexing.Lucene/Views/{1}/{0}.cshtml",
                    "~/Plugins/Kore.Indexing.Lucene/Views/Shared/{0}.cshtml",
                };
            }
        }

        #endregion ILocationFormatProvider Members
    }
}