//using System;
//using System.Data.Entity.ModelConfiguration;
//using Kore.Data;
//using Kore.Data.EntityFramework;

//namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Domain
//{
//    public class PageWidget : IEntity
//    {
//        public Guid Id { get; set; }

//        public Guid PageId { get; set; }

//        public Guid WidgetId { get; set; }

//        #region IEntity Members

//        public object[] KeyValues
//        {
//            get { return new object[] { Id }; }
//        }

//        #endregion IEntity Members
//    }

//    public class PageWidgetMap : EntityTypeConfiguration<PageWidget>, IEntityTypeConfiguration
//    {
//        public PageWidgetMap()
//        {
//            ToTable("Kore_PageWidgets");
//            HasKey(x => x.Id);
//            Property(x => x.PageId).IsRequired();
//            Property(x => x.WidgetId).IsRequired();
//        }
//    }
//}