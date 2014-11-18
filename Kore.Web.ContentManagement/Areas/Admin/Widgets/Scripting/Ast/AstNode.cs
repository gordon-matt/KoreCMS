﻿using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting.Ast
{
    public abstract class AstNode
    {
        public virtual IEnumerable<AstNode> Children
        {
            get
            {
                return Enumerable.Empty<AstNode>();
            }
        }

        public abstract object Accept(AstVisitor visitor);

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(GetType().Name);
            var ewt = (this as IAstNodeWithToken);
            if (ewt != null)
            {
                sb.Append(" - ");
                sb.Append(ewt.Token);
            }
            return sb.ToString();
        }
    }
}