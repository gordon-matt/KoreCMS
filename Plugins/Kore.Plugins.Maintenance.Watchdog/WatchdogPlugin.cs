using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;
using Kore.Plugins.Maintenance.Watchdog.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Maintenance.Watchdog
{
    public class WatchdogPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();
        }

        public override void Uninstall()
        {
            base.Uninstall();
            UninstallLanguagePack<LanguagePackInvariant>();
            var dbContextFactory = EngineContext.Current.Resolve<IDbContextFactory>();
            var dbContext = dbContextFactory.GetContext();

            DropTable(dbContext, Constants.Tables.WatchdogInstances);
        }
    }
}