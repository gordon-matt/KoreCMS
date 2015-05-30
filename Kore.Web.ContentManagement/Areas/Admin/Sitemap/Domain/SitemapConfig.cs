using System;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Web.ContentManagement.Areas.Admin.Sitemap.Domain
{
    [DataContract]
    public class SitemapConfig : IEntity
    {
        public SitemapConfig()
        {
            Priority = .5f;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public Guid PageId { get; set; }

        [DataMember]
        public ChangeFrequency ChangeFrequency { get; set; }

        [DataMember]
        public float Priority { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class SitemapConfigMap : EntityTypeConfiguration<SitemapConfig>, IEntityTypeConfiguration
    {
        public SitemapConfigMap()
        {
            ToTable(CmsConstants.Tables.SitemapConfig);
            HasKey(x => x.Id);
            Property(x => x.ChangeFrequency).IsRequired();
            Property(x => x.Priority).IsRequired();
        }
    }
}