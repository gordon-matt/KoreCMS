using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Plugins.Watchdog.Infrastructure
{
    public class LanguagePackInvariant : ILanguagePack
    {
        #region ILanguagePack Members

        public string CultureCode
        {
            get { return null; }
        }

        public IDictionary<string, string> LocalizedStrings
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { LocalizableStrings.AddService, "Add Service" },
                    { LocalizableStrings.AddServiceError, "There was an error when trying to add the service to the watch list." },
                    { LocalizableStrings.AddServiceSuccess, "Successfully added the service to the watch list." },
                    { LocalizableStrings.ConfirmRemoveService, "Are you sure you want to remove this service from the watch list?" },
                    { LocalizableStrings.ConfirmStopService, "Are you sure you want to stop this service?" },
                    { LocalizableStrings.ManageServices, "Manage Services" },
                    { LocalizableStrings.RemoveService, "Remove Service" },
                    { LocalizableStrings.RemoveServiceError, "There was an error when trying to remove the service from the watch list." },
                    { LocalizableStrings.RemoveServiceSuccess, "Successfully removed the service from the watch list." },
                    { LocalizableStrings.RestartService, "Restart Service" },
                    { LocalizableStrings.RestartServiceError, "There was an error when trying to restart the service." },
                    { LocalizableStrings.RestartServiceSuccess, "Successfully restarted the service." },
                    { LocalizableStrings.Services, "Services" },
                    { LocalizableStrings.StartService, "Start Service" },
                    { LocalizableStrings.StartServiceError, "There was an error when trying to start the service." },
                    { LocalizableStrings.StartServiceSuccess, "Successfully started the service." },
                    { LocalizableStrings.StopService, "Stop Service" },
                    { LocalizableStrings.StopServiceError, "There was an error when trying to stop the service." },
                    { LocalizableStrings.StopServiceSuccess, "Successfully stopped the service." },
                    { LocalizableStrings.Unauthorized, "Unauthorized" },
                    { LocalizableStrings.Watchdog, "Watchdog" },
                    { LocalizableStrings.Model.DisplayName, "Display Name" },
                    { LocalizableStrings.Model.Password, "Password" },
                    { LocalizableStrings.Model.ServiceName, "Service Name" },
                    { LocalizableStrings.Model.Status, "Status" },
                    { LocalizableStrings.Model.Url, "URL" },
                };
            }
        }

        #endregion ILanguagePack Members
    }
}