using System;
using System.Runtime.Serialization;
using Kore.Data;

namespace Kore.Localization.Domain
{
    [DataContract]
    public class LocalizableString : IEntity
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string CultureCode { get; set; }

        [DataMember]
        public string TextKey { get; set; }

        [DataMember]
        public string TextValue { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}