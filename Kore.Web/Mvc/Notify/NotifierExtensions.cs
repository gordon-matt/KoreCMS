using Kore.Localization;

namespace Kore.Web.Mvc.Notify
{
    public static class NotifierExtensions
    {
        /// <summary>
        /// Adds a new UI notification of type Information
        /// </summary>
        /// <seealso cref="Kore.Web.Mvc.Notify.INotifier.Add()"/>
        /// <param name="message">A localized message to display</param>
        public static void Info(this INotifier notifier, string message)
        {
            notifier.Add(NotifyType.Info, message);
        }

        /// <summary>
        /// Adds a new UI notification of type Warning
        /// </summary>
        /// <seealso cref="Kore.Web.Mvc.Notify.INotifier.Add()"/>
        /// <param name="message">A localized message to display</param>
        public static void Warning(this INotifier notifier, string message)
        {
            notifier.Add(NotifyType.Warning, message);
        }

        /// <summary>
        /// Adds a new UI notification of type Error
        /// </summary>
        /// <seealso cref="Kore.Web.Mvc.Notify.INotifier.Add()"/>
        /// <param name="message">A localized message to display</param>
        public static void Error(this INotifier notifier, string message)
        {
            notifier.Add(NotifyType.Error, message);
        }

        /// <summary>
        /// Adds a new UI notification of type Success
        /// </summary>
        /// <seealso cref="Kore.Web.Mvc.Notify.INotifier.Add()"/>
        /// <param name="message">A localized message to display</param>
        public static void Success(this INotifier notifier, string message)
        {
            notifier.Add(NotifyType.Success, message);
        }
    }
}