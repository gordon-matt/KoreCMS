﻿using Kore.Collections;
using System.Data.Entity;
using System.Linq;
using System;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Services
{
    public interface IPageVersionService : IGenericDataService<PageVersion>
    {
        PageVersion GetCurrentVersion(
            Guid pageId,
            string cultureCode = null,
            bool enabledOnly = true,
            bool shownOnMenusOnly = true);

        IEnumerable<PageVersion> GetCurrentVersions(
            string cultureCode = null,
            bool enabledOnly = true,
            bool shownOnMenusOnly = true,
            bool topLevelOnly = false,
            Guid? parentId = null);
    }

    public class PageVersionService : GenericDataService<PageVersion>, IPageVersionService
    {
        private readonly IRepository<Page> pageRepository;

        public PageVersionService(
            ICacheManager cacheManager,
            IRepository<PageVersion> repository,
            IRepository<Page> pageRepository)
            : base(cacheManager, repository)
        {
            this.pageRepository = pageRepository;
        }

        #region IPageVersionService Members

        public PageVersion GetCurrentVersion(
            Guid pageId,
            string cultureCode = null,
            bool enabledOnly = true,
            bool shownOnMenusOnly = true)
        {
            var query = Query().Include(x => x.Page);

            if (enabledOnly)
            {
                query = query.Where(x => x.Page.IsEnabled);
            }

            if (shownOnMenusOnly)
            {
                query = query.Where(x => x.Page.ShowOnMenus);
            }

            return GetCurrentVersionInternal(pageId, query, cultureCode);
        }

        public IEnumerable<PageVersion> GetCurrentVersions(
            string cultureCode = null,
            bool enabledOnly = true,
            bool shownOnMenusOnly = true,
            bool topLevelOnly = false,
            Guid? parentId = null)
        {
            var query = pageRepository.Table;

            if (enabledOnly)
            {
                query = query.Where(x => x.IsEnabled);
            }

            if (shownOnMenusOnly)
            {
                query = query.Where(x => x.ShowOnMenus);
            }

            if (topLevelOnly)
            {
                query = query.Where(x => x.ParentId == null);
            }
            else if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId);
            }

            var pages = query.ToHashSet();
            var pageVersions = Query().Include(x => x.Page).ToHashSet();

            return pages
                .Select(x => GetCurrentVersionInternal(x.Id, pageVersions, cultureCode))
                .Where(x => x != null);
        }

        #endregion

        private static PageVersion GetCurrentVersionInternal(
            Guid pageId,
            IEnumerable<PageVersion> pageVersions,
            string cultureCode = null)
        {
            if (!string.IsNullOrEmpty(cultureCode))
            {
                var localizedVersions = pageVersions
                    .Where(x =>
                        x.PageId == pageId &&
                        x.CultureCode == cultureCode &&
                        x.Status != VersionStatus.Archived);

                var localizedVersion = localizedVersions
                        .Where(x => x.Status == VersionStatus.Published)
                        .OrderByDescending(x => x.DateModifiedUtc)
                        .FirstOrDefault();

                if (localizedVersion == null)
                {
                    localizedVersion = localizedVersions
                        .Where(x => x.Status == VersionStatus.Draft)
                        .OrderByDescending(x => x.DateModifiedUtc)
                        .FirstOrDefault();
                }

                if (localizedVersion != null)
                {
                    return localizedVersion;
                }
            }

            var invariantVersions = pageVersions
                .Where(x =>
                    x.PageId == pageId &&
                    x.CultureCode == null &&
                    x.Status != VersionStatus.Archived);

            var publishedVersion = invariantVersions
                .Where(x => x.Status == VersionStatus.Published)
                .OrderByDescending(x => x.DateModifiedUtc)
                .FirstOrDefault();

            if (publishedVersion == null)
            {
                return invariantVersions
                    .Where(x => x.Status == VersionStatus.Draft)
                    .OrderByDescending(x => x.DateModifiedUtc)
                    .FirstOrDefault();
            }

            return publishedVersion;
        }
    }
}