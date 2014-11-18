using Kore.Web.Events;

namespace Kore.Web.Mvc.RoboUI
{
    public interface IRoboUIDisplayEventHandler : IEventHandler
    {
        void OnRoboUIGridDisplay<TModel>(RoboUIGridResult<TModel> roboGrid) where TModel : class;
    }
}