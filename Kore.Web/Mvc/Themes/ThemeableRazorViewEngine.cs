using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Infrastructure;

namespace Kore.Web.Mvc.Themes
{
    public class ThemeableRazorViewEngine : ThemeableBuildManagerViewEngine
    {
        //TODO: This solution works nicely, but what happens when 2 plugins have the same controller and action names?
        //  That will likely cause the view engine to go with the first 1 found only. We need a way to detect the plugin name and
        //  use it as a paramter in the location formats, same as with the theme, then we can do something like this:
        //  "~/Themes/{2}/Views/Plugins/{3}/{1}/{0}.cshtml" where {3} is the plugin name
        //  then we wont need the ILocationFormatProvider either, which would be perfect

        protected static IEnumerable<ILocationFormatProvider> locationFormatProviders;
        protected static Dictionary<string, List<string>> allViewLocationFormats;
        protected static Dictionary<string, List<string>> allAreaViewLocationFormats;

        public ThemeableRazorViewEngine()
        {
            if (locationFormatProviders == null)
            {
                locationFormatProviders = EngineContext.Current.ResolveAll<ILocationFormatProvider>() ?? Enumerable.Empty<ILocationFormatProvider>();
                allViewLocationFormats = locationFormatProviders.ToDictionary(k => k.GetType().Assembly.FullName, v => GetViewLocationFormats(v));
                allAreaViewLocationFormats = locationFormatProviders.ToDictionary(k => k.GetType().Assembly.FullName, v => GetAreaViewLocationFormats(v));
            }

            #region AreaViewLocationFormats

            var areaViewLocationFormats = new List<string>
            {
                //themes
                "~/Areas/{2}/Themes/{3}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Themes/{3}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Themes/{3}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Themes/{3}/Views/Shared/{0}.vbhtml",

                //default
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            areaViewLocationFormats.InsertRange(0, locationFormatProviders.SelectMany(x => x.AreaViewLocationFormats));
            AreaViewLocationFormats = areaViewLocationFormats.ToArray();

            #endregion AreaViewLocationFormats

            #region AreaMasterLocationFormats

            var areaMasterLocationFormats = new List<string>
            {
                //themes
                "~/Areas/{2}/Themes/{3}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Themes/{3}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Themes/{3}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Themes/{3}/Views/Shared/{0}.vbhtml",

                //default
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            areaMasterLocationFormats.InsertRange(0, locationFormatProviders.SelectMany(x => x.AreaMasterLocationFormats));
            AreaMasterLocationFormats = areaMasterLocationFormats.ToArray();

            #endregion AreaMasterLocationFormats

            #region AreaPartialViewLocationFormats

            var areaPartialViewLocationFormats = new List<string>
            {
                //themes
                "~/Areas/{2}/Themes/{3}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Themes/{3}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Themes/{3}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Themes/{3}/Views/Shared/{0}.vbhtml",

                //default
                "~/Areas/{2}/Views/{1}/{0}.cshtml",
                "~/Areas/{2}/Views/{1}/{0}.vbhtml",
                "~/Areas/{2}/Views/Shared/{0}.cshtml",
                "~/Areas/{2}/Views/Shared/{0}.vbhtml"
            };
            areaPartialViewLocationFormats.InsertRange(0, locationFormatProviders.SelectMany(x => x.AreaPartialViewLocationFormats));
            AreaPartialViewLocationFormats = areaPartialViewLocationFormats.ToArray();

            #endregion AreaPartialViewLocationFormats

            #region ViewLocationFormats

            var viewLocationFormats = new List<string>
            {
                //themes
                "~/Themes/{2}/Views/{1}/{0}.cshtml",
                "~/Themes/{2}/Views/{1}/{0}.vbhtml",
                "~/Themes/{2}/Views/Shared/{0}.cshtml",
                "~/Themes/{2}/Views/Shared/{0}.vbhtml",

                //default
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",

                //Admin
                "~/Areas/Admin/Views/{1}/{0}.cshtml",
                "~/Areas/Admin/Views/{1}/{0}.vbhtml",
                "~/Areas/Admin/Views/Shared/{0}.cshtml",
                "~/Areas/Admin/Views/Shared/{0}.vbhtml",
                //"~/Administration/Views/{1}/{0}.cshtml",
                //"~/Administration/Views/{1}/{0}.vbhtml",
                //"~/Administration/Views/Shared/{0}.cshtml",
                //"~/Administration/Views/Shared/{0}.vbhtml",
            };
            viewLocationFormats.InsertRange(0, locationFormatProviders.SelectMany(x => x.ViewLocationFormats));
            ViewLocationFormats = viewLocationFormats.ToArray();

            #endregion ViewLocationFormats

            #region MasterLocationFormats

            var masterLocationFormats = new List<string>
            {
                //themes
                "~/Themes/{2}/Views/{1}/{0}.cshtml",
                "~/Themes/{2}/Views/{1}/{0}.vbhtml",
                "~/Themes/{2}/Views/Shared/{0}.cshtml",
                "~/Themes/{2}/Views/Shared/{0}.vbhtml",

                //default
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml"
            };
            masterLocationFormats.InsertRange(0, locationFormatProviders.SelectMany(x => x.MasterLocationFormats));
            MasterLocationFormats = masterLocationFormats.ToArray();

            #endregion MasterLocationFormats

            #region PartialViewLocationFormats

            var partialViewLocationFormats = new List<string>
            {
                //themes
                "~/Themes/{2}/Views/{1}/{0}.cshtml",
                "~/Themes/{2}/Views/{1}/{0}.vbhtml",
                "~/Themes/{2}/Views/Shared/{0}.cshtml",
                "~/Themes/{2}/Views/Shared/{0}.vbhtml",

                //default
                "~/Views/{1}/{0}.cshtml",
                "~/Views/{1}/{0}.vbhtml",
                "~/Views/Shared/{0}.cshtml",
                "~/Views/Shared/{0}.vbhtml",

                //Admin
                "~/Areas/Admin/Views/{1}/{0}.cshtml",
                "~/Areas/Admin/Views/{1}/{0}.vbhtml",
                "~/Areas/Admin/Views/Shared/{0}.cshtml",
                "~/Areas/Admin/Views/Shared/{0}.vbhtml",
                //"~/Administration/Views/{1}/{0}.cshtml",
                //"~/Administration/Views/{1}/{0}.vbhtml",
                //"~/Administration/Views/Shared/{0}.cshtml",
                //"~/Administration/Views/Shared/{0}.vbhtml",
            };
            partialViewLocationFormats.InsertRange(0, locationFormatProviders.SelectMany(x => x.PartialViewLocationFormats));
            PartialViewLocationFormats = partialViewLocationFormats.ToArray();

            #endregion PartialViewLocationFormats

            FileExtensions = new[] { "cshtml", "vbhtml" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string layoutPath = null;
            var runViewStartPages = false;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions);
            //return new RazorView(controllerContext, partialPath, layoutPath, runViewStartPages, fileExtensions, base.ViewPageActivator);
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string layoutPath = masterPath;
            var runViewStartPages = true;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, viewPath, layoutPath, runViewStartPages, fileExtensions);
        }

        private List<string> GetViewLocationFormats(ILocationFormatProvider locationFormatProvider)
        {
            var formats = locationFormatProvider.ViewLocationFormats.ToList();
            formats.AddRange(locationFormatProvider.PartialViewLocationFormats);
            formats.AddRange(locationFormatProvider.MasterLocationFormats);
            return formats;
        }

        private List<string> GetAreaViewLocationFormats(ILocationFormatProvider locationFormatProvider)
        {
            var formats = locationFormatProvider.AreaViewLocationFormats.ToList();
            formats.AddRange(locationFormatProvider.AreaPartialViewLocationFormats);
            formats.AddRange(locationFormatProvider.AreaMasterLocationFormats);
            return formats;
        }

        protected override List<ViewLocation> GetViewLocations(string[] viewLocationFormats, string[] areaViewLocationFormats, ControllerContext controllerContext)
        {
            if (locationFormatProviders.IsNullOrEmpty())
            {
                return base.GetViewLocations(viewLocationFormats, areaViewLocationFormats, controllerContext);
            }

            string controllerAssemblyName = controllerContext.Controller.GetType().Assembly.FullName;

            // Remove everything except base (main app) paths and the plugin paths

            var invalidViewLocationFormats = allViewLocationFormats
                .Where(x => x.Key != controllerAssemblyName)
                .SelectMany(x => x.Value);

            var invalidAreaViewLocationFormats = allAreaViewLocationFormats
                .Where(x => x.Key != controllerAssemblyName)
                .SelectMany(x => x.Value);

            var list = new List<ViewLocation>();

            if (areaViewLocationFormats != null)
            {
                var validAreaViewLocationFormats = areaViewLocationFormats
                    .Where(x => !invalidAreaViewLocationFormats.Contains(x));

                list.AddRange(validAreaViewLocationFormats.Select(x => new AreaAwareViewLocation(x)).Cast<ViewLocation>());
            }

            if (viewLocationFormats != null)
            {
                var validViewLocationFormats = viewLocationFormats
                    .Where(x => !invalidViewLocationFormats.Contains(x));

                list.AddRange(validViewLocationFormats.Select(x => new ViewLocation(x)));
            }

            return list;
        }
    }
}