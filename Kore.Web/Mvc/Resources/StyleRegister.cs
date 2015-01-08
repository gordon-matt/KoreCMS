using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.Configuration;

namespace Kore.Web.Mvc.Resources
{
    public class StyleRegister : ResourceRegister
    {
        private readonly UrlHelper urlHelper;

        public StyleRegister(IWebWorkContext workContext)
            : base(workContext)
        {
            urlHelper = EngineContext.Current.Resolve<UrlHelper>();
        }

        protected override string BundleBasePath
        {
            get { return "~/bundles/content/"; }
        }

        protected override string ResourceType
        {
            get { return "text/css"; }
        }

        protected override string VirtualBasePath
        {
            get { return KoreWebConfigurationSection.WebInstance.Resources.Styles.VirtualBasePath; }
        }

        //public override void IncludeBundle(string bundleName, int? order = null)
        //{
        //    if (!BundleCollection.Styles.ContainsKey(bundleName)) return;

        //    var files = BundleCollection.Styles[bundleName];
        //    foreach (var file in files)
        //    {
        //        var resourceEntry = Include(file);
        //        if (order.HasValue)
        //        {
        //            resourceEntry.Order = order.Value;
        //        }
        //    }
        //}

        public override void IncludeInline(string code, bool ignoreExists = false)
        {
            throw new NotSupportedException();
        }

        //public override MvcHtmlString Render(string bundleName)
        //{
        //    if (!BundleCollection.Styles.ContainsKey(bundleName)) return null;

        //    var files = BundleCollection.Styles[bundleName];
        //    var sb = new StringBuilder();

        //    foreach (var file in files)
        //    {
        //        sb.AppendLine(BuildResource(string.Concat(VirtualBasePath, "/", file)));
        //    }

        //    return new MvcHtmlString(sb.ToString());
        //}

        protected override string BuildInlineResources(IEnumerable<string> resources)
        {
            return string.Format("<style type=\"text/css\">{0}</style>", string.Join(System.Environment.NewLine, resources));
        }

        protected override string BuildResource(string url)
        {
            return string.Format("<link type=\"text/css\" rel=\"stylesheet\" href=\"{0}\" />", urlHelper.Content(url));
        }
    }
}