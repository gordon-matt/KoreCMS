using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using Kore.Plugins.Messaging.Forums.Data.Domain;
using Kore.Web.Infrastructure;

namespace Kore.Plugins.Messaging.Forums.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Forum>("ForumApi");
            builder.EntitySet<ForumGroup>("ForumGroupApi");

            config.MapODataServiceRoute("OData_Kore_Plugin_Forums", "odata/kore/plugins/forums", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members
    }
}