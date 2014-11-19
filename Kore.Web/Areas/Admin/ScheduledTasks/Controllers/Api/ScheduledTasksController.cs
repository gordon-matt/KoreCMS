using System;
using System.Web.Http;
using System.Web.Http.OData;
using Kore.Data;
using Kore.Tasks;
using Kore.Tasks.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.Areas.Admin.ScheduledTasks.Controllers.Api
{
    public class ScheduledTasksController : GenericODataController<ScheduledTask, int>
    {
        public ScheduledTasksController(IRepository<ScheduledTask> repository)
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
    }
}