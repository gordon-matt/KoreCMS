using System.Collections.Generic;
using System.Data.Common;

namespace Kore.Data.QueryBuilder
{
    public interface ISelectQueryBuilder<T> : IQueryBuilder
        where T : ISelectQueryBuilder<T>
    {
        T SelectAll();

        T Select(string tableName, string column);

        T Select(string tableName, params string[] columns);

        T Select(IEnumerable<TableColumnPair> columns);

        T SelectCount();

        T Distinct(bool isDistinct = true);

        T From(string tableName);

        T From(params string[] tableNames);

        T Join(JoinType joinType, string toTableName, string toColumnName, ComparisonOperator comparisonOperator, string fromTableName, string fromColumnName);

        T Where(string tableName, string column, ComparisonOperator comparisonOperator, object value);

        T Where(string tableName, string column, ComparisonOperator comparisonOperator, object value, int level);

        T OrderBy(string tableName, string column, SortDirection sortDirection);

        T GroupBy(string tableName, params string[] columns);

        T GroupBy(IEnumerable<TableColumnPair> columns);

        T Having(string tableName, string column, ComparisonOperator comparisonOperator, object value);

        T Having(string tableName, string column, ComparisonOperator comparisonOperator, object value, int level);

        T SetDbProviderFactory(DbProviderFactory factory);

        DbCommand BuildCommand();
    }

    public struct TableColumnPair
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }
    }
}