using System.Linq;
using Kore.Collections;
using Kore.Configuration;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Tasks;
using Kore.Tasks.Domain;

namespace Kore
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            if (KoreConfigurationSection.Instance.Tasks.Enabled)
            {
                var taskRepository = EngineContext.Current.Resolve<IRepository<ScheduledTask>>();
                var allTasks = EngineContext.Current.ResolveAll<ITask>();
                var allTaskNames = allTasks.Select(x => x.Name).ToList();
                var installedTasks = taskRepository.Find();
                var installedTaskNames = installedTasks.Select(x => x.Name).ToList();

                var tasksToAdd = allTasks
                    .Where(x => !installedTaskNames.Contains(x.Name))
                    .Select(x => new ScheduledTask
                    {
                        Name = x.Name,
                        Type = x.GetType().AssemblyQualifiedName,
                        Seconds = x.DefaultInterval
                    });

                if (!tasksToAdd.IsNullOrEmpty())
                {
                    taskRepository.Insert(tasksToAdd);
                }

                var tasksToDelete = installedTasks.Where(x => !allTaskNames.Contains(x.Name));

                if (!tasksToDelete.IsNullOrEmpty())
                {
                    taskRepository.Delete(tasksToDelete);
                }
            }
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion IStartupTask Members
    }
}