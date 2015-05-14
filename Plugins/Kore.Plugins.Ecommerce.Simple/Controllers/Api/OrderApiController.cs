﻿using Kore.Data;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Ecommerce.Simple.Controllers.Api
{
    public class OrderApiController : GenericODataController<Order, int>
    {
        public OrderApiController(IRepository<Order> repository)
            : base(repository)
        {
        }

        protected override int GetId(Order entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Order entity)
        {
        }

        protected override Permission ReadPermission
        {
            get { return SimpleCommercePermissions.ReadOrders; }
        }

        protected override Permission WritePermission
        {
            get { return StandardPermissions.FullAccess; }
        }
    }
}