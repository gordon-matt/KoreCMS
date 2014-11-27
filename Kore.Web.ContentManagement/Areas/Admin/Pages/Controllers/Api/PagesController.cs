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
                    PageId = currentPage.Id,
                    PageTypeId = currentPage.PageTypeId,
                    ArchivedDate = DateTime.UtcNow,
                    Name = currentPage.Name,
                    Slug = currentPage.Slug,
                    Fields = currentPage.Fields,
                    DateCreatedUtc = currentPage.DateCreatedUtc,
                    DateModifiedUtc = currentPage.DateModifiedUtc,
                    IsEnabled = currentPage.IsEnabled,
                    CultureCode = currentPage.CultureCode,
                    RefId = currentPage.RefId,
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

        public override IHttpActionResult Post(Page entity)
        {
            entity.DateCreatedUtc = DateTime.UtcNow;
            entity.DateModifiedUtc = DateTime.UtcNow;
            return base.Post(entity);
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
                    PageId = currentPage.Id,
                    PageTypeId = currentPage.PageTypeId,
                    ArchivedDate = DateTime.UtcNow,
                    Name = currentPage.Name,
                    Slug = currentPage.Slug,
                    Fields = currentPage.Fields,
                    DateCreatedUtc = currentPage.DateCreatedUtc,
                    DateModifiedUtc = currentPage.DateModifiedUtc,
                    IsEnabled = currentPage.IsEnabled,
                    CultureCode = currentPage.CultureCode,
                    RefId = currentPage.RefId
                };
                historicPageRepository.Insert(historicPage);

                entity.DateCreatedUtc = currentPage.DateCreatedUtc;
                entity.DateModifiedUtc = DateTime.UtcNow;
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
                    PageTypeId = invariantPage.PageTypeId,
                    Name = translation.Name,
                    Slug = translation.Slug,
                    Fields = translation.Fields,
                    IsEnabled = translation.IsEnabled,
                    DateCreatedUtc = DateTime.UtcNow,
                    DateModifiedUtc = DateTime.UtcNow,
                    CultureCode = translation.CultureCode,
                    RefId = translation.PageId,
                };
                Repository.Insert(record);
            }
            else
            {
                var record = Repository.Table.FirstOrDefault(x => x.Id == translation.Id);
                record.Name = translation.Name;
                record.Slug = translation.Slug;
                record.Fields = translation.Fields;
                record.IsEnabled = translation.IsEnabled;
                record.DateModifiedUtc = DateTime.UtcNow;
                record.RefId = translation.PageId;
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
                    Name = record.Name,
                    Slug = record.Slug,
                    Fields = record.Fields,
                    IsEnabled = record.IsEnabled,
                    CultureCode = cultureCode,
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
        public Guid Id { get; set; }

        public Guid PageId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Fields { get; set; }

        public bool IsEnabled { get; set; }

        public string CultureCode { get; set; }
    }
}