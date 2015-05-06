using Kore.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Media.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Media.Models;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Services
{
    public interface IImageService
    {
        Guid GetImageEntityId(IEntity entity);

        IEnumerable<TImage> GetImages<TImage>(IEntity entity) where TImage : IImage, new();

        void SetImages(IEntity entity, IEnumerable<IImage> images);
    }

    public class ImageService : IImageService
    {
        private readonly ICacheManager cacheManager;
        private readonly IRepository<Image> imageRepository;
        private readonly IRepository<ImageEntity> imageEntityRepository;

        public ImageService(
            IRepository<Image> imageRepository,
            IRepository<ImageEntity> imageEntityRepository,
            ICacheManager cacheManager)
        {
            this.imageRepository = imageRepository;
            this.imageEntityRepository = imageEntityRepository;
            this.cacheManager = cacheManager;
        }

        public Guid GetImageEntityId(IEntity entity)
        {
            var imageEntities = cacheManager.Get("ImageEntities_GetAllTypes", () =>
            {
                return imageEntityRepository.Table.ToHashSet();
            });

            var type = entity.GetType();
            string typeName = string.Concat(type.FullName, ", ", type.Assembly.GetName().Name);

            var imageEntity = imageEntities.FirstOrDefault(x => x.Type == typeName);
            if (imageEntity != null)
            {
                return imageEntity.Id;
            }

            imageEntity = new ImageEntity
            {
                Id = Guid.NewGuid(),
                Type = typeName
            };

            imageEntityRepository.Insert(imageEntity);
            cacheManager.Remove("ImageEntities_GetAllTypes");

            return imageEntity.Id;
        }

        public IEnumerable<TImage> GetImages<TImage>(IEntity entity) where TImage : IImage, new()
        {
            //TODO: THis will be a problem (key is not always int!!)
            int entityId = (int)entity.KeyValues.First();
            Guid imageEntityId = GetImageEntityId(entity);

            var records = imageRepository.Table
                .Where(x => x.ImageEntityId == imageEntityId && x.EntityId == entityId)
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

        public void SetImages(IEntity entity, IEnumerable<IImage> images)
        {
            Guid imageEntityId = GetImageEntityId(entity);
            int entityId = (int)entity.KeyValues.First();

            //TODO: Revise this delete and insert logic.. better to update instead of delete all and isnert new

            // Delete old media parts
            var records = imageRepository.Table
                .Where(x => x.ImageEntityId == imageEntityId && x.EntityId == entityId)
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
                    ImageEntityId = imageEntityId,
                    EntityId = entityId
                };
                imageRepository.Insert(record);
            }
        }

        private static string GetFullTypeName(Type type)
        {
            return string.Concat(type.FullName, ", ", type.Assembly.GetName().Name);
        }
    }
}