﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Kore.Infrastructure;
//using Kore.Web.Infrastructure;
//using Kore.Web.Mvc.Resources;

//namespace Kore.Web.ContentManagement.Infrastructure
//{
//    public class RequireJSConfigProvider : IRequireJSConfigProvider
//    {
//        #region IRequireJSConfigProvider Members

//        public IDictionary<string, string> Paths
//        {
//            get
//            {
//                var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
//                var scriptRegister = new ScriptRegister(workContext);

//                var paths = new Dictionary<string, string>();

//                paths.Add("blog-posts", scriptRegister.GetBundleUrl("kore-cms/blog-posts"));

//                return paths;
//            }
//        }

//        public IDictionary<string, string[]> Shim
//        {
//            get
//            {
//                var shim = new Dictionary<string, string[]>();

//                shim.Add("blog", new[] { "blog-posts", "blog-categories", "blog-tags" });

//                return shim;
//            }
//        }

//        #endregion
//    }
//}

