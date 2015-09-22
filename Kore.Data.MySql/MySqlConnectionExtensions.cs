using System.Collections.Generic;
using System.Data;
using Kore.Data.Common;
using MySql.Data.MySqlClient;

namespace Kore.Data.MySql
{
    public static class MySqlConnectionExtensions
    {
        public static IEnumerable<string> GetDatabaseNames(this MySqlConnection connection)
        {
            const string CMD_SELECT_DATABASE_NAMES = "SHOW DATABASES;";
            List<string> databaseNames = new List<string>();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = CMD_SELECT_DATABASE_NAMES;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        databaseNames.Add(reader.GetString(0));
                    }
                }
            }

            if (!alreadyOpen)
            {
                connection.Close();
            }

            return databaseNames;
        }

        public static ForeignKeyInfoCollection GetForeignKeyData(this MySqlConnection connection, string tableName)
        {
            const string CMD_FOREIGN_KEYS_FORMAT =
@"SELECT
	TABLE_NAME AS FK_Table,
    COLUMN_NAME AS FK_Column,
    REFERENCED_TABLE_NAME AS PK_Table,
    REFERENCED_COLUMN_NAME AS PK_Column,
    CONSTRAINT_NAME AS Constraint_Name
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
WHERE TABLE_NAME = 'kore_blogposts'
AND CONSTRAINT_NAME <> 'PRIMARY';";

            var foreignKeyData = new ForeignKeyInfoCollection();

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = new MySqlCommand(string.Format(CMD_FOREIGN_KEYS_FORMAT, tableName), connection))
            {
                command.CommandType = CommandType.Text;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        foreignKeyData.Add(new ForeignKeyInfo(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            string.Empty,
                            reader.GetString(4)));
                    }
                }
            }

            if (!alreadyOpen)
            {
                connection.Close();
            }

            return foreignKeyData;
        }

        public static int GetRowCount(this MySqlConnection connection, string tableName)
        {
            return connection.ExecuteScalar(string.Concat("SELECT COUNT(*) FROM ", tableName));
        }

        public static IEnumerable<string> GetTableNames(this MySqlConnection connection, bool includeViews = false)
        {
            if (!string.IsNullOrEmpty(connection.Database))
            {
                return connection.GetTableNames(connection.Database, includeViews);
            }
            else { return new List<string>(); }
        }

        public static IEnumerable<string> GetTableNames(this MySqlConnection connection, string databaseName, bool includeViews = false)
        {
            string query = string.Empty;

            if (includeViews)
            {
                query = string.Concat("USE ", databaseName, "; SHOW FULL TABLES IN ", connection.Database);
            }
            else
            {
                query = string.Concat("USE ", databaseName, "; SHOW FULL TABLES IN ", connection.Database, " WHERE TABLE_TYPE LIKE 'BASE TABLE';");
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

        public static IEnumerable<string> GetViewNames(this MySqlConnection connection)
        {
            if (!string.IsNullOrEmpty(connection.Database))
            {
                return connection.GetViewNames(connection.Database);
            }
            else { return new List<string>(); }
        }

        public static IEnumerable<string> GetViewNames(this MySqlConnection connection, string databaseName)
        {
            string query = string.Concat("USE ", databaseName, "; SHOW FULL TABLES IN ", connection.Database, " WHERE TABLE_TYPE LIKE 'VIEW';");

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

        //TODO
        //        public static ColumnInfoCollection GetColumnData(this MySqlConnection connection, string tableName)
        //        {
        //            const string CMD_COLUMN_INFO_FORMAT =
        //@"SELECT COLUMN_NAME, COLUMN_DEFAULT, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
        //FROM INFORMATION_SCHEMA.COLUMNS
        //WHERE TABLE_NAME = '{0}'";

        //            const string CMD_IS_PRIMARY_KEY_FORMAT =
        //@"SELECT COLUMN_NAME
        //FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
        //WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_NAME), 'IsPrimaryKey') = 1
        //AND TABLE_NAME = '{0}'";

        //            var list = new ColumnInfoCollection();

        //            bool alreadyOpen = (connection.State != ConnectionState.Closed);

        //            try
        //            {
        //                var foreignKeyColumns = connection.GetForeignKeyData(tableName);

        //                if (!alreadyOpen)
        //                {
        //                    connection.Open();
        //                }

        //                using (var command = new MySqlCommand(string.Format(CMD_COLUMN_INFO_FORMAT, tableName), connection))
        //                {
        //                    command.CommandType = CommandType.Text;
        //                    using (var reader = command.ExecuteReader())
        //                    {
        //                        ColumnInfo columnInfo = null;

        //                        while (reader.Read())
        //                        {
        //                            columnInfo = new ColumnInfo();

        //                            if (!reader.IsDBNull(0))
        //                            { columnInfo.ColumnName = reader.GetString(0); }

        //                            if (!reader.IsDBNull(1))
        //                            { columnInfo.DefaultValue = reader.GetString(1); }
        //                            else
        //                            { columnInfo.DefaultValue = string.Empty; }

        //                            if (foreignKeyColumns.Contains(columnInfo.ColumnName))
        //                            {
        //                                columnInfo.KeyType = KeyType.ForeignKey;
        //                            }

        //                            //else
        //                            //{
        //                            try
        //                            {
        //                                columnInfo.DataType = DataTypeConvertor.GetSystemType(reader.GetString(2).ToEnum<SqlDbType>(true));
        //                            }
        //                            catch (ArgumentNullException)
        //                            {
        //                                columnInfo.DataType = typeof(object);
        //                            }
        //                            catch (ArgumentException)
        //                            {
        //                                columnInfo.DataType = typeof(object);
        //                            }

        //                            //}

        //                            if (!reader.IsDBNull(3))
        //                            { columnInfo.MaximumLength = reader.GetInt32(3); }

        //                            if (!reader.IsDBNull(4))
        //                            {
        //                                if (reader.GetString(4).ToUpperInvariant().Equals("NO"))
        //                                { columnInfo.IsNullable = false; }
        //                                else
        //                                { columnInfo.IsNullable = true; }
        //                            }

        //                            list.Add(columnInfo);
        //                        }
        //                    }
        //                }
        //            }
        //            finally
        //            {
        //                if (!alreadyOpen && connection.State != ConnectionState.Closed)
        //                { connection.Close(); }
        //            }

        //            #region Primary Keys

        //            using (var command = connection.CreateCommand())
        //            {
        //                command.CommandText = string.Format(CMD_IS_PRIMARY_KEY_FORMAT, tableName);

        //                alreadyOpen = (connection.State != ConnectionState.Closed);

        //                if (!alreadyOpen)
        //                {
        //                    connection.Open();
        //                }

        //                using (var reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        string pkColumn = reader.GetString(0);
        //                        ColumnInfo match = list[pkColumn];
        //                        if (match != null)
        //                        {
        //                            match.KeyType = KeyType.PrimaryKey;
        //                        }
        //                    }
        //                }

        //                if (!alreadyOpen)
        //                {
        //                    connection.Close();
        //                }
        //            }

        //            #endregion Primary Keys

        //            return list;
        //        }

        //        public static ColumnInfoCollection GetColumnData(this MySqlConnection connection, string tableName, IEnumerable<string> columnNames)
        //        {
        //            var data = from x in connection.GetColumnData(tableName)
        //                       where columnNames.Contains(x.ColumnName)
        //                       select x;

        //            var collection = new ColumnInfoCollection();
        //            collection.AddRange(data);
        //            return collection;
        //        }
    }
}