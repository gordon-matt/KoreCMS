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
            return Json(new
            {
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                Install = T(KoreWebLocalizableStrings.General.Install).Text,
                InstallPluginSuccess = T(KoreWebLocalizableStrings.Plugins.InstallPluginSuccess).Text,
                InstallPluginError = T(KoreWebLocalizableStrings.Plugins.InstallPluginError).Text,
                Uninstall = T(KoreWebLocalizableStrings.General.Uninstall).Text,
                UninstallPluginSuccess = T(KoreWebLocalizableStrings.Plugins.UninstallPluginSuccess).Text,
                UninstallPluginError = T(KoreWebLocalizableStrings.Plugins.UninstallPluginError).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Group = T(KoreWebLocalizableStrings.Plugins.Model.Group).Text,
                    PluginInfo = T(KoreWebLocalizableStrings.Plugins.Model.PluginInfo).Text,
                }
            }, JsonRequestBehavior.AllowGet);
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

            return Json(new { Success = true, Message = T(KoreWebLocalizableStrings.Plugins.InstallPluginSuccess).Text });
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

            return Json(new { Success = true, Message = T(KoreWebLocalizableStrings.Plugins.UninstallPluginSuccess).Text });
            //return RedirectToAction("Index");
        }
    }
}