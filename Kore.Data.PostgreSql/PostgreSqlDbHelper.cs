using System;
using System.Data.Common;
using System.Linq;
using Npgsql;

namespace Kore.Data.PostgreSql
{
    public class PostgreSqlDbHelper : IKoreDbHelper
    {
        public string Escape(string s)
        {
            return string.Concat('\"', s, '\"');
        }

        public bool CheckIfTableExists(DbConnection connection, string tableName)
        {
            if (!(connection is NpgsqlConnection))
            {
                throw new ArgumentException("Specified connection is not an NpgsqlConnection.", "connection");
            }

            var npgsqlConnection = connection as NpgsqlConnection;
            // TODO: Allow any schema and make "public" the default
            return npgsqlConnection.GetTableNames(schema: "dbo").Contains(tableName);
        }

        public DbConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }
    }
}