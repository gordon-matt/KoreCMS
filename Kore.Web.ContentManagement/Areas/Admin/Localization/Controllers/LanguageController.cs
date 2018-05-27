using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Kore.Localization.Domain;
using Kore.Localization.Services;
using Kore.Web.ContentManagement.Areas.Admin.Localization.Models;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Localization)]
    [RoutePrefix("languages")]
    public class LanguageController : KoreController
    {
        private readonly Lazy<ILanguageService> languageService;
        private readonly Lazy<ILocalizableStringService> localizableStringService;

        public LanguageController(
            Lazy<ILanguageService> languageService,
            Lazy<ILocalizableStringService> localizableStringService)
        {
            this.languageService = languageService;
            this.localizableStringService = localizableStringService;
        }

        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(CmsPermissions.LanguagesRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Localization.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Localization.Languages));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Localization.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Localization.Languages);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Localization.Views.Language.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Create = T(KoreWebLocalizableStrings.General.Create).Text,
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                Localize = T(KoreCmsLocalizableStrings.Localization.Localize).Text,
                ResetLocalizableStringsError = T(KoreWebLocalizableStrings.Localization.ResetLocalizableStringsError).Text,
                ResetLocalizableStringsSuccess = T(KoreWebLocalizableStrings.Localization.ResetLocalizableStringsSuccess).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Name = T(KoreCmsLocalizableStrings.Localization.LanguageModel.Name).Text,
                    CultureCode = T(KoreCmsLocalizableStrings.Localization.LanguageModel.CultureCode).Text,
                    IsEnabled = T(KoreCmsLocalizableStrings.Localization.LanguageModel.IsEnabled).Text,
                    SortOrder = T(KoreCmsLocalizableStrings.Localization.LanguageModel.SortOrder).Text,
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("import-language-pack")]
        public JsonResult ImportFile()
        {
            try
            {
                #region Save File

                var file = Request.Files["Upload"];
                var stream = file.InputStream;

                string uploadFileName = Path.Combine(
                    Server.MapPath("~/App_Data/CMS/Localization/Languages/Uploads"),
                    string.Format("LanguagePack_{0:yyyy-MM-dd_HHmmss}.json", DateTime.Now));

                string directory = Path.GetDirectoryName(uploadFileName);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var fs = new FileStream(uploadFileName, FileMode.Create, FileAccess.Write))
                using (var bw = new BinaryWriter(fs))
                {
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    bw.Write(bytes);
                }

                #endregion Save File

                #region Read File

                string json = System.IO.File.ReadAllText(uploadFileName);
                var languagePackFile = json.JsonDeserialize<LanguagePackFile>();

                #endregion Read File

                #region Update Database

                if (string.IsNullOrEmpty(languagePackFile.CultureCode))
                {
                    return Json(new { Success = false, error = "Cannot import language pack for the invariant culture. Please provide a culture code." });
                }

                bool cultureExistsInDb = false;
                using (var connection = languageService.Value.OpenConnection())
                {
                    cultureExistsInDb = connection.Query(x => x.CultureCode == languagePackFile.CultureCode).Any();
                }

                int tenantId = WorkContext.CurrentTenant.Id;
                if (!cultureExistsInDb)
                {
                    try
                    {
                        var culture = CultureInfo.GetCultureInfo(languagePackFile.CultureCode);
                        languageService.Value.Insert(new Language
                        {
                            Id = Guid.NewGuid(),
                            TenantId = tenantId,
                            CultureCode = languagePackFile.CultureCode,
                            Name = culture.DisplayName
                        });
                    }
                    catch (CultureNotFoundException)
                    {
                        return Json(new { Success = false, error = "The specified culture code is not recognized." });
                    }
                }

                var localizedStrings = languagePackFile.LocalizedStrings.Select(x => new LocalizableString
                {
                    TenantId = tenantId,
                    CultureCode = languagePackFile.CultureCode,
                    TextKey = x.Key,
                    TextValue = x.Value
                });

                // Ignore strings that don't have an invariant version
                var allInvariantStrings = localizableStringService.Value
                    .Find(x => x.TenantId == tenantId && x.CultureCode == null)
                    .Select(x => x.TextKey);

                var toInsert = localizedStrings.Where(x => allInvariantStrings.Contains(x.TextKey));

                localizableStringService.Value.Insert(toInsert);

                #endregion Update Database

                return Json(new { Success = true, Message = "Successfully imported language pack!" });
            }
            catch (Exception x)
            {
                return Json(new { Success = false, error = x.GetBaseException().Message });
            }
        }
    }
}