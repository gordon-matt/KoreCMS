using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.RuleEngine;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public interface IContentBlockProvider
    {
        IEnumerable<IContentBlock> GetContentBlocks(string zoneName, string currentCultureCode);
    }

    public class DefaultContentBlockProvider : IContentBlockProvider
    {
        private readonly IContentBlockService contentBlockService;
        private readonly IRuleManager ruleManager;

        public DefaultContentBlockProvider(IContentBlockService contentBlockService, IRuleManager ruleManager)
        {
            this.contentBlockService = contentBlockService;
            this.ruleManager = ruleManager;
        }

        public virtual IEnumerable<IContentBlock> GetContentBlocks(string zoneName, string currentCultureCode)
        {
            var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
            Guid? pageId = workContext.GetState<Guid?>("CurrentPageId");

            var contentBlocks = contentBlockService.GetContentBlocks(zoneName: zoneName, pageId: pageId);
            return contentBlocks.Where(x => IsVisibleContentBlock(contentBlocks, x, currentCultureCode)).ToList();
        }

        protected bool IsVisibleContentBlock(IEnumerable<IContentBlock> contentBlocks, IContentBlock contentBlock, string currentCulture)
        {
            if (contentBlock.RefId.HasValue)
            {
                if (!string.Equals(contentBlock.CultureCode, currentCulture))
                {
                    return false;
                }

                var parentContentBlock = contentBlocks.FirstOrDefault(x => x.Id == contentBlock.RefId.Value);
                if (parentContentBlock == null || !parentContentBlock.Enabled)
                {
                    return false;
                }
            }
            else
            {
                var childContentBlock = contentBlocks.FirstOrDefault(x => x.RefId == contentBlock.Id && x.CultureCode == currentCulture);
                if (childContentBlock != null)
                {
                    return false;
                }
            }

            return string.IsNullOrEmpty(contentBlock.DisplayCondition) || ruleManager.Matches(contentBlock.DisplayCondition);
        }
    }
}