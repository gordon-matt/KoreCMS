using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Optimization;
using Kore.Infrastructure;

namespace Kore.Web.Mvc.Resources
{
    public abstract class ResourceRegister
    {
        private readonly IResourcesManager resourcesManager;
        private readonly IWebWorkContext workContext;

        protected ResourceRegister(IWebWorkContext workContext)
        {
            resourcesManager = EngineContext.Current.Resolve<IResourcesManager>();
            this.workContext = workContext;
        }

        protected abstract string BundleBasePath { get; }

        protected abstract string ResourceType { get; }

        protected abstract string VirtualBasePath { get; }

        public virtual ResourceEntry Include(string path, bool isThemePath = false)
        {
            ResourceEntry resourceEntry;
            if (isThemePath)
            {
                var virtualBasePath = VirtualBasePath.Replace("~/", string.Empty);
                resourceEntry = new ResourceEntry(ResourceType, string.Format("~/Themes/{0}/{1}/{2}", workContext.CurrentDesktopTheme, virtualBasePath, path));
            }
            else
            {
                resourceEntry = new ResourceEntry(ResourceType, string.Concat(VirtualBasePath, "/", path));
            }
            resourcesManager.RegisterResource(resourceEntry);
            return resourceEntry;
        }

        //public abstract void IncludeBundle(string bundleName, int? order = null);

        public virtual string GetBundleUrl(string bundleName)
        {
            string bundleUrl = string.Concat(BundleBasePath, bundleName);
            return BundleTable.Bundles.ResolveBundleUrl(bundleUrl);
        }

        public virtual ResourceEntry IncludeBundle(string bundleName, int? order = null)
        {
            string bundleUrl = string.Concat(BundleBasePath, bundleName);

            var url = BundleTable.Bundles.ResolveBundleUrl(bundleUrl);
            if (!string.IsNullOrEmpty(url))
            {
                var resourceEntry = new ResourceEntry(ResourceType, url);
                resourcesManager.RegisterResource(resourceEntry);

                if (order.HasValue)
                {
                    resourceEntry.Order = order.Value;
                }

                return resourceEntry;
            }

            throw new UnregisteredBundleException(bundleUrl);
        }

        public void IncludeExternal(string path, int? order = null)
        {
            if (order.HasValue)
            {
                resourcesManager.RegisterResource(new ResourceEntry(ResourceType, path).HasOrder(order.Value));
            }
            else
            {
                resourcesManager.RegisterResource(new ResourceEntry(ResourceType, path));
            }
        }

        public virtual void IncludeInline(string code, bool ignoreExists = false)
        {
            resourcesManager.RegisterInlineResource(ResourceType, code, ignoreExists);
        }

        public virtual MvcHtmlString Render()
        {
            var resources = resourcesManager.GetResources(ResourceType);

            var inlineResources = resourcesManager.GetInlineResources(ResourceType);

            if (!resources.Any() && !inlineResources.Any())
            {
                return null;
            }

            var sb = new StringBuilder();

            foreach (var resource in resources)
            {
                sb.AppendLine(BuildResource(resource));
            }

            if (inlineResources.Any())
            {
                sb.Append(BuildInlineResources(inlineResources));
            }

            return new MvcHtmlString(sb.ToString());
        }

        //public abstract MvcHtmlString Render(string bundleName);

        public virtual MvcHtmlString Render(string bundleName)
        {
            string bundleUrl = string.Concat(BundleBasePath, bundleName);

            var url = BundleTable.Bundles.ResolveBundleUrl(bundleUrl);
            if (!string.IsNullOrEmpty(url))
            {
                return new MvcHtmlString(BuildResource(url));
            }

            throw new UnregisteredBundleException(bundleUrl);
        }

        public string ThemePath(string virtualPath)
        {
            var virtualBasePath = VirtualBasePath.Replace("~/", string.Empty);
            var path = string.Format("~/Themes/{0}/{1}/{2}", workContext.CurrentDesktopTheme, virtualBasePath, virtualPath);
            var urlHelper = EngineContext.Current.Resolve<UrlHelper>();
            return urlHelper.Content(path);
        }

        protected abstract string BuildInlineResources(IEnumerable<string> resources);

        protected abstract string BuildResource(string url);
    }
}