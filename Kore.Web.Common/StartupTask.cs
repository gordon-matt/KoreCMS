using System.Data.Entity;
using System.Linq;
using Kore.Infrastructure;

namespace Kore.Web.Common
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            var dbContext = EngineContext.Current.Resolve<DbContext>();

            if (!CheckIfTableExists(dbContext, Constants.Tables.Regions))
            {
                #region CREATE TABLE [dbo].[Kore_Common_Regions]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Common_Regions]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[RegionType] [tinyint] NOT NULL,
	[CountryCode] [nvarchar](10) NULL,
	[HasStates] [bit] NOT NULL,
	[StateCode] [nvarchar](10) NULL,
	[ParentId] [int] NULL,
	CONSTRAINT [PK_dbo.Kore_Common_Regions] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Common_Regions] WITH CHECK ADD
CONSTRAINT [FK_dbo.Kore_Common_Regions_dbo.Kore_Common_Regions_ParentId]
FOREIGN KEY([ParentId])
REFERENCES [dbo].[Kore_Common_Regions] ([Id])");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Common_Regions]
CHECK CONSTRAINT [FK_dbo.Kore_Common_Regions_dbo.Kore_Common_Regions_ParentId]");

                #endregion CREATE TABLE [dbo].[Kore_Common_Regions]
            }

            if (!CheckIfTableExists(dbContext, Constants.Tables.RegionSettings))
            {
                #region CREATE TABLE [dbo].[Kore_Common_RegionSettings]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Common_RegionSettings]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegionId] [int] NOT NULL,
	[SettingsId] [nvarchar](255) NOT NULL,
	[Fields] [nvarchar](max) NOT NULL,
	CONSTRAINT [PK_dbo.Kore_Common_RegionSettings] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

                #endregion CREATE TABLE [dbo].[Kore_Common_RegionSettings]
            }
        }

        public int Order
        {
            get { return 10; }
        }

        #endregion IStartupTask Members

        private bool CheckIfTableExists(DbContext dbContext, string tableName)
        {
            return dbContext.Database
                .SqlQuery<int?>(string.Format("SELECT 1 FROM sys.tables WHERE Name = '{0}'", tableName))
                .SingleOrDefault() != null;
        }
    }
}