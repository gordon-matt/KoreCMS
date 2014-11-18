using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Web.Mvc.Resources;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Mvc
{
    public interface IWebViewPage : IViewDataContainer
    {
        ScriptRegister Script { get; }

        StyleRegister Style { get; }

        Localizer T { get; }
    }

    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>, IWebViewPage
    {
        private bool initializationCompleted;
        private Localizer localizer = NullLocalizer.Instance;
        private IResourcesManager resourcesManager;

        //TODO: can we make this static? Since the items wont change (cant add at runtime)
        public IEnumerable<MenuItem> MenuItems
        {
            get { return EngineContext.Current.Resolve<INavigationManager>().BuildMenu(); }
        }

        public ScriptRegister Script { get; private set; }

        public StyleRegister Style { get; private set; }

        public Localizer T
        {
            get
            {
                if (localizer == NullLocalizer.Instance)
                {
                    localizer = LocalizationUtilities.Resolve(VirtualPath);
                }

                return localizer;
            }
        }

        public IWorkContext WorkContext { get; private set; }

        public void AppendMeta(string name, string content, string contentSeparator)
        {
            AppendMeta(new MetaEntry { Name = name, Content = content }, contentSeparator);
        }

        public virtual void AppendMeta(MetaEntry meta, string contentSeparator)
        {
            resourcesManager.AppendMeta(meta, contentSeparator);
        }

        public bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            if (authorizationService.TryCheckAccess(permission, WorkContext.CurrentUser))
            {
                return true;
            }

            return false;
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            if (initializationCompleted)
            {
                return;
            }

            WorkContext = EngineContext.Current.Resolve<IWorkContext>();
            resourcesManager = EngineContext.Current.Resolve<IResourcesManager>();
            Script = new ScriptRegister(WorkContext, this);
            Style = new StyleRegister(WorkContext);
            initializationCompleted = true;
        }

        public MvcHtmlString RenderScripts()
        {
            return Script.Render();
        }

        public MvcHtmlString RenderStyles()
        {
            return Style.Render();
        }

        public MvcHtmlString RenderMetas()
        {
            var metas = resourcesManager.GetRegisteredMetas();
            if (!metas.Any())
            {
                return null;
            }

            var sb = new StringBuilder();
            foreach (var meta in metas)
            {
                sb.Append(meta);
            }
            return new MvcHtmlString(sb.ToString());
        }

        public void SetMeta(string name, string content)
        {
            SetMeta(new MetaEntry { Name = name, Content = content });
        }

        public virtual void SetMeta(MetaEntry meta)
        {
            resourcesManager.SetMeta(meta);
        }
    }

    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}