using System.Collections.Generic;
using Kore.Collections.Generic;
using Kore.Plugins.Messaging.Forums.Data.Domain;
using Kore.Security.Membership;

namespace Kore.Plugins.Messaging.Forums.Services
{
    public interface IForumService
    {
        void DeleteForumGroup(ForumGroup forumGroup);

        ForumGroup GetForumGroupById(int forumGroupId);

        IEnumerable<ForumGroup> GetAllForumGroups();

        void InsertForumGroup(ForumGroup forumGroup);

        void UpdateForumGroup(ForumGroup forumGroup);

        void DeleteForum(Forum forum);

        Forum GetForumById(int forumId);

        IEnumerable<Forum> GetAllForumsByGroupId(int forumGroupId);

        void InsertForum(Forum forum);

        void UpdateForum(Forum forum);

        void DeleteTopic(ForumTopic forumTopic);

        ForumTopic GetTopicById(int forumTopicId);

        ForumTopic GetTopicById(int forumTopicId, bool increaseViews);

        IPagedList<ForumTopic> GetAllTopics(
            int forumId = 0,
            string userId = null,
            string keywords = null,
            ForumSearchType searchType = ForumSearchType.All,
            int limitDays = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        IPagedList<ForumTopic> GetActiveTopics(
            int forumId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        void InsertTopic(ForumTopic forumTopic, bool sendNotifications);

        void UpdateTopic(ForumTopic forumTopic);

        ForumTopic MoveTopic(int forumTopicId, int newForumId);

        void DeletePost(ForumPost forumPost);

        ForumPost GetPostById(int forumPostId);

        IPagedList<ForumPost> GetAllPosts(
            int forumTopicId = 0,
            string userId = null,
            string keywords = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        IPagedList<ForumPost> GetAllPosts(
            int forumTopicId = 0,
            string userId = null,
            string keywords = null,
            bool ascSort = false,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        void InsertPost(ForumPost forumPost, bool sendNotifications);

        void UpdatePost(ForumPost forumPost);

        void DeletePrivateMessage(PrivateMessage privateMessage);

        PrivateMessage GetPrivateMessageById(int privateMessageId);

        IPagedList<PrivateMessage> GetAllPrivateMessages(
            string fromCustomerId,
            string toCustomerId,
            bool? isRead,
            bool? isDeletedByAuthor,
            bool? isDeletedByRecipient,
            string keywords,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        void InsertPrivateMessage(PrivateMessage privateMessage);

        void UpdatePrivateMessage(PrivateMessage privateMessage);

        void DeleteSubscription(ForumSubscription forumSubscription);

        ForumSubscription GetSubscriptionById(int forumSubscriptionId);

        IPagedList<ForumSubscription> GetAllSubscriptions(
            string userId = null,
            int forumId = 0,
            int topicId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue);

        void InsertSubscription(ForumSubscription forumSubscription);

        void UpdateSubscription(ForumSubscription forumSubscription);

        bool IsUserAllowedToCreateTopic(KoreUser user, Forum forum);

        bool IsUserAllowedToEditTopic(KoreUser user, ForumTopic topic);

        bool IsUserAllowedToMoveTopic(KoreUser user, ForumTopic topic);

        bool IsUserAllowedToDeleteTopic(KoreUser user, ForumTopic topic);

        bool IsUserAllowedToCreatePost(KoreUser user, ForumTopic topic);

        bool IsUserAllowedToEditPost(KoreUser user, ForumPost post);

        bool IsUserAllowedToDeletePost(KoreUser user, ForumPost post);

        bool IsUserAllowedToSetTopicPriority(KoreUser user);

        bool IsUserAllowedToSubscribe(KoreUser user);

        int CalculateTopicPageIndex(int forumTopicId, int pageSize, int postId);
    }
}