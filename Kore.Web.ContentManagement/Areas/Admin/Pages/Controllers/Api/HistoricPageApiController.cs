using System;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.Results;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class HistoricPageApiController : GenericODataController<HistoricPage, Guid>
    {
        private readonly IRepository<Page> pageRepository;

        public HistoricPageApiController(
            IRepository<HistoricPage> repository,
            IRepository<Page> pageRepository)
            : base(repository)
        {
            this.pageRepository = pageRepository;
        }

        protected override Guid GetId(HistoricPage entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(HistoricPage entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [HttpPost]
        public IHttpActionResult RestoreVersion([FromODataUri] Guid key, ODataActionParameters parameters)
        {
            if (!CheckPermission(CmsPermissions.PageHistoryRestore))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            var pageToRestore = Repository.Find(key);

            if (pageToRestore == null)
            {
                return NotFound();
            }

            var page = pageRepository.Find(pageToRestore.PageId);

            // first we save current as a NEW historical page
            var backupPage = new HistoricPage
            {
                Id = Guid.NewGuid(),
                PageId = page.Id,
                ParentId = page.ParentId,
                PageTypeId = page.PageTypeId,
                Name = page.Name,
                Slug = page.Slug,
                Fields = page.Fields,
                IsEnabled = page.IsEnabled,
                Order = page.Order,
                ShowOnMenus = page.ShowOnMenus,
                DateCreatedUtc = page.DateCreatedUtc,
                DateModifiedUtc = page.DateModifiedUtc,
                CultureCode = page.CultureCode,
                RefId = page.RefId,
                ArchivedDate = DateTime.UtcNow
            };
            Repository.Insert(backupPage);

            // then restore the historical page, as requested
            page.ParentId = pageToRestore.ParentId;
            page.PageTypeId = pageToRestore.PageTypeId;
            page.Name = pageToRestore.Name;
            page.Slug = pageToRestore.Slug;
            page.Fields = pageToRestore.Fields;
            page.IsEnabled = pageToRestore.IsEnabled;
            page.Order = pageToRestore.Order;
            page.ShowOnMenus = pageToRestore.ShowOnMenus;
            page.DateCreatedUtc = pageToRestore.DateCreatedUtc;
            page.DateModifiedUtc = pageToRestore.DateModifiedUtc;
            page.CultureCode = pageToRestore.CultureCode;
            page.RefId = pageToRestore.RefId;

            pageRepository.Update(page);

            return Ok();
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.PageHistoryRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.PageHistoryWrite; }
        }
    }
}