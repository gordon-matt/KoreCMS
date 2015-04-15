using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Kore.Data;
using Kore.Data.EntityFramework;

namespace Kore.Plugins.Widgets.Google.Data.Domain
{
    [DataContract]
    public class GoogleSitemapPageConfig : IEntity
    {
        public GoogleSitemapPageConfig()
        {
            Priority = .5f;
        }

        [DataMember]
        public int Id { get; set; }

        public Guid PageId { get; set; }

        public ChangeFrequency ChangeFrequency { get; set; }

        public float Priority { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }

    public class GoogleSitemapPageConfigMap : EntityTypeConfiguration<GoogleSitemapPageConfig>, IEntityTypeConfiguration
    {
        public GoogleSitemapPageConfigMap()
        {
            ToTable("Kore_Plugins_Google_Sitemap");
            HasKey(x => x.Id);
            Property(x => x.ChangeFrequency).IsRequired();
            Property(x => x.Priority).IsRequired();
        }
    }
}