//using Kore.DI;
using Kore.Security.Membership;

namespace Kore.Web.Security.Membership.Permissions
{
    /// <summary>
    /// Entry-point for configured authorization scheme. Role-based system provided by default.
    /// </summary>
    public interface IAuthorizationService //: IDependency
    {
        void CheckAccess(Permission permission, KoreUser user);

        bool TryCheckAccess(Permission permission, KoreUser user);
    }
}