using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace Kore.Data.PostgreSql
{
    public static class NpgsqlConnectionExtensions
    {
        public static IEnumerable<string> GetTableNames(this NpgsqlConnection connection, bool includeViews = false)
        {
            string query = string.Empty;

            if (includeViews)
            {
                query = @"SELECT table_name
FROM information_schema.tables
WHERE table_schema = 'dbo'
ORDER BY table_name";
            }
            else
            {
                query = @"SELECT table_name
FROM information_schema.tables
WHERE table_schema = 'dbo'
AND table_type = 'BASE TABLE'
ORDER BY table_name";
            }

            var tables = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }

            if (!alreadyOpen)
            {
                connection.Close();
            }

            return tables;
        }
    }
}