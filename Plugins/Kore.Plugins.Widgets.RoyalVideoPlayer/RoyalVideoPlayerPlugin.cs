using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer
{
    public class RoyalVideoPlayerPlugin : BasePlugin
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
            DropTable(dbContext, Constants.Tables.PlaylistVideos);
            DropTable(dbContext, Constants.Tables.Videos);
            DropTable(dbContext, Constants.Tables.Playlists);

            base.Uninstall();
        }
    }
}