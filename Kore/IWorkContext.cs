using Kore.Security.Membership;
using Kore.Tenants.Domain;

namespace Kore
{
    public interface IWorkContext
    {
        T GetState<T>(string name);

        void SetState<T>(string name, T value);

        string CurrentCultureCode { get; }

        Tenant CurrentTenant { get; }

        KoreUser CurrentUser { get; }
    }
}