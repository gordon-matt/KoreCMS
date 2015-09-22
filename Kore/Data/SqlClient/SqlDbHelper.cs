using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Kore.Data.SqlClient
{
    public class SqlDbHelper : IKoreDbHelper
    {
        public string Escape(string s)
        {
            return string.Concat('[', s, ']');
        }

        public bool CheckIfTableExists(DbConnection connection, string tableName)
        {
            if (!(connection is SqlConnection))
            {
                throw new ArgumentException("Specified connection is not an SqlConnection.", "connection");
            }

            var sqlConnection = connection as SqlConnection;
            return sqlConnection.GetTableNames().Contains(tableName);
        }
    }
}