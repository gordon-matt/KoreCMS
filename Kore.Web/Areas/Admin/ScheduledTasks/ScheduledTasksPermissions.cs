using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.ScheduledTasks
{
    public class ScheduledTasksPermissions : IPermissionProvider
    {
        public static readonly Permission ManageScheduledTasks = new Permission { Name = "ManageScheduledTasks", Category = "System", Description = "Manage Scheduled Tasks" };

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ManageScheduledTasks;
        }
    }
}