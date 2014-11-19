using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class PagesController : GenericODataController<Page, Guid>
    {
        private readonly IRepository<HistoricPage> historicPageRepository;

        public PagesController(
            IRepository<Page> repository,
            IRepository<HistoricPage> historicPageRepository)
            : base(repository)
        {
            this.historicPageRepository = historicPageRepository;
        }

        public override IQueryable<Page> Get()
        {
            return Repository.Table.Where(x => x.RefId == null);
        }

        public override IHttpActionResult Patch([FromODataUri] Guid key, Delta<Page> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = Repository.Find(key);
            if (entity == null)
            {
                return NotFound();
            }

            patch.Patch(entity);

            try
            {
                var currentPage = Repository.Find(entity.Id);

                // archive current version before updating
                var historicPage = new HistoricPage
                {
                    Id = Guid.NewGuid(),
                    ArchivedDate = DateTime.UtcNow,
                    BodyContent = currentPage.BodyContent,
                    CssClass = currentPage.CssClass,
                    CultureCode = currentPage.CultureCode,
                    IsEnabled = currentPage.IsEnabled,
                    MetaDescription = currentPage.MetaDescription,
                    MetaKeywords = currentPage.MetaKeywords,
                    PageId = currentPage.Id,
                    RefId = currentPage.RefId,
                    Slug = currentPage.Slug,
                    Title = currentPage.Title
                };
                historicPageRepository.Insert(historicPage);

                Repository.Update(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, Page entity)
        {
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
                var currentPage = Repository.Find(entity.Id);

                // archive current version before updating
                var historicPage = new HistoricPage
                {
                    Id = Guid.NewGuid(),
                    ArchivedDate = DateTime.UtcNow,
                    BodyContent = currentPage.BodyContent,
                    CssClass = currentPage.CssClass,
                    CultureCode = currentPage.CultureCode,
                    IsEnabled = currentPage.IsEnabled,
                    MetaDescription = currentPage.MetaDescription,
                    MetaKeywords = currentPage.MetaKeywords,
                    PageId = currentPage.Id,
                    RefId = currentPage.RefId,
                    Slug = currentPage.Slug,
                    Title = currentPage.Title
                };
                historicPageRepository.Insert(historicPage);

                Repository.Update(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        [HttpPost]
        public IHttpActionResult SaveTranslation(ODataActionParameters parameters)
        {
            var translation = (Translation)parameters["translation"];

            var invariantPage = Repository.Find(translation.PageId);

            if (translation.Id == Guid.Empty)
            {
                var record = new Page
                {
                    Id = Guid.NewGuid(),
                    Title = translation.Title,
                    Slug = translation.Title.ToSlugUrl(),
                    IsEnabled = translation.IsEnabled,
                    MetaDescription = translation.MetaDescription,
                    MetaKeywords = translation.MetaKeywords,
                    BodyContent = translation.BodyContent,
                    CultureCode = translation.CultureCode,
                    RefId = translation.PageId,
                    CssClass = invariantPage.CssClass
                };
                Repository.Insert(record);
            }
            else
            {
                var record = Repository.Table.FirstOrDefault(x => x.Id == translation.Id);
                record.Title = translation.Title;
                record.Slug = translation.Title.ToSlugUrl();
                record.IsEnabled = translation.IsEnabled;
                record.MetaDescription = translation.MetaDescription;
                record.MetaKeywords = translation.MetaKeywords;
                record.BodyContent = translation.BodyContent;
                record.CultureCode = translation.CultureCode;
                record.RefId = translation.PageId;
                record.CssClass = invariantPage.CssClass;
                Repository.Update(record);
            }

            return Ok();
        }

        [HttpPost]
        public Translation Translate(ODataActionParameters parameters)
        {
            Guid pageId = (Guid)parameters["pageId"];
            string cultureCode = (string)parameters["cultureCode"];
            var record = Repository.Table.FirstOrDefault(x => x.RefId == pageId && x.CultureCode == cultureCode);

            return record == null
                ? new Translation
                {
                    PageId = pageId,
                    CultureCode = cultureCode,
                }
                : new Translation
                {
                    Id = record.Id,
                    PageId = pageId,
                    CultureCode = cultureCode,
                    Title = record.Title,
                    IsEnabled = record.IsEnabled,
                    MetaKeywords = record.MetaKeywords,
                    MetaDescription = record.MetaDescription,
                    BodyContent = record.BodyContent
                };
        }

        protected override Guid GetId(Page entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Page entity)
        {
            entity.Id = Guid.NewGuid();
        }
    }

    public struct Translation
    {
        public string BodyContent { get; set; }

        public string CultureCode { get; set; }

        public Guid Id { get; set; }

        public bool IsEnabled { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeywords { get; set; }

        public Guid PageId { get; set; }

        public string Title { get; set; }
    }
}