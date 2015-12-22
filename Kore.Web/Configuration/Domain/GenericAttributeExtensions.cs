using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Configuration.Services;

namespace Kore.Web.Configuration.Domain
{
    public static class GenericAttributeExtensions
    {
        public static TPropType GetAttribute<TPropType>(this IEntity entity, string key, IGenericAttributeService service = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (service == null)
            {
                service = EngineContext.Current.Resolve<IGenericAttributeService>();
            }

            string entityId = string.Join("|", entity.KeyValues);
            string entityType = GetUnproxiedEntityType(entity).Name;

            var props = service.GetAttributesForEntity(entityId, entityType);
            //little hack here (only for unit testing). we should write expect-return rules in unit tests for such cases
            if (props == null)
            {
                return default(TPropType);
            }

            if (!props.Any())
            {
                return default(TPropType);
            }

            var prop = props.FirstOrDefault(x => x.Property.Equals(key, StringComparison.InvariantCultureIgnoreCase));

            if (prop == null || string.IsNullOrEmpty(prop.Value))
            {
                return default(TPropType);
            }

            return prop.Value.ConvertTo<TPropType>();
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