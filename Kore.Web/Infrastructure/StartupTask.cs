using System;
using System.Linq;
using Kore.Collections;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Configuration;

namespace Kore.Web.Infrastructure
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            var settingsRepository = EngineContext.Current.Resolve<IRepository<Setting>>();
            var allSettings = EngineContext.Current.ResolveAll<ISettings>();
            var allSettingNames = allSettings.Select(x => x.Name).ToList();
            var installedSettings = settingsRepository.Table.ToList();
            var installedSettingNames = installedSettings.Select(x => x.Name).ToList();

            var settingsToAdd = allSettings.Where(x => !installedSettingNames.Contains(x.Name)).Select(x => new Setting
            {
                Id = Guid.NewGuid(),
                Name = x.Name,
                Type = x.GetType().AssemblyQualifiedName,
                Value = Activator.CreateInstance(x.GetType()).ToJson()
            }).ToList();

            if (!settingsToAdd.IsNullOrEmpty())
            {
                settingsRepository.Insert(settingsToAdd);
            }

            var settingsToDelete = installedSettings.Where(x => !allSettingNames.Contains(x.Name)).ToList();

            if (!settingsToDelete.IsNullOrEmpty())
            {
                settingsRepository.Delete(settingsToDelete);
            }
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IStartupTask Members
    }
}