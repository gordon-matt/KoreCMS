using System;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Messaging.LiveChat.ContentBlocks
{
    public class LiveChatBlock : ContentBlockBase
    {
        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Live Chat Box"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Messaging.LiveChat/Views/Shared/DisplayTemplates/LiveChatBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { throw new NotSupportedException(); }
        }

        #endregion ContentBlockBase Overrides
    }
}