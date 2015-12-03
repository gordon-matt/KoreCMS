using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Localization.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services
{
    public interface IContentBlockService : IGenericDataService<ContentBlock>
    {
        IEnumerable<IContentBlock> GetContentBlocks(Guid pageId, string cultureCode);

        IEnumerable<IContentBlock> GetContentBlocks(string zoneName, string cultureCode, Guid? pageId = null, bool includeDisabled = false);
    }

    public class ContentBlockService : GenericDataService<ContentBlock>, IContentBlockService
    {
        private readonly Lazy<IRepository<Zone>> zoneRepository;
        private readonly Lazy<ILocalizablePropertyService> localizablePropertyService;

        public ContentBlockService(
            ICacheManager cacheManager,
            IRepository<ContentBlock> repository,
            Lazy<IRepository<Zone>> zoneRepository,
            Lazy<ILocalizablePropertyService> localizablePropertyService)
            : base(cacheManager, repository)
        {
            this.zoneRepository = zoneRepository;
            this.localizablePropertyService = localizablePropertyService;
        }

        #region GenericDataService<ContentBlock> Overrides

        //TODO: Override other Delete() methods with similar logic to this one
        public override int Delete(ContentBlock entity)
        {
            string entityType = typeof(ContentBlock).FullName;
            string entityId = entity.Id.ToString();

            var localizedRecords = localizablePropertyService.Value.Find(x =>
                x.EntityType == entityType &&
                x.EntityId == entityId);

            int rowsAffected = localizablePropertyService.Value.Delete(localizedRecords);
            rowsAffected += base.Delete(entity);

            return rowsAffected;
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            CacheManager.RemoveByPattern("Repository_ContentBlocks_ByPageId_.*");
            CacheManager.RemoveByPattern("Repository_ContentBlocks_ByPageIdAndZoneAndIncDisabled_.*");
        }

        #endregion GenericDataService<ContentBlock> Overrides

        #region IContentBlockService Members

        public IEnumerable<IContentBlock> GetContentBlocks(Guid pageId, string cultureCode)
        {
            string key = string.Format("Repository_ContentBlocks_ByPageId_{0}", pageId);
            return CacheManager.Get(key, () =>
            {
                var records = Query(x => x.PageId == pageId).ToList();
                return GetContentBlocks(records, cultureCode);
            });
        }

        public IEnumerable<IContentBlock> GetContentBlocks(string zoneName, string cultureCode, Guid? pageId = null, bool includeDisabled = false)
        {
            string key = string.Format("Repository_ContentBlocks_ByPageIdAndZoneAndIncDisabled_{0}_{1}_{2}", pageId, zoneName, includeDisabled);
            if (includeDisabled)
            {
                return CacheManager.Get(key, () =>
                {
                    var zone = zoneRepository.Value.Table.FirstOrDefault(x => x.Name == zoneName);

                    if (zone == null)
                    {
                        return Enumerable.Empty<IContentBlock>();
                    }

                    var records = pageId.HasValue
                        ? Query(x => x.ZoneId == zone.Id && x.PageId == pageId.Value)
                        : Query(x => x.ZoneId == zone.Id && x.PageId == null);

                    return GetContentBlocks(records, cultureCode);
                });
            }
            else
            {
                return CacheManager.Get(key, () =>
                {
                    var zone = zoneRepository.Value.Table.FirstOrDefault(x => x.Name == zoneName);

                    if (zone == null)
                    {
                        return Enumerable.Empty<IContentBlock>();
                    }

                    var records = Query(x => x.IsEnabled && x.ZoneId == zone.Id && x.PageId == null).ToList();

                    if (pageId.HasValue)
                    {
                        records.AddRange(Query(x => x.IsEnabled && x.ZoneId == zone.Id && x.PageId == pageId.Value));
                    }

                    return GetContentBlocks(records, cultureCode);
                });
            }
        }

        #endregion IContentBlockService Members

        private IEnumerable<IContentBlock> GetContentBlocks(IEnumerable<ContentBlock> records, string cultureCode)
        {
            string ids = string.Join("|", records.Select(x => x.Id));

            var result = new List<IContentBlock>();
            foreach (var record in records)
            {
                IContentBlock contentBlock;
                try
                {
                    if (!string.IsNullOrEmpty(cultureCode))
                    {
                        string entityType = typeof(ContentBlock).FullName;
                        string entityId = record.Id.ToString();

                        var localizedRecord = localizablePropertyService.Value.FindOne(x =>
                            x.CultureCode == cultureCode &&
                            x.EntityType == entityType &&
                            x.EntityId == entityId &&
                            x.Property == "BlockValues");

                        if (localizedRecord != null)
                        {
                            record.BlockValues = localizedRecord.Value;
                        }
                    }

                    var blockType = Type.GetType(record.BlockType);
                    contentBlock = (IContentBlock)record.BlockValues.JsonDeserialize(blockType);
                }
                catch (Exception x)
                {
                    Logger.Error(x.Message, x);
                    continue;
                }

                contentBlock.Id = record.Id;
                contentBlock.Title = record.Title;
                contentBlock.ZoneId = record.ZoneId;
                contentBlock.PageId = record.PageId;
                contentBlock.Order = record.Order;
                contentBlock.Enabled = record.IsEnabled;
                contentBlock.DisplayCondition = record.DisplayCondition;
                contentBlock.CustomTemplatePath = record.CustomTemplatePath;
                result.Add(contentBlock);
            }
            return result;
        }
    }
}