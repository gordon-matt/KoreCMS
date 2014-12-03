using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Services
{
    public interface IWidgetService : IGenericDataService<Widget>
    {
        IEnumerable<IWidget> GetWidgets(Guid? pageId = null, string zoneName = null, bool includeDisabled = false);

        IEnumerable<IWidget> GetWidgets(IEnumerable<Widget> records);

        void ToggleEnabled(Widget record);
    }

    public class WidgetService : GenericDataService<Widget>, IWidgetService
    {
        private readonly ICacheManager cacheManager;
        private readonly Lazy<IRepository<Zone>> zoneRepository;

        public WidgetService(
            ICacheManager cacheManager,
            IRepository<Widget> repository,
            Lazy<IRepository<Zone>> zoneRepository)
            : base(repository)
        {
            this.cacheManager = cacheManager;
            this.zoneRepository = zoneRepository;
        }

        #region IWidgetService Members

        public override int Delete(Widget entity)
        {
            var entities = Repository.Table.Where(x => x.Id == entity.Id || x.RefId == entity.Id);
            return Delete(entities);
        }

        public IEnumerable<IWidget> GetWidgets(IEnumerable<Widget> records)
        {
            var result = new List<IWidget>();
            foreach (var record in records)
            {
                IWidget widget;
                try
                {
                    var widgetType = Type.GetType(record.WidgetType);
                    widget = (IWidget)record.WidgetValues.JsonDeserialize(widgetType);
                }
                catch { continue; }

                widget.Id = record.Id;
                widget.Title = record.Title;
                widget.ZoneId = record.ZoneId;
                widget.PageId = record.PageId;
                widget.Order = record.Order;
                widget.Enabled = record.IsEnabled;
                widget.DisplayCondition = record.DisplayCondition;
                widget.CultureCode = record.CultureCode;
                widget.RefId = record.RefId;
                result.Add(widget);
            }
            return result;
        }

        public void ToggleEnabled(Widget record)
        {
            if (record == null) return;
            record.IsEnabled = !record.IsEnabled;
            base.Update(record);
        }

        public IEnumerable<IWidget> GetWidgets(Guid? pageId = null, string zoneName = null, bool includeDisabled = false)
        {
            var key = string.Format("Widgets_GetWidgets_{0}_{1}", includeDisabled, pageId);
            if (includeDisabled)
            {
                return cacheManager.Get(key, () =>
                {
                    var zone = zoneRepository.Value.Table.FirstOrDefault(x => x.Name == zoneName);

                    if (zone == null)
                    {
                        return Enumerable.Empty<IWidget>();
                    }

                    var records = pageId.HasValue
                        ? Repository.Table.Where(x => x.ZoneId == zone.Id && x.PageId == pageId.Value)
                        : Repository.Table.Where(x => x.ZoneId == zone.Id && x.PageId == null);

                    return GetWidgets(records);
                });
            }
            else
            {
                return cacheManager.Get(key, () =>
                {
                    var zone = zoneRepository.Value.Table.FirstOrDefault(x => x.Name == zoneName);

                    if (zone == null)
                    {
                        return Enumerable.Empty<IWidget>();
                    }

                    var records = pageId.HasValue
                        ? Repository.Table.Where(x => x.IsEnabled && x.ZoneId == zone.Id && x.PageId == pageId.Value)
                        : Repository.Table.Where(x => x.IsEnabled && x.ZoneId == zone.Id && x.PageId == null);

                    return GetWidgets(records);
                });
            }
        }

        #endregion IWidgetService Members
    }
}