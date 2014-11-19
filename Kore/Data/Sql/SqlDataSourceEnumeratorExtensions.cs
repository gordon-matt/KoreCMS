using System.Collections.Generic;
using System.Data;
using System.Data.Sql;

namespace Kore.Data.Sql
{
    public static class SqlDataSourceEnumeratorExtensions
    {
        /// <summary>
        /// Gets a list of all available SQL server instances on the current network.
        /// </summary>
        /// <param name="sqlDataSourceEnumerator">This System.Data.Sql.SqlDataSourceEnumerator.</param>
        /// <returns>SQL server instances as an IEnumerable&lt;string&gt;.</returns>
        public static IEnumerable<string> GetAvailableSqlServers(this SqlDataSourceEnumerator sqlDataSourceEnumerator)
        {
            return from row in SqlDataSourceEnumerator.Instance.GetDataSources().AsEnumerable()
                   orderby row.Field<string>("ServerName"), row.Field<string>("InstanceName")
                   select string.Concat(
                        row.Field<string>("ServerName"),
                        row.Field<string>("InstanceName") != string.Empty
                            ? string.Concat("\\", row.Field<string>("InstanceName"))
                            : string.Empty);
        }
    }
}