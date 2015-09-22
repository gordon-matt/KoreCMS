using System;
using System.Data.Entity;
using System.Transactions;
using Kore.Infrastructure;
using Kore.Logging;

namespace Kore.EntityFramework.Data.EntityFramework
{
    public class CreateTablesIfNotExist<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        public void InitializeDatabase(TContext context)
        {
            bool dbExists;

            try
            {
                using (new TransactionScope(TransactionScopeOption.Suppress))
                {
                    dbExists = context.Database.Exists();
                }
            }
            catch
            {
                dbExists = false;
            }

            if (dbExists)
            {
                var databaseInitializerHelper = EngineContext.Current.Resolve<IKoreEntityFrameworkHelper>();
                databaseInitializerHelper.EnsureTables(context);
            }
            else
            {
                //please don't remove this.. if you want it to work, then add "Persist Security Info=true" to your connection string.
                try
                {
                    var defaultInitializer = new CreateDatabaseIfNotExists<TContext>();
                    defaultInitializer.InitializeDatabase(context);
                }
                catch (Exception x)
                {
                    var logger = LoggingUtilities.Resolve();
                    logger.Error(x.Message, x);
                }
            }
        }
    }
}