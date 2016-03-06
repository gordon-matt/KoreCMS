using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PagesIndexingContentProvider : IIndexingContentProvider
    {
        private readonly IPageService pageService;
        private readonly IPageVersionService pageVersionService;
        private readonly IPageTypeService pageTypeService;
        private readonly UrlHelper urlHelper;
        private readonly static char[] trimCharacters = { ' ', '\r', '\n', '\t' };

        public PagesIndexingContentProvider(
            IPageService pageService,
            IPageVersionService pageVersionService,
            IPageTypeService pageTypeService,
            UrlHelper urlHelper)
        {
            this.pageService = pageService;
            this.pageVersionService = pageVersionService;
            this.pageTypeService = pageTypeService;
            this.urlHelper = urlHelper;
        }

        public IEnumerable<IDocumentIndex> GetDocuments(Func<string, IDocumentIndex> factory)
        {
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

            // TODO: If there are too many records, we may get issues like "OutOfMemoryException".
            //  We should implement paging here.
            //  Also: shouldn't we only be indexing the latest versions of each page, rather than including every old version?
            HashSet<PageVersion> pageVersions = null;
            using (var connection = pageVersionService.OpenConnection())
            {
                pageVersions = connection.Query()
                    .Include(x => x.Page)
                    .ToHashSet();
            }

            foreach (var pageVersion in pageVersions)
            {
                var document = factory(pageVersion.Id.ToString());

                var pageType = pageTypeService.FindOne(pageVersion.Page.PageTypeId);
                var korePageType = pageTypeService.GetKorePageType(pageType.Name);
                korePageType.InstanceName = pageVersion.Title;
                korePageType.LayoutPath = pageType.LayoutPath;
                korePageType.InitializeInstance(pageVersion);

                string description;
                korePageType.PopulateDocumentIndex(document, out description);

                //document.Add("url", urlHelper.Action("Index", "PageContent", new { area = Constants.Areas.Pages, slug = page.Slug })).Store();
                document.Add("url", "/" + pageVersion.Slug).Store();

                description = CreatePageDescription(description);
                if (!string.IsNullOrEmpty(description))
                {
                    document.Add("description", description.Left(256)).Analyze().Store();
                }

                if (!string.IsNullOrEmpty(pageVersion.CultureCode))
                {
                    var cultureInfo = new CultureInfo(pageVersion.CultureCode);
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