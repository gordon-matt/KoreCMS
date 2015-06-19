using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web;
using Kore.Data;
using Kore.EntityFramework;
using Kore.Infrastructure;
using Kore.Web.Infrastructure;
using Kore.Web.Models;

namespace Kore.Web
{
    public static class InstallationHelper
    {
        public static void Install<TDbContext>(HttpRequestBase httpRequest, InstallationModel model) where TDbContext : DbContext, IKoreDbContext, ISupportSeed, new()
        {
            var dataSettings = EngineContext.Current.Resolve<DataSettings>();

            string connectionString = string.Empty;

            if (model.EnterConnectionString)
            {
                connectionString = model.ConnectionString;
            }
            else
            {
                if (model.UseWindowsAuthentication)
                {
                    connectionString = string.Format(
                        @"Server={0};Initial Catalog={1};Integrated Security=True;Persist Security Info=True;MultipleActiveResultSets=True",
                        model.DatabaseServer,
                        model.DatabaseName);
                }
                else
                {
                    connectionString = string.Format(
                        @"Server={0};Initial Catalog={1};User={2};Password={3};Persist Security Info=True;MultipleActiveResultSets=True",
                        model.DatabaseServer,
                        model.DatabaseName,
                        model.DatabaseUsername,
                        model.DatabasePassword);
                }
            }

            dataSettings.ConnectionString = connectionString;

            // We need to save this to settings temporarily in order to setup the login details AFTER restarting the app domain
            //  We then delete the password from the XML file in Kore.Web.Infrastructure.StartupTask.
            dataSettings.AdminEmail = model.AdminEmail;
            dataSettings.AdminPassword = model.AdminPassword;

            DataSettingsManager.SaveSettings(dataSettings);

            using (var context = new TDbContext())
            {
                context.Database.Connection.ConnectionString = connectionString;

                bool dbExists = context.Database.Exists();
                if (dbExists)
                {
                    int numberOfTables = context.Database.SqlQuery<int>(
                        "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE'").FirstAsync().Result;

                    if (numberOfTables == 0)
                    {
                        var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                        context.Database.ExecuteSqlCommand(dbCreationScript);
                    }
                }
                else
                {
                    context.Database.Create();
                }
                context.Seed();
            }

            DataSettingsHelper.ResetCache();

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            webHelper.RestartAppDomain();
        }
    }
}