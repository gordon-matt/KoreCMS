using System.Data.Entity;
using Kore.Infrastructure;
using Kore.Plugins.Widgets.Google.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.Google
{
    public class GooglePlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();

            var dbContext = EngineContext.Current.Resolve<DbContext>();

            if (!CheckIfTableExists(dbContext, Constants.Tables.SitemapConfig))
            {
                #region CREATE TABLE [dbo].[Kore_Plugins_Google_Sitemap]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Plugins_Google_Sitemap]
(
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [PageId] [uniqueidentifier] NOT NULL,
    [ChangeFrequency] [tinyint] NOT NULL,
    [Priority] [real] NOT NULL,
    CONSTRAINT [PK_dbo.Kore_Plugins_Google_Sitemap] PRIMARY KEY CLUSTERED
    (
        [Id] ASC
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]");

                #endregion CREATE TABLE [dbo].[Kore_Plugins_Google_Sitemap]
            }
        }

        public override void Uninstall()
        {
            base.Uninstall();
            UninstallLanguagePack<LanguagePackInvariant>();

            var dbContext = EngineContext.Current.Resolve<DbContext>();
            DropTable(dbContext, Constants.Tables.SitemapConfig);
        }
    }
}