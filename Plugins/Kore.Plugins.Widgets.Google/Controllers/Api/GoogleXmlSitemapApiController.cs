﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.Results;
using Kore.Collections;
using Kore.Data;
using Kore.Plugins.Widgets.Google.Data.Domain;
using Kore.Plugins.Widgets.Google.Models;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Widgets.Google.Controllers.Api
{
    public class GoogleXmlSitemapApiController : GenericODataController<GoogleSitemapPageConfig, int>
    {
        private IPageService pageService;

        public GoogleXmlSitemapApiController(
            IRepository<GoogleSitemapPageConfig> repository,
            IPageService pageService)
            : base(repository)
        {
            this.pageService = pageService;
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

        protected override Permission ReadPermission
        {
            get { return GooglePermissions.SitemapRead; }
        }

        protected override Permission WritePermission
        {
            get { return GooglePermissions.SitemapWrite; }
        }

        #endregion GenericODataController<GoogleSitemapPageConfig, int> Members

        [EnableQuery]
        [HttpPost]
        public virtual IEnumerable<GoogleSitemapPageConfigModel> GetConfig(ODataActionParameters parameters)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<GoogleSitemapPageConfigModel>();
            }

            // First ensure that current pages are in the config
            var config = Service.Find();
            var configPageIds = config.Select(x => x.PageId).ToHashSet();
            var pages = pageService.Find();
            var pageIds = pages.Select(x => x.Id).ToHashSet();

            var newPageIds = pageIds.Except(configPageIds);
            var pageIdsToDelete = configPageIds.Except(pageIds);

            if (pageIdsToDelete.Any())
            {
                Service.Delete(x => pageIdsToDelete.Contains(x.PageId));
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

                Service.Insert(toInsert);
            }
            config = Service.Find();

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
                //return Updated(new GoogleSitemapPageConfigModel
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
            var file = new GoogleSitemapXmlFile();

            var pages = pageService.Find();
            foreach (var item in config)
            {
                var page = pages.First(x => x.Id == item.PageId);
                file.Urls.Add(new UrlElement
                {
                    Location = string.Concat(Request.RequestUri.GetLeftPart(UriPartial.Authority), "/", page.Slug),
                    LastModified = page.DateModifiedUtc.ToString("yyyy-MM-dd"),
                    ChangeFrequency = item.ChangeFrequency,
                    Priority = item.Priority
                });
            }

            try
            {
                file.XmlSerialize(
                    HostingEnvironment.MapPath("~/sitemap.xml"),
                    removeNamespaces: false,
                    omitXmlDeclaration: false);

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