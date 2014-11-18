namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.RuleEngine
{
    public interface IRuleProvider
    {
        void Process(RuleContext ruleContext);
    }
}