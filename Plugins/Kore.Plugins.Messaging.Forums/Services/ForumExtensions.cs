using System;
using Kore.Infrastructure;
using Kore.Plugins.Messaging.Forums.Data.Domain;
using Kore.Security.Membership;
using Kore.Web.Html;

namespace Kore.Plugins.Messaging.Forums.Services
{
    public static class ForumExtensions
    {
        public static string FormatPostText(this ForumPost forumPost)
        {
            string text = forumPost.Text;

            if (String.IsNullOrEmpty(text))
                return string.Empty;

            switch (EngineContext.Current.Resolve<ForumSettings>().ForumEditor)
            {
                case EditorType.SimpleTextBox:
                    {
                        text = HtmlHelper.FormatText(text, false, true, false, false, false, false);
                    }
                    break;

                case EditorType.BBCodeEditor:
                    {
                        text = HtmlHelper.FormatText(text, false, true, false, true, false, false);
                    }
                    break;

                default:
                    break;
            }

            return text;
        }

        public static string StripTopicSubject(this ForumTopic forumTopic)
        {
            string subject = forumTopic.Subject;
            if (String.IsNullOrEmpty(subject))
            {
                return subject;
            }

            int strippedTopicMaxLength = EngineContext.Current.Resolve<ForumSettings>().StrippedTopicMaxLength;
            if (strippedTopicMaxLength > 0)
            {
                if (subject.Length > strippedTopicMaxLength)
                {
                    int index = subject.IndexOf(" ", strippedTopicMaxLength);
                    if (index > 0)
                    {
                        subject = subject.Substring(0, index);
                        subject += "...";
                    }
                }
            }

            return subject;
        }

        public static string FormatForumSignatureText(this string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = HtmlHelper.FormatText(text, false, true, false, false, false, false);
            return text;
        }

        public static string FormatPrivateMessageText(this PrivateMessage pm)
        {
            string text = pm.Text;

            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = HtmlHelper.FormatText(text, false, true, false, true, false, false);

            return text;
        }

        public static ForumTopic GetLastTopic(this Forum forum, IForumService forumService)
        {
            if (forum == null)
                throw new ArgumentNullException("forum");

            return forumService.GetTopicById(forum.LastTopicId);
        }

        public static ForumPost GetLastPost(this Forum forum, IForumService forumService)
        {
            if (forum == null)
                throw new ArgumentNullException("forum");

            return forumService.GetPostById(forum.LastPostId);
        }

        public static KoreUser GetLastPostCustomer(this Forum forum, IMembershipService membershipService)
        {
            if (forum == null)
                throw new ArgumentNullException("forum");

            return membershipService.GetUserById(forum.LastPostUserId);
        }

        public static ForumPost GetFirstPost(this ForumTopic forumTopic, IForumService forumService)
        {
            if (forumTopic == null)
                throw new ArgumentNullException("forumTopic");

            var forumPosts = forumService.GetAllPosts(forumTopic.Id, null, string.Empty, 0, 1);
            if (forumPosts.Count > 0)
                return forumPosts[0];

            return null;
        }

        public static ForumPost GetLastPost(this ForumTopic forumTopic, IForumService forumService)
        {
            if (forumTopic == null)
                throw new ArgumentNullException("forumTopic");

            return forumService.GetPostById(forumTopic.LastPostId);
        }

        public static KoreUser GetLastPostCustomer(this ForumTopic forumTopic, IMembershipService membershipService)
        {
            if (forumTopic == null)
                throw new ArgumentNullException("forumTopic");

            return membershipService.GetUserById(forumTopic.LastPostUserId);
        }
    }
}