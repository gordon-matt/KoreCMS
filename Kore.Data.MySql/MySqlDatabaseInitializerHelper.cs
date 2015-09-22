using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Kore.EntityFramework.Data.EntityFramework;

namespace Kore.Data.MySql
{
    public class MySqlEntityFrameworkHelper : IKoreEntityFrameworkHelper
    {
        public bool SupportsBulkInsert
        {
            get { return false; }
        }

        public bool SupportsEFExtended
        {
            get { return false; }
        }

        public void EnsureTables<TContext>(TContext context) where TContext : DbContext
        {
            var script = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
            if (!string.IsNullOrEmpty(script))
            {
                try
                {
                    var connection = context.Database.Connection;
                    var isConnectionClosed = connection.State == ConnectionState.Closed;
                    if (isConnectionClosed)
                    {
                        connection.Open();
                    }

                    var existingTableNames = new List<string>();
                    var command = connection.CreateCommand();
                    command.CommandText = string.Format("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = '{0}'", connection.Database);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        existingTableNames.Add(reader.GetString(0).ToLowerInvariant());
                    }

                    reader.Close();

                    int index = script.IndexOf("CREATE TABLE", StringComparison.Ordinal);

                    if (index == -1)
                    {
                        return;
                    }

                    var split = script.Substring(index).Split(new[] { "CREATE TABLE " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var sql in split)
                    {
                        var tableName = sql.Substring(0, sql.IndexOf("(", StringComparison.Ordinal));
                        tableName = tableName.Split('.').Last();
                        tableName = tableName.Trim().Trim('`');

                        if (existingTableNames.Contains(tableName.ToLowerInvariant()))
                        {
                            continue;
                        }

                        try
                        {
                            var createCommand = connection.CreateCommand();
                            createCommand.CommandText = "CREATE TABLE" + sql;
                            createCommand.ExecuteNonQuery();
                        }
                        catch (DbException)
                        {
                            // Ignore when existing
                        }
                    }

                    if (isConnectionClosed)
                    {
                        connection.Close();
                    }
                }
                catch (DbException)
                {
                    // Ignore when have database exception
                }
                catch (EntityException)
                {
                    // Ignore when have database exception
                }
            }
        }
    }
}