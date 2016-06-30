using System.Collections.Generic;
using System.Data.Common;

namespace Kore.Data.QueryBuilder
{
    public interface ISelectQueryBuilder : IQueryBuilder
    {
        ISelectQueryBuilder SelectAll();

        ISelectQueryBuilder Select(string tableName, string column);

        ISelectQueryBuilder Select(string tableName, params string[] columns);

        ISelectQueryBuilder Select(IEnumerable<TableColumnPair> columns);

        ISelectQueryBuilder SelectCount();

        ISelectQueryBuilder Distinct(bool isDistinct = true);

        ISelectQueryBuilder From(string tableName);

        ISelectQueryBuilder From(params string[] tableNames);

        ISelectQueryBuilder Join(JoinType joinType, string toTableName, string toColumnName, ComparisonOperator comparisonOperator, string fromTableName, string fromColumnName);

        ISelectQueryBuilder Where(string tableName, string column, ComparisonOperator comparisonOperator, object value);

        ISelectQueryBuilder Where(string tableName, string column, ComparisonOperator comparisonOperator, object value, int level);

        ISelectQueryBuilder OrderBy(string tableName, string column, SortDirection sortDirection);

        ISelectQueryBuilder GroupBy(string tableName, params string[] columns);

        ISelectQueryBuilder GroupBy(IEnumerable<TableColumnPair> columns);

        ISelectQueryBuilder Having(string tableName, string column, ComparisonOperator comparisonOperator, object value);

        ISelectQueryBuilder Having(string tableName, string column, ComparisonOperator comparisonOperator, object value, int level);

        ISelectQueryBuilder Skip(int count);

        ISelectQueryBuilder Take(int count);

        ISelectQueryBuilder SetDbProviderFactory(DbProviderFactory factory);

        DbCommand BuildCommand();
    }

    public struct TableColumnPair
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }
    }
}