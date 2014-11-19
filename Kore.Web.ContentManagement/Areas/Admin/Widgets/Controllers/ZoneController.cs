//using System;
//using System.Linq;
//using System.Web.Mvc;
//using Kore.Web.ContentManagement.Areas.Admin.Widgets.Domain;
//using Kore.Web.ContentManagement.Areas.Admin.Widgets.Models;
//using Kore.Web.ContentManagement.Areas.Admin.Widgets.Services;
//using Kore.Web.Mvc;
//using Kore.Web.Mvc.RoboUI;
//using Kore.Web.Mvc.Routing;

//namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Controllers
//{
//    [Authorize]
//    [RouteArea(Constants.Areas.Widgets)]
//    [RoutePrefix("zones")]
//    public class ZoneController : KoreController
//    {
//        private readonly IZoneService zoneService;

//        public ZoneController(IZoneService zoneService)
//        {
//            this.zoneService = zoneService;
//        }

//        [Route("")]
//        public ActionResult Index()
//        {
//            if (!CheckPermission(WidgetPermissions.ManageWidgets))
//            {
//                return new HttpUnauthorizedResult();
//            }

//            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Widgets.Title), Url.Action("Index", "Widget"));
//            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Widgets.Zones));

//            var result = new RoboUIGridResult<Zone>(ControllerContext)
//            {
//                Title = T(LocalizableStrings.Widgets.ManageZones),
//                FormActionUrl = Url.Action("Update"),
//                FetchAjaxSource = GetZones,
//                EnablePaginate = false,
//                ActionsColumnWidth = 130
//            };

//            result.AddColumn(x => x.Name);

//            result.AddAction()
//                .HasText(T(KoreWebLocalizableStrings.General.Create))
//                .HasIconCssClass("kore-icon kore-icon-add")
//                .HasUrl(Url.Action("Create", RouteData.Values))
//                .HasButtonStyle(ButtonStyle.Primary)
//                .ShowModalDialog();

//            result.AddAction()
//                .HasText(T(LocalizableStrings.Widgets.Title))
//                .HasUrl(Url.Action("Index", "Widget"))
//                .HasButtonStyle(ButtonStyle.Default);

//            result.AddRowAction()
//                .HasText(T(KoreWebLocalizableStrings.General.Edit))
//                .HasUrl(x => Url.Action("Edit", RouteData.Values.Merge(new { id = x.Id })))
//                .HasButtonSize(ButtonSize.ExtraSmall)
//                .ShowModalDialog();

//            result.AddRowAction(true)
//                .HasText(T(KoreWebLocalizableStrings.General.Delete))
//                .HasName("Delete")
//                .HasValue(x => x.Id.ToString())
//                .HasButtonSize(ButtonSize.ExtraSmall)
//                .HasButtonStyle(ButtonStyle.Danger)
//                .HasConfirmMessage(T(KoreWebLocalizableStrings.Messages.ConfirmDeleteRecord));

//            result.AddReloadEvent("UPDATE_ENTITY_COMPLETE");
//            result.AddReloadEvent("DELETE_ENTITY_COMPLETE");

//            return result;
//        }

//        [Route("create")]
//        public ActionResult Create()
//        {
//            if (!CheckPermission(WidgetPermissions.ManageWidgets))
//            {
//                return new HttpUnauthorizedResult();
//            }

//            var model = new ZoneModel();

//            var result = new RoboUIFormResult<ZoneModel>(model, ControllerContext)
//            {
//                Title = T(KoreWebLocalizableStrings.General.Create),
//                FormActionUrl = Url.Action("Update")
//            };

//            return result;
//        }

//        [ActionName("Update")]
//        [Button("Delete")]
//        [HttpPost]
//        [Route("update")]
//        public ActionResult Delete(Guid id)
//        {
//            if (!CheckPermission(WidgetPermissions.ManageWidgets))
//            {
//                return new HttpUnauthorizedResult();
//            }

//            try
//            {
//                var entity = zoneService.Find(id);
//                int rowsAffected = zoneService.Delete(entity);

//                if (rowsAffected > 0)
//                {
//                    return Json(new { Success = true, Message = T(KoreWebLocalizableStrings.Messages.DeleteRecordSuccess).Text });
//                }
//                return Json(new { Success = false, Message = T(KoreWebLocalizableStrings.Messages.DeleteRecordError).Text });
//            }
//            catch (Exception x)
//            {
//                return Json(new { Success = false, Message = string.Format(T(KoreWebLocalizableStrings.Messages.DeleteRecordErrorFormat), x.GetBaseException().Message) });
//            }
//        }

//        [Route("edit/{id}")]
//        public ActionResult Edit(Guid id)
//        {
//            if (!CheckPermission(WidgetPermissions.ManageWidgets))
//            {
//                return new HttpUnauthorizedResult();
//            }

//            var model = zoneService.Find(id);

//            var result = new RoboUIFormResult<ZoneModel>(model, ControllerContext)
//            {
//                Title = T(KoreWebLocalizableStrings.General.Edit),
//                FormActionUrl = Url.Action("Update")
//            };

//            return result;
//        }

//        [Button("Save")]
//        [HttpPost]
//        [Route("update")]
//        public ActionResult Update(ZoneModel model)
//        {
//            if (!CheckPermission(WidgetPermissions.ManageWidgets))
//            {
//                return new HttpUnauthorizedResult();
//            }

//            Zone entity = null;

//            if (model.Id == Guid.Empty)
//            {
//                entity = new Zone
//                {
//                    Id = Guid.NewGuid(),
//                    Name = model.Name
//                };
//                zoneService.Insert(entity);
//            }
//            else
//            {
//                entity = zoneService.Find(model.Id);
//                entity.Name = model.Name;
//                zoneService.Update(entity);
//            }

//            if (Request.IsAjaxRequest())
//            {
//                return Json(new { Success = true, Message = T(KoreWebLocalizableStrings.Messages.UpdateRecordSuccess).Text });
//            }
//            return RedirectToAction("Index");
//        }

//        private RoboUIGridAjaxData<Zone> GetZones(RoboUIGridRequest options)
//        {
//            var records = zoneService.Repository.Table.ToList();
//            return new RoboUIGridAjaxData<Zone>(records);
//        }
//    }
//}