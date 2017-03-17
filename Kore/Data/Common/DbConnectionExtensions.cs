using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;
using Kore.Collections;

namespace Kore.Data.Common
{
    public static class DbConnectionExtensions
    {
        private const string INSERT_INTO_FORMAT = "INSERT INTO {0}({1}) VALUES({2})";

        public static DbParameter CreateParameter(this DbConnection connection, string parameterName, object value)
        {
            var param = GetDbProviderFactory(connection).CreateParameter();
            param.ParameterName = parameterName;
            param.Value = value;
            return param;
        }

        public static int ExecuteScalar(this DbConnection connection, string queryText)
        {
            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            return connection.ExecuteScalar<int>(queryText);
        }

        public static T ExecuteScalar<T>(this DbConnection connection, string queryText)
        {
            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = queryText;
                command.CommandTimeout = 300;
                return (T)command.ExecuteScalar();
            }
        }

        public static DbProviderFactory GetDbProviderFactory(this DbConnection connection)
        {
            return DbProviderFactories.GetFactory(connection);
        }

        public static DataSet ExecuteStoredProcedure(this DbConnection connection, string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            Dictionary<string, object> outputValues;
            return connection.ExecuteStoredProcedure(storedProcedure, parameters, out outputValues);
        }

        public static DataSet ExecuteStoredProcedure(this DbConnection connection, string storedProcedure, IEnumerable<DbParameter> parameters, out Dictionary<string, object> outputValues)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storedProcedure;
                parameters.ForEach(p => command.Parameters.Add(p));
                command.Parameters.EnsureDbNulls();
                var dataSet = new DataSet();

                var factory = connection.GetDbProviderFactory();
                using (var adapter = factory.CreateDataAdapter())
                {
                    adapter.SelectCommand = command;
                    adapter.Fill(dataSet);
                }

                outputValues = new Dictionary<string, object>();

                foreach (DbParameter param in command.Parameters)
                {
                    if (param.Direction == ParameterDirection.Output)
                    {
                        outputValues.Add(param.ParameterName, param.Value);
                    }
                }

                return dataSet;
            }
        }

        public static int ExecuteNonQuery(this DbConnection connection, string queryText)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = queryText;
                
                bool alreadyOpen = (connection.State != ConnectionState.Closed);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                return command.ExecuteNonQuery();
            }
        }

        public static int ExecuteNonQueryStoredProcedure(this DbConnection connection, string storedProcedure, IEnumerable<DbParameter> parameters)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storedProcedure;
                parameters.ForEach(p => command.Parameters.Add(p));
                command.Parameters.EnsureDbNulls();
                
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// <para>Inserts the specified entity into the specified Table. Property names are used to match</para>
        /// <para>with Sql Column Names.</para>
        /// </summary>
        /// <typeparam name="T">The type of entity to persist to Sql database.</typeparam>
        /// <param name="connection">This DbConnection.</param>
        /// <param name="entity">The entity to persist to Sql database.</param>
        /// <param name="tableName">The table to insert the entity into.</param>
        /// <returns>Number of rows affected.</returns>
        public static int Insert<T>(this DbConnection connection, T entity, string tableName, Func<string, string> encloseIdentifier)
        {
            var mappings = typeof(T).GetProperties().ToDictionary(k => k.Name, v => v.Name);
            return connection.Insert(entity, tableName, mappings, encloseIdentifier);
        }

        /// <summary>
        /// <para>Inserts the specified entity into the specified Table. Properties are matched</para>
        /// <para>with Sql Column Names by using the specified mappings dictionary.</para>
        /// </summary>
        /// <typeparam name="T">The type of entity to persist to Sql database.</typeparam>
        /// <param name="connection">This DbConnection.</param>
        /// <param name="entity">The entity to persist to Sql database.</param>
        /// <param name="tableName">The table to insert the entity into.</param>
        /// <param name="mappings">
        ///     <para>A Dictionary to use to map properties to Sql columns.</para>
        ///     <para>Key = Property Name, Value = Sql Column Name.</para>
        /// </param>
        /// <returns>Number of rows affected.</returns>
        public static int Insert<T>(
            this DbConnection connection,
            T entity,
            string tableName,
            IDictionary<string, string> mappings,
            Func<string, string> encloseIdentifier)
        {
            //using (var transactionScope = new TransactionScope()) //MySQL doesn't like this...
            //{
                string fieldNames = mappings.Values.Select(x => encloseIdentifier(x)).Join(",");
                string parameterNames = mappings.Values.Join(",").Replace(",", ",@").Prepend("@");

                PropertyInfo[] properties = typeof(T).GetProperties();

                using (DbCommand command = connection.CreateCommand())
                {
                    string commandText = string.Format(
                        INSERT_INTO_FORMAT,
                        encloseIdentifier(tableName),
                        fieldNames,
                        parameterNames);

                    command.CommandType = CommandType.Text;
                    command.CommandText = commandText;

                    mappings.ForEach(mapping =>
                    {
                        DbParameter parameter = command.CreateParameter();
                        parameter.ParameterName = string.Concat("@", mapping.Value);
                        PropertyInfo property = properties.Single(p => p.Name == mapping.Key);
                        parameter.DbType = Kore.Data.DataTypeConvertor.GetDbType(property.GetType());
                        //parameter.Value = GetFormattedValue(property.PropertyType, property.GetValue(entity, null));
                        parameter.Value = property.GetValue(entity, null);
                        command.Parameters.Add(parameter);
                    });

                    int rowsAffected = command.ExecuteNonQuery();
                    //transactionScope.Complete();

                    return rowsAffected;
                }
            //}
        }

        /// <summary>
        /// <para>Inserts the specified entities into the specified Table. Property names are used to match</para>
        /// <para>with Sql Column Names.</para>
        /// </summary>
        /// <typeparam name="T">The type of entity to persist to Sql database.</typeparam>
        /// <param name="connection">This DbConnection.</param>
        /// <param name="entities">The entities to persist to Sql database.</param>
        /// <param name="tableName">The table to insert the entities into.</param>
        public static void InsertCollection<T>(this DbConnection connection, IEnumerable<T> entities, string tableName, Func<string, string> encloseIdentifier)
        {
            var mappings = typeof(T).GetProperties().ToDictionary(k => k.Name, v => v.Name);
            connection.InsertCollection(entities, tableName, mappings, encloseIdentifier);
        }

        /// <summary>
        /// <para>Inserts the specified entities into the specified Table. Properties are matched</para>
        /// <para>with Sql Column Names by using the specified mappings dictionary.</para>
        /// </summary>
        /// <typeparam name="T">The type of entity to persist to Sql database.</typeparam>
        /// <param name="connection">This DbConnection.</param>
        /// <param name="entities">The entities to persist to Sql database.</param>
        /// <param name="tableName">The table to insert the entities into.</param>
        /// <param name="mappings">
        ///     <para>A Dictionary to use to map properties to Sql columns.</para>
        ///     <para>Key = Property Name, Value = Sql Column Name.</para>
        /// </param>
        public static void InsertCollection<T>(
            this DbConnection connection,
            IEnumerable<T> entities,
            string tableName,
            IDictionary<string, string> mappings,
            Func<string, string> encloseIdentifier)
        {
            //using (var transactionScope = new TransactionScope()) //MySQL doesn't like this...
            //{
            string fieldNames = mappings.Values.Select(x => encloseIdentifier(x)).Join(",");
            string parameterNames = mappings.Values.Join(",").Replace(",", ",@").Prepend("@");

            var properties = typeof(T).GetProperties();

            using (var command = connection.CreateCommand())
            {
                string commandText = string.Format(
                    INSERT_INTO_FORMAT,
                    encloseIdentifier(tableName),
                    fieldNames,
                    parameterNames);

                command.CommandType = CommandType.Text;
                command.CommandText = commandText;

                mappings.ForEach(mapping =>
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = string.Concat("@", mapping.Value);
                    var property = properties.Single(p => p.Name == mapping.Key);
                    parameter.DbType = DataTypeConvertor.GetDbType(property.PropertyType);
                    command.Parameters.Add(parameter);
                });
                
                foreach (T entity in entities)
                {
                    properties.ForEach(property =>
                    {
                        command.Parameters["@" + property.Name].Value = property.GetValue(entity, null);

                        //command.Parameters["@" + property.Name].Value =
                        //    GetFormattedValue(property.PropertyType, property.GetValue(entity, null));
                    });
                    command.ExecuteNonQuery();
                }

                //    transactionScope.Complete();
                //}
            }
        }

        public static void InsertDataTable(
            this DbConnection connection,
            DataTable table,
            string tableName,
            IDictionary<string, string> mappings,
            Func<string, string> encloseIdentifier)
        {
            string fieldNames = mappings.Values.Select(x => encloseIdentifier(x)).Join(",");
            string parameterNames = mappings.Values.Join(",").Replace(",", ",@").Prepend("@");

            var columns = table.Columns.OfType<DataColumn>().Select(x => new { x.ColumnName, x.DataType });

            using (var command = connection.CreateCommand())
            {
                string commandText = string.Format(
                    INSERT_INTO_FORMAT,
                    tableName,
                    fieldNames,
                    parameterNames);

                command.CommandType = CommandType.Text;
                command.CommandText = commandText;

                mappings.ForEach(mapping =>
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = string.Concat("@", mapping.Value);
                    var column = columns.Single(x => x.ColumnName == mapping.Key);
                    parameter.DbType = DataTypeConvertor.GetDbType(column.DataType);
                    command.Parameters.Add(parameter);
                });

                bool alreadyOpen = (connection.State != ConnectionState.Closed);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        command.Parameters["@" + column.ColumnName].Value = row[column];

                        //command.Parameters["@" + column.ColumnName].Value =
                        //    GetFormattedValue(column.DataType, row[column]);
                    }
                    command.ExecuteNonQuery();
                }

                //if (!alreadyOpen)
                //{
                //    connection.Close();
                //}
            }
        }

        /// <summary>
        /// Tries to establish a connection.
        /// </summary>
        /// <param name="connection">The DbConnection</param>
        /// <param name="maxTries">The number of times to try connecting.</param>
        /// <returns>True if successful. Otherwise, false</returns>
        public static bool Validate(this DbConnection connection, byte maxTries = 5)
        {
            try
            {
                bool alreadyOpen = (connection.State != ConnectionState.Closed);

                if (!alreadyOpen)
                {
                    connection.Open();
                }

                byte numberOfTries = 1;
                while (connection.State == ConnectionState.Connecting && numberOfTries <= maxTries)
                {
                    System.Threading.Thread.Sleep(100);
                    numberOfTries++;
                }
                bool valid = connection.State == ConnectionState.Open;

                if (!alreadyOpen)
                {
                    connection.Close();
                }

                return valid;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (DbException)
            {
                return false;
            }
        }

        //private static string GetFormattedValue(Type type, object value)
        //{
        //    if (value == null || value == DBNull.Value)
        //    {
        //        return "NULL";
        //    }

        //    switch (type.Name)
        //    {
        //        case "Boolean": return (bool)value ? "1" : "0";
        //        case "String": return ((string)value).Replace("'", "''");
        //        case "DateTime": return ((DateTime)value).ToISO8601DateString();
        //        case "DBNull": return "NULL";
        //        default: return value.ToString();
        //    }
        //}
    }
}