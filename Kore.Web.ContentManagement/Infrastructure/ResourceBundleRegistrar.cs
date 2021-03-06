﻿using System.Web.Optimization;
using Kore.Web.Infrastructure;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public class ResourceBundleRegistrar : IResourceBundleRegistrar
    {
        #region IResourceBundleRegistrar Members

        public void Register(BundleCollection bundles)
        {
            #region Scripts

            #region 3rd Party

            bundles.Add(new ScriptBundle("~/bundles/js/third-party/jQCloud")
                .Include("~/Scripts/Kore.Web.ContentManagement.Scripts.jqcloud.min.js"));

            #endregion 3rd Party

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/blog")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.Blog.Scripts.blog.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/content-blocks")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripts.contentBlocks.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/entity-type-content-blocks")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripts.entityTypeContentBlocks.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/media")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.Media.Scripts.media.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/menus")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.Menus.Scripts.menus.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/message-templates")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.Messaging.Scripts.messageTemplates.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/newsletters/subscribers")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.Newsletters.Scripts.subscribers.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/pages")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.Pages.Scripts.index.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/queued-emails")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.Messaging.Scripts.queuedEmails.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/kore-cms/xml-sitemap")
                .Include("~/Scripts/Kore.Web.ContentManagement.Areas.Admin.Sitemap.Scripts.index.js"));

            #endregion Scripts

            #region Styles

            IItemTransform cssRewriteUrlTransform = new CssRewriteUrlTransform();

            bundles.Add(new StyleBundle("~/bundles/content/third-party/jQCloud")
                .Include("~/Content/Kore.Web.ContentManagement.Content.jqcloud.min.css", cssRewriteUrlTransform));

            #endregion Styles
        }

        #endregion IResourceBundleRegistrar Members
    }
}