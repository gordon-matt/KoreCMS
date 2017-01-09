using System.Data.Entity;
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
            //var dbContextFactory = EngineContext.Current.Resolve<IDbContextFactory>();
            //var dbContext = dbContextFactory.GetContext();

//            if (!CheckIfTableExists(dbContext, Constants.Tables.Calendars))
//            {
//                #region CREATE TABLE [dbo].[Kore_Plugins_FullCalendar_Calendars]

//                dbContext.Database.ExecuteSqlCommand(
//@"CREATE TABLE [dbo].[Kore_Plugins_FullCalendar_Calendars]
//(
//	[Id] [int] IDENTITY(1,1) NOT NULL,
//	[Name] [nvarchar](255) NOT NULL,
//	CONSTRAINT [PK_dbo.Kore_Plugins_FullCalendar_Calendars] PRIMARY KEY CLUSTERED
//	(
//		[Id] ASC
//	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
//) ON [PRIMARY]");

//                #endregion CREATE TABLE [dbo].[Kore_Plugins_FullCalendar_Calendars]
//            }

//            if (!CheckIfTableExists(dbContext, Constants.Tables.Events))
//            {
//                #region CREATE TABLE [dbo].[Kore_Plugins_FullCalendar_Calendars]

//                dbContext.Database.ExecuteSqlCommand(
//@"CREATE TABLE [dbo].[Kore_Plugins_FullCalendar_Events]
//(
//	[Id] [int] IDENTITY(1,1) NOT NULL,
//	[CalendarId] [int] NOT NULL,
//	[Name] [nvarchar](255) NOT NULL,
//	[StartDateTime] [datetime] NOT NULL,
//	[EndDateTime] [datetime] NOT NULL,
//	CONSTRAINT [PK_dbo.Kore_Plugins_FullCalendar_Events] PRIMARY KEY CLUSTERED
//	(
//		[Id] ASC
//	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
//) ON [PRIMARY]");

//                dbContext.Database.ExecuteSqlCommand(
//@"ALTER TABLE [dbo].[Kore_Plugins_FullCalendar_Events] WITH CHECK
//ADD CONSTRAINT [FK_dbo.Kore_Plugins_FullCalendar_Events_dbo.Kore_Plugins_FullCalendar_Calendars_CalendarId]
//FOREIGN KEY([CalendarId])
//REFERENCES [dbo].[Kore_Plugins_FullCalendar_Calendars] ([Id])
//ON DELETE CASCADE");

//                dbContext.Database.ExecuteSqlCommand(
//@"ALTER TABLE [dbo].[Kore_Plugins_FullCalendar_Events]
//CHECK CONSTRAINT [FK_dbo.Kore_Plugins_FullCalendar_Events_dbo.Kore_Plugins_FullCalendar_Calendars_CalendarId]");

//                #endregion CREATE TABLE [dbo].[Kore_Plugins_FullCalendar_Calendars]
//            }
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