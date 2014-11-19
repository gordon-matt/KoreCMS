using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting
{
    public interface IScriptExpressionEvaluator
    {
        object Evaluate(string expression, IEnumerable<IGlobalMethodProvider> providers, object model = null);
    }
}