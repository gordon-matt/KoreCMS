using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripting
{
    public interface IScriptExpressionEvaluator
    {
        object Evaluate(string expression, IEnumerable<IGlobalMethodProvider> providers, object model = null);
    }
}