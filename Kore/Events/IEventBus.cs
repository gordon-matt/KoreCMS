using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kore.Events
{
    public interface IEventBus
    {
        IEnumerable Notify(string messageName, IDictionary<string, object> eventData);

        void Notify<TEventHandler>(Expression<Action<TEventHandler>> expression) where TEventHandler : IEventHandler;
    }
}