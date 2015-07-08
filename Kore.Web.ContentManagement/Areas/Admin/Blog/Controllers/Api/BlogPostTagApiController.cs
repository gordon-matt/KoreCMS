using System.Linq;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.Controllers.Api
{
    public class BlogPostTagApiController : ODataController
    {
        // Here only for the purpose of making Web API happy (if this is not here, it complains because Post and Tag refer to PostTag)
    }
}