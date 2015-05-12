//using System;
//using System.Collections.Generic;
//using Kore.Web.ContentManagement.Areas.Admin.Media.Models;
//using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
//using Kore.Web.Mvc.RoboUI;

//namespace Kore.Web.ContentManagement.Areas.Admin.Media.ContentBlocks
//{
//    public class NivoSliderBlock : ContentBlockBase
//    {
//        public override string Name
//        {
//            get { return "Nivo Slider"; }
//        }

//        [RoboChoice(RoboChoiceType.DropDownList, IsRequired = true, ContainerCssClass = "col-xs-2 col-sm-2", ContainerRowIndex = 4)]
//        public string Theme { get; set; }

//        [RoboChoice(RoboChoiceType.DropDownList, IsRequired = true, ContainerCssClass = "col-xs-2 col-sm-2", ContainerRowIndex = 4)]
//        public string Effect { get; set; }

//        [RoboText(ContainerCssClass = "col-xs-3 col-sm-1", ContainerRowIndex = 4)]
//        public string Width { get; set; }

//        [RoboText(ContainerCssClass = "col-xs-3 col-sm-1", ContainerRowIndex = 4)]
//        public string Height { get; set; }

//        [RoboNumeric(IsRequired = true, MinimumValue = "1000", LabelText = "Pause Time", ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = 5)]
//        public int PauseTime { get; set; }

//        [RoboChoice(RoboChoiceType.CheckBox, LabelText = "Control Navigation", ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = 5)]
//        public bool ControlNavigation { get; set; }

//        [RoboChoice(RoboChoiceType.CheckBox, LabelText = "Show Thumbnails", ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = 5)]
//        public bool ShowThumbnails { get; set; }

//        [RoboGrid(1, 10, 5, ContainerCssClass = "col-xs-12 col-sm-12", ContainerRowIndex = 6)]
//        [RoboHtmlAttribute("class", "table table-striped table-bordered")]
//        public List<MediaPart> UploadPhotos { get; set; }

//        //public override void BuildDisplay(HtmlTextWriter writer, ViewContext viewContext, IWorkContextAccessor workContextAccessor)
//        //{
//        //    if (UploadPhotos == null || UploadPhotos.Count == 0)
//        //    {
//        //        return;
//        //    }

//        //    var clientId = "nivoSlider_" + Guid.NewGuid().ToString("N").ToLowerInvariant();

//        //    if (ShowTitleOnPage)
//        //    {
//        //        writer.RenderBeginTag("header");
//        //        writer.RenderBeginTag(HtmlTextWriterTag.H2);
//        //        writer.Write(Title);
//        //        writer.RenderEndTag(); // h2
//        //        writer.RenderEndTag(); // header
//        //    }

//        //    writer.AddAttribute(HtmlTextWriterAttribute.Class, "slider-wrapper " + Theme);
//        //    writer.RenderBeginTag(HtmlTextWriterTag.Div);

//        //    writer.AddAttribute(HtmlTextWriterAttribute.Id, clientId);
//        //    writer.AddAttribute(HtmlTextWriterAttribute.Class, "nivoSlider");
//        //    writer.RenderBeginTag(HtmlTextWriterTag.Div);

//        //    var queryString = "";
//        //    if (!string.IsNullOrEmpty(Width))
//        //    {
//        //        queryString += "w=" + Width;
//        //    }

//        //    if (!string.IsNullOrEmpty(Height))
//        //    {
//        //        if (string.IsNullOrEmpty(queryString))
//        //        {
//        //            queryString += "h=" + Height;
//        //        }
//        //        else
//        //        {
//        //            queryString += "&h=" + Height;
//        //        }
//        //    }

//        //    if (!string.IsNullOrEmpty(queryString))
//        //    {
//        //        queryString = "?" + queryString;
//        //    }

//        //    foreach (var slide in UploadPhotos)
//        //    {
//        //        writer.AddAttribute(HtmlTextWriterAttribute.Href, string.IsNullOrEmpty(slide.TargetUrl) ? "javascript:void(0)" : slide.TargetUrl);
//        //        writer.RenderBeginTag(HtmlTextWriterTag.A);

//        //        if (slide.Url.Contains("?"))
//        //        {
//        //            writer.AddAttribute(HtmlTextWriterAttribute.Src, slide.Url);
//        //        }
//        //        else
//        //        {
//        //            writer.AddAttribute(HtmlTextWriterAttribute.Src, slide.Url + queryString);
//        //        }

//        //        writer.AddAttribute(HtmlTextWriterAttribute.Alt, slide.Caption);
//        //        writer.AddAttribute(HtmlTextWriterAttribute.Title, slide.Caption);
//        //        writer.RenderBeginTag(HtmlTextWriterTag.Img);
//        //        writer.RenderEndTag(); // img

//        //        writer.RenderEndTag(); // a
//        //    }

//        //    writer.RenderEndTag(); // div

//        //    writer.RenderEndTag(); // div

//        //    var workContext = workContextAccessor.GetContext();
//        //    var scripRegister = new ScriptRegister(workContext);
//        //    scripRegister.IncludeInline(string.Format("$('#{0}').nivoSlider({{ controlNavThumbs: {2}, controlNav: {1}, effect: '{3}', pauseTime: {4} }});",
//        //        clientId,
//        //        ControlNavigation ? "true" : "false",
//        //        ShowThumbnails ? "true" : "false",
//        //        Effect, PauseTime));
//        //}

//        //public override ActionResult BuildEditor(Controller controller, WorkContext workContext, RoboUIFormResult<IContentBlock> roboForm)
//        //{
//        //    if (PauseTime == 0)
//        //    {
//        //        PauseTime = 1000;
//        //    }

//        //    roboForm.RegisterExternalDataSource("Theme", new Dictionary<string, string>
//        //                                                 {
//        //                                                     {"theme-default", "Default"},
//        //                                                     {"theme-bar", "Bar"},
//        //                                                     {"theme-dark", "Dark"},
//        //                                                     {"theme-light", "Light"},
//        //                                                 });

//        //    roboForm.RegisterExternalDataSource("Effect", new Dictionary<string, string>
//        //                                                 {
//        //                                                     {"random", "Random"},
//        //                                                     {"sliceDown", "Slice Down"},
//        //                                                     {"sliceDownLeft", "Slice Down Left"},
//        //                                                     {"sliceUp", "Slice Up"},
//        //                                                     {"sliceUpLeft", "Slice Up Left"},
//        //                                                     {"sliceUpDown", "Slice Up Down"},
//        //                                                     {"sliceUpDownLeft", "Slice Up Down Left"},
//        //                                                     {"fold", "Fold"},
//        //                                                     {"fade", "Fade"},
//        //                                                     {"slideInRight", "Slide In Right"},
//        //                                                     {"slideInLeft", "Slide In Left"},
//        //                                                     {"boxRandom", "Box Random"},
//        //                                                     {"boxRain", "Box Rain"},
//        //                                                     {"boxRainReverse", "Box Rain Reverse"},
//        //                                                     {"boxRainGrow", "Box Rain Grow"},
//        //                                                     {"boxRainGrowReverse", "Box Rain Grow Reverse"},
//        //                                                 });

//        //    var mediaService = workContext.Resolve<IMediaService>();
//        //    var folders = mediaService.GetMediaFolders(null);
//        //    roboForm.RegisterExternalDataSource("PhotosFolder", folders.Select(x => x.Name).ToArray());

//        //    return base.BuildEditor(controller, workContext, roboForm);
//        //}

//        //public override void RegisterResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
//        //{
//        //    scriptRegister.IncludeBundle("nivoslider");
//        //    styleRegister.IncludeBundle("nivoslider");
//        //}

//        //public override void OnSaving(WorkContext workContext)
//        //{
//        //    if (UploadPhotos != null)
//        //    {
//        //        UploadPhotos.RemoveAll(x => string.IsNullOrEmpty(x.Url));

//        //        if (UploadPhotos.Count > 0)
//        //        {
//        //            var mediaService = workContext.Resolve<IMediaService>();
//        //            mediaService.MoveFiles(UploadPhotos, "Slideshow\\" + Title);
//        //        }
//        //    }
//        //}

//        public override string DisplayTemplatePath
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public override string EditorTemplatePath
//        {
//            get { throw new NotImplementedException(); }
//        }

//        public class MediaPart : IKoreImage
//        {
//            #region IImage Members

//            [RoboFileUpload(EnableFineUploader = true, EnableBrowse = true, ColumnWidth = 250)]
//            public string Url { get; set; }

//            [RoboFileUpload(EnableFineUploader = true, EnableBrowse = true, ColumnWidth = 250)]
//            public string ThumbnailUrl { get; set; }

//            [RoboText(MaxLength = 255)]
//            public string Caption { get; set; }

//            [RoboText(MaxLength = 2048, LabelText = "Target Url")]
//            public string TargetUrl { get; set; }

//            [RoboNumeric(IsRequired = true, LabelText = "Sort Order")]
//            public int SortOrder { get; set; }

//            #endregion
//        }
//    }
//}