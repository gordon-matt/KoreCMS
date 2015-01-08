using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Services;
using System.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PagesIndexingContentProvider : IIndexingContentProvider
    {
        private readonly IPageService pageService;
        private readonly IPageTypeService pageTypeService;
        private readonly UrlHelper urlHelper;
        private readonly static char[] trimCharacters = { ' ', '\r', '\n', '\t' };

        public PagesIndexingContentProvider(
            IPageService pageService,
            IPageTypeService pageTypeService,
            UrlHelper urlHelper)
        {
            this.pageService = pageService;
            this.pageTypeService = pageTypeService;
            this.urlHelper = urlHelper;
        }

        public IEnumerable<IDocumentIndex> GetDocuments(Func<string, IDocumentIndex> factory)
        {
            var pages = pageService.Repository.Table.ToHashSet();
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

            foreach (var page in pages)
            {
                var document = factory(page.Id.ToString());

                var pageType = pageTypeService.Find(page.PageTypeId);
                var korePageType = pageTypeService.GetKorePageType(pageType.Name);
                korePageType.InstanceName = page.Name;
                korePageType.LayoutPath = pageType.LayoutPath;
                korePageType.InitializeInstance(page);

                string description;
                korePageType.PopulateDocumentIndex(document, out description);

                //document.Add("url", urlHelper.Action("Index", "PageContent", new { area = Constants.Areas.Pages, slug = page.Slug })).Store();
                document.Add("url", "/" + page.Slug).Store();

                description = CreatePageDescription(description);
                if (!string.IsNullOrEmpty(description))
                {
                    document.Add("description", description.Left(256)).Analyze().Store();
                }

                if (!string.IsNullOrEmpty(page.CultureCode))
                {
                    var cultureInfo = new CultureInfo(page.CultureCode);
                    document.Add("culture", cultureInfo.LCID).Store();
                }
                else
                {
                    if (defaultCultureInfo != null)
                    {
                        document.Add("culture", defaultCultureInfo.LCID).Store();
                    }
                }

                yield return document;
            }
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