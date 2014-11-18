using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Exceptions;
using Kore.Localization;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.RuleEngine
{
    public interface IRuleManager
    {
        bool Matches(string expression, object model = null);
    }

    public class RuleManager : IRuleManager
    {
        private readonly IEnumerable<IRuleProvider> ruleProviders;
        private readonly IEnumerable<IScriptExpressionEvaluator> evaluators;

        public RuleManager(IEnumerable<IRuleProvider> ruleProviders, IEnumerable<IScriptExpressionEvaluator> evaluators)
        {
            this.ruleProviders = ruleProviders;
            this.evaluators = evaluators;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public bool Matches(string expression, object model = null)
        {
            var evaluator = evaluators.FirstOrDefault();
            if (evaluator == null)
            {
                throw new KoreException(T("There are currently no scripting engines enabled."));
            }

            object result;

            try
            {
                result = evaluator.Evaluate(expression, new List<IGlobalMethodProvider> { new GlobalMethodProvider(this) }, model);
            }
            catch (Exception)
            {
                return false;
            }

            if (result == null)
            {
                throw new KoreException(T("Expression is not a boolean value."));
            }
            return (bool)result;
        }

        private class GlobalMethodProvider : IGlobalMethodProvider
        {
            private readonly RuleManager ruleManager;

            public GlobalMethodProvider(RuleManager ruleManager)
            {
                this.ruleManager = ruleManager;
            }

            public void Process(GlobalMethodContext context, object model = null)
            {
                var ruleContext = new RuleContext
                {
                    FunctionName = context.FunctionName,
                    Model = model,
                    Arguments = context.Arguments.ToArray(),
                    Result = context.Result
                };

                foreach (var ruleProvider in ruleManager.ruleProviders)
                {
                    ruleProvider.Process(ruleContext);
                }

                context.Result = ruleContext.Result;
            }
        }
    }
}