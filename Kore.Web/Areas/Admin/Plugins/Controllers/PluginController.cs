using System;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Logging;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership.Permissions;

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

            return View("Kore.Web.Areas.Admin.Plugins.Views.Plugin.Index");
        }

        [Compress]
        [Route("install/{systemName}")]
        public ActionResult Install(string systemName)
        {
            systemName = systemName.Replace('-', '.');

            if (!CheckPermission(PluginsPermissions.ManagePlugins))
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                var pluginDescriptor = pluginFinder.Value.GetPluginDescriptors(false)
                    .FirstOrDefault(x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));

                if (pluginDescriptor == null)
                {
                    //No plugin found with the specified id
                    return RedirectToAction("Index");
                }

                //check whether plugin is not installed
                if (pluginDescriptor.Installed)
                {
                    return RedirectToAction("Index");
                }

                //install plugin
                pluginDescriptor.Instance().Install();

                //restart application
                webHelper.Value.RestartAppDomain();
            }
            catch (Exception x)
            {
                Logger.Error(x.Message, x);

                //TODO
                //ErrorNotification(x);
            }

            return RedirectToAction("Index");
        }

        [Compress]
        [Route("uninstall/{systemName}")]
        public ActionResult Uninstall(string systemName)
        {
            systemName = systemName.Replace('-', '.');

            if (!CheckPermission(PluginsPermissions.ManagePlugins))
            {
                return new HttpUnauthorizedResult();
            }

            try
            {
                var pluginDescriptor = pluginFinder.Value.GetPluginDescriptors(false)
                    .FirstOrDefault(x => x.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));

                if (pluginDescriptor == null)
                {
                    //No plugin found with the specified id
                    return RedirectToAction("Index");
                }

                //check whether plugin is installed
                if (!pluginDescriptor.Installed)
                {
                    return RedirectToAction("Index");
                }

                //uninstall plugin
                pluginDescriptor.Instance().Uninstall();

                //restart application
                webHelper.Value.RestartAppDomain();
            }
            catch (Exception x)
            {
                Logger.Error(x.Message, x);

                //TODO
                //ErrorNotification(x);
            }

            return RedirectToAction("Index");
        }
    }
}