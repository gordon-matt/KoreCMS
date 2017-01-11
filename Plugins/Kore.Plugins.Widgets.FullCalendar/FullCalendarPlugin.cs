using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;
using Kore.Plugins.Widgets.FullCalendar.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.FullCalendar
{
    public class FullCalendarPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();
        }

        public override void Uninstall()
        {
            UninstallLanguagePack<LanguagePackInvariant>();

            var dbContextFactory = EngineContext.Current.Resolve<IDbContextFactory>();
            var dbContext = dbContextFactory.GetContext();

            DropTable(dbContext, Constants.Tables.Events);
            DropTable(dbContext, Constants.Tables.Calendars);

            base.Uninstall();
        }
    }
}