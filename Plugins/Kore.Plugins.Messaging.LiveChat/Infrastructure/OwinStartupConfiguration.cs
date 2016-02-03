using Kore.Infrastructure;
using Kore.Web.Plugins;
using Owin;

namespace Kore.Plugins.Messaging.LiveChat.Infrastructure
{
    public class OwinStartupConfiguration : IOwinStartupConfiguration
    {
        public void Configuration(IAppBuilder app)
        {
            if (!PluginManager.IsPluginInstalled(Constants.PluginSystemName))
            {
                return;
            }

            app.MapSignalR();
        }
    }
}