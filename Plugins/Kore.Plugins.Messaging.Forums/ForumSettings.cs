using Kore.Plugins.Messaging.Forums.Data.Domain;
using Kore.Web.Configuration;

namespace Kore.Plugins.Messaging.Forums
{
    public class ForumSettings : ISettings
    {
        public bool ForumsEnabled { get; set; }

        public bool RelativeDateTimeFormattingEnabled { get; set; }

        public bool AllowUsersToEditPosts { get; set; }

        public bool AllowUsersToManageSubscriptions { get; set; }

        public bool AllowGuestsToCreatePosts { get; set; }

        public bool AllowGuestsToCreateTopics { get; set; }

        public bool AllowUsersToDeletePosts { get; set; }

        public int TopicSubjectMaxLength { get; set; }

        public int StrippedTopicMaxLength { get; set; }

        public int PostMaxLength { get; set; }

        public int TopicsPageSize { get; set; }

        public int PostsPageSize { get; set; }

        public int SearchResultsPageSize { get; set; }

        public int ActiveDiscussionsPageSize { get; set; }

        public int LatestUserPostsPageSize { get; set; }

        public bool ShowUsersPostCount { get; set; }

        public EditorType ForumEditor { get; set; }

        public bool SignaturesEnabled { get; set; }

        public bool AllowPrivateMessages { get; set; }

        public bool ShowAlertForPM { get; set; }

        public int PrivateMessagesPageSize { get; set; }

        public int ForumSubscriptionsPageSize { get; set; }

        public bool NotifyAboutPrivateMessages { get; set; }

        public int PMSubjectMaxLength { get; set; }

        public int PMTextMaxLength { get; set; }

        public int HomePageActiveDiscussionsTopicCount { get; set; }

        public int ActiveDiscussionsFeedCount { get; set; }

        public bool ActiveDiscussionsFeedEnabled { get; set; }

        public bool ForumFeedsEnabled { get; set; }

        public int ForumFeedCount { get; set; }

        public int ForumSearchTermMinimumLength { get; set; }

        //[LocalizedDisplayName(LocalizableStrings.Settings.LayoutPathOverride)]
        public string LayoutPathOverride { get; set; }

        #region ISettings Members

        public string Name
        {
            get { return "Forum Settings"; }
        }

        public string EditorTemplatePath
        {
            get { return "/Plugins/Messaging.Forums/Views/Shared/EditorTemplates/ForumSettings.cshtml"; }
        }

        #endregion ISettings Members
    }
}