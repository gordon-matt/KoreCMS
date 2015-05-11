using System.Collections.Generic;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.ScheduledTasks
{
    public class ScheduledTasksPermissions : IPermissionProvider
    {
        public static readonly Permission ReadScheduledTasks = new Permission { Name = "Scheduled_Tasks_Read", Category = "System", Description = "Scheduled Tasks: Read" };
        public static readonly Permission WriteScheduledTasks = new Permission { Name = "Scheduled_Tasks_Write", Category = "System", Description = "Scheduled Tasks: Write" };

        public IEnumerable<Permission> GetPermissions()
        {
            yield return ReadScheduledTasks;
            yield return WriteScheduledTasks;
        }
    }
}