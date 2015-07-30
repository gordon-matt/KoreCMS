using System.Collections.Generic;

namespace Kore.Web.Infrastructure
{
    // TODO: Consider exchanging this for a DurandalRouteAttribute instead. Need to test performance though,
    //  as it would mean using reflection...
    public interface IDurandalRouteProvider
    {
        IEnumerable<DurandalRoute> Routes { get; }
    }

    public struct DurandalRoute
    {
        public string ModuleId { get; set; }

        //public string Title { get; set; }

        public string Route { get; set; }

        public string JsPath { get; set; } //note: for embedded scripts, we can use a virtual path provider

        //public bool Nav { get; set; }
    }
}