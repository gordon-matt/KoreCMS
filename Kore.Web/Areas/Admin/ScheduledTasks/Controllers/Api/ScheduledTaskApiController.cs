using System;
using System.Threading.Tasks;
using System.Web.Http;
using Kore.Data;
using Kore.Tasks.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using Microsoft.AspNet.OData;
using KoreTask = Kore.Tasks.Task;

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

        public override async Task<IHttpActionResult> Put(int key, ScheduledTask entity)
        {
            var existingEntity = await Service.FindOneAsync(key);
            existingEntity.Seconds = entity.Seconds;
            existingEntity.Enabled = entity.Enabled;
            existingEntity.StopOnError = entity.StopOnError;
            return await base.Put(key, existingEntity);
        }

        [HttpPost]
        public async Task<IHttpActionResult> RunNow(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            int taskId = (int)parameters["taskId"];

            var scheduleTask = await Service.FindOneAsync(taskId);
            if (scheduleTask == null)
                return NotFound();

            var task = new KoreTask(scheduleTask);
            //ensure that the task is enabled
            task.Enabled = true;

            try
            {
                task.Execute(true);
            }
            catch (Exception x)
            {
                Logger.Error(x.Message, x);
                return InternalServerError(x);
            }

            return Ok();
        }

        protected override Permission ReadPermission
        {
            get { return KoreWebPermissions.ScheduledTasksRead; }
        }

        protected override Permission WritePermission
        {
            get { return KoreWebPermissions.ScheduledTasksWrite; }
        }
    }
}