using System.Collections.Generic;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting.Compiler;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting.Ast
{
    public class UnaryAstNode : AstNode, IAstNodeWithToken
    {
        private readonly AstNode operand;
        private readonly Token token;

        public UnaryAstNode(Token token, AstNode operand)
        {
            this.operand = operand;
            this.token = token;
        }

        public Token Token { get { return token; } }

        public Token Operator { get { return token; } }

        public AstNode Operand { get { return operand; } }

        public override IEnumerable<AstNode> Children
        {
            get
            {
                return new List<AstNode>(1) { operand };
            }
        }

        public override object Accept(AstVisitor visitor)
        {
            return visitor.VisitUnary(this);
        }
    }
}