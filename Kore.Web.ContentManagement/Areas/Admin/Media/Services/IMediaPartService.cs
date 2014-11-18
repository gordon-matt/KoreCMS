using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Media.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Media.Models;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Services
{
    public interface IMediaPartService
    {
        Guid GetMediaPartType(IEntity entity);

        IEnumerable<TMediaPart> GetMediaParts<TMediaPart>(IEntity entity) where TMediaPart : IMediaPart, new();

        void SetMediaParts(IEntity entity, IEnumerable<IMediaPart> mediaParts);
    }

    public class MediaPartService : IMediaPartService
    {
        private readonly ICacheManager cacheManager;
        private readonly IRepository<MediaPart> mediaPartRepository;
        private readonly IRepository<MediaPartType> mediaPartTypeRepository;

        public MediaPartService(
            IRepository<MediaPart> mediaPartRepository,
            IRepository<MediaPartType> mediaPartTypeRepository,
            ICacheManager cacheManager)
        {
            this.mediaPartRepository = mediaPartRepository;
            this.mediaPartTypeRepository = mediaPartTypeRepository;
            this.cacheManager = cacheManager;
        }

        public Guid GetMediaPartType(IEntity entity)
        {
            var types = cacheManager.Get("MediaPartTypes_GetAllTypes", () =>
            {
                return mediaPartTypeRepository.Table.ToList();
            });

            var type = entity.GetType();
            string typeName = string.Concat(type.FullName, ", ", type.Assembly.GetName().Name);

            var part = types.FirstOrDefault(x => x.Type == typeName);
            if (part != null)
            {
                return part.Id;
            }

            part = new MediaPartType
            {
                Id = Guid.NewGuid(),
                Type = typeName
            };

            mediaPartTypeRepository.Insert(part);
            cacheManager.Remove("MediaPartTypes_GetAllTypes");

            return part.Id;
        }

        public IEnumerable<TMediaPart> GetMediaParts<TMediaPart>(IEntity entity) where TMediaPart : IMediaPart, new()
        {
            int id = (int)entity.KeyValues.First();
            Guid partTypeId = GetMediaPartType(entity);

            var records = mediaPartRepository.Table
                .Where(x => x.MediaPartTypeId == partTypeId && x.ParentId == id)
                .OrderBy(x => x.SortOrder)
                .ToList();

            return records.Select(x => new TMediaPart
            {
                Url = x.Url,
                Caption = x.Caption,
                SortOrder = x.SortOrder
            }).ToList();
        }

        public void SetMediaParts(IEntity entity, IEnumerable<IMediaPart> mediaParts)
        {
            var mediaPartType = GetMediaPartType(entity);
            int parentId = (int)entity.KeyValues.First();

            //delete old media parts
            var records = mediaPartRepository.Table
                .Where(x => x.MediaPartTypeId == mediaPartType && x.ParentId == parentId)
                .ToList();

            mediaPartRepository.Delete(records);

            // Insert new parts
            foreach (var mediaPart in mediaParts.Where(mediaPart => !string.IsNullOrEmpty(mediaPart.Url)))
            {
                var record = new MediaPart
                {
                    Id = Guid.NewGuid(),
                    Url = mediaPart.Url,
                    Caption = mediaPart.Caption,
                    SortOrder = mediaPart.SortOrder,
                    MediaPartTypeId = mediaPartType,
                    ParentId = parentId
                };
                mediaPartRepository.Insert(record);
            }
        }

        private static string GetFullTypeName(Type type)
        {
            return string.Concat(type.FullName, ", ", type.Assembly.GetName().Name);
        }
    }
}