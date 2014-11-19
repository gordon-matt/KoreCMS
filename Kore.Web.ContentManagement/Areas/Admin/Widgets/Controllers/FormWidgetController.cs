using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using Kore.Net.Mail;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Controllers
{
    [RouteArea(Constants.Areas.Widgets)]
    public class FormWidgetController : KoreController
    {
        private readonly IEmailSender emailSender;

        public FormWidgetController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        [HttpPost]
        [Route("form-widget/save")]
        public ActionResult Save(bool enableCaptcha, string thankYouMessage, string redirectUrl, string emailAddress, string widgetTitle)
        {
            // Validate captcha
            if (enableCaptcha)
            {
                var captchaChallenge = Request.Form["captcha_challenge"];
                var captchaResponse = Request.Form["captcha_response"];

                if (string.IsNullOrEmpty(captchaResponse))
                {
                    throw new InvalidOperationException(T("Please enter captcha validation field."));
                }

                var isValidCaptcha = Crypto.VerifyHashedPassword(captchaChallenge, captchaResponse);
                if (!isValidCaptcha)
                {
                    throw new InvalidOperationException(T("Please enter correct captcha validation field."));
                }
            }

            var values = Request.Form.AllKeys.ToDictionary(key => key, key => (object)Request.Form[key]);

            // Remove some items
            values.Remove("EnableCaptcha");
            values.Remove("captcha_challenge");
            values.Remove("captcha_response");
            values.Remove("ThankyouMessage");
            values.Remove("RedirectUrl");
            values.Remove("EmailAddress");
            values.Remove("WidgetTitle");
            values.Remove("X-Requested-With");

            var subject = widgetTitle;
            var body = new StringBuilder();
            body.Append(subject);
            body.Append("<br/>");

            body.Append("<table style=\"width: 100%; border-collapse: collapse; border-spacing: 0;\">");

            foreach (var value in values)
            {
                body.Append("<tr>");

                body.Append("<td style=\"border-color: #DDDDDD; border-style: solid; border-width: 1px; color: #000000; font-size: 12px; padding: 7px;\">");
                body.Append(value.Key);
                body.Append("</td>");

                body.Append("<td style=\"border-color: #DDDDDD; border-style: solid; border-width: 1px; color: #000000; font-size: 12px; padding: 7px;\">");
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

                //TODO: Implement this on UI
                return Json(new
                {
                    Success = true,
                    Message = thankYouMessage,
                    RedirectUrl = !string.IsNullOrWhiteSpace(redirectUrl) ? redirectUrl : Url.Content("~/")
                });
            }
            catch (Exception ex)
            {
                //TODO: Implement this on UI
                return Json(new
                {
                    Success = false,
                    Message = ex.GetBaseException().Message,
                    RedirectUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Content("~/")
                });
            }
        }
    }
}