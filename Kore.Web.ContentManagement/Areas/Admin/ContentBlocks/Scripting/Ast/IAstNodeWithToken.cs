using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripting.Compiler;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripting.Ast
{
    public interface IAstNodeWithToken
    {
        Token Token { get; }
    }
}