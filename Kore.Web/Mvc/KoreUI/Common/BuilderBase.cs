using System;
using System.IO;
using System.Web.Mvc;

namespace Kore.Web.Mvc.KoreUI
{
    public abstract class BuilderBase<TModel, T> : IDisposable where T : HtmlElement
    {
        // Fields
        protected readonly T element;

        protected readonly TextWriter textWriter;
        protected readonly HtmlHelper<TModel> htmlHelper;

        // Methods
        public BuilderBase(HtmlHelper<TModel> htmlHelper, T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.element = element;
            this.textWriter = htmlHelper.ViewContext.Writer;
            this.element.StartTag(textWriter);
            this.htmlHelper = htmlHelper;
        }

        public virtual void Dispose()
        {
            this.element.EndTag(textWriter);
        }
    }
}