using System.Linq;
using Kore.Data;
using Kore.Data.Services;
using Kore.Infrastructure;
using Kore.Tenants.Domain;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Http.OData
{
    //TODO: Test this
    public abstract class GenericTenantODataController<TEntity, TKey> : GenericODataController<TEntity, TKey>
        where TEntity : class, ITenantEntity
    {
        private readonly IWorkContext workContext;

        #region Constructors

        public GenericTenantODataController(IGenericDataService<TEntity> service)
            : base(service)
        {
            this.workContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        public GenericTenantODataController(IRepository<TEntity> repository)
            : base(repository)
        {
            this.workContext = EngineContext.Current.Resolve<IWorkContext>();
        }

        #endregion Constructors

        #region GenericODataController<TEntity, TKey> Members

        protected override IQueryable<TEntity> ApplyMandatoryFilter(IQueryable<TEntity> query)
        {
            int? tenantId = GetTenantId();

            if (tenantId.HasValue)
            {
                if (CheckPermission(StandardPermissions.FullAccess))
                {
                    return query.Where(x => x.TenantId == null || x.TenantId == tenantId);
                }
                return query.Where(x => x.TenantId == tenantId);
            }
            else
            {
                // Should never be able to get here... since, workContext.CurrentTenant should never be null
                return query.Where(x => x.TenantId == null);
            }
        }

        #endregion GenericODataController<TEntity, TKey> Members

        protected virtual int? GetTenantId()
        {
            if (workContext.CurrentTenant == null)
            {
                return null;
            }
            return workContext.CurrentTenant.Id;
        }

        protected override bool CanViewEntity(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            if (CheckPermission(StandardPermissions.FullAccess))
            {
                return true; // Only the administrator should have full access
            }

            // If not admin user, but possibly the tenant user...

            if (CheckPermission(ReadPermission))
            {
                int? tenantId = GetTenantId();

                if (tenantId == null)
                {
                    return entity.TenantId == null;
                }

                return entity.TenantId == null || entity.TenantId == tenantId;
            }

            return false;
        }

        protected override bool CanModifyEntity(TEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            if (CheckPermission(StandardPermissions.FullAccess))
            {
                return true; // Only the administrator should have full access
            }

            // If not admin user, but possibly the tenant...

            if (CheckPermission(WritePermission))
            {
                int? tenantId = GetTenantId();

                if (tenantId == null)
                {
                    return entity.TenantId == null;
                }

                return entity.TenantId == null || entity.TenantId == tenantId;
            }

            return false;
        }
    }
}