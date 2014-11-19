//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Web.Mvc;
//using Kore.Data;
//using Kore.Web.Mvc.DynaGrid;
//using Kore.Web.Mvc.Optimization;

//namespace Kore.Web.Mvc.JQGrid
//{
//    public abstract class JQGridController<TKey, TEntity, TModel> : KoreController
//        where TKey : struct
//        where TEntity : class, IEntity<TKey>, new()
//        where TModel : class, IEntity<TKey>, new()
//    {
//        private Dictionary<Expression<Func<TEntity, dynamic>>, Func<TEntity, string>> columns;
//        private readonly bool isEmbeddedView;
//        private readonly string viewFolderPath;
//        private string name;
//        private string pluralizedName;

//        public JQGridController(IDynaGridDataService<TEntity> service, bool isEmbeddedView = false, string viewFolderPath = null)
//        {
//            columns = new Dictionary<Expression<Func<TEntity, dynamic>>, Func<TEntity, string>>();
//            this.Service = service;
//            this.viewFolderPath = viewFolderPath;
//            this.isEmbeddedView = isEmbeddedView;

//            if (isEmbeddedView && string.IsNullOrWhiteSpace(viewFolderPath))
//            {
//                throw new ArgumentException("'viewFolderPath' is required for embedded views.");
//            }
//        }

//        protected virtual bool CanEdit
//        {
//            get { return true; }
//        }

//        protected virtual bool CanDelete
//        {
//            get { return true; }
//        }

//        protected virtual string Name
//        {
//            get { return name ?? (name = typeof(TEntity).Name.SpacePascal()); }
//        }

//        protected virtual string PluralizedName
//        {
//            get { return pluralizedName ?? (pluralizedName = Name.Pluralize(WorkContext.CurrentCultureCode)); }
//        }

//        protected virtual IDynaGridDataService<TEntity> Service { get; private set; }

//        protected void AddColumn(Expression<Func<TEntity, dynamic>> column, Func<TEntity, string> htmlBuilder = null)
//        {
//            columns.Add(column, htmlBuilder);
//        }

//        #region CRUD

//        [CompressFilter]
//        [Route("")]
//        public virtual ActionResult Index()
//        {
//            if (!CheckPermissions())
//            {
//                return new HttpUnauthorizedResult();
//            }

//            ViewBag.Title = PluralizedName;

//            if (isEmbeddedView)
//            {
//                return View(viewFolderPath + ".Index");
//            }
//            return View("Index");
//        }

//        [Route("create")]
//        public virtual ActionResult Create()
//        {
//            if (!CheckPermissions())
//            {
//                return new HttpUnauthorizedResult();
//            }

//            ViewBag.Title = string.Format(T(LocalizableStrings.General.CreateFormat), Name);

//            if (Request.IsAjaxRequest())
//            {
//                if (isEmbeddedView)
//                {
//                    return PartialView(viewFolderPath + ".Edit", new TModel());
//                }
//                return PartialView("Edit", new TModel());
//            }

//            if (isEmbeddedView)
//            {
//                return View(viewFolderPath + ".Edit", new TModel());
//            }
//            return View("Edit", new TModel());
//        }

//        [Route("edit/{id}")]
//        public virtual ActionResult Edit(TKey id)
//        {
//            if (!CheckPermissions())
//            {
//                return new HttpUnauthorizedResult();
//            }

//            ViewBag.Title = string.Format(T(LocalizableStrings.General.EditFormat), Name);

//            var entity = Service.Find(id);
//            var model = ConvertToModel(entity);

//            if (Request.IsAjaxRequest())
//            {
//                if (isEmbeddedView)
//                {
//                    return PartialView(viewFolderPath + ".Edit", model);
//                }
//                return PartialView("Edit", model);
//            }

//            if (isEmbeddedView)
//            {
//                return View(viewFolderPath + ".Edit", model);
//            }
//            return View("Edit", model);
//        }

//        [HttpPost]
//        [Route("delete/{id}")]
//        public virtual ActionResult Delete(TKey id)
//        {
//            if (!CheckPermissions())
//            {
//                return new HttpUnauthorizedResult();
//            }

//            try
//            {
//                var existing = Service.Find(id);
//                int rowsAffected = Service.Delete(existing);

//                if (Request.IsAjaxRequest())
//                {
//                    if (rowsAffected > 0)
//                    {
//                        return Json(new { Success = true, Message = T(LocalizableStrings.Messages.DeleteRecordSuccess).Text });
//                    }
//                    return Json(new { Success = false, Message = T(LocalizableStrings.Messages.DeleteRecordError).Text });
//                }
//                else
//                {
//                    return ReturnToIndex();
//                }
//            }
//            catch (Exception x)
//            {
//                if (Request.IsAjaxRequest())
//                {
//                    return Json(new { Success = false, Message = string.Format(T(LocalizableStrings.Messages.DeleteRecordErrorFormat), x.GetBaseException().Message) });
//                }
//                return ReturnToIndex();
//            }
//        }

//        [ValidateInput(false)]
//        [HttpPost]
//        [Route("update")]
//        public virtual ActionResult Update(TModel model)
//        {
//            if (!CheckPermissions())
//            {
//                return new HttpUnauthorizedResult();
//            }

//            try
//            {
//                int rowsAffected = 0;
//                if (model.Id.Equals(default(TKey)))
//                {
//                    var entity = new TEntity();
//                    ConvertFromModel(model, entity);
//                    rowsAffected = Service.Insert(entity);
//                }
//                else
//                {
//                    var entity = Service.Find(model.Id);
//                    ConvertFromModel(model, entity);
//                    rowsAffected = Service.Update(entity);
//                }

//                if (Request.IsAjaxRequest())
//                {
//                    if (rowsAffected > 0)
//                    {
//                        return Json(new { Success = true, Message = T(LocalizableStrings.Messages.UpdateRecordSuccess).Text });
//                    }

//                    return Json(new { Success = false, Message = T(LocalizableStrings.Messages.UpdateRecordError).Text });
//                }
//                return ReturnToIndex();
//            }
//            catch (Exception x)
//            {
//                if (Request.IsAjaxRequest())
//                {
//                    return Json(new { Success = false, Message = string.Format(T(LocalizableStrings.Messages.UpdateRecordErrorFormat), x.GetBaseException().Message) });
//                }
//                return ReturnToIndex();
//            }
//        }

//        [Route("get-grid-data")]
//        public virtual ActionResult GetGridData(DynaGridAjaxRequest request)
//        {
//            int totalRecordsCount;
//            var records = Service.GetRecords(request, out totalRecordsCount);

//            var result = new DynaGridResult<TEntity>(x => x.Id)
//            {
//                Request = request,
//                Records = records,
//                TotalRecords = totalRecordsCount
//            };

//            foreach (var keyValue in columns)
//            {
//                result.AddColumn(keyValue.Key, keyValue.Value);
//            }

//            if (CanEdit)
//            {
//                result.Actions.Add(x => string.Format("<a href=\"{0}\" class=\"btn btn-default btn-xs\">{1}</a>", Url.Action("Edit", new { id = x.Id }), T(LocalizableStrings.General.Edit)));
//            }

//            if (CanDelete)
//            {
//                result.Actions.Add(x => string.Format("<a href=\"{0}\" class=\"btn btn-danger btn-xs\">{1}</a>", Url.Action("Delete", new { id = x.Id }), T(LocalizableStrings.General.Delete)));
//            }

//            return result;
//        }

//        #endregion

//        protected virtual ActionResult ReturnToIndex()
//        {
//            if (isEmbeddedView)
//            {
//                return View(viewFolderPath + ".Index");
//            }
//            return View("Index");
//        }

//        protected virtual bool CheckPermissions()
//        {
//            return true;
//        }

//        protected abstract void ConvertFromModel(TModel model, TEntity entity);

//        protected abstract TModel ConvertToModel(TEntity entity);
//    }
//}