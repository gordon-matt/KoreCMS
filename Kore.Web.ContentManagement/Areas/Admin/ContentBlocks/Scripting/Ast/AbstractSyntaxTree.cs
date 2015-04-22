using System.Collections.Generic;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripting.Ast
{
    public class AbstractSyntaxTree
    {
        public AstNode Root { get; set; }

        public IEnumerable<ErrorAstNode> GetErrors()
        {
            return new ErrorNodeCollector().Collect(Root);
        }
    }
}