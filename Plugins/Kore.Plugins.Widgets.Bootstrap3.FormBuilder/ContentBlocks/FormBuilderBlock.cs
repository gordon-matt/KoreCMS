﻿using System.Web.Mvc;
using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.Bootstrap3.FormBuilder.ContentBlocks
{
    public class FormBuilderBlock : ContentBlockBase
    {
        [AllowHtml]
        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.FormBuilderBlock.HtmlTemplate)]
        public string HtmlTemplate { get; set; }

        [AllowHtml]
        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.FormBuilderBlock.ThankYouMessage)]
        public string ThankYouMessage { get; set; }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.FormBuilderBlock.RedirectUrl)]
        public string RedirectUrl { get; set; }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.FormBuilderBlock.EmailAddress)]
        public string EmailAddress { get; set; }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.FormBuilderBlock.UseAjax)]
        public bool UseAjax { get; set; }

        public static string GenerateCaptcha()
        {
            return string.Empty;

            //var random = new Random((int)DateTime.Now.Ticks);
            ////generate new question
            //int a = random.Next(10, 99);
            //int b = random.Next(0, 50);
            //var captcha = string.Format("{0} + {1} = ?", a, b);

            //using (var memoryStream = new MemoryStream())
            //using (var bitmap = new Bitmap(130, 30))
            //using (var g = Graphics.FromImage(bitmap))
            //{
            //    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            //    g.SmoothingMode = SmoothingMode.AntiAlias;
            //    g.FillRectangle(Brushes.White, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

            //    // Add noise
            //    int i;
            //    var pen = new Pen(Color.Yellow);
            //    for (i = 1; i < 5; i++)
            //    {
            //        pen.Color = random.NextColor();

            //        var r = random.Next(0, (130 / 3));
            //        var x = random.Next(0, 130);
            //        var y = random.Next(0, 30);

            //        g.DrawEllipse(pen, x - r, y - r, r, r);
            //    }

            //    // Add question
            //    g.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

            //    //render as Jpeg
            //    bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    var base64String = Convert.ToBase64String(memoryStream.GetBuffer());
            //    var hashedPassword = Crypto.HashPassword((a + b).ToString(CultureInfo.InvariantCulture));

            //    return string.Format("<img src=\"data:image/jpeg;base64,{0}\" class=\"form-captcha\" /><input type=\"hidden\" name=\"captcha_challenge\" value=\"{1}\" />", base64String, hashedPassword);
            //}
        }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Bootstrap 3: Form Builder"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Widgets.Bootstrap3.FormBuilder/Views/Shared/DisplayTemplates/FormBuilderBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Widgets.Bootstrap3.FormBuilder/Views/Shared/EditorTemplates/FormBuilderBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}