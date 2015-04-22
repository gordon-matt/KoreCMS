namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.RuleEngine
{
    public interface IRuleProvider
    {
        void Process(RuleContext ruleContext);
    }
}