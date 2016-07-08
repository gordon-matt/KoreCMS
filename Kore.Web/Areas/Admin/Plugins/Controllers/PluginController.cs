using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership.Permissions;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Areas.Admin.Plugins.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Plugins)]
    public class PluginController : KoreController
    {
        private readonly Lazy<IPluginFinder> pluginFinder;
        private readonly Lazy<IWebHelper> webHelper;

        public PluginController(Lazy<IPluginFinder> pluginFinder, Lazy<IWebHelper> webHelper)
        {
            this.pluginFinder = pluginFinder;
            this.webHelper = webHelper;
        }

        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Plugins.Title));

            ViewBag.Title = T(KoreWebLocalizableStrings.Plugins.Title);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.Plugins.ManagePlugins);

            return PartialView("Kore.Web.Areas.Admin.Plugins.Views.Plugin.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    Install: '{0}',
    InstallPluginSuccess: '{1}',
    InstallPluginError: '{2}',
    Uninstall: '{3}',
    UninstallPluginSuccess: '{4}',
    UninstallPluginError: '{5}',

    Columns: {{
        Group: '{6}',
        PluginInfo: '{7}',
    }}
}}",
   T(KoreWebLocalizableStrings.General.Install),
   T(KoreWebLocalizableStrings.Plugins.InstallPluginSuccess),
   T(KoreWebLocalizableStrings.Plugins.InstallPluginError),
   T(KoreWebLocalizableStrings.General.Uninstall),
   T(KoreWebLocalizableStrings.Plugins.UninstallPluginSuccess),
   T(KoreWebLocalizableStrings.Plugins.UninstallPluginError),
   T(KoreWebLocalizableStrings.Plugins.Model.Group),
   T(KoreWebLocalizableStrings.Plugins.Model.PluginInfo));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }

        [Compress]
        [HttpPost]
        [Route("install/{systemName}")]
        public JsonResult Install(string systemName)
        {
            systemName = systemName.Replace('-', '.');

            if (!CheckPermission(PluginsPermissions.ManagePlugins))
            {
                return Json(new { Success = false, Message = "Unauthorized" });
                //return new HttpUnauthorizedResult();
            }

            try
            {
                var pluginDescriptor = pluginFinder.Value.GetPluginDescriptors(false)
                    .FirstOrDefault(x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));

                if (pluginDescriptor == null)
                {
                    //No plugin found with the specified id
                    return Json(new { Success = false, Message = "Plugin Not Found" });
                    //return RedirectToAction("Index");
                }

                //check whether plugin is not installed
                if (pluginDescriptor.Installed)
                {
                    return Json(new { Success = false, Message = "Plugin Not Installed" });
                    //return RedirectToAction("Index");
                }

                //install plugin
                pluginDescriptor.Instance().Install();

                //restart application
                webHelper.Value.RestartAppDomain();
            }
            catch (Exception x)
            {
                Logger.Error(x.Message, x);
                return Json(new { Success = false, Message = x.GetBaseException().Message });
            }

            return Json(new { Success = true, Message = "Successfully installed the specified plugin." });
            //return RedirectToAction("Index");
        }

        [Compress]
        [HttpPost]
        [Route("uninstall/{systemName}")]
        public JsonResult Uninstall(string systemName)
        {
            systemName = systemName.Replace('-', '.');

            if (!CheckPermission(PluginsPermissions.ManagePlugins))
            {
                return Json(new { Success = false, Message = "Unauthorized" });
                //return new HttpUnauthorizedResult();
            }

            try
            {
                var pluginDescriptor = pluginFinder.Value.GetPluginDescriptors(false)
                    .FirstOrDefault(x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));

                if (pluginDescriptor == null)
                {
                    //No plugin found with the specified id
                    return Json(new { Success = false, Message = "Plugin Not Found" });
                    //return RedirectToAction("Index");
                }

                //check whether plugin is installed
                if (!pluginDescriptor.Installed)
                {
                    return Json(new { Success = false, Message = "Plugin Not Installed" });
                    //return RedirectToAction("Index");
                }

                //uninstall plugin
                pluginDescriptor.Instance().Uninstall();

                //restart application
                webHelper.Value.RestartAppDomain();
            }
            catch (Exception x)
            {
                Logger.Error(x.Message, x);
                return Json(new { Success = false, Message = x.GetBaseException().Message });
            }

            return Json(new { Success = true, Message = "Successfully uninstalled the specified plugin." });
            //return RedirectToAction("Index");
        }
    }
}