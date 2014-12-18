using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;

namespace Kore.Indexing
{
    public class SearchWidget : WidgetBase
    {
        //public override string Name
        //{
        //    get { return "Search Form Widget"; }
        //}

        //public override void BuildDisplay(HtmlTextWriter writer, ViewContext viewContext, IWorkContextAccessor workContextAccessor)
        //{
        //    var urlHelper = new UrlHelper(viewContext.RequestContext);
        //    var localizer = LocalizationUtilities.Resolve(viewContext, "MvcCornerstone.Indexing.SearchWidget");
        //    if (!string.IsNullOrEmpty(CssClass))
        //    {
        //        writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
        //    }
        //    writer.RenderBeginTag(HtmlTextWriterTag.Div);

        //    writer.Write("<form class=\"form-search\" method=\"get\" action=\"{0}\">", urlHelper.Action("Search", "Search", new { area = CornerstoneConstants.Areas.Indexing }));
        //    writer.Write("<i class=\"cx-icon cx-icon-search\"></i>");
        //    writer.Write("<input type=\"text\" class=\"search-query\" name=\"q\" value=\"{0}\" placeholder=\"{1}\" />", viewContext.HttpContext.Request.QueryString["q"], localizer("Search"));
        //    writer.Write("<button type=\"submit\" class=\"btn\">Search</button>");
        //    writer.Write("</form>");

        //    writer.RenderEndTag(); // div
        //}
        public override string Name
        {
            get { return "Search Form Widget"; }
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
