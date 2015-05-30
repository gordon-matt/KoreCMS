using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class PageApiController : GenericODataController<Page, Guid>
    {
        private readonly IHistoricPageService historicPageService;

        public PageApiController(
            IPageService service,
            IHistoricPageService historicPageService)
            : base(service)
        {
            this.historicPageService = historicPageService;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<Page> Get()
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<Page>().AsQueryable();
            }
            return Service.Repository.Table.Where(x => x.RefId == null);
        }

        public override IHttpActionResult Patch([FromODataUri] Guid key, Delta<Page> patch)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
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
                var currentPage = Service.FindOne(entity.Id);

                // archive current version before updating
                var historicPage = new HistoricPage
                {
                    Id = Guid.NewGuid(),
                    PageId = currentPage.Id,
                    ParentId = currentPage.ParentId,
                    PageTypeId = currentPage.PageTypeId,
                    Name = currentPage.Name,
                    Slug = currentPage.Slug,
                    Fields = currentPage.Fields,
                    IsEnabled = currentPage.IsEnabled,
                    Order = currentPage.Order,
                    ShowOnMenus = currentPage.ShowOnMenus,
                    AccessRestrictions = currentPage.AccessRestrictions,
                    DateCreatedUtc = currentPage.DateCreatedUtc,
                    DateModifiedUtc = currentPage.DateModifiedUtc,
                    CultureCode = currentPage.CultureCode,
                    RefId = currentPage.RefId,
                    ArchivedDate = DateTime.UtcNow
                };
                historicPageService.Insert(historicPage);

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

        public override IHttpActionResult Post(Page entity)
        {
            entity.DateCreatedUtc = DateTime.UtcNow;
            entity.DateModifiedUtc = DateTime.UtcNow;
            return base.Post(entity);
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, Page entity)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
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
                var currentPage = Service.FindOne(entity.Id);

                // archive current version before updating
                var historicPage = new HistoricPage
                {
                    Id = Guid.NewGuid(),
                    PageId = currentPage.Id,
                    ParentId = currentPage.ParentId,
                    PageTypeId = currentPage.PageTypeId,
                    Name = currentPage.Name,
                    Slug = currentPage.Slug,
                    Fields = currentPage.Fields,
                    IsEnabled = currentPage.IsEnabled,
                    Order = currentPage.Order,
                    ShowOnMenus = currentPage.ShowOnMenus,
                    AccessRestrictions = currentPage.AccessRestrictions,
                    DateCreatedUtc = currentPage.DateCreatedUtc,
                    DateModifiedUtc = currentPage.DateModifiedUtc,
                    CultureCode = currentPage.CultureCode,
                    RefId = currentPage.RefId,
                    ArchivedDate = DateTime.UtcNow
                };
                historicPageService.Insert(historicPage);

                entity.DateCreatedUtc = currentPage.DateCreatedUtc;
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

        protected override Guid GetId(Page entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Page entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [HttpPost]
        public EdmPage Translate(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new EdmPage();
            }

            Guid pageId = (Guid)parameters["pageId"];
            string cultureCode = (string)parameters["cultureCode"];

            var record = Service.FindOne(x => x.RefId == pageId && x.CultureCode == cultureCode);
            if (record == null)
            {
                record = Service.FindOne(pageId);

                var translation = new Page
                {
                    Id = Guid.NewGuid(),
                    ParentId = record.ParentId,
                    PageTypeId = record.PageTypeId,
                    Name = record.Name,
                    Slug = record.Slug,
                    Fields = record.Fields,
                    Order = record.Order,
                    ShowOnMenus = record.ShowOnMenus,
                    AccessRestrictions = record.AccessRestrictions,
                    DateCreatedUtc = DateTime.UtcNow,
                    DateModifiedUtc = DateTime.UtcNow,
                    CultureCode = cultureCode,
                    RefId = pageId
                };

                Service.Insert(translation);

                return new EdmPage
                {
                    Id = translation.Id,
                    ParentId = translation.ParentId,
                    PageTypeId = translation.PageTypeId,
                    Name = translation.Name,
                    Slug = translation.Slug,
                    Fields = translation.Fields,
                    Order = translation.Order,
                    ShowOnMenus = translation.ShowOnMenus,
                    CultureCode = translation.CultureCode,
                    RefId = translation.RefId
                };
            }
            else
            {
                return new EdmPage
                {
                    Id = record.Id,
                    ParentId = record.ParentId,
                    PageTypeId = record.PageTypeId,
                    Name = record.Name,
                    Slug = record.Slug,
                    Fields = record.Fields,
                    IsEnabled = record.IsEnabled,
                    Order = record.Order,
                    ShowOnMenus = record.ShowOnMenus,
                    AccessRestrictions = record.AccessRestrictions,
                    CultureCode = record.CultureCode,
                    RefId = record.RefId
                };
            }
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.PagesRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.PagesWrite; }
        }
    }

    public struct EdmPage
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public Guid PageTypeId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Fields { get; set; }

        public bool IsEnabled { get; set; }

        public int Order { get; set; }

        public bool ShowOnMenus { get; set; }

        public string AccessRestrictions { get; set; }

        public string CultureCode { get; set; }

        public Guid? RefId { get; set; }
    }
}