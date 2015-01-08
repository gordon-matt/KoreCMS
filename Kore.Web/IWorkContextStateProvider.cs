using System;

namespace Kore.Web
{
    public interface IWorkContextStateProvider
    {
        Func<IWebWorkContext, T> Get<T>(string name);
    }
}