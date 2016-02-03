using Owin;

namespace Kore.Infrastructure
{
    public interface IOwinStartupConfiguration
    {
        void Configuration(IAppBuilder app);
    }
}