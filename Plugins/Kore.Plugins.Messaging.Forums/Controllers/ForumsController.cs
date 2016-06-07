using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using Kore.Exceptions;
using Kore.Localization;
using Kore.Plugins.Messaging.Forums.Data.Domain;
using Kore.Plugins.Messaging.Forums.Extensions;
using Kore.Plugins.Messaging.Forums.Models;
using Kore.Plugins.Messaging.Forums.Services;
using Kore.Security.Membership;
using Kore.Web;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Kore.Web.Configuration;
using Kore.Web.Configuration.Domain;
using Kore.Web.Helpers;
using Kore.Web.Html;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Filters;
using Kore.Web.Security.Membership;

namespace Kore.Plugins.Messaging.Forums.Controllers
{
    [Authorize]
    [RouteArea("")]
    [RoutePrefix("forums")]
    public class ForumsController : KoreController
    {
        #region Fields

        private readonly IForumService forumService;

        //private readonly IPictureService _pictureService;
        private readonly IRegionService regionService;

        private readonly IWebHelper webHelper;
        private readonly ForumSettings forumSettings;

        //private readonly UserSettings _customerSettings;
        //private readonly MediaSettings _mediaSettings;
        private readonly IDateTimeHelper dateTimeHelper;

        private readonly IMembershipService membershipService;
        private readonly KoreSiteSettings siteSettings;

        #endregion Fields

        #region Constructors

        public ForumsController(
            IForumService forumService,
            //IPictureService pictureService,
            IRegionService regionService,
            IWebHelper webHelper,
            ForumSettings forumSettings,
            //UserSettings customerSettings,
            //MediaSettings mediaSettings,
            IDateTimeHelper dateTimeHelper,
            IMembershipService membershipService,
            KoreSiteSettings siteSettings)
        {
            this.forumService = forumService;
            //this._pictureService = pictureService;
            this.regionService = regionService;
            this.webHelper = webHelper;
            this.forumSettings = forumSettings;
            //this._customerSettings = customerSettings;
            //this._mediaSettings = mediaSettings;
            this.dateTimeHelper = dateTimeHelper;
            this.membershipService = membershipService;
            this.siteSettings = siteSettings;
        }

        #endregion Constructors

        #region Utilities

        [NonAction]
        protected virtual ForumTopicRowModel PrepareForumTopicRowModel(ForumTopic topic)
        {
            var user = membershipService.GetUserById(topic.UserId);

            var topicModel = new ForumTopicRowModel
            {
                Id = topic.Id,
                Subject = topic.Subject,
                SeName = topic.GetSeName(),
                LastPostId = topic.LastPostId,
                NumPosts = topic.NumPosts,
                Views = topic.Views,
                NumReplies = topic.NumReplies,
                TopicType = topic.TopicType,
                UserId = topic.UserId,
                //AllowViewingProfiles = _customerSettings.AllowViewingProfiles, //TODO
                AllowViewingProfiles = true,
                UserName = membershipService.GetUserDisplayName(user)
            };

            var posts = forumService.GetAllPosts(topic.Id, null, string.Empty, 1, forumSettings.PostsPageSize);
            topicModel.TotalPostPages = posts.ItemCount;

            return topicModel;
        }

        [NonAction]
        protected virtual ForumRowModel PrepareForumRowModel(Forum forum)
        {
            var forumRowModel = new ForumRowModel
            {
                Id = forum.Id,
                Name = forum.Name,
                SeName = forum.GetSeName(),
                Description = forum.Description,
                NumTopics = forum.NumTopics,
                NumPosts = forum.NumPosts,
                LastPostId = forum.LastPostId,
            };
            return forumRowModel;
        }

        [NonAction]
        protected virtual ForumGroupModel PrepareForumGroupModel(ForumGroup forumGroup)
        {
            var forumGroupModel = new ForumGroupModel
            {
                Id = forumGroup.Id,
                Name = forumGroup.Name,
                SeName = forumGroup.GetSeName(),
            };
            var forums = forumService.GetAllForumsByGroupId(forumGroup.Id);
            foreach (var forum in forums)
            {
                var forumModel = PrepareForumRowModel(forum);
                forumGroupModel.Forums.Add(forumModel);
            }
            return forumGroupModel;
        }

        [NonAction]
        protected virtual IEnumerable<SelectListItem> ForumTopicTypesList()
        {
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Text = T(LocalizableStrings.TopicTypes.Normal),
                Value = ((int)ForumTopicType.Normal).ToString()
            });

            list.Add(new SelectListItem
            {
                Text = T(LocalizableStrings.TopicTypes.Sticky),
                Value = ((int)ForumTopicType.Sticky).ToString()
            });

            list.Add(new SelectListItem
            {
                Text = T(LocalizableStrings.TopicTypes.Announcement),
                Value = ((int)ForumTopicType.Announcement).ToString()
            });

            return list;
        }

        [NonAction]
        protected virtual IEnumerable<SelectListItem> ForumGroupsForumsList()
        {
            var forumsSelectList = new List<SelectListItem>();
            var separator = "--";
            var groups = forumService.GetAllForumGroups();

            foreach (var group in groups)
            {
                // Add the forum group with Value of 0 so it won't be used as a target forum
                forumsSelectList.Add(new SelectListItem { Text = group.Name, Value = "0" });

                var forums = forumService.GetAllForumsByGroupId(group.Id);
                foreach (var f in forums)
                {
                    forumsSelectList.Add(new SelectListItem { Text = string.Format("{0}{1}", separator, f.Name), Value = f.Id.ToString() });
                }
            }

            return forumsSelectList;
        }

        [NonAction]
        private bool IsForumModerator(KoreUser user)
        {
            var roles = membershipService.GetRolesForUser(user.Id);
            return roles.Any(x => x.Name == Constants.Roles.ForumModerators);
        }

        #endregion Utilities

        #region Methods

        [Route("")]
        public ActionResult Index()
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var groups = forumService.GetAllForumGroups();

            var model = new IndexModel();
            foreach (var group in groups)
            {
                var groupModel = PrepareForumGroupModel(group);
                model.ForumGroups.Add(groupModel);
            }
            return View(model);
        }

        [Route("active-discussions")]
        [Route("active-discussions/{forumId}/{page}")]
        public ActionResult ActiveDiscussions(int forumId = 0, int page = 1)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            int pageSize = forumSettings.ActiveDiscussionsPageSize > 0 ? forumSettings.ActiveDiscussionsPageSize : 50;
            var topics = forumService.GetActiveTopics(forumId, (page - 1), pageSize);

            var model = new ActiveDiscussionsModel
            {
                TopicPageSize = topics.PageSize,
                TopicTotalRecords = topics.ItemCount,
                TopicPageIndex = topics.PageIndex,
                ViewAllLinkEnabled = false,
                ActiveDiscussionsFeedEnabled = forumSettings.ActiveDiscussionsFeedEnabled,
                PostsPageSize = forumSettings.PostsPageSize
            };

            foreach (var topic in topics)
            {
                var topicModel = PrepareForumTopicRowModel(topic);
                model.ForumTopics.Add(topicModel);
            }

            return View(model);
        }

        [ChildActionOnly]
        [Route("active-discussions/small")]
        public ActionResult ActiveDiscussionsSmall()
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var topics = forumService.GetActiveTopics(0, 0, forumSettings.HomePageActiveDiscussionsTopicCount);
            if (topics.Count == 0)
            {
                return Content(string.Empty);
            }

            var model = new ActiveDiscussionsModel
            {
                ViewAllLinkEnabled = true,
                ActiveDiscussionsFeedEnabled = forumSettings.ActiveDiscussionsFeedEnabled,
                PostsPageSize = forumSettings.PostsPageSize
            };

            foreach (var topic in topics)
            {
                var topicModel = PrepareForumTopicRowModel(topic);
                model.ForumTopics.Add(topicModel);
            }

            return PartialView(model);
        }

        [Route("active-discussions/rss")]
        [Route("active-discussions/rss/{forumId}")]
        public ActionResult ActiveDiscussionsRss(int forumId = 0)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            if (!forumSettings.ActiveDiscussionsFeedEnabled)
            {
                return RedirectToAction("Index");
            }

            var topics = forumService.GetActiveTopics(forumId, 0, forumSettings.ActiveDiscussionsFeedCount);
            string url = Url.Action("ActiveDiscussionsRSS", "Forums", null, "http");

            string feedTitle = T(LocalizableStrings.ActiveDiscussionsFeedTitle);
            string feedDescription = T(LocalizableStrings.ActiveDiscussionsFeedDescription);

            var feed = new SyndicationFeed(
                string.Format(feedTitle, siteSettings.SiteName),
                feedDescription,
                new Uri(url),
                "ActiveDiscussionsRSS",
                DateTime.UtcNow);

            var items = new List<SyndicationItem>();

            string viewsText = T(LocalizableStrings.Views);
            string repliesText = T(LocalizableStrings.Replies);

            foreach (var topic in topics)
            {
                string topicUrl = Url.Action("Topic", "Forums", new { id = topic.Id, slug = topic.GetSeName() }, "http");

                string content = string.Format(
                    "{2}: {0}, {3}: {1}",
                    topic.NumReplies.ToString(),
                    topic.Views.ToString(),
                    repliesText,
                    viewsText);

                items.Add(new SyndicationItem(
                    topic.Subject,
                    content,
                    new Uri(topicUrl),
                    string.Format("Topic:{0}", topic.Id), (topic.LastPostTime ?? topic.UpdatedOnUtc)));
            }
            feed.Items = items;

            return new RssActionResult { Feed = feed };
        }

        [Route("forum-group/{id}/{slug}")]
        public ActionResult ForumGroup(int id)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var group = forumService.GetForumGroupById(id);

            if (group == null)
            {
                return RedirectToAction("Index");
            }

            var model = PrepareForumGroupModel(group);
            return View(model);
        }

        [Route("forum/{id}/{slug}")]
        [Route("forum/{id}/{slug}/page/{page}")]
        public ActionResult Forum(int id, int page = 1)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var forum = forumService.GetForumById(id);

            if (forum != null)
            {
                int pageSize = forumSettings.TopicsPageSize > 0 ? forumSettings.TopicsPageSize : 10;
                var topics = forumService.GetAllTopics(forum.Id, null, string.Empty, ForumSearchType.All, 0, (page - 1), pageSize);

                var model = new ForumPageModel
                {
                    Id = forum.Id,
                    Name = forum.Name,
                    SeName = forum.GetSeName(),
                    Description = forum.Description,
                    TopicPageSize = topics.PageSize,
                    TopicTotalRecords = topics.ItemCount,
                    TopicPageIndex = topics.PageIndex,
                    IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser),
                    ForumFeedsEnabled = forumSettings.ForumFeedsEnabled,
                    PostsPageSize = forumSettings.PostsPageSize
                };

                foreach (var topic in topics)
                {
                    var topicModel = PrepareForumTopicRowModel(topic);
                    model.ForumTopics.Add(topicModel);
                }

                //subscription
                if (forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser))
                {
                    model.WatchForumText = T(LocalizableStrings.WatchForum);

                    var forumSubscription = forumService.GetAllSubscriptions(WorkContext.CurrentUser.Id, forum.Id, 0, 0, 1).FirstOrDefault();
                    if (forumSubscription != null)
                    {
                        model.WatchForumText = T(LocalizableStrings.UnwatchForum);
                    }
                }

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [Route("forum/rss/{id}")]
        public ActionResult ForumRss(int id)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            if (!forumSettings.ForumFeedsEnabled)
            {
                return RedirectToAction("Index");
            }

            int topicLimit = forumSettings.ForumFeedCount;
            var forum = forumService.GetForumById(id);

            if (forum != null)
            {
                //Order by newest topic posts & limit the number of topics to return
                var topics = forumService.GetAllTopics(forum.Id, null, string.Empty, ForumSearchType.All, 0, 0, topicLimit);

                string url = Url.Action("ForumRSS", "Forums", new { id = forum.Id }, "http");

                string feedTitle = T(LocalizableStrings.ForumFeedTitle);
                string feedDescription = T(LocalizableStrings.ForumFeedDescription);

                var feed = new SyndicationFeed(
                    string.Format(feedTitle, siteSettings.SiteName, forum.Name),
                    feedDescription,
                    new Uri(url),
                    string.Format("ForumRSS:{0}", forum.Id),
                    DateTime.UtcNow);

                var items = new List<SyndicationItem>();

                string viewsText = T(LocalizableStrings.Views);
                string repliesText = T(LocalizableStrings.Replies);

                foreach (var topic in topics)
                {
                    string topicUrl = Url.Action("Topic", "Forums", new { id = topic.Id, slug = topic.GetSeName() }, "http");

                    string content = string.Format(
                        "{2}: {0}, {3}: {1}",
                        topic.NumReplies.ToString(),
                        topic.Views.ToString(),
                        repliesText,
                        viewsText);

                    items.Add(new SyndicationItem(
                        topic.Subject,
                        content,
                        new Uri(topicUrl),
                        string.Format("Topic:{0}", topic.Id),
                        (topic.LastPostTime ?? topic.UpdatedOnUtc)));
                }

                feed.Items = items;

                return new RssActionResult { Feed = feed };
            }

            return new RssActionResult { Feed = new SyndicationFeed() };
        }

        [HttpPost]
        [Route("forum/watch/{id}")]
        public ActionResult ForumWatch(int id)
        {
            string watchTopic = T(LocalizableStrings.WatchForum);
            string unwatchTopic = T(LocalizableStrings.UnwatchForum);
            string returnText = watchTopic;

            var forum = forumService.GetForumById(id);
            if (forum == null)
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            if (!forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser))
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            var subscription = forumService
                .GetAllSubscriptions(WorkContext.CurrentUser.Id, forum.Id, 0, 0, 1)
                .FirstOrDefault();

            bool subscribed;
            if (subscription == null)
            {
                subscription = new ForumSubscription
                {
                    //SubscriptionGuid = Guid.NewGuid(),
                    UserId = WorkContext.CurrentUser.Id,
                    ForumId = forum.Id,
                    CreatedOnUtc = DateTime.UtcNow
                };
                forumService.InsertSubscription(subscription);
                subscribed = true;
                returnText = unwatchTopic;
            }
            else
            {
                forumService.DeleteSubscription(subscription);
                subscribed = false;
            }

            return Json(new { Subscribed = subscribed, Text = returnText, Error = false });
        }

        [Route("topic/{id}/{slug}")]
        [Route("topic/{id}/{slug}/page/{page}")]
        public ActionResult Topic(int id, int page = 1)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var topic = forumService.GetTopicById(id);

            if (topic != null)
            {
                //load posts
                var posts = forumService.GetAllPosts(topic.Id, null, string.Empty, page - 1, forumSettings.PostsPageSize);

                //if not posts loaded, redirect to the first page
                if (posts.Count == 0 && page > 1)
                {
                    return RedirectToAction("Topic", new { id = topic.Id, slug = topic.GetSeName() });
                }

                //update view count
                topic.Views += 1;
                forumService.UpdateTopic(topic);

                //prepare model
                var model = new ForumTopicPageModel
                {
                    Id = topic.Id,
                    Subject = topic.Subject,
                    SeName = topic.GetSeName(),
                    IsUserAllowedToEditTopic = forumService.IsUserAllowedToEditTopic(WorkContext.CurrentUser, topic),
                    IsUserAllowedToDeleteTopic = forumService.IsUserAllowedToDeleteTopic(WorkContext.CurrentUser, topic),
                    IsUserAllowedToMoveTopic = forumService.IsUserAllowedToMoveTopic(WorkContext.CurrentUser, topic),
                    IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser),
                    PostsPageIndex = posts.PageIndex,
                    PostsPageSize = posts.PageSize,
                    PostsTotalRecords = posts.ItemCount
                };

                if (model.IsUserAllowedToSubscribe)
                {
                    model.WatchTopicText = T(LocalizableStrings.WatchTopic);

                    var forumTopicSubscription = forumService
                        .GetAllSubscriptions(WorkContext.CurrentUser.Id, 0, topic.Id, 0, 1)
                        .FirstOrDefault();

                    if (forumTopicSubscription != null)
                    {
                        model.WatchTopicText = T(LocalizableStrings.UnwatchTopic);
                    }
                }

                foreach (var post in posts)
                {
                    var postUser = membershipService.GetUserById(post.UserId);

                    var postModel = new ForumPostModel
                    {
                        Id = post.Id,
                        ForumTopicId = post.TopicId,
                        ForumTopicSeName = topic.GetSeName(),
                        FormattedText = post.FormatPostText(),
                        IsCurrentUserAllowedToEditPost = forumService.IsUserAllowedToEditPost(WorkContext.CurrentUser, post),
                        IsCurrentUserAllowedToDeletePost = forumService.IsUserAllowedToDeletePost(WorkContext.CurrentUser, post),
                        UserId = post.UserId,
                        //AllowViewingProfiles = _customerSettings.AllowViewingProfiles, //TODO
                        AllowViewingProfiles = true,
                        UserName = membershipService.GetUserDisplayName(postUser),
                        IsUserForumModerator = IsForumModerator(postUser),
                        ShowUsersPostCount = forumSettings.ShowUsersPostCount,
                        ForumPostCount = postUser.GetAttribute<int>(SystemUserAttributeNames.ForumPostCount),
                        //ShowUsersJoinDate = _customerSettings.ShowUsersJoinDate, // TODO
                        ShowUsersJoinDate = false,
                        //UserJoinDate = postUser.CreatedOnUtc, //TODO
                        AllowPrivateMessages = forumSettings.AllowPrivateMessages,
                        SignaturesEnabled = forumSettings.SignaturesEnabled,
                        FormattedSignature = postUser.GetAttribute<string>(SystemUserAttributeNames.Signature).FormatForumSignatureText(),
                    };
                    //created on string
                    if (forumSettings.RelativeDateTimeFormattingEnabled)
                    {
                        postModel.PostCreatedOnStr = post.CreatedOnUtc.RelativeFormat(true, "f");
                    }
                    else
                    {
                        postModel.PostCreatedOnStr = dateTimeHelper.ConvertToUserTime(post.CreatedOnUtc, DateTimeKind.Utc).ToString("f");
                    }

                    // TODO
                    ////avatar
                    //if (_customerSettings.AllowUsersToUploadAvatars)
                    //{
                    //    postModel.UserAvatarUrl = _pictureService.GetPictureUrl(
                    //        postUser.GetAttribute<int>(SystemUserAttributeNames.AvatarPictureId),
                    //        _mediaSettings.AvatarPictureSize,
                    //        _customerSettings.DefaultAvatarEnabled,
                    //        defaultPictureType: PictureType.Avatar);
                    //}

                    ////location
                    //postModel.ShowUsersLocation = _customerSettings.ShowUsersLocation;
                    //if (_customerSettings.ShowUsersLocation)
                    //{
                    //    var countryId = postUser.GetAttribute<int>(SystemUserAttributeNames.CountryId);
                    //    var country = regionService.FindOne(countryId);
                    //    postModel.UserLocation = country != null ? country.GetLocalized(x => x.Name) : string.Empty;
                    //}

                    // page number is needed for creating post link in _ForumPost partial view
                    postModel.CurrentTopicPage = page;
                    model.ForumPostModels.Add(postModel);
                }

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("topic/watch/{id}")]
        public ActionResult TopicWatch(int id)
        {
            string watchTopic = T(LocalizableStrings.WatchTopic);
            string unwatchTopic = T(LocalizableStrings.UnwatchTopic);
            string returnText = watchTopic;

            var topic = forumService.GetTopicById(id);
            if (topic == null)
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            if (!forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser))
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            var subscription = forumService
                .GetAllSubscriptions(WorkContext.CurrentUser.Id, 0, topic.Id, 0, 1)
                .FirstOrDefault();

            bool subscribed;
            if (subscription == null)
            {
                subscription = new ForumSubscription
                {
                    //SubscriptionGuid = Guid.NewGuid(),
                    UserId = WorkContext.CurrentUser.Id,
                    TopicId = topic.Id,
                    CreatedOnUtc = DateTime.UtcNow
                };
                forumService.InsertSubscription(subscription);
                subscribed = true;
                returnText = unwatchTopic;
            }
            else
            {
                forumService.DeleteSubscription(subscription);
                subscribed = false;
            }

            return Json(new { Subscribed = subscribed, Text = returnText, Error = false });
        }

        [Route("topic/move/{id}")]
        public ActionResult TopicMove(int id)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var topic = forumService.GetTopicById(id);
            if (topic == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TopicMoveModel
            {
                ForumList = ForumGroupsForumsList(),
                Id = topic.Id,
                TopicSeName = topic.GetSeName(),
                ForumSelected = topic.ForumId
            };

            return View(model);
        }

        [HttpPost]
        [FrontendAntiForgery]
        [Route("topic/move-post")]
        public ActionResult TopicMovePost(TopicMoveModel model)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var topic = forumService.GetTopicById(model.Id);
            if (topic == null)
            {
                return RedirectToAction("Index");
            }

            int newForumId = model.ForumSelected;
            var forum = forumService.GetForumById(newForumId);

            if (forum != null && topic.ForumId != newForumId)
            {
                forumService.MoveTopic(topic.Id, newForumId);
            }

            return RedirectToAction("Topic", new { id = topic.Id, slug = topic.GetSeName() });
        }

        [Route("topic/delete/{id}")]
        public ActionResult TopicDelete(int id)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var topic = forumService.GetTopicById(id);
            if (topic != null)
            {
                if (!forumService.IsUserAllowedToDeleteTopic(WorkContext.CurrentUser, topic))
                {
                    return new HttpUnauthorizedResult();
                }
                var forum = forumService.GetForumById(topic.ForumId);

                forumService.DeleteTopic(topic);

                if (forum != null)
                {
                    return RedirectToAction("Forum", new { id = forum.Id, slug = forum.GetSeName() });
                }
            }

            return RedirectToAction("Index");
        }

        [Route("topic/create/{id}")]
        public ActionResult TopicCreate(int id)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var forum = forumService.GetForumById(id);
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            if (forumService.IsUserAllowedToCreateTopic(WorkContext.CurrentUser, forum) == false)
            {
                return new HttpUnauthorizedResult();
            }

            var model = new EditForumTopicModel
            {
                Id = 0,
                IsEdit = false,
                ForumId = forum.Id,
                ForumName = forum.Name,
                ForumSeName = forum.GetSeName(),
                ForumEditor = forumSettings.ForumEditor,
                IsUserAllowedToSetTopicPriority = forumService.IsUserAllowedToSetTopicPriority(WorkContext.CurrentUser),
                TopicPriorities = ForumTopicTypesList(),
                IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser),
                Subscribed = false
            };
            return View(model);
        }

        [HttpPost]
        [FrontendAntiForgery]
        [Route("topic/create-post")]
        [ValidateInput(false)]
        public ActionResult TopicCreatePost(EditForumTopicModel model)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var forum = forumService.GetForumById(model.ForumId);
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!forumService.IsUserAllowedToCreateTopic(WorkContext.CurrentUser, forum))
                    {
                        return new HttpUnauthorizedResult();
                    }

                    string subject = model.Subject;
                    int maxSubjectLength = forumSettings.TopicSubjectMaxLength;

                    if (maxSubjectLength > 0 && subject.Length > maxSubjectLength)
                    {
                        subject = subject.Substring(0, maxSubjectLength);
                    }

                    string text = model.Text;
                    int maxPostLength = forumSettings.PostMaxLength;

                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    var topicType = ForumTopicType.Normal;

                    string ipAddress = webHelper.GetCurrentIpAddress();

                    var nowUtc = DateTime.UtcNow;

                    if (forumService.IsUserAllowedToSetTopicPriority(WorkContext.CurrentUser))
                    {
                        topicType = model.TopicType;
                    }

                    //forum topic
                    var topic = new ForumTopic
                    {
                        ForumId = forum.Id,
                        UserId = WorkContext.CurrentUser.Id,
                        TopicType = topicType,
                        Subject = subject,
                        CreatedOnUtc = nowUtc,
                        UpdatedOnUtc = nowUtc
                    };
                    forumService.InsertTopic(topic, true);

                    //forum post
                    var post = new ForumPost
                    {
                        TopicId = topic.Id,
                        UserId = WorkContext.CurrentUser.Id,
                        Text = text,
                        IPAddress = ipAddress,
                        CreatedOnUtc = nowUtc,
                        UpdatedOnUtc = nowUtc
                    };
                    forumService.InsertPost(post, false);

                    //update forum topic
                    topic.NumPosts = 1;
                    topic.LastPostId = post.Id;
                    topic.LastPostUserId = post.UserId;
                    topic.LastPostTime = post.CreatedOnUtc;
                    topic.UpdatedOnUtc = nowUtc;
                    forumService.UpdateTopic(topic);

                    //subscription
                    if (forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser))
                    {
                        if (model.Subscribed)
                        {
                            var forumSubscription = new ForumSubscription
                            {
                                //SubscriptionGuid = Guid.NewGuid(),
                                UserId = WorkContext.CurrentUser.Id,
                                TopicId = topic.Id,
                                CreatedOnUtc = nowUtc
                            };

                            forumService.InsertSubscription(forumSubscription);
                        }
                    }

                    return RedirectToAction("Topic", new { id = topic.Id, slug = topic.GetSeName() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // redisplay form
            model.TopicPriorities = ForumTopicTypesList();
            model.IsEdit = false;
            model.ForumId = forum.Id;
            model.ForumName = forum.Name;
            model.ForumSeName = forum.GetSeName();
            model.Id = 0;
            model.IsUserAllowedToSetTopicPriority = forumService.IsUserAllowedToSetTopicPriority(WorkContext.CurrentUser);
            model.IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser);
            model.ForumEditor = forumSettings.ForumEditor;

            return View("TopicCreate", model);
        }

        [Route("topic/edit/{id}")]
        public ActionResult TopicEdit(int id)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var topic = forumService.GetTopicById(id);
            if (topic == null)
            {
                return RedirectToAction("Index");
            }

            if (!forumService.IsUserAllowedToEditTopic(WorkContext.CurrentUser, topic))
            {
                return new HttpUnauthorizedResult();
            }

            var forum = topic.Forum ?? forumService.GetForumById(topic.ForumId);
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            var firstPost = topic.GetFirstPost(forumService);

            var model = new EditForumTopicModel
            {
                IsEdit = true,
                TopicPriorities = ForumTopicTypesList(),
                ForumName = forum.Name,
                ForumSeName = forum.GetSeName(),
                Text = firstPost.Text,
                Subject = topic.Subject,
                TopicType = topic.TopicType,
                Id = topic.Id,
                ForumId = forum.Id,
                ForumEditor = forumSettings.ForumEditor,
                IsUserAllowedToSetTopicPriority = forumService.IsUserAllowedToSetTopicPriority(WorkContext.CurrentUser),
                IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser)
            };

            //subscription
            if (model.IsUserAllowedToSubscribe)
            {
                var subscription = forumService
                    .GetAllSubscriptions(WorkContext.CurrentUser.Id, 0, topic.Id, 0, 1)
                    .FirstOrDefault();

                model.Subscribed = subscription != null;
            }

            return View(model);
        }

        [HttpPost]
        [FrontendAntiForgery]
        [Route("topic/edit-post")]
        [ValidateInput(false)]
        public ActionResult TopicEditPost(EditForumTopicModel model)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var topic = forumService.GetTopicById(model.Id);
            if (topic == null)
            {
                return RedirectToAction("Index");
            }

            var forum = topic.Forum ?? forumService.GetForumById(topic.ForumId);
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!forumService.IsUserAllowedToEditTopic(WorkContext.CurrentUser, topic))
                    {
                        return new HttpUnauthorizedResult();
                    }

                    string subject = model.Subject;
                    int maxSubjectLength = forumSettings.TopicSubjectMaxLength;

                    if (maxSubjectLength > 0 && subject.Length > maxSubjectLength)
                    {
                        subject = subject.Substring(0, maxSubjectLength);
                    }

                    string text = model.Text;
                    int maxPostLength = forumSettings.PostMaxLength;

                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    var topicType = ForumTopicType.Normal;

                    string ipAddress = webHelper.GetCurrentIpAddress();

                    DateTime nowUtc = DateTime.UtcNow;

                    if (forumService.IsUserAllowedToSetTopicPriority(WorkContext.CurrentUser))
                    {
                        topicType = model.TopicType;
                    }

                    //forum topic
                    topic.TopicType = topicType;
                    topic.Subject = subject;
                    topic.UpdatedOnUtc = nowUtc;
                    forumService.UpdateTopic(topic);

                    //forum post
                    var firstPost = topic.GetFirstPost(forumService);
                    if (firstPost != null)
                    {
                        firstPost.Text = text;
                        firstPost.UpdatedOnUtc = nowUtc;
                        forumService.UpdatePost(firstPost);
                    }
                    else
                    {
                        //error (not possible)
                        firstPost = new ForumPost
                        {
                            TopicId = topic.Id,
                            UserId = topic.UserId,
                            Text = text,
                            IPAddress = ipAddress,
                            UpdatedOnUtc = nowUtc
                        };

                        forumService.InsertPost(firstPost, false);
                    }

                    //subscription
                    if (forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser))
                    {
                        var subscription = forumService
                            .GetAllSubscriptions(WorkContext.CurrentUser.Id, 0, topic.Id, 0, 1)
                            .FirstOrDefault();

                        if (model.Subscribed)
                        {
                            if (subscription == null)
                            {
                                subscription = new ForumSubscription
                                {
                                    //SubscriptionGuid = Guid.NewGuid(),
                                    UserId = WorkContext.CurrentUser.Id,
                                    TopicId = topic.Id,
                                    CreatedOnUtc = nowUtc
                                };

                                forumService.InsertSubscription(subscription);
                            }
                        }
                        else
                        {
                            if (subscription != null)
                            {
                                forumService.DeleteSubscription(subscription);
                            }
                        }
                    }

                    // redirect to the topic page with the topic slug
                    return RedirectToAction("Topic", new { id = topic.Id, slug = topic.GetSeName() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // redisplay form
            model.TopicPriorities = ForumTopicTypesList();
            model.IsEdit = true;
            model.ForumName = forum.Name;
            model.ForumSeName = forum.GetSeName();
            model.ForumId = forum.Id;
            model.ForumEditor = forumSettings.ForumEditor;

            model.IsUserAllowedToSetTopicPriority = forumService.IsUserAllowedToSetTopicPriority(WorkContext.CurrentUser);
            model.IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser);

            return View("TopicEdit", model);
        }

        [HttpPost]
        [FrontendAntiForgery]
        [Route("topic/save")]
        [ValidateInput(false)]
        public ActionResult TopicSave(EditForumTopicModel model)
        {
            if (model.IsEdit)
            {
                return TopicEditPost(model);
            }
            else
            {
                return TopicCreatePost(model);
            }
        }

        [Route("post/delete/{id}")]
        public ActionResult PostDelete(int id)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var post = forumService.GetPostById(id);
            if (post != null)
            {
                if (!forumService.IsUserAllowedToDeletePost(WorkContext.CurrentUser, post))
                {
                    return new HttpUnauthorizedResult();
                }

                var topic = post.ForumTopic ?? forumService.GetTopicById(post.TopicId);
                var forum = topic.Forum ?? forumService.GetForumById(topic.ForumId);

                int forumId = forum.Id;
                string forumSlug = forum.GetSeName();

                forumService.DeletePost(post);

                //get topic one more time because it can be deleted (first or only post deleted)
                topic = forumService.GetTopicById(post.TopicId);
                if (topic == null)
                {
                    return RedirectToAction("Forum", new { id = forumId, slug = forumSlug });
                }
                return RedirectToAction("Topic", new { id = topic.Id, slug = topic.GetSeName() });
            }

            return RedirectToAction("Index");
        }

        [Route("post/create/{id}/{quote?}")]
        public ActionResult PostCreate(int id, int? quote)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var topic = forumService.GetTopicById(id);
            if (topic == null)
            {
                return RedirectToAction("Index");
            }

            if (!forumService.IsUserAllowedToCreatePost(WorkContext.CurrentUser, topic))
            {
                return new HttpUnauthorizedResult();
            }

            var forum = topic.Forum ?? forumService.GetForumById(topic.ForumId);
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            var model = new EditForumPostModel
            {
                Id = 0,
                ForumTopicId = topic.Id,
                IsEdit = false,
                ForumEditor = forumSettings.ForumEditor,
                ForumName = forum.Name,
                ForumTopicSubject = topic.Subject,
                ForumTopicSeName = topic.GetSeName(),
                IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser),
                Subscribed = false,
            };

            //subscription
            if (model.IsUserAllowedToSubscribe)
            {
                var subscription = forumService
                    .GetAllSubscriptions(WorkContext.CurrentUser.Id, 0, topic.Id, 0, 1)
                    .FirstOrDefault();

                model.Subscribed = subscription != null;
            }

            // Insert the quoted text
            string text = string.Empty;
            if (quote.HasValue)
            {
                var quotePost = forumService.GetPostById(quote.Value);
                var quotePostUser = membershipService.GetUserById(quotePost.UserId);

                if (quotePost != null && quotePost.TopicId == topic.Id)
                {
                    switch (forumSettings.ForumEditor)
                    {
                        case EditorType.SimpleTextBox:
                            text = string.Format(
                                "{0}:\n{1}\n",
                                membershipService.GetUserDisplayName(quotePostUser),
                                quotePost.Text);
                            break;

                        case EditorType.BBCodeEditor:
                            text = string.Format(
                                "[quote={0}]{1}[/quote]",
                                membershipService.GetUserDisplayName(quotePostUser),
                                BBCodeHelper.RemoveQuotes(quotePost.Text));
                            break;
                    }
                    model.Text = text;
                }
            }

            return View(model);
        }

        [HttpPost]
        [FrontendAntiForgery]
        [Route("post/create-post")]
        [ValidateInput(false)]
        public ActionResult PostCreatePost(EditForumPostModel model)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var topic = forumService.GetTopicById(model.ForumTopicId);
            if (topic == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!forumService.IsUserAllowedToCreatePost(WorkContext.CurrentUser, topic))
                    {
                        return new HttpUnauthorizedResult();
                    }

                    string text = model.Text;
                    var maxPostLength = forumSettings.PostMaxLength;

                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    string ipAddress = webHelper.GetCurrentIpAddress();

                    DateTime nowUtc = DateTime.UtcNow;

                    var post = new ForumPost
                    {
                        TopicId = topic.Id,
                        UserId = WorkContext.CurrentUser.Id,
                        Text = text,
                        IPAddress = ipAddress,
                        CreatedOnUtc = nowUtc,
                        UpdatedOnUtc = nowUtc
                    };
                    forumService.InsertPost(post, true);

                    //subscription
                    if (forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser))
                    {
                        var forumSubscription = forumService
                            .GetAllSubscriptions(WorkContext.CurrentUser.Id, 0, post.TopicId, 0, 1)
                            .FirstOrDefault();

                        if (model.Subscribed)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = new ForumSubscription
                                {
                                    //SubscriptionGuid = Guid.NewGuid(),
                                    UserId = WorkContext.CurrentUser.Id,
                                    TopicId = post.TopicId,
                                    CreatedOnUtc = nowUtc
                                };

                                forumService.InsertSubscription(forumSubscription);
                            }
                        }
                        else
                        {
                            if (forumSubscription != null)
                            {
                                forumService.DeleteSubscription(forumSubscription);
                            }
                        }
                    }

                    int pageSize = forumSettings.PostsPageSize > 0 ? forumSettings.PostsPageSize : 10;

                    int pageIndex = (forumService.CalculateTopicPageIndex(post.TopicId, pageSize, post.Id) + 1);
                    string url = string.Empty;

                    if (pageIndex > 1)
                    {
                        url = Url.Action("Topic", new { id = post.TopicId, slug = topic.GetSeName(), page = pageIndex });
                    }
                    else
                    {
                        url = Url.Action("Topic", new { id = post.TopicId, slug = topic.GetSeName() });
                    }
                    return Redirect(string.Format("{0}#{1}", url, post.Id));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // redisplay form
            var forum = topic.Forum ?? forumService.GetForumById(topic.ForumId);
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            model.IsEdit = false;
            model.ForumName = forum.Name;
            model.ForumTopicId = topic.Id;
            model.ForumTopicSubject = topic.Subject;
            model.ForumTopicSeName = topic.GetSeName();
            model.Id = 0;
            model.IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser);
            model.ForumEditor = forumSettings.ForumEditor;

            return View("PostCreate", model);
        }

        [Route("post/edit/{id}")]
        public ActionResult PostEdit(int id)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var post = forumService.GetPostById(id);
            if (post == null)
            {
                return RedirectToAction("Index");
            }
            if (!forumService.IsUserAllowedToEditPost(WorkContext.CurrentUser, post))
            {
                return new HttpUnauthorizedResult();
            }

            var topic = post.ForumTopic ?? forumService.GetTopicById(post.TopicId);
            if (topic == null)
            {
                return RedirectToAction("Index");
            }

            var forum = topic.Forum ?? forumService.GetForumById(topic.ForumId);
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            var model = new EditForumPostModel
            {
                Id = post.Id,
                ForumTopicId = topic.Id,
                IsEdit = true,
                ForumEditor = forumSettings.ForumEditor,
                ForumName = forum.Name,
                ForumTopicSubject = topic.Subject,
                ForumTopicSeName = topic.GetSeName(),
                IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser),
                Subscribed = false,
                Text = post.Text,
            };

            //subscription
            if (model.IsUserAllowedToSubscribe)
            {
                var subscription = forumService
                    .GetAllSubscriptions(WorkContext.CurrentUser.Id, 0, topic.Id, 0, 1)
                    .FirstOrDefault();

                model.Subscribed = subscription != null;
            }

            return View(model);
        }

        [HttpPost]
        [FrontendAntiForgery]
        [Route("post/edit-post")]
        [ValidateInput(false)]
        public ActionResult PostEditPost(EditForumPostModel model)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var post = forumService.GetPostById(model.Id);
            if (post == null)
            {
                return RedirectToAction("Index");
            }

            if (!forumService.IsUserAllowedToEditPost(WorkContext.CurrentUser, post))
            {
                return new HttpUnauthorizedResult();
            }

            var topic = post.ForumTopic ?? forumService.GetTopicById(post.TopicId);
            if (topic == null)
            {
                return RedirectToAction("Index");
            }

            var forum = topic.Forum ?? forumService.GetForumById(topic.ForumId);
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var nowUtc = DateTime.UtcNow;

                    string text = model.Text;
                    var maxPostLength = forumSettings.PostMaxLength;
                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    post.UpdatedOnUtc = nowUtc;
                    post.Text = text;
                    forumService.UpdatePost(post);

                    //subscription
                    if (forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser))
                    {
                        var subscription = forumService
                            .GetAllSubscriptions(WorkContext.CurrentUser.Id, 0, post.TopicId, 0, 1)
                            .FirstOrDefault();

                        if (model.Subscribed)
                        {
                            if (subscription == null)
                            {
                                subscription = new ForumSubscription
                                {
                                    //SubscriptionGuid = Guid.NewGuid(),
                                    UserId = WorkContext.CurrentUser.Id,
                                    TopicId = post.TopicId,
                                    CreatedOnUtc = nowUtc
                                };
                                forumService.InsertSubscription(subscription);
                            }
                        }
                        else
                        {
                            if (subscription != null)
                            {
                                forumService.DeleteSubscription(subscription);
                            }
                        }
                    }

                    int pageSize = forumSettings.PostsPageSize > 0 ? forumSettings.PostsPageSize : 10;
                    int pageIndex = (forumService.CalculateTopicPageIndex(post.TopicId, pageSize, post.Id) + 1);
                    var url = string.Empty;
                    if (pageIndex > 1)
                    {
                        url = Url.Action("Topic", new { id = post.TopicId, slug = topic.GetSeName(), page = pageIndex });
                    }
                    else
                    {
                        url = Url.Action("Topic", new { id = post.TopicId, slug = topic.GetSeName() });
                    }
                    return Redirect(string.Format("{0}#{1}", url, post.Id));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            //redisplay form
            model.IsEdit = true;
            model.ForumName = forum.Name;
            model.ForumTopicId = topic.Id;
            model.ForumTopicSubject = topic.Subject;
            model.ForumTopicSeName = topic.GetSeName();
            model.Id = post.Id;
            model.IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(WorkContext.CurrentUser);
            model.ForumEditor = forumSettings.ForumEditor;

            return View("PostEdit", model);
        }

        [HttpPost]
        [FrontendAntiForgery]
        [Route("post/save")]
        [ValidateInput(false)]
        public ActionResult PostSave(EditForumPostModel model)
        {
            if (model.IsEdit)
            {
                return PostEditPost(model);
            }
            else
            {
                return PostCreatePost(model);
            }
        }

        [Route("search")]
        public ActionResult Search(
            string searchterms,
            bool? adv,
            string forumId,
            string within,
            string limitDays,
            int page = 1)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            int pageSize = 10;

            var model = new SearchModel();

            // Create the values for the "Limit results to previous" select list
            var limitList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = T(LocalizableStrings.Models.Search.LimitResultsToPrevious.AllResults),
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = T(LocalizableStrings.Models.Search.LimitResultsToPrevious.OneDay),
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = T(LocalizableStrings.Models.Search.LimitResultsToPrevious.SevenDays),
                    Value = "7"
                },
                new SelectListItem
                {
                    Text = T(LocalizableStrings.Models.Search.LimitResultsToPrevious.TwoWeeks),
                    Value = "14"
                },
                new SelectListItem
                {
                    Text = T(LocalizableStrings.Models.Search.LimitResultsToPrevious.OneMonth),
                    Value = "30"
                },
                new SelectListItem
                {
                    Text = T(LocalizableStrings.Models.Search.LimitResultsToPrevious.ThreeMonths),
                    Value = "92"
                },
                new SelectListItem
                {
                    Text= T(LocalizableStrings.Models.Search.LimitResultsToPrevious.SixMonths),
                    Value = "183"
                },
                new SelectListItem
                {
                    Text = T(LocalizableStrings.Models.Search.LimitResultsToPrevious.OneYear),
                    Value = "365"
                }
            };
            model.LimitList = limitList;

            // Create the values for the "Search in forum" select list
            var forumsSelectList = new List<SelectListItem>();

            forumsSelectList.Add(
                new SelectListItem
                {
                    Text = T(LocalizableStrings.Models.Search.SearchInForum.All),
                    Value = "0",
                    Selected = true,
                });

            string separator = "--";

            var groups = forumService.GetAllForumGroups();
            foreach (var group in groups)
            {
                // Add the forum group with value as '-' so it can't be used as a target forum id
                forumsSelectList.Add(new SelectListItem { Text = group.Name, Value = "-" });

                var forums = forumService.GetAllForumsByGroupId(group.Id);
                foreach (var forum in forums)
                {
                    forumsSelectList.Add(
                        new SelectListItem
                        {
                            Text = string.Format("{0}{1}", separator, forum.Name),
                            Value = forum.Id.ToString()
                        });
                }
            }
            model.ForumList = forumsSelectList;

            // Create the values for "Search within" select list
            var withinList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = ((int)ForumSearchType.All).ToString(),
                    Text = T(LocalizableStrings.Models.Search.SearchWithin.All)
                },
                new SelectListItem
                {
                    Value = ((int)ForumSearchType.TopicTitlesOnly).ToString(),
                    Text = T(LocalizableStrings.Models.Search.SearchWithin.TopicTitlesOnly)
                },
                new SelectListItem
                {
                    Value = ((int)ForumSearchType.PostTextOnly).ToString(),
                    Text = T(LocalizableStrings.Models.Search.SearchWithin.PostTextOnly)
                }
            };
            model.WithinList = withinList;

            int forumIdSelected;
            int.TryParse(forumId, out forumIdSelected);
            model.ForumIdSelected = forumIdSelected;

            int withinSelected;
            int.TryParse(within, out withinSelected);
            model.WithinSelected = withinSelected;

            int limitDaysSelected;
            int.TryParse(limitDays, out limitDaysSelected);
            model.LimitDaysSelected = limitDaysSelected;

            int searchTermMinimumLength = forumSettings.ForumSearchTermMinimumLength;

            model.ShowAdvancedSearch = adv.GetValueOrDefault();
            model.SearchResultsVisible = false;
            model.NoResultsVisisble = false;
            model.PostsPageSize = forumSettings.PostsPageSize;

            try
            {
                if (!string.IsNullOrWhiteSpace(searchterms))
                {
                    searchterms = searchterms.Trim();
                    model.SearchTerms = searchterms;

                    if (searchterms.Length < searchTermMinimumLength)
                    {
                        throw new KoreException(string.Format(
                            T(LocalizableStrings.SearchTermMinimumLengthIsNCharacters),
                            searchTermMinimumLength));
                    }

                    ForumSearchType searchWithin = 0;
                    int limitResultsToPrevious = 0;
                    if (adv.GetValueOrDefault())
                    {
                        searchWithin = (ForumSearchType)withinSelected;
                        limitResultsToPrevious = limitDaysSelected;
                    }

                    if (forumSettings.SearchResultsPageSize > 0)
                    {
                        pageSize = forumSettings.SearchResultsPageSize;
                    }

                    var topics = forumService.GetAllTopics(
                        forumIdSelected,
                        null,
                        searchterms,
                        searchWithin,
                        limitResultsToPrevious,
                        page - 1,
                        pageSize);

                    model.TopicPageSize = topics.PageSize;
                    model.TopicTotalRecords = topics.ItemCount;
                    model.TopicPageIndex = topics.PageIndex;

                    foreach (var topic in topics)
                    {
                        var topicModel = PrepareForumTopicRowModel(topic);
                        model.ForumTopics.Add(topicModel);
                    }

                    model.SearchResultsVisible = (topics.Count > 0);
                    model.NoResultsVisisble = !(model.SearchResultsVisible);

                    return View(model);
                }
                model.SearchResultsVisible = false;
            }
            catch (Exception ex)
            {
                model.Error = ex.Message;
            }

            //some exception raised
            model.TopicPageSize = pageSize;
            model.TopicTotalRecords = 0;
            model.TopicPageIndex = page - 1;

            return View(model);
        }

        [ChildActionOnly]
        [Route("last-post/{forumPostId}/{showTopic}")]
        public ActionResult LastPost(int forumPostId, bool showTopic)
        {
            var post = forumService.GetPostById(forumPostId);
            var model = new LastPostModel();

            if (post != null)
            {
                var postUser = membershipService.GetUserById(post.UserId);
                var topic = post.ForumTopic ?? forumService.GetTopicById(post.TopicId);

                model.Id = post.Id;
                model.ForumTopicId = post.TopicId;
                model.ForumTopicSeName = topic.GetSeName();
                model.ForumTopicSubject = topic.StripTopicSubject();
                model.UserId = post.UserId;
                //model.AllowViewingProfiles = _customerSettings.AllowViewingProfiles; //TODO
                model.AllowViewingProfiles = true;
                model.UserName = membershipService.GetUserDisplayName(postUser);
                //created on string
                if (forumSettings.RelativeDateTimeFormattingEnabled)
                {
                    model.PostCreatedOnStr = post.CreatedOnUtc.RelativeFormat(true, "f");
                }
                else
                {
                    model.PostCreatedOnStr = dateTimeHelper.ConvertToUserTime(post.CreatedOnUtc, DateTimeKind.Utc).ToString("f");
                }
            }
            model.ShowTopic = showTopic;
            return PartialView(model);
        }

        [ChildActionOnly]
        [Route("breadcrumb")]
        public ActionResult ForumBreadcrumb(int? forumGroupId, int? forumId, int? forumTopicId)
        {
            var model = new ForumBreadcrumbModel();

            ForumTopic topic = null;
            if (forumTopicId.HasValue)
            {
                topic = forumService.GetTopicById(forumTopicId.Value);
                if (topic != null)
                {
                    model.ForumTopicId = topic.Id;
                    model.ForumTopicSubject = topic.Subject;
                    model.ForumTopicSeName = topic.GetSeName();
                }
            }

            Forum forum = forumService.GetForumById(topic != null ? topic.ForumId : (forumId.HasValue ? forumId.Value : 0));
            if (forum != null)
            {
                model.ForumId = forum.Id;
                model.ForumName = forum.Name;
                model.ForumSeName = forum.GetSeName();
            }

            var forumGroup = forumService.GetForumGroupById(forum != null ? forum.ForumGroupId : (forumGroupId.HasValue ? forumGroupId.Value : 0));
            if (forumGroup != null)
            {
                model.ForumGroupId = forumGroup.Id;
                model.ForumGroupName = forumGroup.Name;
                model.ForumGroupSeName = forumGroup.GetSeName();
            }

            return PartialView(model);
        }

        [Route("subscribe/{page?}")]
        public ActionResult UserForumSubscriptions(int? page)
        {
            if (!forumSettings.AllowUsersToManageSubscriptions)
            {
                return RedirectToAction("Index");
                //return RedirectToRoute("UserInfo"); // TODO: Is this meant o be the user's profile page? 
            }

            int pageIndex = 0;
            if (page > 0)
            {
                pageIndex = page.Value - 1;
            }

            var customer = WorkContext.CurrentUser;

            var pageSize = forumSettings.ForumSubscriptionsPageSize;

            var subscriptions = forumService.GetAllSubscriptions(customer.Id, 0, 0, pageIndex, pageSize);

            var model = new UserForumSubscriptionsModel();

            foreach (var subscription in subscriptions)
            {
                int topicId = subscription.TopicId;
                int forumId = subscription.ForumId;
                bool topicSubscription = false;
                string title = string.Empty;
                string slug = string.Empty;

                if (topicId > 0)
                {
                    topicSubscription = true;
                    var topic = forumService.GetTopicById(topicId);
                    if (topic != null)
                    {
                        title = topic.Subject;
                        slug = topic.GetSeName();
                    }
                }
                else
                {
                    var forum = forumService.GetForumById(forumId);
                    if (forum != null)
                    {
                        title = forum.Name;
                        slug = forum.GetSeName();
                    }
                }

                model.ForumSubscriptions.Add(new UserForumSubscriptionsModel.ForumSubscriptionModel
                {
                    Id = subscription.Id,
                    ForumTopicId = topicId,
                    ForumId = subscription.ForumId,
                    TopicSubscription = topicSubscription,
                    Title = title,
                    Slug = slug,
                });
            }

            model.PagerModel = new PagerModel
            {
                PageSize = subscriptions.PageSize,
                TotalRecords = subscriptions.ItemCount,
                PageIndex = subscriptions.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "UserForumSubscriptionsPaged",
                //UseRouteLinks = true,
                RouteValues = new ForumSubscriptionsRouteValues { page = pageIndex }
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("UserForumSubscriptions")]
        [Route("subscribe-post")]
        public ActionResult UserForumSubscriptionsPOST(FormCollection formCollection)
        {
            foreach (string key in formCollection.AllKeys)
            {
                string value = formCollection[key];

                if (value.Equals("on") && key.StartsWith("fs", StringComparison.InvariantCultureIgnoreCase))
                {
                    string id = key.Replace("fs", string.Empty).Trim();
                    int subscriptionId;
                    if (int.TryParse(id, out subscriptionId))
                    {
                        var subscription = forumService.GetSubscriptionById(subscriptionId);
                        if (subscription != null && subscription.UserId == WorkContext.CurrentUser.Id)
                        {
                            forumService.DeleteSubscription(subscription);
                        }
                    }
                }
            }

            return RedirectToAction("UserForumSubscriptions");
        }


        [HttpPost]
        [Route("upload-file")]
        public JsonResult UploadFile()
        {
            try
            {
                #region Save File

                var file = Request.Files["Upload"];
                var stream = file.InputStream;

                string uploadFileName = Path.Combine(
                    Server.MapPath("~/Media/Uploads/_Users/" + WorkContext.CurrentUser.Id + "/"),
                    file.FileName);

                string directory = Path.GetDirectoryName(uploadFileName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var fs = new FileStream(uploadFileName, FileMode.Create, FileAccess.Write))
                using (var bw = new BinaryWriter(fs))
                {
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    bw.Write(bytes);
                }

                #endregion Save File

                return Json(new
                {
                    Success = true,
                    Url = string.Format("/Media/Uploads/_Users/{0}/{1}", WorkContext.CurrentUser.Id, file.FileName),
                    FileName = file.FileName
                });
            }
            catch (Exception x)
            {
                return Json(new { Success = false, error = x.GetBaseException().Message });
            }
        }

        #endregion Methods
    }
}