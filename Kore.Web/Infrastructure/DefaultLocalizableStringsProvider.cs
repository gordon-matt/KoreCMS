using System.Collections.Generic;
using Kore.Localization;

namespace Kore.Web.Infrastructure
{
    public class DefaultLocalizableStringsProvider : IDefaultLocalizableStringsProvider
    {
        #region IDefaultLocalizableStringsProvider Members

        public ICollection<Translation> GetTranslations()
        {
            return new[]
            {
                new Translation
                {
                    CultureCode = null,
                    LocalizedStrings = new Dictionary<string, string>
                    {
                        { KoreWebLocalizableStrings.General.Actions, "Actions" },
                        { KoreWebLocalizableStrings.General.Cancel, "Cancel" },
                        { KoreWebLocalizableStrings.General.Clear, "Clear" },
                        { KoreWebLocalizableStrings.General.Close, "Close" },
                        { KoreWebLocalizableStrings.General.Configuration, "Configuration" },
                        { KoreWebLocalizableStrings.General.Create, "Create" },
                        { KoreWebLocalizableStrings.General.CreateFormat, "Create {0}" },
                        { KoreWebLocalizableStrings.General.Delete, "Delete" },
                        { KoreWebLocalizableStrings.General.Edit, "Edit" },
                        { KoreWebLocalizableStrings.General.EditFormat, "Edit {0}" },
                        { KoreWebLocalizableStrings.General.Enabled, "Enabled" },
                        { KoreWebLocalizableStrings.General.Miscellaneous, "Miscellaneous" },
                        { KoreWebLocalizableStrings.General.OK, "OK" },
                        { KoreWebLocalizableStrings.General.OnOff, "On/Off" },
                        { KoreWebLocalizableStrings.General.Preview, "Preview" },
                        { KoreWebLocalizableStrings.General.Save, "Save" },
                        { KoreWebLocalizableStrings.General.SaveAndContinue, "Save & Continue" },
                        { KoreWebLocalizableStrings.General.Search, "Search" },
                        { KoreWebLocalizableStrings.General.SetDefault, "Set Default" },
                        { KoreWebLocalizableStrings.General.Settings, "Settings" },
                        { KoreWebLocalizableStrings.General.Submit, "Submit" },
                        { KoreWebLocalizableStrings.General.Themes, "Theme" },
                        { KoreWebLocalizableStrings.General.Toggle, "Toggle" },
                        { KoreWebLocalizableStrings.General.View, "View" },
                        { KoreWebLocalizableStrings.General.ViewFormat, "View {0}" },
                        { KoreWebLocalizableStrings.Messages.ConfirmDeleteRecord, "Are you sure that you want to delete this record?" },
                        { KoreWebLocalizableStrings.Messages.DeleteRecordError, "There was an error when deleting the record." },
                        { KoreWebLocalizableStrings.Messages.DeleteRecordErrorFormat, "There was an error when deleting the record. Additional information as follows: {0}" },
                        { KoreWebLocalizableStrings.Messages.DeleteRecordSuccess, "Successfully deleted record." },
                        { KoreWebLocalizableStrings.Messages.GetRecordError, "There was an error when retrieving the record." },
                        { KoreWebLocalizableStrings.Messages.InsertRecordError, "There was an error when inserting the record." },
                        { KoreWebLocalizableStrings.Messages.InsertRecordSuccess, "Successfully inserted record." },
                        { KoreWebLocalizableStrings.Messages.UpdateRecordError, "There was an error when updating the record." },
                        { KoreWebLocalizableStrings.Messages.UpdateRecordErrorFormat, "There was an error when updating the record. Additional information as follows: {0}" },
                        { KoreWebLocalizableStrings.Messages.UpdateRecordSuccess, "Successfully updated record." },
                        { KoreWebLocalizableStrings.Messages.InstallPluginError, "There was an error when installing the plugin." },
                        { KoreWebLocalizableStrings.Messages.InstallPluginSuccess, "Successfully installed plugin." },
                        { KoreWebLocalizableStrings.Messages.UninstallPluginError, "There was an error when uninstalling the plugin." },
                        { KoreWebLocalizableStrings.Messages.UninstallPluginSuccess, "Successfully uninstalled plugin." },
                        { KoreWebLocalizableStrings.Messages.SetDesktopThemeError, "Error when setting default desktop theme." },
                        { KoreWebLocalizableStrings.Messages.SetDesktopThemeSuccess, "Successfully set default desktop theme." },
                        { KoreWebLocalizableStrings.Navigation.Configuration, "Configuration" },
                        { KoreWebLocalizableStrings.Plugins.ManagePlugins, "Manage Plugins" },
                        { KoreWebLocalizableStrings.Plugins.Title, "Plugins" },
                        { KoreWebLocalizableStrings.ScheduledTasks.ManageScheduledTasks, "Manage Scheduled Tasks" },
                        { KoreWebLocalizableStrings.ScheduledTasks.Title, "Scheduled Tasks" },
                        { KoreWebLocalizableStrings.Validation.Date, "Please enter a valid date." },
                        { KoreWebLocalizableStrings.Validation.Digits, "Please enter only digits." },
                        { KoreWebLocalizableStrings.Validation.Email, "Please enter a valid email address." },
                        { KoreWebLocalizableStrings.Validation.EqualTo, "Please enter the same value again." },
                        { KoreWebLocalizableStrings.Validation.MaxLength, "Please enter no more than {0} characters." },
                        { KoreWebLocalizableStrings.Validation.MinLength, "Please enter at least {0} characters." },
                        { KoreWebLocalizableStrings.Validation.Number, "Please enter a valid number." },
                        { KoreWebLocalizableStrings.Validation.Range, "Please enter a value between {0} and {1}." },
                        { KoreWebLocalizableStrings.Validation.RangeLength, "Please enter a value between {0} and {1} characters long." },
                        { KoreWebLocalizableStrings.Validation.RangeMax, "Please enter a value less than or equal to {0}." },
                        { KoreWebLocalizableStrings.Validation.RangeMin, "Please enter a value greater than or equal to {0}." },
                        { KoreWebLocalizableStrings.Validation.Required, "This field is required." },
                        { KoreWebLocalizableStrings.Validation.Url, "Please enter a valid URL." }
                    }
                }
            };
        }

        #endregion IDefaultLocalizableStringsProvider Members
    }
}