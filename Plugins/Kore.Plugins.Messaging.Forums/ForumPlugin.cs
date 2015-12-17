using System.Data.Entity;
using Kore.Infrastructure;
using Kore.Plugins.Messaging.Forums.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Messaging.Forums
{
    public class ForumPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();
            var dbContext = EngineContext.Current.Resolve<DbContext>();

            if (!CheckIfTableExists(dbContext, Constants.Tables.Groups))
            {
                // TODO
            }

            if (!CheckIfTableExists(dbContext, Constants.Tables.Forums))
            {
                // TODO
            }

            // TODO: etc
        }

        public override void Uninstall()
        {
            UninstallLanguagePack<LanguagePackInvariant>();

            var dbContext = EngineContext.Current.Resolve<DbContext>();
            DropTable(dbContext, Constants.Tables.PrivateMessages);
            DropTable(dbContext, Constants.Tables.Subscriptions);
            DropTable(dbContext, Constants.Tables.Posts);
            DropTable(dbContext, Constants.Tables.Topics);
            DropTable(dbContext, Constants.Tables.Forums);
            DropTable(dbContext, Constants.Tables.Groups);

            base.Uninstall();
        }
    }
}