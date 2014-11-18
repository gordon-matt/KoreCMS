using System;

namespace Kore.Web.ContentManagement.Messaging
{
    public class MessageTemplate
    {
        public MessageTemplate(string name, string subject = null)
        {
            Name = name;
            Subject = subject ?? name;
        }

        public Guid? OwnerId { get; set; }

        public string Name { get; private set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}