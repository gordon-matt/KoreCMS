using System;
using System.Collections.Generic;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripting.Ast;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripting.Compiler
{
    public class EvaluationContext
    {
        public AbstractSyntaxTree Tree { get; set; }

        public Func<string, IList<object>, object> MethodInvocationCallback { get; set; }
    }
}