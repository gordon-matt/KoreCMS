using System;
using System.Web.Mvc;

namespace Kore.Web.Mvc.Bootstrap.Version3
{
    public class Bootstrap3<TModel>
    {
        private readonly HtmlHelper<TModel> helper;

        internal Bootstrap3(HtmlHelper<TModel> helper)
        {
            this.helper = helper;
        }

        #region Modal (Dialog)

        public ModalBuilder<TModel> Begin(Modal modal)
        {
            if (modal == null)
            {
                throw new ArgumentNullException("modal");
            }

            return new ModalBuilder<TModel>(this.helper, modal);
        }

        #endregion Modal (Dialog)
    }
}