using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class WidgetsController : GenericODataController<Widget, Guid>
    {
        public WidgetsController(IRepository<Widget> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(Widget entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Widget entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<Widget> Get()
        {
            return Repository.Table.Where(x => x.PageId == null);
        }

        [EnableQuery]
        [HttpPost]
        public virtual IQueryable<Widget> GetByPageId(ODataActionParameters parameters)
        {
            var pageId = (Guid)parameters["pageId"];

            return Repository.Table
                .Where(x => x.PageId == pageId && x.RefId == null)
                .OrderBy(x => x.ZoneId)
                .ThenBy(x => x.Order);
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, Widget entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(entity.Id))
            {
                return BadRequest();
            }

            try
            {
                Save(entity);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(key))
                {
                    return NotFound();
                }
                else { throw; }
            }

            return Updated(entity);
        }

        public override IHttpActionResult Post(Widget entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //SetNewId(entity);

            Save(entity);

            return Created(entity);
        }

        private void Save(Widget entity)
        {
            var widgetType = Type.GetType(entity.WidgetType);
            var widgets = EngineContext.Current.ResolveAll<IWidget>();
            var widget = widgets.First(x => x.GetType() == widgetType);

            var records = entity.Id == Guid.Empty
                ? new List<Widget> { new Widget() }
                : Repository.Table.Where(x => x.Id == entity.Id || x.RefId == entity.Id).ToList();

            // TODO: this shouldn't be from widget values, but from the localized properties on the page
            //var values = JsonConvert.DeserializeObject<ExpandoObject>(entity.WidgetValues) as IDictionary<string, object>;

            if (entity.Id != Guid.Empty)
            {
                var languageManager = EngineContext.Current.Resolve<ILanguageManager>();
                var languages = languageManager.GetActiveLanguages();
                if (languages.Count() > 1)
                {
                    foreach (var language in languages)
                    {
                        bool localized = false;
                        try
                        {
                            //localized = Convert.ToBoolean(values["Localized." + language.CultureCode].ToString().Split(',')[0]);
                        }
                        catch
                        {
                            localized = false;
                        }

                        var translatedRecord = records.FirstOrDefault(x => x.CultureCode == language.CultureCode);
                        if (localized)
                        {
                            if (translatedRecord == null)
                            {
                                translatedRecord = new Widget
                                {
                                    CultureCode = language.CultureCode,
                                    RefId = entity.Id
                                };
                                records.Add(translatedRecord);
                            }
                        }
                        else
                        {
                            if (translatedRecord != null)
                            {
                                records.Remove(translatedRecord);
                                Repository.Delete(translatedRecord);
                            }
                        }
                    }
                }
            }

            var toInsert = new List<Widget>();
            var toUpdate = new List<Widget>();

            foreach (var record in records)
            {
                record.WidgetName = widget.Name;
                record.WidgetType = entity.WidgetType;
                record.WidgetValues = entity.WidgetValues;

                if (string.IsNullOrWhiteSpace(record.WidgetValues))
                {
                    // Fix to ensure it can be deserialized. If the value is an empty string, it won't deserialize
                    //  properly. For example, here: IWidgetService.GetWidgets(IEnumerable<Widget> records).
                    //  The following line in that method fails (returns null) if WidgetValues is null or empty:
                    //  widget = (IWidget)record.WidgetValues.JsonDeserialize(widgetType);
                    record.WidgetValues = "{}";
                }

                if (entity.PageId.HasValue)
                {
                    record.PageId = entity.PageId;
                }

                widget.Title = entity.Title;
                widget.Order = entity.Order;
                widget.Enabled = entity.IsEnabled;
                widget.ZoneId = entity.ZoneId;
                widget.DisplayCondition = entity.DisplayCondition;

                if (string.IsNullOrEmpty(record.CultureCode)) // main record
                {
                    if (entity.Id == Guid.Empty)
                    {
                        record.Id = Guid.NewGuid();
                        toInsert.Add(record);
                    }
                    else
                    {
                        record.Id = entity.Id;
                        toUpdate.Add(record);
                    }

                    widget.Id = record.Id;
                }
                else
                {
                    //For now, just continue
                    continue;

                    //var localizedValues =
                    //    values.Keys.Where(key => key.Contains("." + record.CultureCode))
                    //        .ToDictionary(
                    //            k => k.Replace("." + record.CultureCode, ""),
                    //            v => values[v]);

                    //if (!localizedValues.Any())
                    //{
                    //    continue;
                    //}

                    widget.RefId = entity.Id;
                    widget.CultureCode = record.CultureCode;

                    if (entity.Id == Guid.Empty)
                    {
                        toInsert.Add(record);
                    }
                    else
                    {
                        toUpdate.Add(record);
                    }
                }

                record.Title = widget.Title;
                record.Order = widget.Order;
                record.IsEnabled = widget.Enabled;
                record.ZoneId = widget.ZoneId;
                record.DisplayCondition = widget.DisplayCondition;
            }

            Repository.Insert(toInsert);
            Repository.Update(toUpdate);
        }
    }
}