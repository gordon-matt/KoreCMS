﻿using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Common.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().SingleInstance();
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();
            builder.RegisterType<RegionService>().As<IRegionService>().InstancePerDependency();
            builder.RegisterType<Permissions>().As<IPermissionProvider>().SingleInstance();
            //builder.RegisterType<BlogSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<ResourceBundleRegistrar>().As<IResourceBundleRegistrar>().SingleInstance();
            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IDependencyRegistrar Members
    }
}