using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using Kore.Collections;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Infrastructure
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            var settingsRepository = EngineContext.Current.Resolve<IRepository<Setting>>();
            var allSettings = EngineContext.Current.ResolveAll<ISettings>();
            var allSettingNames = allSettings.Select(x => x.Name).ToList();
            var installedSettings = settingsRepository.Table.ToList();
            var installedSettingNames = installedSettings.Select(x => x.Name).ToList();

            var settingsToAdd = allSettings.Where(x => !installedSettingNames.Contains(x.Name)).Select(x => new Setting
            {
                Id = Guid.NewGuid(),
                Name = x.Name,
                Type = x.GetType().FullName,
                Value = Activator.CreateInstance(x.GetType()).ToJson()
            }).ToList();

            if (!settingsToAdd.IsNullOrEmpty())
            {
                settingsRepository.Insert(settingsToAdd);
            }

            var settingsToDelete = installedSettings.Where(x => !allSettingNames.Contains(x.Name)).ToList();

            if (!settingsToDelete.IsNullOrEmpty())
            {
                settingsRepository.Delete(settingsToDelete);
            }

            #region One Time URL Registration

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
                string url = HttpContext.Current.Request.Url.ToString();

                if (!string.IsNullOrEmpty(url) && !url.ContainsAny("localhost", "127.0.0.1"))
                {
                    using (var client = new HttpClient())
                    {
                        var data = new
                        {
                            Url = url
                        };

                        var stringContent = new StringContent(data.ToJson());
                        var response = client.PostAsync("http://www.widecommerce.com/kore-site-registration/register", stringContent).Result;

                        if (response.IsSuccessStatusCode)
                        {
                            File.WriteAllText(filePath, "Registered:True");
                        }
                    }
                }
            }

            #endregion
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IStartupTask Members
    }
}