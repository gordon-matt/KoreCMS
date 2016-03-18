using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Kore.Web.Configuration;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog
{
    public class BlogIndexingContentProvider : IIndexingContentProvider
    {
        private readonly IBlogPostService blogService;
        private readonly KoreSiteSettings siteSettings;
        private readonly UrlHelper urlHelper;
        private readonly static char[] trimCharacters = { ' ', '\r', '\n', '\t' };

        public BlogIndexingContentProvider(
            IBlogPostService blogService,
            KoreSiteSettings siteSettings,
            UrlHelper urlHelper)
        {
            this.blogService = blogService;
            this.siteSettings = siteSettings;
            this.urlHelper = urlHelper;
        }

        #region IIndexingContentProvider Members

        public IEnumerable<IDocumentIndex> GetDocuments(Func<string, IDocumentIndex> factory)
        {
            var entries = blogService.Find();
            foreach (var entry in entries)
            {
                var document = factory(entry.Id.ToString());

                document.Add("url", "/blog/" + entry.Slug).Store();

                string description = CreateDescription(entry.ShortDescription);
                if (!string.IsNullOrEmpty(description))
                {
                    document.Add("description", description.Left(256)).Analyze().Store();
                }

                document.Add("title", entry.Headline).Analyze().Store();
                document.Add("meta_keywords", entry.MetaKeywords).Analyze();
                document.Add("meta_description", entry.MetaDescription).Analyze();
                document.Add("body", entry.FullDescription).Analyze().Store();

                var cultureInfo = string.IsNullOrEmpty(siteSettings.DefaultLanguage)
                    ? CultureInfo.InvariantCulture
                    : new CultureInfo(siteSettings.DefaultLanguage);

                document.Add("culture", cultureInfo.LCID).Store();

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