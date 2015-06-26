using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Kore.Logging;

namespace Kore.Web
{
    internal static class SiteRegistrar
    {
        private static bool hasRun;

        public static void Register(HttpContextBase httpContext)
        {
            if (!hasRun)
            {
                try
                {
                    bool registered = false;

                    var filePath = HostingEnvironment.MapPath("~/App_Data/Registration.txt");
                    if (!File.Exists(filePath))
                    {
                        using (File.Create(filePath))
                        {
                            //we use 'using' to close the file after it's created
                        }
                        File.WriteAllText(filePath, "Registered:False");
                    }
                    else
                    {
                        string text = File.ReadAllText(filePath);
                        if (!string.IsNullOrEmpty(text) && text.StartsWith("Registered:True", StringComparison.CurrentCultureIgnoreCase))
                        {
                            registered = true;
                        }
                    }

                    if (!registered)
                    {
                        // Register this site
                        string url = httpContext.Request.Url.ToString();

                        if (!string.IsNullOrEmpty(url) && !url.ContainsAny("localhost", "127.0.0.1"))
                        {
                            using (var client = new HttpClient())
                            {
                                var data = new
                                {
                                    url = url
                                };

                                var stringContent = new StringContent(data.ToJson(), Encoding.UTF8, "application/json");
                                var response = client.PostAsync("http://widecommerce.com/odata/kore/plugins/widecommerce/KoreSiteRegistrationApi/Register", stringContent).Result;

                                if (response.IsSuccessStatusCode)
                                {
                                    File.WriteAllText(filePath, "Registered:True");
                                }
                            }
                        }
                    }

                    hasRun = true;
                }
                catch (Exception x)
                {
                    // If there is a problem registering, we don't want to prevent the site from running..
                    //  so just ignore it...
                    var logger = LoggingUtilities.Resolve();
                    logger.Error("SiteRegistrar Error", x);
                    hasRun = true;
                }
            }
        }
    }
}