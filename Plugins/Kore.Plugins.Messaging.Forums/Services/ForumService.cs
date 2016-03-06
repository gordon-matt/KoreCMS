using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Collections.Generic;
using Kore.Data;
using Kore.Exceptions;
using Kore.Plugins.Messaging.Forums.Data.Domain;
using Kore.Security.Membership;
using Kore.Web.Configuration.Services;
using Kore.Web.Security.Membership;

namespace Kore.Plugins.Messaging.Forums.Services
{
    public class ForumService : IForumService
    {
        #region Private Members

        private readonly ForumSettings forumSettings;
        private readonly ICacheManager cacheManager;
        private readonly IGenericAttributeService genericAttributeService;
        private readonly IMembershipService membershipService;
        private readonly IRepository<Forum> forumRepository;
        private readonly IRepository<ForumGroup> forumGroupRepository;
        private readonly IRepository<ForumPost> forumPostRepository;
        private readonly IRepository<ForumSubscription> forumSubscriptionRepository;
        private readonly IRepository<ForumTopic> forumTopicRepository;
        private readonly IRepository<PrivateMessage> forumPrivateMessageRepository;
        private readonly IWorkContext workContext;

        private const string CacheKey_ForumGroupAll = "Kore.ForumGroup.All";
        private const string CacheKey_ForumAllByForumGroupId = "Kore.Forum.AllByForumGroupId-{0}";
        private const string CacheKey_Pattern_ForumGroup = "Kore.ForumGroup.";
        private const string CacheKey_Pattern_Forum = "Kore.Forum.";

        #endregion Private Members

        #region Ctor

        public ForumService(
            ForumSettings forumSettings,
            ICacheManager cacheManager,
            IGenericAttributeService genericAttributeService,
            IMembershipService membershipService,
            IRepository<Forum> forumRepository,
            IRepository<ForumGroup> forumGroupRepository,
            IRepository<ForumPost> forumPostRepository,
            IRepository<ForumSubscription> forumSubscriptionRepository,
            IRepository<ForumTopic> forumTopicRepository,
            IRepository<PrivateMessage> forumPrivateMessageRepository,
            IWorkContext workContext)
        {
            this.cacheManager = cacheManager;
            this.forumGroupRepository = forumGroupRepository;
            this.forumPostRepository = forumPostRepository;
            this.forumPrivateMessageRepository = forumPrivateMessageRepository;
            this.forumRepository = forumRepository;
            this.forumSettings = forumSettings;
            this.forumSubscriptionRepository = forumSubscriptionRepository;
            this.forumTopicRepository = forumTopicRepository;
            this.genericAttributeService = genericAttributeService;
            this.membershipService = membershipService;
            this.workContext = workContext;
        }

        #endregion Ctor

        #region Methods

        public virtual void DeleteForumGroup(ForumGroup forumGroup)
        {
            if (forumGroup == null)
            {
                throw new ArgumentNullException("forumGroup");
            }

            forumGroupRepository.Delete(forumGroup);

            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityDeleted(forumGroup);
        }

        public virtual ForumGroup GetForumGroupById(int forumGroupId)
        {
            if (forumGroupId == 0)
            {
                return null;
            }

            return forumGroupRepository.FindOne(forumGroupId);
        }

        public virtual IEnumerable<ForumGroup> GetAllForumGroups()
        {
            string key = string.Format(CacheKey_ForumGroupAll);
            return cacheManager.Get(key, () =>
            {
                using (var connection = forumGroupRepository.OpenConnection())
                {
                    var query = from fg in connection.Query()
                                orderby fg.DisplayOrder
                                select fg;

                    return query.ToList();
                }
            });
        }

        public virtual void InsertForumGroup(ForumGroup forumGroup)
        {
            if (forumGroup == null)
            {
                throw new ArgumentNullException("forumGroup");
            }

            forumGroupRepository.Insert(forumGroup);

            //cache
            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityInserted(forumGroup);
        }

        public virtual void UpdateForumGroup(ForumGroup forumGroup)
        {
            if (forumGroup == null)
            {
                throw new ArgumentNullException("forumGroup");
            }

            forumGroupRepository.Update(forumGroup);

            //cache
            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityUpdated(forumGroup);
        }

        public virtual void DeleteForum(Forum forum)
        {
            if (forum == null)
            {
                throw new ArgumentNullException("forum");
            }

            using (var forumTopicConnection = forumTopicRepository.OpenConnection())
            using (var forumSubscriptionConnection = forumSubscriptionRepository.UseConnection(forumTopicConnection))
            {
                //delete forum subscriptions (topics)
                var queryTopicIds = from ft in forumTopicConnection.Query()
                                    where ft.ForumId == forum.Id
                                    select ft.Id;

                var queryFs1 = from fs in forumSubscriptionConnection.Query()
                               where queryTopicIds.Contains(fs.TopicId)
                               select fs;

                foreach (var fs in queryFs1.ToList())
                {
                    forumSubscriptionRepository.Delete(fs);
                    //event notification
                    //_eventPublisher.EntityDeleted(fs);
                }

                //delete forum subscriptions (forum)
                var queryFs2 = from fs in forumSubscriptionConnection.Query()
                               where fs.ForumId == forum.Id
                               select fs;

                foreach (var fs2 in queryFs2.ToList())
                {
                    forumSubscriptionRepository.Delete(fs2);
                    //event notification
                    //_eventPublisher.EntityDeleted(fs2);
                }
            }

            //delete forum
            forumRepository.Delete(forum);

            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityDeleted(forum);
        }

        public virtual Forum GetForumById(int forumId)
        {
            if (forumId == 0)
                return null;

            return forumRepository.FindOne(forumId);
        }

        public virtual IEnumerable<Forum> GetAllForumsByGroupId(int forumGroupId)
        {
            string key = string.Format(CacheKey_ForumAllByForumGroupId, forumGroupId);
            return cacheManager.Get(key, () =>
            {
                using (var connection = forumRepository.OpenConnection())
                {
                    return connection
                        .Query(f => f.ForumGroupId == forumGroupId)
                        .OrderBy(f => f.DisplayOrder)
                        .ToList();
                }
            });
        }

        public virtual void InsertForum(Forum forum)
        {
            if (forum == null)
            {
                throw new ArgumentNullException("forum");
            }

            forumRepository.Insert(forum);

            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityInserted(forum);
        }

        public virtual void UpdateForum(Forum forum)
        {
            if (forum == null)
            {
                throw new ArgumentNullException("forum");
            }

            forumRepository.Update(forum);

            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityUpdated(forum);
        }

        public virtual void DeleteTopic(ForumTopic forumTopic)
        {
            if (forumTopic == null)
            {
                throw new ArgumentNullException("forumTopic");
            }

            string userId = forumTopic.UserId;
            int forumId = forumTopic.ForumId;

            //delete topic
            forumTopicRepository.Delete(forumTopic);

            using (var forumSubscriptionConnection = forumSubscriptionRepository.OpenConnection())
            {
                //delete forum subscriptions
                var queryFs = from ft in forumSubscriptionConnection.Query()
                              where ft.TopicId == forumTopic.Id
                              select ft;

                var forumSubscriptions = queryFs.ToList();
                foreach (var fs in forumSubscriptions)
                {
                    forumSubscriptionRepository.Delete(fs);
                    //event notification
                    //_eventPublisher.EntityDeleted(fs);
                }
            }

            //update stats
            UpdateForumStats(forumId);
            UpdateUserStats(userId);

            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityDeleted(forumTopic);
        }

        public virtual ForumTopic GetTopicById(int forumTopicId)
        {
            return GetTopicById(forumTopicId, false);
        }

        public virtual ForumTopic GetTopicById(int forumTopicId, bool increaseViews)
        {
            if (forumTopicId == 0)
            {
                return null;
            }

            var forumTopic = forumTopicRepository.FindOne(forumTopicId);
            if (forumTopic == null)
            {
                return null;
            }

            if (increaseViews)
            {
                forumTopic.Views = ++forumTopic.Views;
                UpdateTopic(forumTopic);
            }

            return forumTopic;
        }

        public virtual IPagedList<ForumTopic> GetAllTopics(
            int forumId = 0,
            string userId = null,
            string keywords = null,
            ForumSearchType searchType = ForumSearchType.All,
            int limitDays = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            DateTime? limitDate = null;
            if (limitDays > 0)
            {
                limitDate = DateTime.UtcNow.AddDays(-limitDays);
            }
            //we need to cast it to int, otherwise it won't work in SQLCE4
            //we cannot use string.IsNullOrEmpty in query because it causes SqlCeException on SQLCE4
            bool searchKeywords = !string.IsNullOrEmpty(keywords);
            bool searchTopicTitles = searchType == ForumSearchType.All || searchType == ForumSearchType.TopicTitlesOnly;
            bool searchPostText = searchType == ForumSearchType.All || searchType == ForumSearchType.PostTextOnly;

            using (var forumTopicConnection = forumTopicRepository.OpenConnection())
            using (var forumPostConnection = forumPostRepository.UseConnection(forumTopicConnection))
            {
                var query1 = from ft in forumTopicConnection.Query()
                             join fp in forumPostConnection.Query() on ft.Id equals fp.TopicId
                             where
                             (forumId == 0 || ft.ForumId == forumId) &&
                             (userId == null || ft.UserId == userId) &&
                             (
                                !searchKeywords ||
                                (searchTopicTitles && ft.Subject.Contains(keywords)) ||
                                (searchPostText && fp.Text.Contains(keywords))) &&
                             (!limitDate.HasValue || limitDate.Value <= ft.LastPostTime)
                             select ft.Id;

                var query2 = from ft in forumTopicConnection.Query()
                             where query1.Contains(ft.Id)
                             orderby ft.TopicType descending, ft.LastPostTime descending, ft.Id descending
                             select ft;

                var topics = new PagedList<ForumTopic>(query2, pageIndex, pageSize);
                return topics;
            }
        }

        public virtual IPagedList<ForumTopic> GetActiveTopics(
            int forumId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            using (var forumTopicConnection = forumTopicRepository.OpenConnection())
            {
                var query1 = from ft in forumTopicConnection.Query()
                             where
                             (forumId == 0 || ft.ForumId == forumId) &&
                             (ft.LastPostTime.HasValue)
                             select ft.Id;

                var query2 = from ft in forumTopicConnection.Query()
                             where query1.Contains(ft.Id)
                             orderby ft.LastPostTime descending
                             select ft;

                var topics = new PagedList<ForumTopic>(query2, pageIndex, pageSize);
                return topics;
            }
        }

        public virtual void InsertTopic(ForumTopic forumTopic, bool sendNotifications)
        {
            if (forumTopic == null)
            {
                throw new ArgumentNullException("forumTopic");
            }

            forumTopicRepository.Insert(forumTopic);

            //update stats
            UpdateForumStats(forumTopic.ForumId);

            //cache
            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityInserted(forumTopic);

            //send notifications
            if (sendNotifications)
            {
                var forum = forumTopic.Forum;
                var subscriptions = GetAllSubscriptions(forumId: forum.Id);
                //var languageId = workContext.WorkingLanguage.Id;

                foreach (var subscription in subscriptions)
                {
                    if (subscription.UserId == forumTopic.UserId)
                    {
                        continue;
                    }

                    //if (!string.IsNullOrEmpty(subscription.User.Email))
                    //{
                    //    //_workflowMessageService.SendNewForumTopicMessage(subscription.User, forumTopic,
                    //    //    forum, languageId);
                    //}
                }
            }
        }

        public virtual void UpdateTopic(ForumTopic forumTopic)
        {
            if (forumTopic == null)
            {
                throw new ArgumentNullException("forumTopic");
            }

            forumTopicRepository.Update(forumTopic);

            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityUpdated(forumTopic);
        }

        public virtual ForumTopic MoveTopic(int forumTopicId, int newForumId)
        {
            var forumTopic = GetTopicById(forumTopicId);
            if (forumTopic == null)
                return null;

            if (this.IsUserAllowedToMoveTopic(workContext.CurrentUser, forumTopic))
            {
                int previousForumId = forumTopic.ForumId;
                var newForum = GetForumById(newForumId);

                if (newForum != null)
                {
                    if (previousForumId != newForumId)
                    {
                        forumTopic.ForumId = newForum.Id;
                        forumTopic.UpdatedOnUtc = DateTime.UtcNow;
                        UpdateTopic(forumTopic);

                        //update forum stats
                        UpdateForumStats(previousForumId);
                        UpdateForumStats(newForumId);
                    }
                }
            }
            return forumTopic;
        }

        public virtual void DeletePost(ForumPost forumPost)
        {
            if (forumPost == null)
            {
                throw new ArgumentNullException("forumPost");
            }

            int forumTopicId = forumPost.TopicId;
            string userId = forumPost.UserId;
            var forumTopic = this.GetTopicById(forumTopicId);
            int forumId = forumTopic.ForumId;

            //delete topic if it was the first post
            bool deleteTopic = false;
            ForumPost firstPost = forumTopic.GetFirstPost(this);
            if (firstPost != null && firstPost.Id == forumPost.Id)
            {
                deleteTopic = true;
            }

            //delete forum post
            forumPostRepository.Delete(forumPost);

            //delete topic
            if (deleteTopic)
            {
                DeleteTopic(forumTopic);
            }

            //update stats
            if (!deleteTopic)
            {
                UpdateForumTopicStats(forumTopicId);
            }
            UpdateForumStats(forumId);
            UpdateUserStats(userId);

            //clear cache
            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityDeleted(forumPost);
        }

        public virtual ForumPost GetPostById(int forumPostId)
        {
            if (forumPostId == 0)
            {
                return null;
            }

            return forumPostRepository.FindOne(forumPostId);
        }

        public virtual IPagedList<ForumPost> GetAllPosts(
            int forumTopicId = 0,
            string userId = null,
            string keywords = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            return GetAllPosts(forumTopicId, userId, keywords, true, pageIndex, pageSize);
        }

        public virtual IPagedList<ForumPost> GetAllPosts(
            int forumTopicId = 0,
            string userId = null,
            string keywords = null,
            bool ascSort = false,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            using (var forumPostConnection = forumPostRepository.OpenConnection())
            {
                var query = forumPostConnection.Query();
                if (forumTopicId > 0)
                {
                    query = query.Where(fp => forumTopicId == fp.TopicId);
                }
                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(fp => userId == fp.UserId);
                }
                if (!string.IsNullOrEmpty(keywords))
                {
                    query = query.Where(fp => fp.Text.Contains(keywords));
                }

                query = ascSort ?
                    query.OrderBy(fp => fp.CreatedOnUtc).ThenBy(fp => fp.Id) :
                    query.OrderByDescending(fp => fp.CreatedOnUtc).ThenBy(fp => fp.Id);

                var forumPosts = new PagedList<ForumPost>(query, pageIndex, pageSize);
                return forumPosts;
            }
        }

        public virtual void InsertPost(ForumPost forumPost, bool sendNotifications)
        {
            if (forumPost == null)
            {
                throw new ArgumentNullException("forumPost");
            }

            forumPostRepository.Insert(forumPost);

            //update stats
            string userId = forumPost.UserId;
            var forumTopic = this.GetTopicById(forumPost.TopicId);
            int forumId = forumTopic.ForumId;
            UpdateForumTopicStats(forumPost.TopicId);
            UpdateForumStats(forumId);
            UpdateUserStats(userId);

            //clear cache
            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityInserted(forumPost);

            //notifications
            if (sendNotifications)
            {
                var forum = forumTopic.Forum;
                var subscriptions = GetAllSubscriptions(topicId: forumTopic.Id);

                //var languageId = workContext.WorkingLanguage.Id;

                int friendlyTopicPageIndex = CalculateTopicPageIndex(forumPost.TopicId,
                    forumSettings.PostsPageSize > 0 ? forumSettings.PostsPageSize : 10,
                    forumPost.Id) + 1;

                foreach (ForumSubscription subscription in subscriptions)
                {
                    if (subscription.UserId == forumPost.UserId)
                    {
                        continue;
                    }

                    //if (!string.IsNullOrEmpty(subscription.User.Email))
                    //{
                    //    //_workflowMessageService.SendNewForumPostMessage(subscription.User, forumPost,
                    //    //    forumTopic, forum, friendlyTopicPageIndex, languageId);
                    //}
                }
            }
        }

        public virtual void UpdatePost(ForumPost forumPost)
        {
            //validation
            if (forumPost == null)
            {
                throw new ArgumentNullException("forumPost");
            }

            forumPostRepository.Update(forumPost);

            cacheManager.RemoveByPattern(CacheKey_Pattern_ForumGroup);
            cacheManager.RemoveByPattern(CacheKey_Pattern_Forum);

            //event notification
            //_eventPublisher.EntityUpdated(forumPost);
        }

        public virtual void DeletePrivateMessage(PrivateMessage privateMessage)
        {
            if (privateMessage == null)
            {
                throw new ArgumentNullException("privateMessage");
            }

            forumPrivateMessageRepository.Delete(privateMessage);

            //event notification
            //_eventPublisher.EntityDeleted(privateMessage);
        }

        public virtual PrivateMessage GetPrivateMessageById(int privateMessageId)
        {
            if (privateMessageId == 0)
            {
                return null;
            }

            return forumPrivateMessageRepository.FindOne(privateMessageId);
        }

        public virtual IPagedList<PrivateMessage> GetAllPrivateMessages(
            string fromUserId,
            string toUserId,
            bool? isRead,
            bool? isDeletedByAuthor,
            bool? isDeletedByRecipient,
            string keywords,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            using (var forumPrivateMessageConnection = forumPrivateMessageRepository.OpenConnection())
            {
                var query = forumPrivateMessageConnection.Query();
                //if (storeId > 0)
                //    query = query.Where(pm => storeId == pm.StoreId);
                if (!string.IsNullOrEmpty(fromUserId))
                {
                    query = query.Where(pm => fromUserId == pm.FromUserId);
                }
                if (!string.IsNullOrEmpty(toUserId))
                {
                    query = query.Where(pm => toUserId == pm.ToUserId);
                }
                if (isRead.HasValue)
                {
                    query = query.Where(pm => isRead.Value == pm.IsRead);
                }
                if (isDeletedByAuthor.HasValue)
                {
                    query = query.Where(pm => isDeletedByAuthor.Value == pm.IsDeletedByAuthor);
                }
                if (isDeletedByRecipient.HasValue)
                {
                    query = query.Where(pm => isDeletedByRecipient.Value == pm.IsDeletedByRecipient);
                }
                if (!string.IsNullOrEmpty(keywords))
                {
                    query = query.Where(pm => pm.Subject.Contains(keywords));
                    query = query.Where(pm => pm.Text.Contains(keywords));
                }
                query = query.OrderByDescending(pm => pm.CreatedOnUtc);

                var privateMessages = new PagedList<PrivateMessage>(query, pageIndex, pageSize);
                return privateMessages;
            }
        }

        public virtual void InsertPrivateMessage(PrivateMessage privateMessage)
        {
            if (privateMessage == null)
            {
                throw new ArgumentNullException("privateMessage");
            }

            forumPrivateMessageRepository.Insert(privateMessage);

            //event notification
            //_eventPublisher.EntityInserted(privateMessage);

            var userTo = membershipService.GetUserById(privateMessage.ToUserId);
            if (userTo == null)
            {
                throw new KoreException("Recipient could not be loaded");
            }

            //UI notification
            genericAttributeService.SaveAttribute(userTo, SystemUserAttributeNames.NotifiedAboutNewPrivateMessages, false);

            //Email notification
            if (forumSettings.NotifyAboutPrivateMessages)
            {
                //_workflowMessageService.SendPrivateMessageNotification(privateMessage, workContext.WorkingLanguage.Id);
            }
        }

        public virtual void UpdatePrivateMessage(PrivateMessage privateMessage)
        {
            if (privateMessage == null)
            {
                throw new ArgumentNullException("privateMessage");
            }

            if (privateMessage.IsDeletedByAuthor && privateMessage.IsDeletedByRecipient)
            {
                forumPrivateMessageRepository.Delete(privateMessage);
                //event notification
                //_eventPublisher.EntityDeleted(privateMessage);
            }
            else
            {
                forumPrivateMessageRepository.Update(privateMessage);
                //event notification
                //_eventPublisher.EntityUpdated(privateMessage);
            }
        }

        public virtual void DeleteSubscription(ForumSubscription forumSubscription)
        {
            if (forumSubscription == null)
            {
                throw new ArgumentNullException("forumSubscription");
            }

            forumSubscriptionRepository.Delete(forumSubscription);

            //event notification
            //_eventPublisher.EntityDeleted(forumSubscription);
        }

        public virtual ForumSubscription GetSubscriptionById(int forumSubscriptionId)
        {
            if (forumSubscriptionId == 0)
            {
                return null;
            }

            return forumSubscriptionRepository.FindOne(forumSubscriptionId);
        }

        public virtual IPagedList<ForumSubscription> GetAllSubscriptions(
            string userId = null,
            int forumId = 0,
            int topicId = 0,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            bool isLockedOut = string.IsNullOrEmpty(userId)
                ? false
                : membershipService.GetUserById(userId).IsLockedOut;

            using (var forumSubscriptionConnection = forumSubscriptionRepository.OpenConnection())
            {
                var fsQuery = from fs in forumSubscriptionConnection.Query()
                              where
                                  (userId == null || fs.UserId == userId) &&
                                  (forumId == 0 || fs.ForumId == forumId) &&
                                  (topicId == 0 || fs.TopicId == topicId) &&
                                  (!isLockedOut)
                              select fs.Id;

                var query = from fs in forumSubscriptionConnection.Query()
                            where fsQuery.Contains(fs.Id)
                            orderby fs.CreatedOnUtc descending, fs.Id descending
                            select fs;

                var forumSubscriptions = new PagedList<ForumSubscription>(query, pageIndex, pageSize);
                return forumSubscriptions;
            }
        }

        public virtual void InsertSubscription(ForumSubscription forumSubscription)
        {
            if (forumSubscription == null)
            {
                throw new ArgumentNullException("forumSubscription");
            }

            forumSubscriptionRepository.Insert(forumSubscription);

            //event notification
            //_eventPublisher.EntityInserted(forumSubscription);
        }

        public virtual void UpdateSubscription(ForumSubscription forumSubscription)
        {
            if (forumSubscription == null)
            {
                throw new ArgumentNullException("forumSubscription");
            }

            forumSubscriptionRepository.Update(forumSubscription);

            //event notification
            //_eventPublisher.EntityUpdated(forumSubscription);
        }

        public virtual bool IsUserAllowedToCreateTopic(KoreUser user, Forum forum)
        {
            if (forum == null)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            //if (user.IsGuest() && !forumSettings.AllowGuestsToCreateTopics)
            //{
            //    return false;
            //}

            if (IsForumModerator(user))
            {
                return true;
            }

            return true;
        }

        public virtual bool IsUserAllowedToEditTopic(KoreUser user, ForumTopic topic)
        {
            if (topic == null)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            //if (user.IsGuest())
            //{
            //    return false;
            //}

            if (IsForumModerator(user))
            {
                return true;
            }

            if (forumSettings.AllowUsersToEditPosts)
            {
                bool ownTopic = user.Id == topic.UserId;
                return ownTopic;
            }

            return false;
        }

        public virtual bool IsUserAllowedToMoveTopic(KoreUser user, ForumTopic topic)
        {
            if (topic == null)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            //if (user.IsGuest())
            //{
            //    return false;
            //}

            if (IsForumModerator(user))
            {
                return true;
            }

            return false;
        }

        public virtual bool IsUserAllowedToDeleteTopic(KoreUser user, ForumTopic topic)
        {
            if (topic == null)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            //if (user.IsGuest())
            //{
            //    return false;
            //}

            if (IsForumModerator(user))
            {
                return true;
            }

            if (forumSettings.AllowUsersToDeletePosts)
            {
                bool ownTopic = user.Id == topic.UserId;
                return ownTopic;
            }

            return false;
        }

        public virtual bool IsUserAllowedToCreatePost(KoreUser user, ForumTopic topic)
        {
            if (topic == null)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            //if (user.IsGuest() && !forumSettings.AllowGuestsToCreatePosts)
            //{
            //    return false;
            //}

            return true;
        }

        public virtual bool IsUserAllowedToEditPost(KoreUser user, ForumPost post)
        {
            if (post == null)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            //if (user.IsGuest())
            //{
            //    return false;
            //}

            if (IsForumModerator(user))
            {
                return true;
            }

            if (forumSettings.AllowUsersToEditPosts)
            {
                bool ownPost = user.Id == post.UserId;
                return ownPost;
            }

            return false;
        }

        public virtual bool IsUserAllowedToDeletePost(KoreUser user, ForumPost post)
        {
            if (post == null)
            {
                return false;
            }

            if (user == null)
            {
                return false;
            }

            //if (user.IsGuest())
            //{
            //    return false;
            //}

            if (IsForumModerator(user))
            {
                return true;
            }

            if (forumSettings.AllowUsersToDeletePosts)
            {
                bool ownPost = user.Id == post.UserId;
                return ownPost;
            }

            return false;
        }

        public virtual bool IsUserAllowedToSetTopicPriority(KoreUser user)
        {
            if (user == null)
            {
                return false;
            }

            //if (user.IsGuest())
            //{
            //    return false;
            //}

            if (IsForumModerator(user))
            {
                return true;
            }

            return false;
        }

        public virtual bool IsUserAllowedToSubscribe(KoreUser user)
        {
            if (user == null)
            {
                return false;
            }

            //if (user.IsGuest())
            //{
            //    return false;
            //}

            return true;
        }

        public virtual int CalculateTopicPageIndex(int forumTopicId, int pageSize, int postId)
        {
            int pageIndex = 0;
            var forumPosts = GetAllPosts(forumTopicId: forumTopicId, ascSort: true);

            for (int i = 0; i < forumPosts.ItemCount; i++)
            {
                if (forumPosts[i].Id == postId)
                {
                    if (pageSize > 0)
                    {
                        pageIndex = i / pageSize;
                    }
                }
            }

            return pageIndex;
        }

        #endregion Methods

        #region Utilities

        private void UpdateForumStats(int forumId)
        {
            if (forumId == 0)
            {
                return;
            }
            var forum = GetForumById(forumId);
            if (forum == null)
            {
                return;
            }

            using (var forumTopicConnection = forumTopicRepository.OpenConnection())
            using (var forumPostConnection = forumPostRepository.UseConnection(forumTopicConnection))
            {
                //number of topics
                var queryNumTopics = forumTopicConnection.Query(x => x.ForumId == forumId).Select(x => x.Id);

                int numTopics = queryNumTopics.Count();

                //number of posts
                var queryNumPosts = from ft in forumTopicConnection.Query()
                                    join fp in forumPostConnection.Query() on ft.Id equals fp.TopicId
                                    where ft.ForumId == forumId
                                    select fp.Id;

                int numPosts = queryNumPosts.Count();

                //last values
                int lastTopicId = 0;
                int lastPostId = 0;
                string lastPostUserId = null;
                DateTime? lastPostTime = null;

                var queryLastValues = from ft in forumTopicConnection.Query()
                                      join fp in forumPostConnection.Query() on ft.Id equals fp.TopicId
                                      where ft.ForumId == forumId
                                      orderby fp.CreatedOnUtc descending, ft.CreatedOnUtc descending
                                      select new
                                      {
                                          LastTopicId = ft.Id,
                                          LastPostId = fp.Id,
                                          LastPostUserId = fp.UserId,
                                          LastPostTime = fp.CreatedOnUtc
                                      };

                var lastValues = queryLastValues.FirstOrDefault();

                if (lastValues != null)
                {
                    lastTopicId = lastValues.LastTopicId;
                    lastPostId = lastValues.LastPostId;
                    lastPostUserId = lastValues.LastPostUserId;
                    lastPostTime = lastValues.LastPostTime;
                }

                //update forum
                forum.NumTopics = numTopics;
                forum.NumPosts = numPosts;
                forum.LastTopicId = lastTopicId;
                forum.LastPostId = lastPostId;
                forum.LastPostUserId = lastPostUserId;
                forum.LastPostTime = lastPostTime;
            }

            UpdateForum(forum);
        }

        private void UpdateForumTopicStats(int forumTopicId)
        {
            if (forumTopicId == 0)
            {
                return;
            }
            var forumTopic = GetTopicById(forumTopicId);
            if (forumTopic == null)
            {
                return;
            }

            using (var forumPostConnection = forumPostRepository.OpenConnection())
            {
                //number of posts
                var queryNumPosts = from fp in forumPostConnection.Query()
                                    where fp.TopicId == forumTopicId
                                    select fp.Id;

                int numPosts = queryNumPosts.Count();

                //last values
                int lastPostId = 0;
                string lastPostUserId = null;
                DateTime? lastPostTime = null;

                var queryLastValues = from fp in forumPostConnection.Query()
                                      where fp.TopicId == forumTopicId
                                      orderby fp.CreatedOnUtc descending
                                      select new
                                      {
                                          LastPostId = fp.Id,
                                          LastPostUserId = fp.UserId,
                                          LastPostTime = fp.CreatedOnUtc
                                      };

                var lastValues = queryLastValues.FirstOrDefault();
                if (lastValues != null)
                {
                    lastPostId = lastValues.LastPostId;
                    lastPostUserId = lastValues.LastPostUserId;
                    lastPostTime = lastValues.LastPostTime;
                }

                //update topic
                forumTopic.NumPosts = numPosts;
                forumTopic.LastPostId = lastPostId;
                forumTopic.LastPostUserId = lastPostUserId;
                forumTopic.LastPostTime = lastPostTime;
            }

            UpdateTopic(forumTopic);
        }

        private void UpdateUserStats(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            var user = membershipService.GetUserById(userId);

            if (user == null)
            {
                return;
            }

            using (var forumPostConnection = forumPostRepository.OpenConnection())
            {
                var query = from fp in forumPostConnection.Query()
                            where fp.UserId == userId
                            select fp.Id;

                int numPosts = query.Count();

                genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.ForumPostCount, numPosts);
            }
        }

        private bool IsForumModerator(KoreUser user)
        {
            var roles = membershipService.GetRolesForUser(user.Id);
            return roles.Any(x => x.Name == Constants.Roles.ForumModerators);
        }

        #endregion Utilities
    }
}