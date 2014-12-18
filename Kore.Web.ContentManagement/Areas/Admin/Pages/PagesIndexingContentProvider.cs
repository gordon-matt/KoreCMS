using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PagesIndexingContentProvider : IIndexingContentProvider
    {
        private readonly IPageService pageService;
        private readonly UrlHelper urlHelper;
        private readonly static char[] trimCharacters = { ' ', '\r', '\n', '\t' };

        public PagesIndexingContentProvider(IPageService pageService, UrlHelper urlHelper)
        {
            this.pageService = pageService;
            this.urlHelper = urlHelper;
        }

        public IEnumerable<IDocumentIndex> GetDocuments(Func<string, IDocumentIndex> factory)
        {
            var pages = pageService.Repository.Table.ToHashSet();
            CultureInfo defaultCultureInfo = null;
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
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

            throw new NotImplementedException();

            // TODO
            //foreach (var page in pages)
            //{
            //    var document = factory(page.Id.ToString());
            //    document.Add("title", page.Title).Analyze().Store();
            //    document.Add("meta_keywords", page.MetaKeywords).Analyze();
            //    document.Add("meta_description", page.MetaDescription).Analyze();
            //    document.Add("body", page.BodyContent).Analyze().Store();
            //    document.Add("url", urlHelper.Action("PageContent", "Home", new { area = CornerstoneConstants.Areas.Pages, url = page.Slug })).Store();

            //    var description = CreatePageDescription(page.BodyContent);
            //    if (!string.IsNullOrEmpty(description))
            //    {
            //        document.Add("description", description.Substring(0, Math.Min(250, description.Length))).Analyze().Store();
            //    }

            //    if (!string.IsNullOrEmpty(page.CultureCode))
            //    {
            //        var cultureInfo = new CultureInfo(page.CultureCode);
            //        document.Add("culture", cultureInfo.LCID).Store();
            //    }
            //    else
            //    {
            //        if (defaultCultureInfo != null)
            //        {
            //            document.Add("culture", defaultCultureInfo.LCID).Store();
            //        }
            //    }

            //    yield return document;
            //}
        }

        private static string CreatePageDescription(string bodyContent)
        {
            if (string.IsNullOrEmpty(bodyContent))
            {
                return string.Empty;
            }

            return bodyContent.RemoveTags().Replace("&nbsp;", string.Empty).Trim(trimCharacters);
        }
    }
}