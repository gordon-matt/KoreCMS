﻿using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;
using Kore.Web.Plugins;

namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public class SimpleCommerceProductImage : IEntity
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Caption { get; set; }

        public int Order { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class ProductImageMap : EntityTypeConfiguration<SimpleCommerceProductImage>, IEntityTypeConfiguration
    {
        public ProductImageMap()
        {
            ToTable(Constants.Tables.ProductImages);
            HasKey(x => x.Id);
            Property(x => x.ProductId).IsRequired();
            Property(x => x.Url).IsRequired().HasMaxLength(255);
            Property(x => x.ThumbnailUrl).IsRequired().HasMaxLength(255);
            Property(x => x.Caption).HasMaxLength(255);
            Property(x => x.Order).IsRequired();
        }

        #region IEntityTypeConfiguration Members

        public bool IsEnabled
        {
            get { return PluginManager.IsPluginInstalled(Constants.PluginSystemName); }
        }

        #endregion IEntityTypeConfiguration Members
    }
}