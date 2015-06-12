using System;
using System.Web.Mvc;

namespace Kore.Web.Common.Html
{
    public class Metro<TModel>
    {
        private readonly HtmlHelper<TModel> helper;

        internal Metro(HtmlHelper<TModel> helper)
        {
            this.helper = helper;
        }

        public TilesBuilder<TModel> Begin(Tiles tiles)
        {
            if (tiles == null)
            {
                throw new ArgumentNullException("tiles");
            }

            return new TilesBuilder<TModel>(this.helper, tiles);
        }
    }
}