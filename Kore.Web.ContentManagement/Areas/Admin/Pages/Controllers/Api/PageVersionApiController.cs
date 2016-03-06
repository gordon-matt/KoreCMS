﻿using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.Http.Results;
using Kore.Localization;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    public class PageVersionApiController : GenericODataController<PageVersion, Guid>
    {
        private readonly Lazy<IPageService> pageService;
        private readonly PageSettings settings;

        public PageVersionApiController(
            IPageVersionService service,
            Lazy<IPageService> pageService,
            PageSettings settings)
            : base(service)
        {
            this.pageService = pageService;
            this.settings = settings;
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        //public override IQueryable<PageVersion> Get()
        //{
        //    if (!CheckPermission(ReadPermission))
        //    {
        //        return Enumerable.Empty<PageVersion>().AsQueryable();
        //    };
        //}

        public override IHttpActionResult Delete([FromODataUri] Guid key)
        {
            var entity = Service.FindOne(key);

            // First find previous version and set it to be the current
            PageVersion previous = null;
            using (var connection = Service.OpenConnection())
            {
                previous = connection
                    .Query(x =>
                        x.Id != entity.Id &&
                        x.PageId == entity.PageId &&
                        x.CultureCode == entity.CultureCode)
                    .OrderByDescending(x => x.DateModifiedUtc)
                    .FirstOrDefault();
            }

            if (previous == null)
            {
                var localizer = LocalizationUtilities.Resolve();
                return BadRequest(localizer(KoreCmsLocalizableStrings.Pages.CannotDeleteOnlyVersion).Text);
            }

            previous.Status = VersionStatus.Published;
            Service.Update(previous);

            return base.Delete(key);
        }

        public override IHttpActionResult Patch([FromODataUri] Guid key, Delta<PageVersion> patch)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = Service.FindOne(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                //TODO: might have a big bug here:
                // shouldn't we be gettig BY culture code?
                var currentVersion = Service.FindOne(entity.Id);

                if (currentVersion.Status == VersionStatus.Published)
                {
                    // archive current version before updating
                    var backup = new PageVersion
                    {
                        Id = Guid.NewGuid(),
                        PageId = currentVersion.PageId,
                        CultureCode = currentVersion.CultureCode,
                        DateCreatedUtc = currentVersion.DateCreatedUtc,
                        DateModifiedUtc = currentVersion.DateModifiedUtc,
                        Status = VersionStatus.Archived,
                        Title = currentVersion.Title,
                        Slug = currentVersion.Slug,
                        Fields = currentVersion.Fields,
                    };
                    Service.Insert(backup);

                    RemoveOldVersions(currentVersion.PageId, currentVersion.CultureCode);
                }

                entity.DateModifiedUtc = DateTime.UtcNow;
                Service.Update(entity);
            }
            catch (DbUpdateConcurrencyException x)
            {
                Logger.Error(x.Message, x);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        public override IHttpActionResult Post(PageVersion entity)
        {
            entity.DateCreatedUtc = DateTime.UtcNow;
            entity.DateModifiedUtc = DateTime.UtcNow;
            return base.Post(entity);
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, PageVersion entity)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(GetId(entity)))
            {
                return BadRequest();
            }

            try
            {
                // Getting by ID only is not good enough in this instance, because GetCurrentVersion()
                //  will return the invariant record if no localized record exists. That means when an entity is updated to here,
                //  we are trying to update a localized one, but using the ID of the invariant one! So, we need to get by culture code AND ID
                //  and then if not exists, create it.
                var currentVersion = Service.FindOne(entity.Id);

                if (currentVersion.CultureCode != entity.CultureCode)
                {
                    // We need to duplicate it to a new record!
                    var newRecord = new PageVersion
                    {
                        Id = Guid.NewGuid(),
                        PageId = entity.PageId,
                        CultureCode = entity.CultureCode,
                        Status = currentVersion.Status,
                        Title = entity.Title,
                        Slug = entity.Slug,
                        Fields = entity.Fields,
                        DateCreatedUtc = DateTime.UtcNow,
                        DateModifiedUtc = DateTime.UtcNow,
                    };
                    Service.Insert(newRecord);
                }
                else
                {
                    if (currentVersion.Status == VersionStatus.Published)
                    {
                        // archive current version before updating
                        var backup = new PageVersion
                        {
                            Id = Guid.NewGuid(),
                            PageId = currentVersion.PageId,
                            CultureCode = currentVersion.CultureCode,
                            DateCreatedUtc = currentVersion.DateCreatedUtc,
                            DateModifiedUtc = currentVersion.DateModifiedUtc,
                            Status = VersionStatus.Archived,
                            Title = currentVersion.Title,
                            Slug = currentVersion.Slug,
                            Fields = currentVersion.Fields,
                        };
                        Service.Insert(backup);

                        RemoveOldVersions(currentVersion.PageId, currentVersion.CultureCode);
                    }

                    entity.DateCreatedUtc = currentVersion.DateCreatedUtc;
                    entity.DateModifiedUtc = DateTime.UtcNow;
                    Service.Update(entity);
                }
            }
            catch (DbUpdateConcurrencyException x)
            {
                Logger.Error(x.Message, x);

                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        protected override Guid GetId(PageVersion entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(PageVersion entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [HttpPost]
        public IHttpActionResult RestoreVersion([FromODataUri] Guid key, ODataActionParameters parameters)
        {
            if (!CheckPermission(CmsPermissions.PageHistoryRestore))
            {
                return Unauthorized();
            }

            var versionToRestore = Service.FindOne(key);

            if (versionToRestore == null)
            {
                return NotFound();
            }

            var current = ((IPageVersionService)Service).GetCurrentVersion(
                versionToRestore.PageId,
                versionToRestore.CultureCode,
                enabledOnly: false,
                shownOnMenusOnly: false);

            if (current == null)
            {
                return NotFound();
            }

            // Archive the current one...
            var backup = new PageVersion
            {
                Id = Guid.NewGuid(),
                PageId = current.PageId,
                CultureCode = current.CultureCode,
                DateCreatedUtc = current.DateCreatedUtc,
                DateModifiedUtc = current.DateModifiedUtc,
                Status = VersionStatus.Archived,
                Title = current.Title,
                Slug = current.Slug,
                Fields = current.Fields,
            };
            Service.Insert(backup);

            RemoveOldVersions(current.PageId, current.CultureCode);

            // then restore the historical page, as requested
            current.CultureCode = versionToRestore.CultureCode;
            current.DateCreatedUtc = versionToRestore.DateCreatedUtc;
            current.DateModifiedUtc = versionToRestore.DateModifiedUtc;
            current.Status = VersionStatus.Published;
            current.Title = versionToRestore.Title;
            current.Slug = versionToRestore.Slug;
            current.Fields = versionToRestore.Fields;

            Service.Update(current);

            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetCurrentVersion([FromODataUri] Guid pageId, [FromODataUri] string cultureCode)
        {
            if (!CheckPermission(CmsPermissions.PagesWrite))
            {
                return Unauthorized();
            }

            var currentVersion = ((IPageVersionService)Service).GetCurrentVersion(
                pageId,
                cultureCode,
                enabledOnly: false,
                shownOnMenusOnly: false);

            if (currentVersion == null)
            {
                return NotFound();
            }

            var pageVersion = new EdmPageVersion
            {
                Id = currentVersion.Id,
                PageId = currentVersion.PageId,
                CultureCode = currentVersion.CultureCode,
                Status = currentVersion.Status,
                Title = currentVersion.Title,
                Slug = currentVersion.Slug,
                Fields = currentVersion.Fields
            };

            return Ok(pageVersion);
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.PagesRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.PagesWrite; }
        }

        private void RemoveOldVersions(Guid pageId, string cultureCode)
        {
            List<Guid> pageIdsToKeep = null;
            using (var connection = Service.OpenConnection())
            {
                pageIdsToKeep = connection
                    .Query(x =>
                        x.PageId == pageId &&
                        x.CultureCode == cultureCode)
                    .OrderByDescending(x => x.DateModifiedUtc)
                    .Take(settings.NumberOfPageVersionsToKeep)
                    .Select(x => x.Id)
                    .ToList();
            }

            Service.Delete(x =>
                x.PageId == pageId &&
                x.CultureCode == cultureCode &&
                !pageIdsToKeep.Contains(x.Id));
        }
    }

    public struct EdmPageVersion
    {
        public Guid Id { get; set; }

        public Guid PageId { get; set; }

        public string CultureCode { get; set; }

        public VersionStatus Status { get; set; }

        public string Title { get; set; }

        public string Slug { get; set; }

        public string Fields { get; set; }
    }
}