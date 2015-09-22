﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Kore.EntityFramework.Data.EntityFramework
{
    public interface IKoreEntityFrameworkHelper
    {
        bool SupportsBulkInsert { get; }

        bool SupportsEFExtended { get; }

        void EnsureTables<TContext>(TContext context) where TContext : DbContext;
    }

    public class SqlEntityFrameworkHelper : IKoreEntityFrameworkHelper
    {
        public bool SupportsBulkInsert
        {
            get { return true; }
        }

        public bool SupportsEFExtended
        {
            get { return true; }
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
                    command.CommandText =
                        "SELECT table_name from INFORMATION_SCHEMA.TABLES WHERE table_type = 'base table'";
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        existingTableNames.Add(reader.GetString(0).ToLowerInvariant());
                    }

                    reader.Close();

                    var split = script.Split(new[] { "create table " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var sql in split)
                    {
                        var tableName = sql.Substring(0, sql.IndexOf("(", StringComparison.Ordinal));
                        tableName = tableName.Split('.').Last();
                        tableName = tableName.Trim().TrimStart('[').TrimEnd(']').ToLowerInvariant();

                        if (existingTableNames.Contains(tableName))
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