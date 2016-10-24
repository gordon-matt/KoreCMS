using System;
using Kore.Data;
using Kore.Tenants.Domain;

namespace Kore.Logging.Domain
{
    public class LogEntry : ITenantEntity
    {
        public Guid Id { get; set; }

        public int? TenantId { get; set; }

        public DateTime EventDateTime { get; set; }

        public string EventLevel { get; set; }

        public string UserName { get; set; }

        public string MachineName { get; set; }

        public string EventMessage { get; set; }

        public string ErrorSource { get; set; }

        public string ErrorClass { get; set; }

        public string ErrorMethod { get; set; }

        public string ErrorMessage { get; set; }

        public string InnerErrorMessage { get; set; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members
    }
}