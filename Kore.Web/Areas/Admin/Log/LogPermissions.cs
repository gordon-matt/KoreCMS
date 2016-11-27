using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Log
{
    public class LogPermissions : IPermissionProvider
    {
        public static readonly Permission ReadLog = new Permission { Name = "ReadLog", Category = "Log", Description = "Read Log" };

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ReadLog;
        }
    }
}