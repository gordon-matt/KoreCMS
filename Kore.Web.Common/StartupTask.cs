using System.Data.Entity;
using System.Linq;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;

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
    [Order] [smallint] NULL,
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

            EnsureData(dbContext);
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

        private void EnsureData(DbContext dbContext)
        {
            var count = dbContext.Database
                .SqlQuery<int?>("SELECT COUNT(*) FROM [dbo].[Kore_Common_Regions]")
                .SingleOrDefault();

            if (count.HasValue && count > 0)
            {
                return;
            }

            var dataSettings = EngineContext.Current.Resolve<DataSettings>();

            if (dataSettings.CreateSampleData)
            {
                #region Insert Continents & Countries

                dbContext.Database.ExecuteSqlCommand(
@"SET IDENTITY_INSERT [dbo].[Kore_Common_Regions] ON
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (1, N'North America', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (2, N'South America', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (3, N'Australasia', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (4, N'Asia and Middle East', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (5, N'Africa', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (6, N'Europe', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (7, N'Antarctica', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (8, N'Afghanistan', 2, N'AF', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (9, N'Åland Islands', 2, N'AX', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (10, N'Albania', 2, N'AL', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (11, N'Algeria', 2, N'DZ', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (12, N'American Samoa', 2, N'AS', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (13, N'Andorra', 2, N'AD', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (14, N'Anla', 2, N'AO', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (15, N'Anguilla', 2, N'AI', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (16, N'Antarctica', 2, N'AQ', 0, NULL, 7)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (17, N'Antigua and Barbuda', 2, N'AG', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (18, N'Argentina', 2, N'AR', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (19, N'Armenia', 2, N'AM', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (20, N'Aruba', 2, N'AW', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (21, N'Australia', 2, N'AU', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (22, N'Austria', 2, N'AT', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (23, N'Azerbaijan', 2, N'AZ', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (24, N'Bahamas', 2, N'BS', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (25, N'Bahrain', 2, N'BH', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (26, N'Bangladesh', 2, N'BD', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (27, N'Barbados', 2, N'BB', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (28, N'Belarus', 2, N'BY', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (29, N'Belgium', 2, N'BE', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (30, N'Belize', 2, N'BZ', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (31, N'Benin', 2, N'BJ', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (32, N'Bermuda', 2, N'BM', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (33, N'Bhutan', 2, N'BT', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (34, N'Bolivia, Plurinational State of', 2, N'BO', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (35, N'Bonaire, Sint Eustatius and Saba', 2, N'BQ', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (36, N'Bosnia and Herzevina', 2, N'BA', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (37, N'Botswana', 2, N'BW', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (38, N'Bouvet Island', 2, N'BV', 0, NULL, 7)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (39, N'Brazil', 2, N'BR', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (40, N'British Indian Ocean Territory', 2, N'IO', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (41, N'Brunei Darussalam', 2, N'BN', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (42, N'Bulgaria', 2, N'BG', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (43, N'Burkina Faso', 2, N'BF', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (44, N'Burundi', 2, N'BI', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (45, N'Cambodia', 2, N'KH', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (46, N'Cameroon', 2, N'CM', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (47, N'Canada', 2, N'CA', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (48, N'Cape Verde', 2, N'CV', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (49, N'Cayman Islands', 2, N'KY', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (50, N'Central African Republic', 2, N'CF', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (51, N'Chad', 2, N'TD', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (52, N'Chile', 2, N'CL', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (53, N'China', 2, N'CN', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (54, N'Christmas Island', 2, N'CX', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (55, N'Cocos (Keeling) Islands', 2, N'CC', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (56, N'Colombia', 2, N'CO', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (57, N'Comoros', 2, N'KM', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (58, N'Con', 2, N'CG', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (59, N'Con, Democratic Republic of the', 2, N'CD', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (60, N'Cook Islands', 2, N'CK', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (61, N'Costa Rica', 2, N'CR', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (62, N'Côte d''Ivoire', 2, N'CI', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (63, N'Croatia', 2, N'HR', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (64, N'Cuba', 2, N'CU', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (65, N'Curaçao', 2, N'CW', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (66, N'Cyprus', 2, N'CY', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (67, N'Czech Republic', 2, N'CZ', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (68, N'Denmark', 2, N'DK', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (69, N'Djibouti', 2, N'DJ', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (70, N'Dominica', 2, N'DM', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (71, N'Dominican Republic', 2, N'DO', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (72, N'Ecuador', 2, N'EC', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (73, N'Egypt', 2, N'EG', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (74, N'El Salvador', 2, N'SV', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (75, N'Equatorial Guinea', 2, N'GQ', 0, NULL, 5)INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (76, N'Eritrea', 2, N'ER', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (77, N'Estonia', 2, N'EE', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (78, N'Ethiopia', 2, N'ET', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (79, N'Falkland Islands (Malvinas)', 2, N'FK', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (80, N'Faroe Islands', 2, N'FO', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (81, N'Fiji', 2, N'FJ', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (82, N'Finland', 2, N'FI', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (83, N'France', 2, N'FR', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (84, N'French Guiana', 2, N'GF', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (85, N'French Polynesia', 2, N'PF', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (86, N'French Southern Territories', 2, N'TF', 0, NULL, 7)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (87, N'Gabon', 2, N'GA', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (88, N'Gambia', 2, N'GM', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (89, N'Georgia', 2, N'GE', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (90, N'Germany', 2, N'DE', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (91, N'Ghana', 2, N'GH', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (92, N'Gibraltar', 2, N'GI', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (93, N'Greece', 2, N'GR', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (94, N'Greenland', 2, N'GL', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (95, N'Grenada', 2, N'GD', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (96, N'Guadeloupe', 2, N'GP', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (97, N'Guam', 2, N'GU', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (98, N'Guatemala', 2, N'GT', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (99, N'Guernsey', 2, N'GG', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (100, N'Guinea', 2, N'GN', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (101, N'Guinea-Bissau', 2, N'GW', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (102, N'Guyana', 2, N'GY', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (103, N'Haiti', 2, N'HT', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (104, N'Heard Island and McDonald Islands', 2, N'HM', 0, NULL, 7)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (105, N'Holy See (Vatican City State)', 2, N'VA', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (106, N'Honduras', 2, N'HN', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (107, N'Hong Kong', 2, N'HK', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (108, N'Hungary', 2, N'HU', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (109, N'Iceland', 2, N'IS', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (110, N'India', 2, N'IN', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (111, N'Indonesia', 2, N'ID', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (112, N'Iran, Islamic Republic of', 2, N'IR', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (113, N'Iraq', 2, N'IQ', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (114, N'Ireland', 2, N'IE', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (115, N'Isle of Man', 2, N'IM', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (116, N'Israel', 2, N'IL', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (117, N'Italy', 2, N'IT', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (118, N'Jamaica', 2, N'JM', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (119, N'Japan', 2, N'JP', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (120, N'Jersey', 2, N'JE', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (121, N'Jordan', 2, N'JO', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (122, N'Kazakhstan', 2, N'KZ', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (123, N'Kenya', 2, N'KE', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (124, N'Kiribati', 2, N'KI', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (125, N'Korea, Democratic People''s Republic of', 2, N'KP', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (126, N'Korea, Republic of', 2, N'KR', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (127, N'Kuwait', 2, N'KW', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (128, N'Kyrgyzstan', 2, N'KG', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (129, N'Lao People''s Democratic Republic', 2, N'LA', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (130, N'Latvia', 2, N'LV', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (131, N'Lebanon', 2, N'LB', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (132, N'Lesotho', 2, N'LS', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (133, N'Liberia', 2, N'LR', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (134, N'Libya', 2, N'LY', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (135, N'Liechtenstein', 2, N'LI', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (136, N'Lithuania', 2, N'LT', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (137, N'Luxembourg', 2, N'LU', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (138, N'Macao', 2, N'MO', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (139, N'Macedonia, the former Yuslav Republic of', 2, N'MK', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (140, N'Madagascar', 2, N'MG', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (141, N'Malawi', 2, N'MW', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (142, N'Malaysia', 2, N'MY', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (143, N'Maldives', 2, N'MV', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (144, N'Mali', 2, N'ML', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (145, N'Malta', 2, N'MT', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (146, N'Marshall Islands', 2, N'MH', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (147, N'Martinique', 2, N'MQ', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (148, N'Mauritania', 2, N'MR', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (149, N'Mauritius', 2, N'MU', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (150, N'Mayotte', 2, N'YT', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (151, N'Mexico', 2, N'MX', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (152, N'Micronesia, Federated States of', 2, N'FM', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (153, N'Moldova, Republic of', 2, N'MD', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (154, N'Monaco', 2, N'MC', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (155, N'Monlia', 2, N'MN', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (156, N'Montenegro', 2, N'ME', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (157, N'Montserrat', 2, N'MS', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (158, N'Morocco', 2, N'MA', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (159, N'Mozambique', 2, N'MZ', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (160, N'Myanmar', 2, N'MM', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (161, N'Namibia', 2, N'NA', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (162, N'Nauru', 2, N'NR', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (163, N'Nepal', 2, N'NP', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (164, N'Netherlands', 2, N'NL', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (165, N'New Caledonia', 2, N'NC', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (166, N'New Zealand', 2, N'NZ', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (167, N'Nicaragua', 2, N'NI', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (168, N'Niger', 2, N'NE', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (169, N'Nigeria', 2, N'NG', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (170, N'Niue', 2, N'NU', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (171, N'Norfolk Island', 2, N'NF', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (172, N'Northern Mariana Islands', 2, N'MP', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (173, N'Norway', 2, N'NO', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (174, N'Oman', 2, N'OM', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (175, N'Pakistan', 2, N'PK', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (176, N'Palau', 2, N'PW', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (177, N'Palestinian Territory Occupied', 2, N'PS', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (178, N'Panama', 2, N'PA', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (179, N'Papua New Guinea', 2, N'PG', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (180, N'Paraguay', 2, N'PY', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (181, N'Peru', 2, N'PE', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (182, N'Philippines', 2, N'PH', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (183, N'Pitcairn', 2, N'PN', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (184, N'Poland', 2, N'PL', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (185, N'Portugal', 2, N'PT', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (186, N'Puerto Rico', 2, N'PR', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (187, N'Qatar', 2, N'QA', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (188, N'Réunion', 2, N'RE', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (189, N'Romania', 2, N'RO', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (190, N'Russia', 3, N'RU', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (191, N'Rwanda', 2, N'RW', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (192, N'Saint Barthélemy', 2, N'BL', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (193, N'Saint Helena, Ascension and Tristan da Cunha', 2, N'SH', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (194, N'Saint Kitts and Nevis', 2, N'KN', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (195, N'Saint Lucia', 2, N'LC', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (196, N'Saint Martin (French part)', 2, N'MF', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (197, N'Saint Pierre and Miquelon', 2, N'PM', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (198, N'Saint Vincent and the Grenadines', 2, N'VC', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (199, N'Samoa', 2, N'WS', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (200, N'San Marino', 2, N'SM', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (201, N'Sao Tome and Principe', 2, N'ST', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (202, N'Saudi Arabia', 2, N'SA', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (203, N'Senegal', 2, N'SN', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (204, N'Serbia', 2, N'RS', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (205, N'Seychelles', 2, N'SC', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (206, N'Sierra Leone', 2, N'SL', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (207, N'Singapore', 2, N'SG', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (208, N'Sint Maarten (Dutch part)', 2, N'SX', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (209, N'Slovakia', 2, N'SK', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (210, N'Slovenia', 2, N'SI', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (211, N'Solomon Islands', 2, N'SB', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (212, N'Somalia', 2, N'SO', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (213, N'South Africa', 2, N'ZA', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (214, N'South Georgia and the South Sandwich Islands', 2, N'GS', 0, NULL, 7)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (215, N'South Sudan', 2, N'SS', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (216, N'Spain', 2, N'ES', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (217, N'Sri Lanka', 2, N'LK', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (218, N'Sudan', 2, N'SD', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (219, N'Suriname', 2, N'SR', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (220, N'Svalbard and Jan Mayen', 2, N'SJ', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (221, N'Swaziland', 2, N'SZ', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (222, N'Sweden', 2, N'SE', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (223, N'Switzerland', 2, N'CH', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (224, N'Syrian Arab Republic', 2, N'SY', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (225, N'Taiwan, Province of China', 2, N'TW', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (226, N'Tajikistan', 2, N'TJ', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (227, N'Tanzania, United Republic of', 2, N'TZ', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (228, N'Thailand', 2, N'TH', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (229, N'Timor-Leste', 2, N'TL', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (230, N'To', 2, N'TG', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (231, N'Tokelau', 2, N'TK', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (232, N'Tonga', 2, N'TO', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (233, N'Trinidad and Toba', 2, N'TT', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (234, N'Tunisia', 2, N'TN', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (235, N'Turkey', 2, N'TR', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (236, N'Turkmenistan', 2, N'TM', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (237, N'Turks and Caicos Islands', 2, N'TC', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (238, N'Tuvalu', 2, N'TV', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (239, N'Uganda', 2, N'UG', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (240, N'Ukraine', 2, N'UA', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (241, N'United Arab Emirates', 2, N'AE', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (242, N'United Kingdom', 2, N'GB', 0, NULL, 6)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (243, N'United States', 2, N'US', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (244, N'United States Minor Outlying Islands', 2, N'UM', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (245, N'Uruguay', 2, N'UY', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (246, N'Uzbekistan', 2, N'UZ', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (247, N'Vanuatu', 2, N'VU', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (248, N'Venezuela, Bolivarian Republic of', 2, N'VE', 0, NULL, 2)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (249, N'Viet Nam', 2, N'VN', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (250, N'Virgin Islands, British', 2, N'VG', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (251, N'Virgin Islands, U.S.', 2, N'VI', 0, NULL, 1)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (252, N'Wallis and Futuna', 2, N'WF', 0, NULL, 3)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (253, N'Western Sahara', 2, N'EH', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (254, N'Yemen', 2, N'YE', 0, NULL, 4)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (255, N'Zambia', 2, N'ZM', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (256, N'Zimbabwe', 2, N'ZW', 0, NULL, 5)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (265, N'Ha Noi', 4, NULL, 0, NULL, 249)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (281, N'Cambridge', 4, NULL, 0, NULL, 242)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (282, N'Ho Chi Minh City', 4, NULL, 0, NULL, 249)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (283, N'Singapore', 4, NULL, 0, NULL, 207)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (284, N'Bintan Island', 4, NULL, 0, NULL, 111)
SET IDENTITY_INSERT [dbo].[Kore_Common_Regions] OFF");

                #endregion
            }
            else
            {
                #region Insert Continents Only

                dbContext.Database.ExecuteSqlCommand(
@"SET IDENTITY_INSERT [dbo].[Kore_Common_Regions] ON
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (1, N'North America', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (2, N'South America', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (3, N'Australasia', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (4, N'Asia and Middle East', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (5, N'Africa', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (6, N'Europe', 1, NULL, 0, NULL, NULL)
INSERT [dbo].[Kore_Common_Regions] ([Id], [Name], [RegionType], [CountryCode], [HasStates], [StateCode], [ParentId]) VALUES (7, N'Antarctica', 1, NULL, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Kore_Common_Regions] OFF");

                #endregion
            }
        }
    }
}