using Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting.Compiler;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting.Ast
{
    public interface IAstNodeWithToken
    {
        Token Token { get; }
    }
}