using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services
{
    public interface IEntityTypeContentBlockService : IGenericDataService<EntityTypeContentBlock>
    {
        IEnumerable<IContentBlock> GetContentBlocks(string entityType, string entityId, string zoneName, bool includeDisabled = false);
    }

    public class EntityTypeContentBlockService : GenericDataService<EntityTypeContentBlock>, IEntityTypeContentBlockService
    {
        private readonly Lazy<IRepository<Zone>> zoneRepository;

        public EntityTypeContentBlockService(
            ICacheManager cacheManager,
            IRepository<EntityTypeContentBlock> repository,
            Lazy<IRepository<Zone>> zoneRepository)
            : base(cacheManager, repository)
        {
            this.zoneRepository = zoneRepository;
        }

        #region IEntityTypeContentBlockService Members

        public IEnumerable<IContentBlock> GetContentBlocks(string entityType, string entityId, string zoneName, bool includeDisabled = false)
        {
            string key = string.Format(
                "Repository_EntityTypeContentBlocks_GetContentBlocks_{0}_{1}_{2}_{3}",
                entityType,
                entityId,
                zoneName,
                includeDisabled);

            return CacheManager.Get(key, () =>
            {
                var zone = zoneRepository.Value.Table.FirstOrDefault(x => x.Name == zoneName);

                if (zone == null)
                {
                    return Enumerable.Empty<IContentBlock>();
                }

                var query = Repository.Table.Where(x =>
                    x.EntityType == entityType &&
                    x.EntityId == entityId &&
                    x.ZoneId == zone.Id);

                if (!includeDisabled)
                {
                    query = query.Where(x => x.IsEnabled);
                }

                var records = query.ToList();
                return GetContentBlocks(records);
            });
        }

        #endregion IEntityTypeContentBlockService Members

        protected override void ClearCache()
        {
            base.ClearCache();
            CacheManager.RemoveByPattern("Repository_EntityTypeContentBlocks_GetContentBlocks_.*");
        }

        private IEnumerable<IContentBlock> GetContentBlocks(IEnumerable<EntityTypeContentBlock> records)
        {
            string ids = string.Join("|", records.Select(x => x.Id));

            var result = new List<IContentBlock>();
            foreach (var record in records)
            {
                IContentBlock contentBlock;
                try
                {
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
                contentBlock.Order = record.Order;
                contentBlock.Enabled = record.IsEnabled;
                contentBlock.CustomTemplatePath = record.CustomTemplatePath;
                result.Add(contentBlock);
            }
            return result;
        }
    }
}