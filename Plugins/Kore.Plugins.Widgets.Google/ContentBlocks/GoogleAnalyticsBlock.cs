﻿using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.Google.ContentBlocks
{
    public class GoogleAnalyticsBlock : ContentBlockBase
    {
        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.AnalyticsBlock.AccountNumber)]
        public string AccountNumber { get; set; }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.AnalyticsBlock.DomainName)]
        public string DomainName { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Google: Analytics"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Widgets.Google/Views/Shared/DisplayTemplates/GoogleAnalyticsBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Widgets.Google/Views/Shared/EditorTemplates/GoogleAnalyticsBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}