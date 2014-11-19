using System;

//using Kore.DI;

namespace Kore.Web
{
    public interface IWorkContextStateProvider //: IDependency
    {
        Func<WorkContext, T> Get<T>(string name);
    }
}