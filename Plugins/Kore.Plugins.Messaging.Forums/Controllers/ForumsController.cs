using System;
using System.Collections.Generic;
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
        private readonly Localizer localizer;

        //private readonly IPictureService _pictureService;
        private readonly IRegionService regionService;

        private readonly IWebHelper webHelper;
        private readonly IWorkContext workContext;
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
            IWorkContext workContext,
            ForumSettings forumSettings,
            //UserSettings customerSettings,
            //MediaSettings mediaSettings,
            IDateTimeHelper dateTimeHelper,
            IMembershipService membershipService,
            KoreSiteSettings siteSettings)
        {
            this.forumService = forumService;
            this.localizer = LocalizationUtilities.Resolve();
            //this._pictureService = pictureService;
            this.regionService = regionService;
            this.webHelper = webHelper;
            this.workContext = workContext;
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

            var forumPosts = forumService.GetAllPosts(topic.Id, null, string.Empty, 1, forumSettings.PostsPageSize);
            topicModel.TotalPostPages = forumPosts.ItemCount;

            return topicModel;
        }

        [NonAction]
        protected virtual ForumRowModel PrepareForumRowModel(Forum forum)
        {
            var forumModel = new ForumRowModel
            {
                Id = forum.Id,
                Name = forum.Name,
                SeName = forum.GetSeName(),
                Description = forum.Description,
                NumTopics = forum.NumTopics,
                NumPosts = forum.NumPosts,
                LastPostId = forum.LastPostId,
            };
            return forumModel;
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
                Text = localizer(LocalizableStrings.TopicTypes.Normal),
                Value = ((int)ForumTopicType.Normal).ToString()
            });

            list.Add(new SelectListItem
            {
                Text = localizer(LocalizableStrings.TopicTypes.Sticky),
                Value = ((int)ForumTopicType.Sticky).ToString()
            });

            list.Add(new SelectListItem
            {
                Text = localizer(LocalizableStrings.TopicTypes.Announcement),
                Value = ((int)ForumTopicType.Announcement).ToString()
            });

            return list;
        }

        [NonAction]
        protected virtual IEnumerable<SelectListItem> ForumGroupsForumsList()
        {
            var forumsList = new List<SelectListItem>();
            var separator = "--";
            var forumGroups = forumService.GetAllForumGroups();

            foreach (var fg in forumGroups)
            {
                // Add the forum group with Value of 0 so it won't be used as a target forum
                forumsList.Add(new SelectListItem { Text = fg.Name, Value = "0" });

                var forums = forumService.GetAllForumsByGroupId(fg.Id);
                foreach (var f in forums)
                {
                    forumsList.Add(new SelectListItem { Text = string.Format("{0}{1}", separator, f.Name), Value = f.Id.ToString() });
                }
            }

            return forumsList;
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

            var forumGroups = forumService.GetAllForumGroups();

            var model = new IndexModel();
            foreach (var forumGroup in forumGroups)
            {
                var forumGroupModel = PrepareForumGroupModel(forumGroup);
                model.ForumGroups.Add(forumGroupModel);
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

            var feedTitle = localizer(LocalizableStrings.ActiveDiscussionsFeedTitle);
            var feedDescription = localizer(LocalizableStrings.ActiveDiscussionsFeedDescription);

            var feed = new SyndicationFeed(
                string.Format(feedTitle, siteSettings.SiteName),
                feedDescription,
                new Uri(url),
                "ActiveDiscussionsRSS",
                DateTime.UtcNow);

            var items = new List<SyndicationItem>();

            var viewsText = localizer(LocalizableStrings.Views);
            var repliesText = localizer(LocalizableStrings.Replies);

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

            var forumGroup = forumService.GetForumGroupById(id);

            if (forumGroup == null)
            {
                return RedirectToAction("Index");
            }

            var model = PrepareForumGroupModel(forumGroup);
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
                    IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser),
                    ForumFeedsEnabled = forumSettings.ForumFeedsEnabled,
                    PostsPageSize = forumSettings.PostsPageSize
                };

                foreach (var topic in topics)
                {
                    var topicModel = PrepareForumTopicRowModel(topic);
                    model.ForumTopics.Add(topicModel);
                }

                //subscription
                if (forumService.IsUserAllowedToSubscribe(workContext.CurrentUser))
                {
                    model.WatchForumText = localizer(LocalizableStrings.WatchForum);

                    var forumSubscription = forumService.GetAllSubscriptions(workContext.CurrentUser.Id, forum.Id, 0, 0, 1).FirstOrDefault();
                    if (forumSubscription != null)
                    {
                        model.WatchForumText = localizer(LocalizableStrings.UnwatchForum);
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

                var feedTitle = localizer(LocalizableStrings.ForumFeedTitle);
                var feedDescription = localizer(LocalizableStrings.ForumFeedDescription);

                var feed = new SyndicationFeed(
                    string.Format(feedTitle, siteSettings.SiteName, forum.Name),
                    feedDescription,
                    new Uri(url),
                    string.Format("ForumRSS:{0}", forum.Id),
                    DateTime.UtcNow);

                var items = new List<SyndicationItem>();

                var viewsText = localizer(LocalizableStrings.Views);
                var repliesText = localizer(LocalizableStrings.Replies);

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
            string watchTopic = localizer(LocalizableStrings.WatchForum);
            string unwatchTopic = localizer(LocalizableStrings.UnwatchForum);
            string returnText = watchTopic;

            var forum = forumService.GetForumById(id);
            if (forum == null)
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            if (!forumService.IsUserAllowedToSubscribe(workContext.CurrentUser))
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            var forumSubscription = forumService
                .GetAllSubscriptions(workContext.CurrentUser.Id, forum.Id, 0, 0, 1)
                .FirstOrDefault();

            bool subscribed;
            if (forumSubscription == null)
            {
                forumSubscription = new ForumSubscription
                {
                    //SubscriptionGuid = Guid.NewGuid(),
                    UserId = workContext.CurrentUser.Id,
                    ForumId = forum.Id,
                    CreatedOnUtc = DateTime.UtcNow
                };
                forumService.InsertSubscription(forumSubscription);
                subscribed = true;
                returnText = unwatchTopic;
            }
            else
            {
                forumService.DeleteSubscription(forumSubscription);
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

            var forumTopic = forumService.GetTopicById(id);

            if (forumTopic != null)
            {
                //load posts
                var posts = forumService.GetAllPosts(forumTopic.Id, null, string.Empty, page - 1, forumSettings.PostsPageSize);

                //if not posts loaded, redirect to the first page
                if (posts.Count == 0 && page > 1)
                {
                    return RedirectToAction("Topic", new { id = forumTopic.Id, slug = forumTopic.GetSeName() });
                }

                //update view count
                forumTopic.Views += 1;
                forumService.UpdateTopic(forumTopic);

                //prepare model
                var model = new ForumTopicPageModel
                {
                    Id = forumTopic.Id,
                    Subject = forumTopic.Subject,
                    SeName = forumTopic.GetSeName(),
                    IsUserAllowedToEditTopic = forumService.IsUserAllowedToEditTopic(workContext.CurrentUser, forumTopic),
                    IsUserAllowedToDeleteTopic = forumService.IsUserAllowedToDeleteTopic(workContext.CurrentUser, forumTopic),
                    IsUserAllowedToMoveTopic = forumService.IsUserAllowedToMoveTopic(workContext.CurrentUser, forumTopic),
                    IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser),
                    PostsPageIndex = posts.PageIndex,
                    PostsPageSize = posts.PageSize,
                    PostsTotalRecords = posts.ItemCount
                };

                if (model.IsUserAllowedToSubscribe)
                {
                    model.WatchTopicText = localizer(LocalizableStrings.WatchTopic);

                    var forumTopicSubscription = forumService
                        .GetAllSubscriptions(workContext.CurrentUser.Id, 0, forumTopic.Id, 0, 1)
                        .FirstOrDefault();

                    if (forumTopicSubscription != null)
                    {
                        model.WatchTopicText = localizer(LocalizableStrings.UnwatchTopic);
                    }
                }

                foreach (var post in posts)
                {
                    var postUser = membershipService.GetUserById(post.UserId);

                    var forumPostModel = new ForumPostModel
                    {
                        Id = post.Id,
                        ForumTopicId = post.TopicId,
                        ForumTopicSeName = forumTopic.GetSeName(),
                        FormattedText = post.FormatPostText(),
                        IsCurrentUserAllowedToEditPost = forumService.IsUserAllowedToEditPost(workContext.CurrentUser, post),
                        IsCurrentUserAllowedToDeletePost = forumService.IsUserAllowedToDeletePost(workContext.CurrentUser, post),
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
                        forumPostModel.PostCreatedOnStr = post.CreatedOnUtc.RelativeFormat(true, "f");
                    }
                    else
                    {
                        forumPostModel.PostCreatedOnStr = dateTimeHelper.ConvertToUserTime(post.CreatedOnUtc, DateTimeKind.Utc).ToString("f");
                    }

                    // TODO
                    ////avatar
                    //if (_customerSettings.AllowUsersToUploadAvatars)
                    //{
                    //    forumPostModel.UserAvatarUrl = _pictureService.GetPictureUrl(
                    //        postUser.GetAttribute<int>(SystemUserAttributeNames.AvatarPictureId),
                    //        _mediaSettings.AvatarPictureSize,
                    //        _customerSettings.DefaultAvatarEnabled,
                    //        defaultPictureType: PictureType.Avatar);
                    //}

                    ////location
                    //forumPostModel.ShowUsersLocation = _customerSettings.ShowUsersLocation;
                    //if (_customerSettings.ShowUsersLocation)
                    //{
                    //    var countryId = postUser.GetAttribute<int>(SystemUserAttributeNames.CountryId);
                    //    var country = regionService.FindOne(countryId);
                    //    forumPostModel.UserLocation = country != null ? country.GetLocalized(x => x.Name) : string.Empty;
                    //}

                    // page number is needed for creating post link in _ForumPost partial view
                    forumPostModel.CurrentTopicPage = page;
                    model.ForumPostModels.Add(forumPostModel);
                }

                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("topic/watch/{id}")]
        public ActionResult TopicWatch(int id)
        {
            string watchTopic = localizer(LocalizableStrings.WatchTopic);
            string unwatchTopic = localizer(LocalizableStrings.UnwatchTopic);
            string returnText = watchTopic;

            var forumTopic = forumService.GetTopicById(id);
            if (forumTopic == null)
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            if (!forumService.IsUserAllowedToSubscribe(workContext.CurrentUser))
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            var forumSubscription = forumService
                .GetAllSubscriptions(workContext.CurrentUser.Id, 0, forumTopic.Id, 0, 1)
                .FirstOrDefault();

            bool subscribed;
            if (forumSubscription == null)
            {
                forumSubscription = new ForumSubscription
                {
                    //SubscriptionGuid = Guid.NewGuid(),
                    UserId = workContext.CurrentUser.Id,
                    TopicId = forumTopic.Id,
                    CreatedOnUtc = DateTime.UtcNow
                };
                forumService.InsertSubscription(forumSubscription);
                subscribed = true;
                returnText = unwatchTopic;
            }
            else
            {
                forumService.DeleteSubscription(forumSubscription);
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

            var forumTopic = forumService.GetTopicById(id);

            if (forumTopic == null)
            {
                return RedirectToAction("Index");
            }

            var model = new TopicMoveModel
            {
                ForumList = ForumGroupsForumsList(),
                Id = forumTopic.Id,
                TopicSeName = forumTopic.GetSeName(),
                ForumSelected = forumTopic.ForumId
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

            var forumTopic = forumService.GetTopicById(model.Id);

            if (forumTopic == null)
            {
                return RedirectToAction("Index");
            }

            int newForumId = model.ForumSelected;
            var forum = forumService.GetForumById(newForumId);

            if (forum != null && forumTopic.ForumId != newForumId)
            {
                forumService.MoveTopic(forumTopic.Id, newForumId);
            }

            return RedirectToAction("Topic", new { id = forumTopic.Id, slug = forumTopic.GetSeName() });
        }

        [Route("topic/delete/{id}")]
        public ActionResult TopicDelete(int id)
        {
            if (!forumSettings.ForumsEnabled)
            {
                return RedirectToHomePage();
            }

            var forumTopic = forumService.GetTopicById(id);
            if (forumTopic != null)
            {
                if (!forumService.IsUserAllowedToDeleteTopic(workContext.CurrentUser, forumTopic))
                {
                    return new HttpUnauthorizedResult();
                }
                var forum = forumService.GetForumById(forumTopic.ForumId);

                forumService.DeleteTopic(forumTopic);

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

            if (forumService.IsUserAllowedToCreateTopic(workContext.CurrentUser, forum) == false)
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
                IsUserAllowedToSetTopicPriority = forumService.IsUserAllowedToSetTopicPriority(workContext.CurrentUser),
                TopicPriorities = ForumTopicTypesList(),
                IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser),
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
                    if (!forumService.IsUserAllowedToCreateTopic(workContext.CurrentUser, forum))
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

                    if (forumService.IsUserAllowedToSetTopicPriority(workContext.CurrentUser))
                    {
                        topicType = model.TopicType;
                    }

                    //forum topic
                    var forumTopic = new ForumTopic
                    {
                        ForumId = forum.Id,
                        UserId = workContext.CurrentUser.Id,
                        TopicType = topicType,
                        Subject = subject,
                        CreatedOnUtc = nowUtc,
                        UpdatedOnUtc = nowUtc
                    };
                    forumService.InsertTopic(forumTopic, true);

                    //forum post
                    var forumPost = new ForumPost
                    {
                        TopicId = forumTopic.Id,
                        UserId = workContext.CurrentUser.Id,
                        Text = text,
                        IPAddress = ipAddress,
                        CreatedOnUtc = nowUtc,
                        UpdatedOnUtc = nowUtc
                    };
                    forumService.InsertPost(forumPost, false);

                    //update forum topic
                    forumTopic.NumPosts = 1;
                    forumTopic.LastPostId = forumPost.Id;
                    forumTopic.LastPostUserId = forumPost.UserId;
                    forumTopic.LastPostTime = forumPost.CreatedOnUtc;
                    forumTopic.UpdatedOnUtc = nowUtc;
                    forumService.UpdateTopic(forumTopic);

                    //subscription
                    if (forumService.IsUserAllowedToSubscribe(workContext.CurrentUser))
                    {
                        if (model.Subscribed)
                        {
                            var forumSubscription = new ForumSubscription
                            {
                                //SubscriptionGuid = Guid.NewGuid(),
                                UserId = workContext.CurrentUser.Id,
                                TopicId = forumTopic.Id,
                                CreatedOnUtc = nowUtc
                            };

                            forumService.InsertSubscription(forumSubscription);
                        }
                    }

                    return RedirectToAction("Topic", new { id = forumTopic.Id, slug = forumTopic.GetSeName() });
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
            model.IsUserAllowedToSetTopicPriority = forumService.IsUserAllowedToSetTopicPriority(workContext.CurrentUser);
            model.IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser);
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

            var forumTopic = forumService.GetTopicById(id);

            if (forumTopic == null)
            {
                return RedirectToAction("Index");
            }

            if (!forumService.IsUserAllowedToEditTopic(workContext.CurrentUser, forumTopic))
            {
                return new HttpUnauthorizedResult();
            }

            var forum = forumTopic.Forum;
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            var firstPost = forumTopic.GetFirstPost(forumService);

            var model = new EditForumTopicModel
            {
                IsEdit = true,
                TopicPriorities = ForumTopicTypesList(),
                ForumName = forum.Name,
                ForumSeName = forum.GetSeName(),

                Text = firstPost.Text,
                Subject = forumTopic.Subject,
                TopicType = forumTopic.TopicType,
                Id = forumTopic.Id,
                ForumId = forum.Id,
                ForumEditor = forumSettings.ForumEditor,

                IsUserAllowedToSetTopicPriority = forumService.IsUserAllowedToSetTopicPriority(workContext.CurrentUser),
                IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser)
            };

            //subscription
            if (model.IsUserAllowedToSubscribe)
            {
                var forumSubscription = forumService
                    .GetAllSubscriptions(workContext.CurrentUser.Id, 0, forumTopic.Id, 0, 1)
                    .FirstOrDefault();

                model.Subscribed = forumSubscription != null;
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

            var forumTopic = forumService.GetTopicById(model.Id);

            if (forumTopic == null)
            {
                return RedirectToAction("Index");
            }

            var forum = forumTopic.Forum;

            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!forumService.IsUserAllowedToEditTopic(workContext.CurrentUser, forumTopic))
                    {
                        return new HttpUnauthorizedResult();
                    }

                    string subject = model.Subject;
                    int maxSubjectLength = forumSettings.TopicSubjectMaxLength;

                    if (maxSubjectLength > 0 && subject.Length > maxSubjectLength)
                    {
                        subject = subject.Substring(0, maxSubjectLength);
                    }

                    var text = model.Text;
                    int maxPostLength = forumSettings.PostMaxLength;

                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    var topicType = ForumTopicType.Normal;

                    string ipAddress = webHelper.GetCurrentIpAddress();

                    DateTime nowUtc = DateTime.UtcNow;

                    if (forumService.IsUserAllowedToSetTopicPriority(workContext.CurrentUser))
                    {
                        topicType = model.TopicType;
                    }

                    //forum topic
                    forumTopic.TopicType = topicType;
                    forumTopic.Subject = subject;
                    forumTopic.UpdatedOnUtc = nowUtc;
                    forumService.UpdateTopic(forumTopic);

                    //forum post
                    var firstPost = forumTopic.GetFirstPost(forumService);
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
                            TopicId = forumTopic.Id,
                            UserId = forumTopic.UserId,
                            Text = text,
                            IPAddress = ipAddress,
                            UpdatedOnUtc = nowUtc
                        };

                        forumService.InsertPost(firstPost, false);
                    }

                    //subscription
                    if (forumService.IsUserAllowedToSubscribe(workContext.CurrentUser))
                    {
                        var forumSubscription = forumService.GetAllSubscriptions(workContext.CurrentUser.Id,
                            0, forumTopic.Id, 0, 1).FirstOrDefault();
                        if (model.Subscribed)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = new ForumSubscription
                                {
                                    //SubscriptionGuid = Guid.NewGuid(),
                                    UserId = workContext.CurrentUser.Id,
                                    TopicId = forumTopic.Id,
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

                    // redirect to the topic page with the topic slug
                    return RedirectToAction("Topic", new { id = forumTopic.Id, slug = forumTopic.GetSeName() });
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

            model.IsUserAllowedToSetTopicPriority = forumService.IsUserAllowedToSetTopicPriority(workContext.CurrentUser);
            model.IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser);

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

            var forumPost = forumService.GetPostById(id);

            if (forumPost != null)
            {
                if (!forumService.IsUserAllowedToDeletePost(workContext.CurrentUser, forumPost))
                {
                    return new HttpUnauthorizedResult();
                }

                var forumTopic = forumPost.ForumTopic;
                int forumId = forumTopic.Forum.Id;
                string forumSlug = forumTopic.Forum.GetSeName();

                forumService.DeletePost(forumPost);

                //get topic one more time because it can be deleted (first or only post deleted)
                forumTopic = forumService.GetTopicById(forumPost.TopicId);
                if (forumTopic == null)
                {
                    return RedirectToAction("Forum", new { id = forumId, slug = forumSlug });
                }
                return RedirectToAction("Topic", new { id = forumTopic.Id, slug = forumTopic.GetSeName() });
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

            var forumTopic = forumService.GetTopicById(id);

            if (forumTopic == null)
            {
                return RedirectToAction("Index");
            }

            if (!forumService.IsUserAllowedToCreatePost(workContext.CurrentUser, forumTopic))
            {
                return new HttpUnauthorizedResult();
            }

            var forum = forumTopic.Forum;
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            var model = new EditForumPostModel
            {
                Id = 0,
                ForumTopicId = forumTopic.Id,
                IsEdit = false,
                ForumEditor = forumSettings.ForumEditor,
                ForumName = forum.Name,
                ForumTopicSubject = forumTopic.Subject,
                ForumTopicSeName = forumTopic.GetSeName(),
                IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser),
                Subscribed = false,
            };

            //subscription
            if (model.IsUserAllowedToSubscribe)
            {
                var forumSubscription = forumService
                    .GetAllSubscriptions(workContext.CurrentUser.Id, 0, forumTopic.Id, 0, 1)
                    .FirstOrDefault();

                model.Subscribed = forumSubscription != null;
            }

            // Insert the quoted text
            string text = string.Empty;
            if (quote.HasValue)
            {
                var quotePost = forumService.GetPostById(quote.Value);
                var quotePostUser = membershipService.GetUserById(quotePost.UserId);

                if (quotePost != null && quotePost.TopicId == forumTopic.Id)
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

            var forumTopic = forumService.GetTopicById(model.ForumTopicId);
            if (forumTopic == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!forumService.IsUserAllowedToCreatePost(workContext.CurrentUser, forumTopic))
                    {
                        return new HttpUnauthorizedResult();
                    }

                    var text = model.Text;
                    var maxPostLength = forumSettings.PostMaxLength;

                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    string ipAddress = webHelper.GetCurrentIpAddress();

                    DateTime nowUtc = DateTime.UtcNow;

                    var forumPost = new ForumPost
                    {
                        TopicId = forumTopic.Id,
                        UserId = workContext.CurrentUser.Id,
                        Text = text,
                        IPAddress = ipAddress,
                        CreatedOnUtc = nowUtc,
                        UpdatedOnUtc = nowUtc
                    };
                    forumService.InsertPost(forumPost, true);

                    //subscription
                    if (forumService.IsUserAllowedToSubscribe(workContext.CurrentUser))
                    {
                        var forumSubscription = forumService
                            .GetAllSubscriptions(workContext.CurrentUser.Id, 0, forumPost.TopicId, 0, 1)
                            .FirstOrDefault();

                        if (model.Subscribed)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = new ForumSubscription
                                {
                                    //SubscriptionGuid = Guid.NewGuid(),
                                    UserId = workContext.CurrentUser.Id,
                                    TopicId = forumPost.TopicId,
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

                    int pageIndex = (forumService.CalculateTopicPageIndex(forumPost.TopicId, pageSize, forumPost.Id) + 1);
                    string url = string.Empty;

                    if (pageIndex > 1)
                    {
                        url = Url.Action("Topic", new { id = forumPost.TopicId, slug = forumPost.ForumTopic.GetSeName(), page = pageIndex });
                    }
                    else
                    {
                        url = Url.Action("Topic", new { id = forumPost.TopicId, slug = forumPost.ForumTopic.GetSeName() });
                    }
                    return Redirect(string.Format("{0}#{1}", url, forumPost.Id));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // redisplay form
            var forum = forumTopic.Forum;

            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            model.IsEdit = false;
            model.ForumName = forum.Name;
            model.ForumTopicId = forumTopic.Id;
            model.ForumTopicSubject = forumTopic.Subject;
            model.ForumTopicSeName = forumTopic.GetSeName();
            model.Id = 0;
            model.IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser);
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

            var forumPost = forumService.GetPostById(id);

            if (forumPost == null)
            {
                return RedirectToAction("Index");
            }
            if (!forumService.IsUserAllowedToEditPost(workContext.CurrentUser, forumPost))
            {
                return new HttpUnauthorizedResult();
            }

            var forumTopic = forumPost.ForumTopic;
            if (forumTopic == null)
            {
                return RedirectToAction("Index");
            }

            var forum = forumTopic.Forum;
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            var model = new EditForumPostModel
            {
                Id = forumPost.Id,
                ForumTopicId = forumTopic.Id,
                IsEdit = true,
                ForumEditor = forumSettings.ForumEditor,
                ForumName = forum.Name,
                ForumTopicSubject = forumTopic.Subject,
                ForumTopicSeName = forumTopic.GetSeName(),
                IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser),
                Subscribed = false,
                Text = forumPost.Text,
            };

            //subscription
            if (model.IsUserAllowedToSubscribe)
            {
                var forumSubscription = forumService
                    .GetAllSubscriptions(workContext.CurrentUser.Id, 0, forumTopic.Id, 0, 1)
                    .FirstOrDefault();

                model.Subscribed = forumSubscription != null;
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

            var forumPost = forumService.GetPostById(model.Id);
            if (forumPost == null)
            {
                return RedirectToAction("Index");
            }

            if (!forumService.IsUserAllowedToEditPost(workContext.CurrentUser, forumPost))
            {
                return new HttpUnauthorizedResult();
            }

            var forumTopic = forumPost.ForumTopic;
            if (forumTopic == null)
            {
                return RedirectToAction("Index");
            }

            var forum = forumTopic.Forum;
            if (forum == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var nowUtc = DateTime.UtcNow;

                    var text = model.Text;
                    var maxPostLength = forumSettings.PostMaxLength;
                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    forumPost.UpdatedOnUtc = nowUtc;
                    forumPost.Text = text;
                    forumService.UpdatePost(forumPost);

                    //subscription
                    if (forumService.IsUserAllowedToSubscribe(workContext.CurrentUser))
                    {
                        var forumSubscription = forumService.GetAllSubscriptions(workContext.CurrentUser.Id,
                            0, forumPost.TopicId, 0, 1).FirstOrDefault();
                        if (model.Subscribed)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = new ForumSubscription
                                {
                                    //SubscriptionGuid = Guid.NewGuid(),
                                    UserId = workContext.CurrentUser.Id,
                                    TopicId = forumPost.TopicId,
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
                    int pageIndex = (forumService.CalculateTopicPageIndex(forumPost.TopicId, pageSize, forumPost.Id) + 1);
                    var url = string.Empty;
                    if (pageIndex > 1)
                    {
                        url = Url.Action("Topic", new { id = forumPost.TopicId, slug = forumPost.ForumTopic.GetSeName(), page = pageIndex });
                    }
                    else
                    {
                        url = Url.Action("Topic", new { id = forumPost.TopicId, slug = forumPost.ForumTopic.GetSeName() });
                    }
                    return Redirect(string.Format("{0}#{1}", url, forumPost.Id));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            //redisplay form
            model.IsEdit = true;
            model.ForumName = forum.Name;
            model.ForumTopicId = forumTopic.Id;
            model.ForumTopicSubject = forumTopic.Subject;
            model.ForumTopicSeName = forumTopic.GetSeName();
            model.Id = forumPost.Id;
            model.IsUserAllowedToSubscribe = forumService.IsUserAllowedToSubscribe(workContext.CurrentUser);
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
                    Text = localizer(LocalizableStrings.Models.Search.LimitResultsToPrevious.AllResults),
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = localizer(LocalizableStrings.Models.Search.LimitResultsToPrevious.OneDay),
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = localizer(LocalizableStrings.Models.Search.LimitResultsToPrevious.SevenDays),
                    Value = "7"
                },
                new SelectListItem
                {
                    Text = localizer(LocalizableStrings.Models.Search.LimitResultsToPrevious.TwoWeeks),
                    Value = "14"
                },
                new SelectListItem
                {
                    Text = localizer(LocalizableStrings.Models.Search.LimitResultsToPrevious.OneMonth),
                    Value = "30"
                },
                new SelectListItem
                {
                    Text = localizer(LocalizableStrings.Models.Search.LimitResultsToPrevious.ThreeMonths),
                    Value = "92"
                },
                new SelectListItem
                {
                    Text= localizer(LocalizableStrings.Models.Search.LimitResultsToPrevious.SixMonths),
                    Value = "183"
                },
                new SelectListItem
                {
                    Text = localizer(LocalizableStrings.Models.Search.LimitResultsToPrevious.OneYear),
                    Value = "365"
                }
            };
            model.LimitList = limitList;

            // Create the values for the "Search in forum" select list
            var forumsSelectList = new List<SelectListItem>();
            forumsSelectList.Add(
                new SelectListItem
                {
                    Text = localizer(LocalizableStrings.Models.Search.SearchInForum.All),
                    Value = "0",
                    Selected = true,
                });
            var separator = "--";
            var forumGroups = forumService.GetAllForumGroups();
            foreach (var fg in forumGroups)
            {
                // Add the forum group with value as '-' so it can't be used as a target forum id
                forumsSelectList.Add(new SelectListItem { Text = fg.Name, Value = "-" });

                var forums = forumService.GetAllForumsByGroupId(fg.Id);
                foreach (var f in forums)
                {
                    forumsSelectList.Add(
                        new SelectListItem
                        {
                            Text = string.Format("{0}{1}", separator, f.Name),
                            Value = f.Id.ToString()
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
                    Text = localizer(LocalizableStrings.Models.Search.SearchWithin.All)
                },
                new SelectListItem
                {
                    Value = ((int)ForumSearchType.TopicTitlesOnly).ToString(),
                    Text = localizer(LocalizableStrings.Models.Search.SearchWithin.TopicTitlesOnly)
                },
                new SelectListItem
                {
                    Value = ((int)ForumSearchType.PostTextOnly).ToString(),
                    Text = localizer(LocalizableStrings.Models.Search.SearchWithin.PostTextOnly)
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
                            localizer(LocalizableStrings.SearchTermMinimumLengthIsNCharacters),
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

                model.Id = post.Id;
                model.ForumTopicId = post.TopicId;
                model.ForumTopicSeName = post.ForumTopic.GetSeName();
                model.ForumTopicSubject = post.ForumTopic.StripTopicSubject();
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

            ForumTopic forumTopic = null;
            if (forumTopicId.HasValue)
            {
                forumTopic = forumService.GetTopicById(forumTopicId.Value);
                if (forumTopic != null)
                {
                    model.ForumTopicId = forumTopic.Id;
                    model.ForumTopicSubject = forumTopic.Subject;
                    model.ForumTopicSeName = forumTopic.GetSeName();
                }
            }

            Forum forum = forumService.GetForumById(forumTopic != null ? forumTopic.ForumId : (forumId.HasValue ? forumId.Value : 0));
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

            var customer = workContext.CurrentUser;

            var pageSize = forumSettings.ForumSubscriptionsPageSize;

            var list = forumService.GetAllSubscriptions(customer.Id, 0, 0, pageIndex, pageSize);

            var model = new UserForumSubscriptionsModel();

            foreach (var forumSubscription in list)
            {
                int forumTopicId = forumSubscription.TopicId;
                int forumId = forumSubscription.ForumId;
                bool topicSubscription = false;
                string title = string.Empty;
                string slug = string.Empty;

                if (forumTopicId > 0)
                {
                    topicSubscription = true;
                    var forumTopic = forumService.GetTopicById(forumTopicId);
                    if (forumTopic != null)
                    {
                        title = forumTopic.Subject;
                        slug = forumTopic.GetSeName();
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
                    Id = forumSubscription.Id,
                    ForumTopicId = forumTopicId,
                    ForumId = forumSubscription.ForumId,
                    TopicSubscription = topicSubscription,
                    Title = title,
                    Slug = slug,
                });
            }

            model.PagerModel = new PagerModel
            {
                PageSize = list.PageSize,
                TotalRecords = list.ItemCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "UserForumSubscriptionsPaged",
                UseRouteLinks = true,
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
                    int forumSubscriptionId;
                    if (int.TryParse(id, out forumSubscriptionId))
                    {
                        var forumSubscription = forumService.GetSubscriptionById(forumSubscriptionId);
                        if (forumSubscription != null && forumSubscription.UserId == workContext.CurrentUser.Id)
                        {
                            forumService.DeleteSubscription(forumSubscription);
                        }
                    }
                }
            }

            return RedirectToAction("UserForumSubscriptions");
        }

        #endregion Methods
    }
}