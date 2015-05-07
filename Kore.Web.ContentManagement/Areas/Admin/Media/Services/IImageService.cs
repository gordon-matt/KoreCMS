using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Collections;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Media.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Media.Models;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Services
{
    public interface IImageService
    {
        Guid GeEntityTypeId(IEntity entity);

        IEnumerable<TImage> GetImages<TImage>(IEntity entity) where TImage : IKoreImage, new();

        void SetImages(IEntity entity, IEnumerable<IKoreImage> images);
    }

    public class ImageService : IImageService
    {
        private readonly ICacheManager cacheManager;
        private readonly IRepository<Image> imageRepository;
        private readonly IRepository<ImageEntityType> imageEntityTypeRepository;

        public ImageService(
            IRepository<Image> imageRepository,
            IRepository<ImageEntityType> imageEntityTypeRepository,
            ICacheManager cacheManager)
        {
            this.imageRepository = imageRepository;
            this.imageEntityTypeRepository = imageEntityTypeRepository;
            this.cacheManager = cacheManager;
        }

        public Guid GeEntityTypeId(IEntity entity)
        {
            var entityTypes = cacheManager.Get("ImageEntityTypes_GetAll", () =>
            {
                return imageEntityTypeRepository.Table.ToDictionary(k => k.Type, v => v.Id);
            });

            var type = entity.GetType();
            string typeName = string.Concat(type.FullName, ", ", type.Assembly.GetName().Name);

            if (entityTypes.ContainsKey(typeName))
            {
                return entityTypes[typeName];
            }

            var entityType = new ImageEntityType
            {
                Id = Guid.NewGuid(),
                Type = typeName
            };

            imageEntityTypeRepository.Insert(entityType);
            cacheManager.Remove("ImageEntityTypes_GetAll");

            return entityType.Id;
        }

        public IEnumerable<TImage> GetImages<TImage>(IEntity entity) where TImage : IKoreImage, new()
        {
            string entityId = string.Join(",", entity.KeyValues);
            Guid entityTypeId = GeEntityTypeId(entity);

            var records = imageRepository.Table
                .Where(x => x.EntityTypeId == entityTypeId && x.EntityId == entityId)
                .OrderBy(x => x.SortOrder)
                .ToHashSet();

            return records.Select(x => new TImage
            {
                Url = x.Url,
                ThumbnailUrl = x.ThumbnailUrl,
                Caption = x.Caption,
                SortOrder = x.SortOrder
            }).ToHashSet();
        }

        public void SetImages(IEntity entity, IEnumerable<IKoreImage> images)
        {
            string entityId = string.Join(",", entity.KeyValues);
            Guid entityTypeId = GeEntityTypeId(entity);

            //TODO: Revise this delete and insert logic.. better to update instead of delete all and isnert new

            // Delete old media parts
            var records = imageRepository.Table
                .Where(x => x.EntityTypeId == entityTypeId && x.EntityId == entityId)
                .ToHashSet();

            imageRepository.Delete(records);

            // Insert new parts
            foreach (var image in images.Where(x => !string.IsNullOrEmpty(x.Url)))
            {
                var record = new Image
                {
                    Id = Guid.NewGuid(),
                    Url = image.Url,
                    Caption = image.Caption,
                    SortOrder = image.SortOrder,
                    EntityTypeId = entityTypeId,
                    EntityId = entityId
                };
                imageRepository.Insert(record);
            }
        }
    }
}