using System.Collections.Generic;

//using Kore.DI;

namespace Kore.Web.Security.Membership.Permissions
{
    /// <summary>
    /// Implemented by modules to enumerate the types of permissions the which may be granted
    /// </summary>
    public interface IPermissionProvider //: IDependency
    {
        IEnumerable<Permission> GetPermissions();
    }
}