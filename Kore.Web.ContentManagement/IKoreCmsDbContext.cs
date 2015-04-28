using System.Data.Entity;
using Kore.Logging.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Media.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Messaging.Domain;

namespace Kore.Web.ContentManagement
{
    public interface IKoreCmsDbContext
    {
        DbSet<BlogEntry> Blog { get; set; }

        DbSet<ContentBlock> ContentBlocks { get; set; }

        DbSet<HistoricPage> HistoricPages { get; set; }

        DbSet<LogEntry> Log { get; set; }

        DbSet<MediaPart> MediaParts { get; set; }

        DbSet<MediaPartType> MediaPartTypes { get; set; }

        DbSet<MenuItem> MenuItems { get; set; }

        DbSet<Menu> Menus { get; set; }

        DbSet<MessageTemplate> MessageTemplates { get; set; }

        DbSet<Page> Pages { get; set; }

        DbSet<QueuedEmail> QueuedEmails { get; set; }

        DbSet<QueuedSms> QueuedSms { get; set; }

        DbSet<Zone> Zones { get; set; }
    }
}