using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Common
{
    public class Permissions : IPermissionProvider
    {
        #region Regions

        public static readonly Permission RegionsRead = new Permission { Name = "Regions_Read", Category = "Common Lib", Description = "Regions: Read" };
        public static readonly Permission RegionsWrite = new Permission { Name = "Regions_Write", Category = "Common Lib", Description = "Regions: Write" };

        #endregion Regions

        public IEnumerable<Permission> GetPermissions()
        {
            yield return RegionsRead;
            yield return RegionsWrite;
        }
    }
}