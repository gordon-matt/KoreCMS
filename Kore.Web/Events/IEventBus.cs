using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

//using Kore.DI;

namespace Kore.Web.Events
{
    public interface IEventBus //: IDependency
    {
        IEnumerable Notify(string messageName, IDictionary<string, object> eventData);

        void Notify<TEventHandler>(Expression<Action<TEventHandler>> expression) where TEventHandler : IEventHandler;
    }
}