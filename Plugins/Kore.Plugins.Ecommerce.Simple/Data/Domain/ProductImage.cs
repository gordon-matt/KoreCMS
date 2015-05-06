﻿using System.Data.Entity.ModelConfiguration;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Plugins.Ecommerce.Simple.Data.Domain
{
    public class ProductImage : IEntity
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Caption { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class ProductImageMap : EntityTypeConfiguration<ProductImage>, IEntityTypeConfiguration
    {
        public ProductImageMap()
        {
            ToTable("Kore_Plugins_SimpleCommerce_ProductImages");
            HasKey(x => x.Id);
        }
    }
}