using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.OData;
using System.Web.Http.Results;
using System.Xml.Serialization;
using Kore.Collections;
using Kore.Data;
using Kore.Localization.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Models;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using System.Web.OData.Query;

namespace Kore.Web.ContentManagement.Areas.Admin.Sitemap.Controllers.Api
{
    public class XmlSitemapApiController : GenericODataController<SitemapConfig, int>
    {
        private readonly IPageService pageService;
        private readonly IPageVersionService pageVersionService;
        private readonly Lazy<ILanguageService> languageService;

        public XmlSitemapApiController(
            IRepository<SitemapConfig> repository,
            IPageService pageService,
            IPageVersionService pageVersionService,
            Lazy<ILanguageService> languageService)
            : base(repository)
        {
            this.pageService = pageService;
            this.pageVersionService = pageVersionService;
            this.languageService = languageService;
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

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        public virtual IEnumerable<SitemapConfigModel> GetConfig()
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<SitemapConfigModel>();
            }

            // First ensure that current pages are in the config
            var config = Service.Find();
            var configPageIds = config.Select(x => x.PageId).ToHashSet();
            var pageVersions = pageVersionService.GetCurrentVersions(shownOnMenusOnly: false); // temp fix: since we don't support localized routes yet
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
            return collection.OrderBy(x => x.Location);
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

            var cultures = languageService.Value.Query().Select(x => x.CultureCode).ToList();

            string siteUrl = Request.RequestUri.GetLeftPart(UriPartial.Authority);

            // For each Page
            foreach (var item in config)
            {
                var invariantVersion = pageVersions.First(x => x.CultureCode == null && x.Id == item.PageId);

                if (cultures.Count > 1)
                {
                    var localizedVersions = pageVersions
                        .Where(x =>
                            x.PageId == invariantVersion.PageId &&
                            x.CultureCode != null);

                    // For each Language
                    foreach (string culture1 in cultures)
                    {
                        var localizedVersion1 = localizedVersions
                            .Where(x => x.CultureCode == culture1)
                            .OrderByDescending(x => x.DateModifiedUtc)
                            .FirstOrDefault();

                        if (localizedVersion1 == null)
                        {
                            localizedVersion1 = invariantVersion;
                        }

                        var links = new List<LinkElement>();

                        // For each Language (again)
                        foreach (string culture2 in cultures)
                        {
                            // If this language is the same as the one in the outer loop
                            if (culture2 == culture1)
                            {
                                // ignore this loop and continue to next...
                                continue;
                            }

                            var localizedVersion2 = localizedVersions
                                .Where(x => x.CultureCode == culture2)
                                .OrderByDescending(x => x.DateModifiedUtc)
                                .FirstOrDefault();

                            if (localizedVersion2 == null)
                            {
                                localizedVersion2 = invariantVersion;
                            }

                            links.Add(new LinkElement
                            {
                                Rel = "alternate",
                                HrefLang = culture2,
                                Href = string.Concat(siteUrl, "/", localizedVersion2.Slug)
                            });
                        }

                        links.Add(new LinkElement
                        {
                            Rel = "alternate",
                            HrefLang = culture1,
                            Href = string.Concat(siteUrl, "/", localizedVersion1.Slug)
                        });
                        urls.Add(new UrlElement
                        {
                            Location = string.Concat(siteUrl, "/", localizedVersion1.Slug),
                            LastModified = localizedVersion1.DateModifiedUtc.ToString("yyyy-MM-dd"),
                            ChangeFrequency = item.ChangeFrequency,
                            Priority = item.Priority,
                            Links = links.OrderBy(x => x.HrefLang).ToList()
                        });
                    }
                }
                else
                {
                    if (invariantVersion == null && cultures.Count == 1)
                    {
                        // If there's only 1 language configured, then we use that as the default
                        string cultureCode = cultures.First();
                        invariantVersion = pageVersions.First(x => x.CultureCode == cultureCode && x.Id == item.PageId);
                    }

                    urls.Add(new UrlElement
                    {
                        Location = string.Concat(siteUrl, "/", invariantVersion.Slug),
                        LastModified = invariantVersion.DateModifiedUtc.ToString("yyyy-MM-dd"),
                        ChangeFrequency = item.ChangeFrequency,
                        Priority = item.Priority,
                        Links = null
                    });
                }
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