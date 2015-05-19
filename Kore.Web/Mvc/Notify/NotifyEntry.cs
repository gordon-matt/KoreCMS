namespace Kore.Web.Mvc.Notify
{
    public enum NotifyType
    {
        Info,
        Error,
        Warning,
        Success
    }

    public class NotifyEntry
    {
        public NotifyEntry()
        {
            Type = NotifyType.Info;
        }

        public NotifyType Type { get; set; }

        public string Message { get; set; }
    }
}