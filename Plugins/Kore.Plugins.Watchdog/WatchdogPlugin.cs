using System.Data.Entity;
using Kore.Infrastructure;
using Kore.Plugins.Watchdog.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Watchdog
{
    public class WatchdogPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLocalizableStrings<DefaultLocalizableStringsProvider>();

            var dbContext = EngineContext.Current.Resolve<DbContext>();
            if (!CheckIfTableExists(dbContext, Constants.Tables.WatchdogInstances))
            {
                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Maintenance_WatchdogInstances]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_dbo.Kore_Maintenance_WatchdogInstances] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]");
            }
        }

        public override void Uninstall()
        {
            base.Uninstall();
            UninstallLocalizableStrings<DefaultLocalizableStringsProvider>();
            var dbContext = EngineContext.Current.Resolve<DbContext>();
            DropTable(dbContext, Constants.Tables.WatchdogInstances);
        }
    }
}