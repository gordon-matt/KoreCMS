using System;
using Kore.Tenants.Domain;

namespace Kore.Configuration.Domain
{
    public class Setting : ITenantEntity
    {
        public Guid Id { get; set; }

        public int? TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        public override string ToString()
        {
            return Name;
        }
    }
}