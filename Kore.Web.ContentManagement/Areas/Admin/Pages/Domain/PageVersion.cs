//using System;
//using Kore.Data;

//namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Domain
//{
//    public class PageVersion : IEntity
//    {
//        public Guid Id { get; set; }

//        public Guid PageTypeId { get; set; }

//        public string Data { get; set; }

//        public DateTime DateCreated { get; set; }

//        public VersionStatus Status { get; set; }

//        public string CultureCode { get; set; }

//        #region IEntity Members

//        public object[] KeyValues
//        {
//            get { return new object[] { Id }; }
//        }

//        #endregion IEntity Members
//    }

//    public enum VersionStatus
//    {
//        CheckedOut = 0,// TODO: Not used yet
//        CheckedIn = 1,// TODO: Not used yet
//        Published = 2,
//        Rejected = 3,// TODO: Not used yet
//        Replaced = 4,// TODO: Not used yet
//        Delayed = 5 // TODO: Not used yet
//    }
//}