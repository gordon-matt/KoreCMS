using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using Castle.Core.Logging;
using Kore.Net.Mail;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Models;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Controllers
{
    [RouteArea("")]
    [RoutePrefix("kore-cms")]
    public class FormBlockController : KoreController
    {
        private readonly IEmailSender emailSender;

        public FormBlockController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        [Compress]
        [HttpPost]
        [Route("form-content-block/save")]
        [ValidateInput(false)]
        public ActionResult Save(FormCollection formCollection)
        {
            bool enableCaptcha = Convert.ToBoolean(formCollection["EnableCaptcha"]);
            string thankYouMessage = formCollection["ThankYouMessage"];
            string redirectUrl = formCollection["RedirectUrl"];
            string emailAddress = formCollection["EmailAddress"];
            string contentBlockTitle = formCollection["ContentBlockTitle"];

            // Validate captcha
            if (enableCaptcha)
            {
                var captchaChallenge = Request.Form["captcha_challenge"];
                var captchaResponse = Request.Form["captcha_response"];

                if (string.IsNullOrEmpty(captchaResponse))
                {
                    throw new InvalidOperationException(T(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.PleaseEnterCaptcha));
                }

                var isValidCaptcha = Crypto.VerifyHashedPassword(captchaChallenge, captchaResponse);
                if (!isValidCaptcha)
                {
                    throw new InvalidOperationException(T(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.PleaseEnterCorrectCaptcha));
                }
            }

            var values = Request.Form.AllKeys.ToDictionary(key => key, key => (object)formCollection[key]);
            //var values = Request.Form.AllKeys.ToDictionary(key => key, key => (object)Request.Form[key]);

            // Remove some items
            values.Remove("EnableCaptcha");
            values.Remove("captcha_challenge");
            values.Remove("captcha_response");
            values.Remove("ThankYouMessage");
            values.Remove("RedirectUrl");
            values.Remove("EmailAddress");
            values.Remove("ContentBlockTitle");
            values.Remove("X-Requested-With");

            var subject = contentBlockTitle;
            var body = new StringBuilder();
            body.Append(subject);
            body.Append("<br/>");

            body.Append("<table style=\"font-family: 'Arial' , Source Sans Pro, sans-serif; font-size: 1.2em; padding: 7px; width: 80%; border-collapse: collapse; border-spacing: 0;\">");

            foreach (var value in values)
            {
                body.Append("<tr>");

                body.Append("<td style=\"border-color: #DDDDDD; border-style: solid; border-width: 1px; color: #000000; font-size: 1.2em; padding: 7px;\">");
                body.Append(value.Key);
                body.Append("</td>");

                body.Append("<td style=\"border-color: #DDDDDD; border-style: solid; border-width: 1px; color: #000000; font-size: 1.2em; padding: 7px;\">");
                body.Append(value.Value);
                body.Append("</td>");
            }

            body.Append("</table>");

            var mailMessage = new MailMessage
            {
                Subject = subject,
                SubjectEncoding = Encoding.UTF8,
                Body = body.ToString(),
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };
            mailMessage.To.Add(emailAddress);

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

            try
            {
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