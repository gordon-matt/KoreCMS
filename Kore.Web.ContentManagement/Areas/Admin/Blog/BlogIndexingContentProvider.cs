using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog
{
    public class BlogIndexingContentProvider : IIndexingContentProvider
    {
        private readonly IPostService blogService;
        private readonly UrlHelper urlHelper;
        private readonly static char[] trimCharacters = { ' ', '\r', '\n', '\t' };

        public BlogIndexingContentProvider(
            IPostService blogService,
            UrlHelper urlHelper)
        {
            this.blogService = blogService;
            this.urlHelper = urlHelper;
        }

        #region IIndexingContentProvider Members

        public IEnumerable<IDocumentIndex> GetDocuments(Func<string, IDocumentIndex> factory)
        {
            var entries = blogService.Find();
            CultureInfo defaultCultureInfo = null;
            var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
            if (!string.IsNullOrEmpty(workContext.CurrentCultureCode))
            {
                try
                {
                    defaultCultureInfo = new CultureInfo(workContext.CurrentCultureCode);
                }
                catch (Exception)
                {
                    defaultCultureInfo = null;
                }
            }

            foreach (var entry in entries)
            {
                var document = factory(entry.Id.ToString());

                //document.Add("url", urlHelper.Action("Details", "BlogContent", new { slug = entry.Slug })).Store();
                document.Add("url", "/blog/" + entry.Slug).Store();

                string description = CreateDescription(entry.ShortDescription);
                if (!string.IsNullOrEmpty(description))
                {
                    document.Add("description", description.Left(256)).Analyze().Store();
                }

                document.Add("title", entry.Headline).Analyze().Store();
                //document.Add("meta_keywords", entry.me).Analyze();
                //document.Add("meta_description", ).Analyze();
                document.Add("body", entry.FullDescription).Analyze().Store();

                //if (!string.IsNullOrEmpty(entry.CultureCode))
                //{
                //    var cultureInfo = new CultureInfo(page.CultureCode);
                //    document.Add("culture", cultureInfo.LCID).Store();
                //}
                //else
                //{
                if (defaultCultureInfo != null)
                {
                    document.Add("culture", defaultCultureInfo.LCID).Store();
                }
                //}

                yield return document;
            }
        }

        #endregion IIndexingContentProvider Members

        private static string CreateDescription(string bodyContent)
        {
            if (string.IsNullOrEmpty(bodyContent))
            {
                return string.Empty;
            }

            return bodyContent.RemoveTags().Replace("&nbsp;", string.Empty).Trim(trimCharacters);
        }
    }
}