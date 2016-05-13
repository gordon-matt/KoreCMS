using System.Collections.Generic;
using System.Data.Common;

namespace Kore.Data.QueryBuilder
{
    public abstract class BaseSelectQueryBuilder<T> : ISelectQueryBuilder<T>
        where T : BaseSelectQueryBuilder<T>
    {
        #region Non-Public Members

        protected string schema;
        protected readonly ICollection<string> groupByColumns = new List<string>();
        protected readonly ICollection<string> selectedColumns = new List<string>();
        protected DbProviderFactory dbProviderFactory;
        protected WhereStatement havingStatement = new WhereStatement();
        protected bool isDistinct;
        protected ICollection<JoinClause> joins = new List<JoinClause>();
        protected ICollection<OrderByClause> orderByStatement = new List<OrderByClause>();
        protected ICollection<string> selectedTables = new List<string>();
        protected WhereStatement whereStatement = new WhereStatement();
        protected IDictionary<string, string> tableAliases = new Dictionary<string, string>();

        #endregion Non-Public Members

        #region Constructors

        public BaseSelectQueryBuilder()
        {
        }

        public BaseSelectQueryBuilder(DbProviderFactory factory)
        {
            dbProviderFactory = factory;
        }

        #endregion Constructors

        #region Public Methods

        public virtual T SelectAll()
        {
            selectedColumns.Clear();
            return (T)this;
        }

        public virtual T Select(string tableName, string column)
        {
            selectedColumns.Clear();
            selectedColumns.Add(string.Concat(EncloseTable(tableName), '.', EncloseIdentifier(column)));
            return (T)this;
        }

        public virtual T Select(string tableName, params string[] columns)
        {
            selectedColumns.Clear();
            string enclosedTableName = EncloseTable(tableName);
            foreach (string column in columns)
            {
                selectedColumns.Add(string.Concat(enclosedTableName, '.', EncloseIdentifier(column)));
            }
            return (T)this;
        }

        public virtual T Select(IEnumerable<TableColumnPair> columns)
        {
            selectedColumns.Clear();
            foreach (var column in columns)
            {
                selectedColumns.Add(string.Concat(EncloseTable(column.TableName), '.', EncloseIdentifier(column.ColumnName)));
            }
            return (T)this;
        }

        public virtual T SelectCount()
        {
            selectedColumns.Clear();
            selectedColumns.Add("COUNT(1)");
            return (T)this;
        }

        public virtual T Distinct(bool isDistinct = true)
        {
            this.isDistinct = isDistinct;
            return (T)this;
        }

        public virtual T From(string tableName)
        {
            selectedTables.Clear();
            selectedTables.Add(EncloseTable(tableName));
            return (T)this;
        }

        public virtual T From(params string[] tableNames)
        {
            selectedTables.Clear();
            foreach (string table in tableNames)
            {
                selectedTables.Add(EncloseTable(table));
            }
            return (T)this;
        }

        public virtual T Join(
            JoinType joinType,
            string toTableName,
            string toColumnName,
            ComparisonOperator comparisonOperator,
            string fromTableName,
            string fromColumnName)
        {
            var join = new JoinClause(
                joinType,
                EncloseTable(toTableName),
                EncloseIdentifier(toColumnName),
                comparisonOperator,
                EncloseTable(fromTableName),
                EncloseIdentifier(fromColumnName));

            joins.Add(join);
            return (T)this;
        }

        public virtual T Where(string tableName, string column, ComparisonOperator comparisonOperator, object value)
        {
            Where(tableName, column, comparisonOperator, value, 1);
            return (T)this;
        }

        public virtual T Where(string tableName, string column, ComparisonOperator comparisonOperator, object value, int level)
        {
            var whereClause = new WhereClause(
                string.Concat(EncloseTable(tableName), '.', EncloseIdentifier(column)),
                comparisonOperator,
                value);

            whereStatement.Add(whereClause, level);
            return (T)this;
        }

        public virtual T OrderBy(string tableName, string column, SortDirection order)
        {
            var orderByClause = new OrderByClause(
                string.Concat(EncloseTable(tableName), '.', EncloseIdentifier(column)),
                order);

            orderByStatement.Add(orderByClause);
            return (T)this;
        }

        public virtual T GroupBy(string tableName, params string[] columns)
        {
            string enclosedTableName = EncloseTable(tableName);
            foreach (string column in columns)
            {
                groupByColumns.Add(string.Concat(enclosedTableName, '.', EncloseIdentifier(column)));
            }
            return (T)this;
        }

        public virtual T GroupBy(IEnumerable<TableColumnPair> columns)
        {
            foreach (var column in columns)
            {
                groupByColumns.Add(string.Concat(EncloseTable(column.TableName), '.', EncloseIdentifier(column.ColumnName)));
            }
            return (T)this;
        }

        public virtual T Having(string tableName, string column, ComparisonOperator comparisonOperator, object value)
        {
            Having(tableName, column, comparisonOperator, value, 1);
            return (T)this;
        }

        public virtual T Having(string tableName, string column, ComparisonOperator comparisonOperator, object value, int level)
        {
            var whereClause = new WhereClause(
                string.Concat(EncloseTable(tableName), '.', EncloseIdentifier(column)),
                comparisonOperator,
                value);

            havingStatement.Add(whereClause, level);
            return (T)this;
        }

        public virtual T SetDbProviderFactory(DbProviderFactory factory)
        {
            dbProviderFactory = factory;
            return (T)this;
        }

        public virtual DbCommand BuildCommand()
        {
            return (DbCommand)BuildQuery(true);
        }

        public virtual string BuildQuery()
        {
            return (string)BuildQuery(false);
        }

        #endregion Public Methods

        #region Non-Public Methods

        protected abstract object BuildQuery(bool buildCommand);

        protected abstract string EncloseIdentifier(string identifier);

        protected virtual string EncloseTable(string tableName)
        {
            if (!string.IsNullOrEmpty(schema))
            {
                return string.Concat(schema, '.', EncloseIdentifier(tableName));
            }
            return EncloseIdentifier(tableName);
        }

        #endregion Non-Public Methods
    }
}