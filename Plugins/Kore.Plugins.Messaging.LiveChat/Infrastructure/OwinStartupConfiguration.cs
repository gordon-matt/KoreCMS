using System.Collections.Generic;
using System.Linq;
using Kore.Infrastructure;
using Kore.Web.Plugins;
using Owin;

namespace Kore.Plugins.Messaging.LiveChat.Infrastructure
{
    public class OwinStartupConfiguration : IOwinStartupConfiguration
    {
        public void Configuration(IAppBuilder app, ICollection<string> existingConfigurations)
        {
            if (!PluginManager.IsPluginInstalled(Constants.PluginSystemName))
            {
                return;
            }

            if (!existingConfigurations.Contains("SignalR"))
            {
                app.MapSignalR();
                existingConfigurations.Add("SignalR");
            }
        }
    }
}