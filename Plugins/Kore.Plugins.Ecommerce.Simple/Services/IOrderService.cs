﻿using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;

namespace Kore.Plugins.Ecommerce.Simple.Services
{
    public interface IOrderService : IGenericDataService<Order>
    {
    }

    public class OrderService : GenericDataService<Order>, IOrderService
    {
        public OrderService(
            ICacheManager cacheManager,
            IRepository<Order> repository)
            : base(cacheManager, repository)
        {
        }
    }
}