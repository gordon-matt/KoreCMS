using Kore.Collections;
using System.Data.Entity;
using System.Linq;
using System;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using System.Collections.Generic;
using Kore.Web.Configuration;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Services
{
    public interface IPageVersionService : IGenericDataService<PageVersion>
    {
        PageVersion GetCurrentVersion(
            int tenantId,
            Guid pageId,
            string cultureCode = null,
            bool enabledOnly = true,
            bool shownOnMenusOnly = true,
            bool forceGetInvariantIfLocalizedUnavailable = false);

        IEnumerable<PageVersion> GetCurrentVersions(
            int tenantId,
            string cultureCode = null,
            bool enabledOnly = true,
            bool shownOnMenusOnly = true,
            bool topLevelOnly = false,
            Guid? parentId = null,
            bool forceGetInvariantIfLocalizedUnavailable = false);
    }

    public class PageVersionService : GenericDataService<PageVersion>, IPageVersionService
    {
        private readonly IRepository<Page> pageRepository;
        private readonly PageSettings pageSettings;
        private readonly KoreSiteSettings siteSettings;

        public PageVersionService(
            ICacheManager cacheManager,
            IRepository<PageVersion> repository,
            IRepository<Page> pageRepository,
            PageSettings pageSettings,
            KoreSiteSettings siteSettings)
            : base(cacheManager, repository)
        {
            this.pageRepository = pageRepository;
            this.pageSettings = pageSettings;
            this.siteSettings = siteSettings;
        }

        #region IPageVersionService Members

        /// <summary>
        /// Gets the most recent localized version of a page in the specified culture
        /// </summary>
        /// <param name="tenantId">Tenant ID</param>
        /// <param name="pageId">Page ID</param>
        /// <param name="cultureCode">Culture Code</param>
        /// <param name="enabledOnly">Only search enabled pages.</param>
        /// <param name="shownOnMenusOnly">Only search for pages that are shown on the menu.</param>
        /// <param name="forceGetInvariantIfLocalizedUnavailable">Only use this for admin purposes (example: create new localized version of a page)</param>
        /// <returns></returns>
        public PageVersion GetCurrentVersion(
            int tenantId,
            Guid pageId,
            string cultureCode = null,
            bool enabledOnly = true,
            bool shownOnMenusOnly = true,
            bool forceGetInvariantIfLocalizedUnavailable = false)
        {
            using (var pageVersionConnection = OpenConnection())
            {
                var query = pageVersionConnection.Query(x => x.TenantId == tenantId).Include(x => x.Page);

                if (enabledOnly)
                {
                    query = query.Where(x => x.Page.IsEnabled);
                }

                if (shownOnMenusOnly)
                {
                    query = query.Where(x => x.Page.ShowOnMenus);
                }

                return GetCurrentVersionInternal(pageId, query, cultureCode, forceGetInvariantIfLocalizedUnavailable);
            }
        }

        public IEnumerable<PageVersion> GetCurrentVersions(
            int tenantId,
            string cultureCode = null,
            bool enabledOnly = true,
            bool shownOnMenusOnly = true,
            bool topLevelOnly = false,
            Guid? parentId = null,
            bool forceGetInvariantIfLocalizedUnavailable = false)
        {
            ICollection<Page> pages = null;

            using (var pageConnection = pageRepository.OpenConnection())
            {
                var query = pageConnection.Query(x => x.TenantId == tenantId);

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

                pages = query.ToHashSet();
            }

            using (var pageVersionConnection = OpenConnection())
            {
                var pageVersions = pageVersionConnection
                    .Query(x => x.TenantId == tenantId)
                    .Include(x => x.Page)
                    .ToHashSet();

                return pages
                    .Select(x => GetCurrentVersionInternal(x.Id, pageVersions, cultureCode, forceGetInvariantIfLocalizedUnavailable))
                    .Where(x => x != null);
            }
        }

        #endregion

        /// <summary>
        /// Gets the most recent localized version of a page in the specified culture
        /// </summary>
        /// <param name="pageId">Page ID</param>
        /// <param name="pageVersions">A collection of page versions to search through.</param>
        /// <param name="cultureCode">Culture Code</param>
        /// <param name="forceGetInvariantIfLocalizedUnavailable">Only use this for admin purposes (example: create new localized version of a page)</param>
        /// <returns></returns>
        private PageVersion GetCurrentVersionInternal(
            Guid pageId,
            IEnumerable<PageVersion> pageVersions,
            string cultureCode = null,
            bool forceGetInvariantIfLocalizedUnavailable = false)
        {
            // The default culture is ALWAYS treated as the invariant, so if the requested culture is the same as the default, then
            //  we skip this section and go straight to the invariant version below
            if (!string.IsNullOrEmpty(cultureCode) && cultureCode != siteSettings.DefaultLanguage)
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

                if (!pageSettings.ShowInvariantVersionIfLocalizedUnavailable && !forceGetInvariantIfLocalizedUnavailable)
                {
                    return null;
                }
            }

            // Get invariant version
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