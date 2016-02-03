using System.Collections.Generic;
using Owin;

namespace Kore.Infrastructure
{
    public interface IOwinStartupConfiguration
    {
        void Configuration(IAppBuilder app, ICollection<string> existingConfigurations);
    }
}