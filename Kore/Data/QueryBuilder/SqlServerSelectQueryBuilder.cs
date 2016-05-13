// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Kore.Data.QueryBuilder
{
    using System;
    using System.Data.Common;
    using System.Linq;
    using System.Text;

    public class SqlServerSelectQueryBuilder : BaseSelectQueryBuilder<SqlServerSelectQueryBuilder>
    {
        protected TopClause topClause = new TopClause(100, TopUnit.Percent);

        public SqlServerSelectQueryBuilder()
            : base()
        {
        }

        public SqlServerSelectQueryBuilder(DbProviderFactory factory)
            : base(factory)
        {
        }

        public override DbCommand BuildCommand()
        {
            return (DbCommand)this.BuildQuery(true);
        }

        public override string BuildQuery()
        {
            return (string)this.BuildQuery(false);
        }

        protected override object BuildQuery(bool buildCommand)
        {
            if (buildCommand && dbProviderFactory == null)
            {
                throw new Exception("Cannot build a command when the Db Factory hasn't been specified. Call SetDbProviderFactory first.");
            }

            DbCommand command = null;
            if (buildCommand)
            {
                command = dbProviderFactory.CreateCommand();
            }

            var query = new StringBuilder();
            query.Append("SELECT ");

            // Output Distinct
            if (isDistinct)
            {
                query.Append("DISTINCT ");
            }

            // Output Top clause
            if (!(topClause.Quantity == 100 & topClause.Unit == TopUnit.Percent))
            {
                query.Append("TOP ");
                query.Append(topClause.Quantity);
                if (topClause.Unit == TopUnit.Percent)
                {
                    query.Append(" PERCENT");
                }
                query.Append(" ");
            }

            // Output column names
            if (selectedColumns.Count == 0)
            {
                if (selectedTables.Count == 1)
                {
                    query.Append(selectedTables.First());
                    query.Append("."); // By default only select * from the table that was selected. If there are any joins, it is the responsibility of the user to select the needed columns.
                }
                query.Append("*");
            }
            else
            {
                foreach (string columnName in selectedColumns)
                {
                    query.Append(columnName);
                    query.Append(',');
                }
                query.Remove(query.Length - 1, 1); // Trim the last comma inserted by foreach loop
                query.Append(' ');
            }
            // Output table names
            if (selectedTables.Count > 0)
            {
                query.Append(" FROM ");
                foreach (string tableName in selectedTables)
                {
                    query.Append(tableName);
                    query.Append(',');
                }
                query.Remove(query.Length - 1, 1); // Trim the last comma inserted by foreach loop
                query.Append(' ');
            }

            // Output joins
            if (joins.Count > 0)
            {
                foreach (JoinClause clause in joins)
                {
                    string joinString = string.Empty;
                    switch (clause.JoinType)
                    {
                        case JoinType.InnerJoin: joinString = "INNER JOIN"; break;
                        case JoinType.OuterJoin: joinString = "OUTER JOIN"; break;
                        case JoinType.LeftJoin: joinString = "LEFT JOIN"; break;
                        case JoinType.RightJoin: joinString = "RIGHT JOIN"; break;
                    }
                    joinString += " " + clause.ToTable + " ON ";
                    joinString += WhereStatement.CreateComparisonClause(clause.FromTable + '.' + clause.FromColumn, clause.ComparisonOperator, new SqlLiteral(clause.ToTable + '.' + clause.ToColumn));
                    query.Append(joinString);
                    query.Append(' ');
                }
            }

            // Output where statement
            if (whereStatement.ClauseLevels > 0)
            {
                if (buildCommand)
                {
                    query.Append(" WHERE ");
                    query.Append(whereStatement.BuildWhereStatement(true, ref command));
                }
                else
                {
                    query.Append(" WHERE ");
                    query.Append(whereStatement.BuildWhereStatement());
                }
            }

            // Output GroupBy statement
            if (groupByColumns.Count > 0)
            {
                query.Append(" GROUP BY ");
                foreach (string column in groupByColumns)
                {
                    query.Append(column);
                    query.Append(',');
                }
                query.Remove(query.Length - 1, 1);
                query.Append(' ');
            }

            // Output having statement
            if (havingStatement.ClauseLevels > 0)
            {
                // Check if a Group By Clause was set
                if (groupByColumns.Count == 0)
                {
                    throw new Exception("Having statement was set without Group By");
                }
                if (buildCommand)
                {
                    query.Append(" HAVING ");
                    query.Append(havingStatement.BuildWhereStatement(true, ref command));
                }
                else
                {
                    query.Append(" HAVING ");
                    query.Append(havingStatement.BuildWhereStatement());
                }
            }

            // Output OrderBy statement
            if (orderByStatement.Count > 0)
            {
                query.Append(" ORDER BY ");
                foreach (var clause in orderByStatement)
                {
                    string orderByClause = string.Empty;
                    switch (clause.SortDirection)
                    {
                        case SortDirection.Ascending: orderByClause = clause.FieldName + " ASC"; break;
                        case SortDirection.Descending: orderByClause = clause.FieldName + " DESC"; break;
                    }
                    query.Append(orderByClause);
                    query.Append(',');
                }
                query.Remove(query.Length - 1, 1); // Trim the last comma inserted by foreach loop
                query.Append(' ');
            }

            if (buildCommand)
            {
                // Return the build command
                command.CommandText = query.ToString();
                return command;
            }
            else
            {
                // Return the built query
                return query.ToString();
            }
        }

        protected override string EncloseIdentifier(string identifier)
        {
            return string.Concat('[', identifier, ']');
        }
    }
}