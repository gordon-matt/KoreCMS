using System.Transactions;
using System.Web.Mvc;

namespace Kore.Web.Mvc
{
    public class TransactionAttribute : ActionFilterAttribute
    {
        private TransactionScope scope;

        public TransactionAttribute()
        {
            ScopeOption = TransactionScopeOption.Required;
        }

        public TransactionAttribute(TransactionScopeOption scopeOption)
        {
            ScopeOption = scopeOption;
        }

        public TransactionScopeOption ScopeOption { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            scope = new TransactionScope(ScopeOption, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted });
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (filterContext.Exception == null)
            {
                scope.Complete();
            }

            scope.Dispose();
        }
    }
}