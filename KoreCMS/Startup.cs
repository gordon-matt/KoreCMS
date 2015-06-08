using Kore.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KoreCMS.Startup))]

namespace KoreCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            if (!DatabaseHelper.IsDatabaseInstalled())
            {
                return;
            }
            ConfigureAuth(app);
        }
    }
}