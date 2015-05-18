using Autofac;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Plugins.Ecommerce.Simple.Services;
using Kore.Web.Configuration;
using Kore.Web.ContentManagement.Infrastructure;
using Kore.Web.Indexing.Services;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar<ContainerBuilder> Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            if (!PluginManager.IsPluginInstalled("Kore.Plugins.Ecommerce.Simple"))
            {
                return;
            }

            builder.RegisterType<DefaultLocalizableStringsProvider>().As<IDefaultLocalizableStringsProvider>().SingleInstance();

            builder.RegisterType<SimpleCommercePermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();
            builder.RegisterType<NavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();

            builder.RegisterType<AutoMenuProvider>().As<IAutoMenuProvider>().SingleInstance();

            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerDependency();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerDependency();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerDependency();

            builder.RegisterType<PayPalSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<StoreSettings>().As<ISettings>().SingleInstance();

            builder.RegisterType<StoreProductsIndexingContentProvider>().As<IIndexingContentProvider>().InstancePerDependency();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IDependencyRegistrar<ContainerBuilder> Members
    }
}