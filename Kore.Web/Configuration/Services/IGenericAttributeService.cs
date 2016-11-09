using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Reflection;
using Kore.Web.Configuration.Domain;

namespace Kore.Web.Configuration.Services
{
    public interface IGenericAttributeService : IGenericDataService<GenericAttribute>
    {
        IEnumerable<GenericAttribute> GetAttributesForEntity(string entityId, string entityType);

        TPropType GetAttribute<TPropType>(IEntity entity, string property);

        void SaveAttribute<TPropType>(IEntity entity, string property, TPropType value);
    }

    public class GenericAttributeService : GenericDataService<GenericAttribute>, IGenericAttributeService
    {
        private const string GenericAttributeKeyFormat = "Kore.GenericAttribute.{0}-{1}";
        private const string GenericAttributePatternKey = "Kore.GenericAttribute.*";

        public GenericAttributeService(ICacheManager cacheManager, IRepository<GenericAttribute> repository)
            : base(cacheManager, repository)
        {
        }

        public virtual IEnumerable<GenericAttribute> GetAttributesForEntity(string entityId, string entityType)
        {
            string cacheKey = string.Format(GenericAttributeKeyFormat, entityId, entityType);
            return CacheManager.Get(cacheKey, () =>
            {
                return Find(x => x.EntityId == entityId && x.EntityType == entityType);
            });
        }

        public virtual TPropType GetAttribute<TPropType>(IEntity entity, string property)
        {
            string entityType = GetUnproxiedEntityType(entity).Name;
            string entityId = string.Join("|", entity.KeyValues);
            var props = GetAttributesForEntity(entityId, entityType);

            if (props == null)
            {
                return default(TPropType);
            }

            var prop = props.FirstOrDefault(ga =>
                ga.Property.Equals(property, StringComparison.InvariantCultureIgnoreCase));

            if (prop == null || string.IsNullOrEmpty(prop.Value))
            {
                return default(TPropType);
            }

            if (typeof(TPropType).IsSimple())
            {
                return prop.Value.ConvertTo<TPropType>();
            }
            else
            {
                return prop.Value.JsonDeserialize<TPropType>();
            }
        }

        public virtual void SaveAttribute<TPropType>(IEntity entity, string property, TPropType value)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (property == null)
            {
                throw new ArgumentNullException("key");
            }

            string entityId = string.Join("|", entity.KeyValues);
            string entityType = GetUnproxiedEntityType(entity).Name;

            var props = GetAttributesForEntity(entityId, entityType).ToList();

            var prop = props.FirstOrDefault(x =>
                x.Property.Equals(property, StringComparison.InvariantCultureIgnoreCase));

            string valueStr = null;
            if (typeof(TPropType).IsSimple())
            {
                valueStr = value.ToString();
            }
            else
            {
                valueStr = value.ToJson();
            }

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(valueStr))
                {
                    //delete
                    Delete(prop);
                }
                else
                {
                    //update
                    prop.Value = valueStr;
                    Update(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(valueStr))
                {
                    //insert
                    prop = new GenericAttribute
                    {
                        EntityType = entityType,
                        EntityId = entityId,
                        Property = property,
                        Value = valueStr
                    };
                    Insert(prop);
                }
            }
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            CacheManager.RemoveByPattern(GenericAttributePatternKey); // TODO: Test
        }

        /// <summary>
        /// Get unproxied entity type
        /// </summary>
        /// <remarks> If your Entity Framework context is proxy-enabled,
        /// the runtime will create a proxy instance of your entities,
        /// i.e. a dynamically generated class which inherits from your entity class
        /// and overrides its virtual properties by inserting specific code useful for example
        /// for tracking changes and lazy loading.
        /// </remarks>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static Type GetUnproxiedEntityType(IEntity entity)
        {
            var userType = ObjectContext.GetObjectType(entity.GetType());
            return userType;
        }
    }
}