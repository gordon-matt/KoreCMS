using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class ContentBlocksController : GenericODataController<ContentBlock, Guid>
    {
        public ContentBlocksController(IRepository<ContentBlock> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(ContentBlock entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(ContentBlock entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<ContentBlock> Get()
        {
            return Repository.Table.Where(x => x.PageId == null);
        }

        [EnableQuery]
        [HttpPost]
        public virtual IQueryable<ContentBlock> GetByPageId(ODataActionParameters parameters)
        {
            var pageId = (Guid)parameters["pageId"];

            return Repository.Table
                .Where(x => x.PageId == pageId && x.RefId == null)
                .OrderBy(x => x.ZoneId)
                .ThenBy(x => x.Order);
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, ContentBlock entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(entity.Id))
            {
                return BadRequest();
            }

            try
            {
                Save(entity);
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

        public override IHttpActionResult Post(ContentBlock entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //SetNewId(entity);

            Save(entity);

            return Created(entity);
        }

        private void Save(ContentBlock entity)
        {
            var blockType = Type.GetType(entity.BlockType);
            var contentBlocks = EngineContext.Current.ResolveAll<IContentBlock>();
            var contentBlock = contentBlocks.First(x => x.GetType() == blockType);

            var records = entity.Id == Guid.Empty
                ? new List<ContentBlock> { new ContentBlock() }
                : Repository.Table.Where(x => x.Id == entity.Id || x.RefId == entity.Id).ToList();

            // TODO: this shouldn't be from contentBlock values, but from the localized properties on the page
            //var values = JsonConvert.DeserializeObject<ExpandoObject>(entity.BlockValues) as IDictionary<string, object>;

            if (entity.Id != Guid.Empty)
            {
                var languageManager = EngineContext.Current.Resolve<ILanguageManager>();
                var languages = languageManager.GetActiveLanguages();
                if (languages.Count() > 1)
                {
                    foreach (var language in languages)
                    {
                        bool localized = false;
                        try
                        {
                            //localized = Convert.ToBoolean(values["Localized." + language.CultureCode].ToString().Split(',')[0]);
                        }
                        catch
                        {
                            localized = false;
                        }

                        var translatedRecord = records.FirstOrDefault(x => x.CultureCode == language.CultureCode);
                        if (localized)
                        {
                            if (translatedRecord == null)
                            {
                                translatedRecord = new ContentBlock
                                {
                                    CultureCode = language.CultureCode,
                                    RefId = entity.Id
                                };
                                records.Add(translatedRecord);
                            }
                        }
                        else
                        {
                            if (translatedRecord != null)
                            {
                                records.Remove(translatedRecord);
                                Repository.Delete(translatedRecord);
                            }
                        }
                    }
                }
            }

            var toInsert = new List<ContentBlock>();
            var toUpdate = new List<ContentBlock>();

            foreach (var record in records)
            {
                record.BlockName = contentBlock.Name;
                record.BlockType = entity.BlockType;
                record.BlockValues = entity.BlockValues;

                if (string.IsNullOrWhiteSpace(record.BlockValues))
                {
                    // Fix to ensure it can be deserialized. If the value is an empty string, it won't deserialize
                    //  properly. For example, here: IContentBlockService.GetContentBlocks(IEnumerable<ContentBlock> records).
                    //  The following line in that method fails (returns null) if BlockValues is null or empty:
                    //  contentBlock = (IContentBlock)record.BlockValues.JsonDeserialize(blockType);
                    record.BlockValues = "{}";
                }

                if (entity.PageId.HasValue)
                {
                    record.PageId = entity.PageId;
                }

                contentBlock.Title = entity.Title;
                contentBlock.Order = entity.Order;
                contentBlock.Enabled = entity.IsEnabled;
                contentBlock.ZoneId = entity.ZoneId;
                contentBlock.DisplayCondition = entity.DisplayCondition;

                if (string.IsNullOrEmpty(record.CultureCode)) // main record
                {
                    if (entity.Id == Guid.Empty)
                    {
                        record.Id = Guid.NewGuid();
                        toInsert.Add(record);
                    }
                    else
                    {
                        record.Id = entity.Id;
                        toUpdate.Add(record);
                    }

                    contentBlock.Id = record.Id;
                }
                else
                {
                    //For now, just continue
                    continue;

                    //var localizedValues =
                    //    values.Keys.Where(key => key.Contains("." + record.CultureCode))
                    //        .ToDictionary(
                    //            k => k.Replace("." + record.CultureCode, ""),
                    //            v => values[v]);

                    //if (!localizedValues.Any())
                    //{
                    //    continue;
                    //}

                    contentBlock.RefId = entity.Id;
                    contentBlock.CultureCode = record.CultureCode;

                    if (entity.Id == Guid.Empty)
                    {
                        toInsert.Add(record);
                    }
                    else
                    {
                        toUpdate.Add(record);
                    }
                }

                record.Title = contentBlock.Title;
                record.Order = contentBlock.Order;
                record.IsEnabled = contentBlock.Enabled;
                record.ZoneId = contentBlock.ZoneId;
                record.DisplayCondition = contentBlock.DisplayCondition;
            }

            Repository.Insert(toInsert);
            Repository.Update(toUpdate);
        }
    }
}