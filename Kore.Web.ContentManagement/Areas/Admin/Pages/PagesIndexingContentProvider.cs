﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Kore.Localization.Services;
using Kore.Web.Configuration;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PagesIndexingContentProvider : IIndexingContentProvider
    {
        private readonly ILanguageService languageService;
        private readonly IPageService pageService;
        private readonly IPageVersionService pageVersionService;
        private readonly IPageTypeService pageTypeService;
        private readonly KoreSiteSettings siteSettings;
        private readonly UrlHelper urlHelper;
        private static readonly char[] trimCharacters = { ' ', '\r', '\n', '\t' };

        public PagesIndexingContentProvider(
            ILanguageService languageService,
            IPageService pageService,
            IPageVersionService pageVersionService,
            IPageTypeService pageTypeService,
            KoreSiteSettings siteSettings,
            UrlHelper urlHelper)
        {
            this.languageService = languageService;
            this.pageService = pageService;
            this.pageVersionService = pageVersionService;
            this.pageTypeService = pageTypeService;
            this.siteSettings = siteSettings;
            this.urlHelper = urlHelper;
        }

        public IEnumerable<IDocumentIndex> GetDocuments(Func<string, IDocumentIndex> factory)
        {
            var pageVersions = pageVersionService.GetCurrentVersions(shownOnMenusOnly: false);
            foreach (var document in ProcessPageVersions(pageVersions, factory, siteSettings.DefaultLanguage))
            {
                yield return document;
            }

            List<string> cultureCodes = null;
            using (var connection = languageService.OpenConnection())
            {
                cultureCodes = connection.Query().Select(x => x.CultureCode).ToList();
            }

            foreach (string cultureCode in cultureCodes)
            {
                if (siteSettings.DefaultLanguage == cultureCode)
                {
                    // Already added (see top of this method)...
                    continue;
                }

                pageVersions = pageVersionService.GetCurrentVersions(cultureCode: cultureCode, shownOnMenusOnly: false);
                foreach (var document in ProcessPageVersions(pageVersions, factory, cultureCode))
                {
                    yield return document;
                }
            }
        }

        private IEnumerable<IDocumentIndex> ProcessPageVersions(IEnumerable<PageVersion> pageVersions, Func<string, IDocumentIndex> factory, string cultureCode)
        {
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

                document.Add("url", "/" + pageVersion.Slug).Store();

                description = CreatePageDescription(description);
                if (!string.IsNullOrEmpty(description))
                {
                    document.Add("description", description.Left(256)).Analyze().Store();
                }

                var cultureInfo = string.IsNullOrEmpty(cultureCode)
                    ? CultureInfo.InvariantCulture
                    : new CultureInfo(cultureCode);

                document.Add("culture", cultureInfo.LCID).Store();

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