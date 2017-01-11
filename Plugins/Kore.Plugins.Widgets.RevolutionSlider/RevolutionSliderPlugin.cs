using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;
using Kore.Plugins.Widgets.RevolutionSlider.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RevolutionSlider
{
    public class RevolutionSliderPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();
        }

        public override void Uninstall()
        {
            base.Uninstall();

            var dbContextFactory = EngineContext.Current.Resolve<IDbContextFactory>();
            var dbContext = dbContextFactory.GetContext();

            DropTable(dbContext, Constants.Tables.Layers);
            DropTable(dbContext, Constants.Tables.Slides);
            DropTable(dbContext, Constants.Tables.Sliders);

            UninstallLanguagePack<LanguagePackInvariant>();
        }
    }
}