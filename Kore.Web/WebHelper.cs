using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Hosting;
using Castle.Core.Logging;
using Kore.Exceptions;

namespace Kore.Web
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public partial class WebHelper : IWebHelper
    {
        private readonly HttpContextBase _httpContext;
        private readonly Lazy<ILogger> logger;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        public WebHelper(
            HttpContextBase httpContext)
        {
            this._httpContext = httpContext;
            this.logger = logger;
        }

        /// <summary>
        /// Get URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        public virtual string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;

            //URL referrer is null in some case (for example, in IE 8)
            if (_httpContext != null &&
                _httpContext.Request != null &&
                _httpContext.Request.UrlReferrer != null)
                referrerUrl = _httpContext.Request.UrlReferrer.PathAndQuery;

            return referrerUrl;
        }

        /// <summary>
        /// Get context IP address
        /// </summary>
        /// <returns>URL referrer</returns>
        public virtual string GetCurrentIpAddress()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return string.Empty;

            var result = "";
            if (_httpContext.Request.Headers != null)
            {
                //look for the X-Forwarded-For (XFF) HTTP header field
                //it's used for identifying the originating IP address of a client connecting to a web server through an HTTP proxy or load balancer.
                string xff = _httpContext.Request.Headers.AllKeys
                    .Where(x => "X-FORWARDED-FOR".Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    .Select(k => _httpContext.Request.Headers[k])
                    .FirstOrDefault();

                //if you want to exclude private IP addresses, then see http://stackoverflow.com/questions/2577496/how-can-i-get-the-clients-ip-address-in-asp-net-mvc

                if (!String.IsNullOrEmpty(xff))
                {
                    string lastIp = xff.Split(new char[] { ',' }).FirstOrDefault();
                    result = lastIp;
                }
            }

            if (String.IsNullOrEmpty(result) && _httpContext.Request.UserHostAddress != null)
            {
                result = _httpContext.Request.UserHostAddress;
            }

            //some validation
            if (result == "::1")
                result = "127.0.0.1";
            //remove port
            if (!String.IsNullOrEmpty(result))
            {
                int index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                    result = result.Substring(0, index);
            }
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether current connection is secured
        /// </summary>
        /// <returns>true - secured, false - not secured</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            bool useSsl = false;
            if (_httpContext != null && _httpContext.Request != null)
            {
                useSsl = _httpContext.Request.IsSecureConnection;
                //when your hosting uses a load balancer on their server then the Request.IsSecureConnection is never got set to true, use the statement below
                //just uncomment it
                //useSSL = _httpContext.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSsl;
        }

        protected virtual bool IsRequestAvailable(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                return false;
            }

            try
            {
                if (httpContext.Request == null)
                {
                    return false;
                }
            }
            catch (HttpException)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Server variable</returns>
        public virtual string ServerVariables(string name)
        {
            string result = string.Empty;

            try
            {
                if (_httpContext == null || _httpContext.Request == null)
                    return result;

                //put this method is try-catch
                //as described here
                if (_httpContext.Request.ServerVariables[name] != null)
                {
                    result = _httpContext.Request.ServerVariables[name];
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
        /// </summary>
        /// <param name="request">HTTP Request</param>
        /// <returns>True if the request targets a static resource file.</returns>
        /// <remarks>
        /// These are the file extensions considered to be static resources:
        /// .css
        ///	.gif
        /// .png
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        public virtual bool IsStaticResource(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string path = request.Path;
            string extension = VirtualPathUtility.GetExtension(path);

            if (extension == null) return false;

            switch (extension.ToLower())
            {
                case ".axd":
                case ".ashx":
                case ".bmp":
                case ".css":
                case ".gif":
                case ".htm":
                case ".html":
                case ".ico":
                case ".jpeg":
                case ".jpg":
                case ".js":
                case ".png":
                case ".rar":
                case ".zip":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public virtual string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //hosted
                return HostingEnvironment.MapPath(path);
            }
            else
            {
                //not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                return Path.Combine(baseDirectory, path);
            }
        }

        ///// <summary>
        ///// Restart application domain
        ///// </summary>
        ///// <param name="makeRedirect">A value indicating whether </param>
        ///// <param name="redirectUrl">Redirect URL; empty string if you want to redirect to the current page URL</param>
        //public virtual void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "")
        //{
        //    if (CommonHelper.GetTrustLevel() > AspNetHostingPermissionLevel.Medium)
        //    {
        //        //full trust
        //        HttpRuntime.UnloadAppDomain();

        //        TryWriteGlobalAsax();
        //    }
        //    else
        //    {
        //        //medium trust
        //        bool success = TryWriteWebConfig();
        //        if (!success)
        //        {
        //            throw new KoreException("kore needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
        //                "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
        //                "- run the application in a full trust environment, or" + Environment.NewLine +
        //                "- give the application write access to the 'web.config' file.");
        //        }

        //        success = TryWriteGlobalAsax();
        //        if (!success)
        //        {
        //            throw new KoreException("kore needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
        //                "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
        //                "- run the application in a full trust environment, or" + Environment.NewLine +
        //                "- give the application write access to the 'Global.asax' file.");
        //        }
        //    }

        //    // If setting up extensions/modules requires an AppDomain restart, it's very unlikely the
        //    // current request can be processed correctly.  So, we redirect to the same URL, so that the
        //    // new request will come to the newly started AppDomain.
        //    if (_httpContext != null && makeRedirect)
        //    {
        //        if (String.IsNullOrEmpty(redirectUrl))
        //            redirectUrl = GetThisPageUrl(true);
        //        _httpContext.Response.Redirect(redirectUrl, true /*endResponse*/);
        //    }
        //}

        private bool TryWriteWebConfig()
        {
            try
            {
                // In medium trust, "UnloadAppDomain" is not supported. Touch web.config
                // to force an AppDomain restart.
                File.SetLastWriteTimeUtc(MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryWriteGlobalAsax()
        {
            try
            {
                //When a new plugin is dropped in the Plugins folder and is installed into kore,
                //even if the plugin has registered routes for its controllers,
                //these routes will not be working as the MVC framework couldn't
                //find the new controller types and couldn't instantiate the requested controller.
                //That's why you get these nasty errors
                //i.e "Controller does not implement IController".
                //The solution is to touch global.asax file
                File.SetLastWriteTimeUtc(MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Get a value indicating whether the request is made by search engine (web crawler)
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>Result</returns>
        public virtual bool IsSearchEngine(HttpContextBase context)
        {
            //we accept HttpContext instead of HttpRequest and put required logic in try-catch block
            if (context == null)
                return false;

            bool result = false;
            try
            {
                result = context.Request.Browser.Crawler;
                if (!result)
                {
                    //put any additional known crawlers in the Regex below for some custom validation
                    //var regEx = new Regex("Twiceler|twiceler|BaiDuSpider|baduspider|Slurp|slurp|ask|Ask|Teoma|teoma|Yahoo|yahoo");
                    //result = regEx.Match(request.UserAgent).Success;
                }
            }
            catch (Exception x)
            {
                logger.Value.Error(x.Message, x);
            }
            return result;
        }

        /// <summary>
        /// Gets a value that indicates whether the client is being redirected to a new location
        /// </summary>
        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                var response = _httpContext.Response;
                return response.IsRequestBeingRedirected;
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the client is being redirected to a new location using POST
        /// </summary>
        public virtual bool IsPostBeingDone
        {
            get
            {
                if (_httpContext.Items["kore.IsPOSTBeingDone"] == null)
                    return false;
                return Convert.ToBoolean(_httpContext.Items["kore.IsPOSTBeingDone"]);
            }
            set
            {
                _httpContext.Items["kore.IsPOSTBeingDone"] = value;
            }
        }

        private static AspNetHostingPermissionLevel? _trustLevel = null;

        /// <summary>
        /// Finds the trust level of the running application (http://blogs.msdn.com/dmitryr/archive/2007/01/23/finding-out-the-current-trust-level-in-asp-net.aspx)
        /// </summary>
        /// <returns>The current trust level.</returns>
        public static AspNetHostingPermissionLevel GetTrustLevel()
        {
            if (!_trustLevel.HasValue)
            {
                //set minimum
                _trustLevel = AspNetHostingPermissionLevel.None;

                //determine maximum
                foreach (AspNetHostingPermissionLevel trustLevel in new[]
                {
                    AspNetHostingPermissionLevel.Unrestricted,
                    AspNetHostingPermissionLevel.High,
                    AspNetHostingPermissionLevel.Medium,
                    AspNetHostingPermissionLevel.Low,
                    AspNetHostingPermissionLevel.Minimal
                })
                {
                    try
                    {
                        new AspNetHostingPermission(trustLevel).Demand();
                        _trustLevel = trustLevel;
                        break; //we've set the highest permission we can
                    }
                    catch (SecurityException x)
                    {
                        continue;
                    }
                }
            }
            return _trustLevel.Value;
        }

        ///// <summary>
        ///// Gets query string value by name
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="name">Parameter name</param>
        ///// <returns>Query string value</returns>
        //public virtual T QueryString<T>(string name)
        //{
        //    string queryParam = null;
        //    if (IsRequestAvailable(_httpContext) && _httpContext.Request.QueryString[name] != null)
        //    {
        //        queryParam = _httpContext.Request.QueryString[name];
        //    }

        //    if (!string.IsNullOrEmpty(queryParam))
        //    {
        //        return queryParam.ConvertTo<T>();
        //    }

        //    return default(T);
        //}

        /// <summary>
        /// Restart application domain
        /// </summary>
        /// <param name="makeRedirect">A value indicating whether we should made redirection after restart</param>
        /// <param name="redirectUrl">Redirect URL; empty string if you want to redirect to the current page URL</param>
        public virtual void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "")
        {
            if (GetTrustLevel() > AspNetHostingPermissionLevel.Medium)
            {
                //full trust
                HttpRuntime.UnloadAppDomain();

                TryWriteGlobalAsax();
            }
            else
            {
                //medium trust
                bool success = TryWriteWebConfig();
                if (!success)
                {
                    throw new KoreException("Kore needs to be restarted due to a configuration change, but was unable to do so." + System.Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" + System.Environment.NewLine +
                        "- run the application in a full trust environment, or" + System.Environment.NewLine +
                        "- give the application write access to the 'web.config' file.");
                }

                success = TryWriteGlobalAsax();
                if (!success)
                {
                    throw new KoreException("nopCommerce needs to be restarted due to a configuration change, but was unable to do so." + System.Environment.NewLine +
                        "To prevent this issue in the future, a change to the web server configuration is required:" + System.Environment.NewLine +
                        "- run the application in a full trust environment, or" + System.Environment.NewLine +
                        "- give the application write access to the 'Global.asax' file.");
                }
            }

            // If setting up extensions/modules requires an AppDomain restart, it's very unlikely the
            // current request can be processed correctly.  So, we redirect to the same URL, so that the
            // new request will come to the newly started AppDomain.
            if (_httpContext != null && makeRedirect)
            {
                if (string.IsNullOrEmpty(redirectUrl))
                    redirectUrl = GetThisPageUrl(true);
                _httpContext.Response.Redirect(redirectUrl, true /*endResponse*/);
            }
        }

        /// <summary>
        /// Gets this page name
        /// </summary>
        /// <param name="includeQueryString">Value indicating whether to include query strings</param>
        /// <returns>Page name</returns>
        public virtual string GetThisPageUrl(bool includeQueryString)
        {
            bool useSsl = IsCurrentConnectionSecured();
            return GetThisPageUrl(includeQueryString, useSsl);
        }

        /// <summary>
        /// Gets this page name
        /// </summary>
        /// <param name="includeQueryString">Value indicating whether to include query strings</param>
        /// <param name="useSsl">Value indicating whether to get SSL protected page</param>
        /// <returns>Page name</returns>
        public virtual string GetThisPageUrl(bool includeQueryString, bool useSsl)
        {
            string url = string.Empty;
            if (!IsRequestAvailable(_httpContext))
                return url;

            if (includeQueryString)
            {
                string tenantHost = GetTenantHost(useSsl);
                if (tenantHost.EndsWith("/"))
                    tenantHost = tenantHost.Substring(0, tenantHost.Length - 1);
                url = tenantHost + _httpContext.Request.RawUrl;
            }
            else
            {
                if (_httpContext.Request.Url != null)
                {
                    url = _httpContext.Request.Url.GetLeftPart(UriPartial.Path);
                }
            }
            url = url.ToLowerInvariant();
            return url;
        }

        /// <summary>
        /// Gets tenant host location
        /// </summary>
        /// <param name="useSsl">Use SSL</param>
        /// <returns>Tenant host location</returns>
        public virtual string GetTenantHost(bool useSsl)
        {
            string result = "";
            string httpHost = ServerVariables("HTTP_HOST");
            if (!string.IsNullOrEmpty(httpHost))
            {
                result = "http://" + httpHost;
                if (!result.EndsWith("/"))
                {
                    result += "/";
                }
            }

            // TODO: Support Tenants
            //if (DataSettingsHelper.DatabaseIsInstalled())
            //{
            //    #region Database is installed

            //    //let's resolve IWorkContext  here.
            //    //Do not inject it via contructor because it'll cause circular references
            //    var tenantContext = EngineContext.Current.Resolve<ITenantContext>();
            //    var currentTenant = tenantContext.CurrentTenant;
            //    if (currentTenant == null)
            //        throw new Exception("Current tenant cannot be loaded");

            //    if (string.IsNullOrWhiteSpace(httpHost))
            //    {
            //        //HTTP_HOST variable is not available.
            //        //This scenario is possible only when HttpContext is not available (for example, running in a schedule task)
            //        //in this case use URL of a tenant entity configured in admin area
            //        result = currentTenant.Url;
            //        if (!result.EndsWith("/"))
            //        {
            //            result += "/";
            //        }
            //    }

            //    if (useSsl)
            //    {
            //        if (!string.IsNullOrWhiteSpace(currentTenant.SecureUrl))
            //        {
            //            //Secure URL specified.
            //            //So a tenant owner don't want it to be detected automatically.
            //            //In this case let's use the specified secure URL
            //            result = currentTenant.SecureUrl;
            //        }
            //        else
            //        {
            //            //Secure URL is not specified.
            //            //So a tenant owner wants it to be detected automatically.
            //            result = result.Replace("http:/", "https:/");
            //        }
            //    }
            //    else
            //    {
            //        if (currentTenant.SslEnabled && !string.IsNullOrWhiteSpace(currentTenant.SecureUrl))
            //        {
            //            //SSL is enabled in this tenant and secure URL is specified.
            //            //So a tenant owner don't want it to be detected automatically.
            //            //In this case let's use the specified non-secure URL
            //            result = currentTenant.Url;
            //        }
            //    }
            //    #endregion
            //}
            //else
            //{

            #region Database is not installed

            if (useSsl)
            {
                //Secure URL is not specified.
                //So a tenant owner wants it to be detected automatically.
                result = result.Replace("http:/", "https:/");
            }

            #endregion Database is not installed

            //}

            if (!result.EndsWith("/"))
            {
                result += "/";
            }
            return result.ToLowerInvariant();
        }
    }
}