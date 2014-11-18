using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Kore.Web.ContentManagement.Areas.Admin.Media.Models;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Widgets
{
    public class PhotoGalleryWidget : WidgetBase
    {
        private static readonly Regex isPhotoFile = new Regex(@"^.*\.(jpg|gif|png)$", RegexOptions.IgnoreCase);

        public override string Name
        {
            get { return "Photo Gallery Widget"; }
        }

        [RoboText(LabelText = "Item Css Class", Order = 505, ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = 3)]
        public string ItemCssClass { get; set; }

        [RoboChoice(RoboChoiceType.DropDownList, IsRequired = true, LabelText = "Photos Folder", Order = 510, ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = 3)]
        public string PhotosFolder { get; set; }

        [RoboChoice(RoboChoiceType.CheckBox, LabelText = "Auto Show", ContainerCssClass = "col-xs-2 col-sm-2", ContainerRowIndex = 3)]
        public bool AutoShow { get; set; }

        [RoboChoice(RoboChoiceType.CheckBox, LabelText = "Auto Play", ContainerCssClass = "col-xs-1 col-sm-1", ContainerRowIndex = 3)]
        public bool AutoPlay { get; set; }

        [RoboGrid(Order = 515, ContainerCssClass = "col-xs-12 col-sm-12", ContainerRowIndex = 4)]
        [RoboHtmlAttribute("class", "table table-bordered table-condensed")]
        public List<FileCaption> Captions { get; set; }

        //public override ActionResult BuildEditor(Controller controller, WorkContext workContext, RoboUIFormResult<IWidget> form)
        //{
        //    var mediaService = EngineContext.Current.Resolve<IMediaService>();

        //    var folders = mediaService.GetMediaFolders(null);
        //    form.RegisterExternalDataSource("PhotosFolder", folders.Select(x => x.Name));

        //    var oldCaptions = new List<FileCaption>();
        //    if (Captions != null)
        //    {
        //        oldCaptions.AddRange(Captions);
        //        Captions.Clear();
        //    }
        //    else
        //    {
        //        Captions = new List<FileCaption>();
        //    }

        //    if (!string.IsNullOrEmpty(PhotosFolder))
        //    {
        //        var files = (mediaService.GetMediaFiles(PhotosFolder)).Where(IsPhotoFile).OrderBy(x => x.Name);
        //        Captions.AddRange(files.Select(file => oldCaptions.FirstOrDefault(x => x.FileName == file.Name)
        //                                               ?? new FileCaption { FileName = file.Name }));
        //    }
        //    else
        //    {
        //        form.ExcludeProperty("Captions");
        //    }

        //    return base.BuildEditor(controller, workContext, form);
        //}

        private static bool IsPhotoFile(MediaFile file)
        {
            return isPhotoFile.IsMatch(file.Name);
        }

        //public override void BuildDisplay(HtmlTextWriter writer, ViewContext viewContext, IWorkContextAccessor workContextAccessor)
        //{
        //    var workContext = workContextAccessor.GetContext(viewContext.HttpContext);
        //    var mediaService = workContext.Resolve<IMediaService>();
        //    var files = mediaService.GetMediaFiles(PhotosFolder).Where(IsPhotoFile).OrderBy(x => x.Name).ToList();

        //    if (files.Count == 0)
        //    {
        //        return;
        //    }

        //    var captions = Captions ?? new List<FileCaption>();

        //    var clientId = "gallery-" + Guid.NewGuid().ToString("N");

        //    if (!string.IsNullOrEmpty(CssClass))
        //    {
        //        writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
        //    }
        //    else
        //    {
        //        writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
        //    }

        //    writer.AddAttribute(HtmlTextWriterAttribute.Id, clientId);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Ul);

        //    foreach (var file in files)
        //    {
        //        var caption = captions.FirstOrDefault(x => x.FileName == file.Name) ?? new FileCaption();

        //        if (!string.IsNullOrEmpty(ItemCssClass))
        //        {
        //            writer.AddAttribute(HtmlTextWriterAttribute.Class, ItemCssClass);
        //        }
        //        else
        //        {
        //            writer.AddAttribute(HtmlTextWriterAttribute.Class, "col-sm-6 col-md-4");
        //        }
        //        writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //        writer.Write("<a class=\"fancybox\" rel=\"{2}\" href=\"{0}\" title=\"{1}\"><img src=\"{0}\" alt=\"{1}\" /></a>", file.MediaPath, caption.Caption, clientId);
        //        writer.RenderEndTag(); // li
        //    }

        //    writer.RenderEndTag(); // ul

        //    if (files.Count > 0)
        //    {
        //        var scriptRegister = new ScriptRegister(workContext);
        //        scriptRegister.IncludeInline(string.Format("$('#{0} .fancybox').fancybox({{ autoPlay: {1} }});", clientId, AutoPlay ? "true" : "false"));
        //        if (AutoShow)
        //        {
        //            scriptRegister.IncludeInline(string.Format("$('#{0} .fancybox:first').trigger('click');", clientId));
        //        }
        //    }
        //}

        //public override void RegisterResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        //{
        //    scriptRegister.IncludeBundle("fancybox");
        //    styleRegister.IncludeBundle("fancybox");
        //}

        public class FileCaption
        {
            [RoboText(IsReadOnly = true, LabelText = "File Name")]
            public string FileName { get; set; }

            [RoboText(MaxLength = 255)]
            public string Caption { get; set; }
        }

        public override string DisplayTemplatePath
        {
            get { throw new NotImplementedException(); }
        }

        public override string EditorTemplatePath
        {
            get { throw new NotImplementedException(); }
        }
    }
}