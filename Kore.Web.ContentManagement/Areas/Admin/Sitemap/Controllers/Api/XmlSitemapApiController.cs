using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.Results;
using System.Xml.Serialization;
using Kore.Collections;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Models;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Sitemap.Controllers.Api
{
    public class XmlSitemapApiController : GenericODataController<SitemapConfig, int>
    {
        private readonly IPageService pageService;
        private readonly IPageVersionService pageVersionService;

        public XmlSitemapApiController(
            IRepository<SitemapConfig> repository,
            IPageService pageService,
            IPageVersionService pageVersionService)
            : base(repository)
        {
            this.pageService = pageService;
            this.pageVersionService = pageVersionService;
        }

        #region GenericODataController<GoogleSitemapPageConfig, int> Members

        protected override int GetId(SitemapConfig entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(SitemapConfig entity)
        {
            // Do nothing
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.SitemapRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.SitemapWrite; }
        }

        #endregion GenericODataController<GoogleSitemapPageConfig, int> Members

        [EnableQuery]
        [HttpPost]
        public virtual IEnumerable<SitemapConfigModel> GetConfig(ODataActionParameters parameters)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<SitemapConfigModel>();
            }

            // First ensure that current pages are in the config
            var config = Service.Find();
            var configPageIds = config.Select(x => x.PageId).ToHashSet();
            var pageVersions = pageVersionService.GetCurrentVersions(); // temp fix: since we don't support localized routes yet
            var pageVersionIds = pageVersions.Select(x => x.Id).ToHashSet();

            var newPageVersionIds = pageVersionIds.Except(configPageIds);
            var pageVersionIdsToDelete = configPageIds.Except(pageVersionIds);

            if (pageVersionIdsToDelete.Any())
            {
                Service.Delete(x => pageVersionIdsToDelete.Contains(x.PageId));
            }

            if (newPageVersionIds.Any())
            {
                var toInsert = pageVersions
                    .Where(x => newPageVersionIds.Contains(x.Id))
                    .Select(x => new SitemapConfig
                    {
                        PageId = x.Id,
                        ChangeFrequency = ChangeFrequency.Weekly,
                        Priority = .5f
                    });

                Service.Insert(toInsert);
            }
            config = Service.Find();

            var collection = new HashSet<SitemapConfigModel>();
            foreach (var item in config)
            {
                var page = pageVersions.First(x => x.Id == item.PageId);
                collection.Add(new SitemapConfigModel
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
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            int id = (int)parameters["id"];
            byte changeFrequency = (byte)parameters["changeFrequency"];
            float priority = (float)parameters["priority"];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = Service.FindOne(id);

            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                entity.ChangeFrequency = (ChangeFrequency)changeFrequency;
                entity.Priority = priority;
                Service.Update(entity);

                return Updated(entity);
                //return Updated(new SitemapConfigModel
                //{
                //    Id = entity.Id,
                //    Location = pageRepository.Table.First(x => x.Id == entity.PageId).Slug,
                //    ChangeFrequency = entity.ChangeFrequency,
                //    Priority = entity.Priority
                //});
            }
        }

        [HttpPost]
        public virtual IHttpActionResult Generate(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            var config = Service.Find();
            var file = new SitemapXmlFile();

            var pageVersions = pageVersionService.Find();
            var urls = new HashSet<UrlElement>();

            string siteUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);
            foreach (var item in config)
            {
                var page = pageVersions.First(x => x.Id == item.PageId);

                var localizedVersions = pageVersions
                    .Where(x =>
                        x.PageId == page.PageId &&
                        x.CultureCode != null);

                var cultures = localizedVersions.Select(x => x.CultureCode).Distinct().ToList();

                var links = new List<LinkElement>();
                foreach (string culture in cultures)
                {
                    var localizedVersion = localizedVersions
                        .OrderByDescending(x => x.DateModifiedUtc)
                        .First();

                    links.Add(new LinkElement
                    {
                        Rel = "alternate",
                        HrefLang = culture.ToLowerInvariant(),
                        Href = string.Concat(siteUrl, "/", localizedVersion.Slug)
                    });
                }

                urls.Add(new UrlElement
                {
                    Location = string.Concat(siteUrl, "/", page.Slug),
                    LastModified = page.DateModifiedUtc.ToString("yyyy-MM-dd"),
                    ChangeFrequency = item.ChangeFrequency,
                    Priority = item.Priority,
                    Links = links
                });
            }
            file.Urls = urls.OrderBy(x => x.Location).ToHashSet();

            try
            {
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add("xhtml", "http://www.w3.org/1999/xhtml");

                file.XmlSerialize(
                    HostingEnvironment.MapPath("~/sitemap.xml"),
                    omitXmlDeclaration: false,
                    xmlns: xmlns,
                    encoding: Encoding.UTF8);

                // For some reason, just returning Ok() with no parameter causes the following client-side error:
                //  "unexpected end of data at line 1 column 1 of the JSON data"
                //  TODO: Perhaps we should be returning null instead? Or perhaps we should change the method to return void
                //  Also, we should look throughout the solution for the same issue in other controllers.
                return Ok(string.Empty);
            }
            catch (Exception x)
            {
                Logger.Error(x.Message, x);
                return InternalServerError(x);
            }
        }
    }
}