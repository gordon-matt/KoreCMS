using System;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.Results;
using Kore.Data;
using Kore.Tasks;
using Kore.Tasks.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.ScheduledTasks.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class ScheduledTaskApiController : GenericODataController<ScheduledTask, int>
    {
        public ScheduledTaskApiController(IRepository<ScheduledTask> repository)
            : base(repository)
        {
        }

        protected override int GetId(ScheduledTask entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(ScheduledTask entity)
        {
            // Do nothing (int is auto incremented)
        }

        [HttpPost]
        public IHttpActionResult RunNow(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            int taskId = (int)parameters["taskId"];

            var scheduleTask = Repository.Find(taskId);
            if (scheduleTask == null)
                return NotFound();

            var task = new Task(scheduleTask);
            //ensure that the task is enabled
            task.Enabled = true;

            try
            {
                task.Execute(true);
            }
            catch (Exception x)
            {
                return InternalServerError(x);
            }

            return Ok();
        }

        protected override Permission ReadPermission
        {
            get { return ScheduledTasksPermissions.ReadScheduledTasks; }
        }

        protected override Permission WritePermission
        {
            get { return ScheduledTasksPermissions.WriteScheduledTasks; }
        }
    }
}