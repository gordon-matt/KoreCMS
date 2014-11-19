using System;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting.Compiler;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting.Ast
{
    public class ErrorAstNode : AstNode, IAstNodeWithToken
    {
        private readonly Token token;
        private readonly string message;

        public ErrorAstNode(Token token, string message)
        {
            this.token = token;
            this.message = message;
        }

        public Token Token
        {
            get { return token; }
        }

        public string Message
        {
            get { return message; }
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", GetType().Name, Message);
        }

        public override object Accept(AstVisitor visitor)
        {
            return visitor.VisitError(this);
        }
    }
}