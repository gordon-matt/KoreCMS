using System;
using System.Web.Http;
using System.Web.Http.OData;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class HistoricPagesController : GenericODataController<HistoricPage, Guid>
    {
        private readonly IRepository<Page> pageRepository;

        public HistoricPagesController(
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
                Title = page.Title,
                Slug = page.Slug,
                IsEnabled = page.IsEnabled,
                MetaDescription = page.MetaDescription,
                MetaKeywords = page.MetaKeywords,
                BodyContent = page.BodyContent,
                CssClass = page.CssClass,
                CultureCode = page.CultureCode,
                RefId = page.RefId,
                ArchivedDate = DateTime.UtcNow
            };
            Repository.Insert(backupPage);

            // then restore the historical page, as requested
            page.BodyContent = pageToRestore.BodyContent;
            page.CssClass = pageToRestore.CssClass;
            page.CultureCode = pageToRestore.CultureCode;
            page.IsEnabled = pageToRestore.IsEnabled;
            page.MetaDescription = pageToRestore.MetaDescription;
            page.MetaKeywords = pageToRestore.MetaKeywords;
            page.RefId = pageToRestore.RefId;
            page.Slug = pageToRestore.Slug;
            page.Title = pageToRestore.Title;

            pageRepository.Update(page);

            return Ok();
        }
    }
}