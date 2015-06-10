using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using Kore.Net.Mail;
using Kore.Web.Configuration;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Models;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Mvc.Recaptcha;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Controllers
{
    [RouteArea("")]
    [RoutePrefix("kore-cms")]
    public class FormBlockController : KoreController
    {
        private readonly IEmailSender emailSender;
        private readonly IEnumerable<IFormBlockProcessor> processors;
        private readonly Lazy<CaptchaSettings> captchaSettings;

        public FormBlockController(
            IEmailSender emailSender,
            IEnumerable<IFormBlockProcessor> processors,
            Lazy<CaptchaSettings> captchaSettings)
        {
            this.emailSender = emailSender;
            this.processors = processors;
            this.captchaSettings = captchaSettings;
        }

        [Compress]
        [HttpPost]
        [Route("form-content-block/save")]
        [ValidateInput(false)]
        public ActionResult Save(FormCollection formCollection)
        {
            string id = formCollection["Id"];
            bool enableCaptcha = Convert.ToBoolean(formCollection["EnableCaptcha"]);
            string thankYouMessage = formCollection["ThankYouMessage"];
            string redirectUrl = formCollection["RedirectUrl"];
            string emailAddress = formCollection["EmailAddress"];
            string contentBlockTitle = formCollection["ContentBlockTitle"];

            #region Validate Captcha

            if (enableCaptcha)
            {
                var recaptchaVerificationHelper = this.GetRecaptchaVerificationHelper(captchaSettings.Value.PrivateKey);

                if (string.IsNullOrEmpty(recaptchaVerificationHelper.Response))
                {
                    var result = new SaveResultModel
                    {
                        Success = false,
                        Message = T(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.PleaseEnterCaptcha).Text,
                        RedirectUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Content("~/")
                    };

                    if (Request.IsAjaxRequest())
                    {
                        return Json(result);
                    }

                    return View("Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.FormBlock.SaveResult", result);
                }

                var recaptchaVerificationResult = recaptchaVerificationHelper.VerifyRecaptchaResponse();

                if (recaptchaVerificationResult != RecaptchaVerificationResult.Success)
                {
                    var result = new SaveResultModel
                    {
                        Success = false,
                        Message = T(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.PleaseEnterCorrectCaptcha).Text,
                        RedirectUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Content("~/")
                    };

                    if (Request.IsAjaxRequest())
                    {
                        return Json(result);
                    }

                    return View("Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.FormBlock.SaveResult", result);
                }
            }

            #endregion Validate Captcha

            var values = Request.Form.AllKeys.ToDictionary(key => key, key => (object)formCollection[key]);

            // Remove some items
            values.Remove("Id");
            values.Remove("EnableCaptcha");
            values.Remove("captcha_challenge");
            values.Remove("captcha_response");
            values.Remove("ThankYouMessage");
            values.Remove("RedirectUrl");
            values.Remove("EmailAddress");
            values.Remove("ContentBlockTitle");
            values.Remove("X-Requested-With");

            var subject = contentBlockTitle;

            #region Render Email Body

            string body = string.Empty;

            var viewEngineResult = ViewEngines.Engines.FindView(ControllerContext, "MessageTemplate", null);

            // If someone has provided a custom template (see LocationFormatProvider)
            if (viewEngineResult.View != null)
            {
                body = RenderRazorPartialViewToString("MessageTemplate", values);
            }
            else
            {
                body = RenderRazorPartialViewToString(
                    "Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.FormBlock.MessageTemplate",
                    values);
            }

            #endregion Render Email Body

            var mailMessage = new MailMessage
            {
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            //mailMessage.To.Add(emailAddress);

            if (Request.Files.Count > 0)
            {
                foreach (var fileName in Request.Files.AllKeys)
                {
                    var file = Request.Files[fileName];
                    if (file != null && file.ContentLength > 0)
                    {
                        mailMessage.Attachments.Add(new Attachment(file.InputStream, file.FileName));
                    }
                }
            }

            #region Custom Processing

            try
            {
                foreach (var processor in processors)
                {
                    processor.Process(formCollection, mailMessage);
                }
            }
            catch (Exception x)
            {
                Logger.Error("Error while trying to process form block.", x);
            }

            #endregion Custom Processing

            try
            {
                // Clear the Recipients list in case it's been set by an IFormBlockProcessor
                mailMessage.To.Clear();
                mailMessage.To.Add(emailAddress);
                emailSender.Send(mailMessage);

                var result = new SaveResultModel
                {
                    Success = true,
                    Message = thankYouMessage,
                    RedirectUrl = !string.IsNullOrWhiteSpace(redirectUrl) ? redirectUrl : Url.Content("~/")
                };

                if (Request.IsAjaxRequest())
                {
                    return Json(result);
                }

                return View("Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.FormBlock.SaveResult", result);
            }
            catch (Exception x)
            {
                Logger.Error(x.Message, x);

                var result = new SaveResultModel
                {
                    Success = false,
                    Message = x.GetBaseException().Message,
                    RedirectUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Content("~/")
                };

                if (Request.IsAjaxRequest())
                {
                    return Json(result);
                }

                return View("Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.FormBlock.SaveResult", result);
            }
        }
    }
}