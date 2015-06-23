using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    public class PageApiController : GenericODataController<Page, Guid>
    {
        private readonly IPageVersionService pageVersionService;
        private readonly IWebWorkContext workContext;

        public PageApiController(
            IPageService service,
            IPageVersionService pageVersionService,
            IWebWorkContext workContext)
            : base(service)
        {
            this.pageVersionService = pageVersionService;
            this.workContext = workContext;
        }

        public override SingleResult<Page> Get([FromODataUri] Guid key)
        {
            if (!CheckPermission(ReadPermission))
            {
                return SingleResult.Create(Enumerable.Empty<Page>().AsQueryable());
            }
            var entity = Service.FindOne(key);

            string currentCulture = workContext.CurrentCultureCode;

            if (!string.IsNullOrEmpty(currentCulture))
            {
                var localized = pageVersionService.FindOne(x => x.PageId == key && x.CultureCode == currentCulture);
                if (localized == null)
                {
                    var invariantVersion = pageVersionService.FindOne(x => x.PageId == key && x.CultureCode == null);

                    pageVersionService.Insert(new PageVersion
                    {
                        Id = Guid.NewGuid(),
                        PageId = key,
                        CultureCode = currentCulture,
                        DateCreatedUtc = DateTime.UtcNow,
                        DateModifiedUtc = DateTime.UtcNow,
                        Status = invariantVersion.Status,
                        Title = invariantVersion.Title,
                        Slug = string.Concat(currentCulture.ToLowerInvariant(), "/", invariantVersion.Slug),
                        Fields = invariantVersion.Fields
                    });
                }
            }

            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        protected override Guid GetId(Page entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Page entity)
        {
            entity.Id = Guid.NewGuid();
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
}