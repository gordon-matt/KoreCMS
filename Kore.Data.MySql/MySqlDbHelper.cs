using System;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;

namespace Kore.Data.MySql
{
    public class MySqlDbHelper : IKoreDbHelper
    {
        public string Escape(string s)
        {
            return string.Concat('`', s, '`');
        }

        public bool CheckIfTableExists(DbConnection connection, string tableName)
        {
            if (!(connection is MySqlConnection))
            {
                throw new ArgumentException("Specified connection is not an SqlConnection.", "connection");
            }

            var mySqlConnection = connection as MySqlConnection;
            return mySqlConnection.GetTableNames().Contains(tableName);
        }

        public DbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}