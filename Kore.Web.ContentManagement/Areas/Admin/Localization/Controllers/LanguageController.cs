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
using Kore.Web.Security.Membership.Permissions;
using Newtonsoft.Json.Linq;

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
            if (!CheckPermission(StandardPermissions.FullAccess))
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
            string json = string.Format(
@"{{
    Create: '{0}',
    Delete: '{1}',
    DeleteRecordConfirm: '{2}',
    DeleteRecordError: '{3}',
    DeleteRecordSuccess: '{4}',
    Edit: '{5}',
    GetRecordError: '{6}',
    InsertRecordError: '{7}',
    InsertRecordSuccess: '{8}',
    Localize: '{9}',
    ResetLocalizableStringsError: '{10}',
    ResetLocalizableStringsSuccess: '{11}',
    UpdateRecordError: '{12}',
    UpdateRecordSuccess: '{13}',
    Columns: {{
        Name: '{14}',
        CultureCode: '{15}',
        IsEnabled: '{16}',
        SortOrder: '{17}',
    }}
}}",
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(KoreCmsLocalizableStrings.Localization.Localize),
   T(KoreWebLocalizableStrings.Localization.ResetLocalizableStringsError),
   T(KoreWebLocalizableStrings.Localization.ResetLocalizableStringsSuccess),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(KoreCmsLocalizableStrings.Localization.LanguageModel.Name),
   T(KoreCmsLocalizableStrings.Localization.LanguageModel.CultureCode),
   T(KoreCmsLocalizableStrings.Localization.LanguageModel.IsEnabled),
   T(KoreCmsLocalizableStrings.Localization.LanguageModel.SortOrder));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
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

                if (!cultureExistsInDb)
                {
                    try
                    {
                        var culture = CultureInfo.GetCultureInfo(languagePackFile.CultureCode);
                        languageService.Value.Insert(new Language
                        {
                            Id = Guid.NewGuid(),
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
                    CultureCode = languagePackFile.CultureCode,
                    TextKey = x.Key,
                    TextValue = x.Value
                });

                // Ignore strings that don't have an invariant version
                var allInvariantStrings = localizableStringService.Value
                    .Find(x => x.CultureCode == null)
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