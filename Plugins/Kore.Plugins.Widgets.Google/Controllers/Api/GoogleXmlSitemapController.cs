using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Kore.Collections;
using Kore.Data;
using Kore.Plugins.Widgets.Google.Data.Domain;
using Kore.Plugins.Widgets.Google.Models;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Http.OData;

namespace Kore.Plugins.Widgets.Google.Controllers.Api
{
    public class GoogleXmlSitemapController : GenericODataController<GoogleSitemapPageConfig, int>
    {
        private IRepository<Page> pageRepository;

        public GoogleXmlSitemapController(
            IRepository<GoogleSitemapPageConfig> repository,
            IRepository<Page> pageRepository)
            : base(repository)
        {
            this.pageRepository = pageRepository;
        }

        #region GenericODataController<GoogleSitemapPageConfig, int> Members

        protected override int GetId(GoogleSitemapPageConfig entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(GoogleSitemapPageConfig entity)
        {
            // Do nothing
        }

        #endregion GenericODataController<GoogleSitemapPageConfig, int> Members

        [EnableQuery]
        [HttpPost]
        public virtual IEnumerable<GoogleSitemapPageConfigModel> GetConfig(ODataActionParameters parameters)
        {
            // First ensure that current pages are in the config
            var config = Repository.Table.ToHashSet();
            var configPageIds = config.Select(x => x.PageId).ToHashSet();
            var pages = pageRepository.Table.ToHashSet();
            var pageIds = pages.Select(x => x.Id).ToHashSet();

            var newPageIds = pageIds.Except(configPageIds);
            var pageIdsToDelete = configPageIds.Except(pageIds);

            if (pageIdsToDelete.Any())
            {
                Repository.Delete(x => pageIdsToDelete.Contains(x.PageId));
            }

            if (newPageIds.Any())
            {
                var toInsert = pages
                    .Where(x => newPageIds.Contains(x.Id))
                    .Select(x => new GoogleSitemapPageConfig
                    {
                        PageId = x.Id,
                        ChangeFrequency = ChangeFrequency.Weekly,
                        Priority = .5f
                    });

                Repository.Insert(toInsert);
            }
            config = Repository.Table.ToHashSet();

            var collection = new HashSet<GoogleSitemapPageConfigModel>();
            foreach (var item in config)
            {
                var page = pages.First(x => x.Id == item.PageId);
                collection.Add(new GoogleSitemapPageConfigModel
                {
                    Id = item.Id,
                    Location = page.Slug,
                    ChangeFrequency = item.ChangeFrequency,
                    Priority = item.Priority
                });
            }
            return collection;
        }

        [HttpPost]
        public virtual IHttpActionResult SetConfig(ODataActionParameters parameters)
        {
            int id = (int)parameters["id"];
            byte changeFrequency = (byte)parameters["changeFrequency"];
            float priority = (float)parameters["priority"];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = Repository.Find(id);

            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                entity.ChangeFrequency = (ChangeFrequency)changeFrequency;
                entity.Priority = priority;
                Repository.Update(entity);

                return Updated(new GoogleSitemapPageConfigModel
                {
                    Id = entity.Id,
                    //Location = 
                    ChangeFrequency = entity.ChangeFrequency,
                    Priority = entity.Priority
                });
            }
        }
    }
}