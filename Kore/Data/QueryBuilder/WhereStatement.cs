// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Kore.Data.QueryBuilder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Text;
    using Kore.Reflection;

    public class WhereStatement : List<List<WhereClause>>
    {
        public int ClauseLevels
        {
            get { return this.Count; }
        }

        private void AssertLevelExistance(int level)
        {
            if (this.Count < (level - 1))
            {
                throw new Exception(string.Concat("Level ", level, " not allowed because level ", (level - 1), " does not exist."));
            }
            // Check if new level must be created
            else if (this.Count < level)
            {
                this.Add(new List<WhereClause>());
            }
        }

        public void Add(WhereClause clause)
        {
            this.Add(clause, 1);
        }

        public void Add(WhereClause clause, int level)
        {
            this.AddWhereClauseToLevel(clause, level);
        }

        public WhereClause Add(string field, ComparisonOperator comparisonOperator, object compareValue)
        {
            return this.Add(field, comparisonOperator, compareValue, 1);
        }

        public WhereClause Add(Enum field, ComparisonOperator comparisonOperator, object compareValue)
        {
            return this.Add(field.ToString(), comparisonOperator, compareValue, 1);
        }

        public WhereClause Add(string field, ComparisonOperator comparisonOperator, object compareValue, int level)
        {
            var whereClause = new WhereClause(field, comparisonOperator, compareValue);
            this.AddWhereClauseToLevel(whereClause, level);
            return whereClause;
        }

        private void AddWhereClause(WhereClause clause)
        {
            AddWhereClauseToLevel(clause, 1);
        }

        private void AddWhereClauseToLevel(WhereClause clause, int level)
        {
            // Add the new clause to the array at the right level
            AssertLevelExistance(level);
            this[level - 1].Add(clause);
        }

        public string BuildWhereStatement()
        {
            DbCommand dummyCommand = null; // = DataAccess.UsedDbProviderFactory.CreateCommand();
            return BuildWhereStatement(false, ref dummyCommand);
        }

        //TODO: Use StringBuilder more...
        public string BuildWhereStatement(bool useCommandObject, ref DbCommand usedDbCommand)
        {
            string result = string.Empty;
            foreach (var whereStatement in this) // Loop through all statement levels, OR them together
            {
                string levelWhere = string.Empty;
                foreach (var clause in whereStatement) // Loop through all conditions, AND them together
                {
                    var whereClause = new StringBuilder();

                    if (useCommandObject)
                    {
                        // Create a parameter
                        string parameterName = string.Format(
                            "@p{0}_{1}",
                            usedDbCommand.Parameters.Count + 1,
                            clause.FieldName.Replace('.', '_'));

                        var parameter = usedDbCommand.CreateParameter();
                        parameter.ParameterName = parameterName;
                        parameter.Value = clause.Value;
                        usedDbCommand.Parameters.Add(parameter);

                        // Create a where clause using the parameter, instead of its value
                        whereClause.Append(CreateComparisonClause(clause.FieldName, clause.ComparisonOperator, new SqlLiteral(parameterName)));
                    }
                    else
                    {
                        whereClause.Append(CreateComparisonClause(clause.FieldName, clause.ComparisonOperator, clause.Value));
                    }

                    foreach (var subClause in clause.SubClauses)	// Loop through all subclauses, append them together with the specified logic operator
                    {
                        switch (subClause.LogicOperator)
                        {
                            case LogicOperator.And: whereClause.Append(" AND "); break;
                            case LogicOperator.Or: whereClause.Append(" OR "); break;
                        }

                        if (useCommandObject)
                        {
                            // Create a parameter
                            string parameterName = string.Format(
                                "@p{0}_{1}",
                                usedDbCommand.Parameters.Count + 1,
                                clause.FieldName.Replace('.', '_'));

                            var parameter = usedDbCommand.CreateParameter();
                            parameter.ParameterName = parameterName;
                            parameter.Value = subClause.Value;
                            usedDbCommand.Parameters.Add(parameter);

                            // Create a where clause using the parameter, instead of its value
                            whereClause.Append(CreateComparisonClause(clause.FieldName, subClause.ComparisonOperator, new SqlLiteral(parameterName)));
                        }
                        else
                        {
                            whereClause.Append(CreateComparisonClause(clause.FieldName, subClause.ComparisonOperator, subClause.Value));
                        }
                    }
                    levelWhere += "(" + whereClause.ToString() + ") AND ";
                }
                levelWhere = levelWhere.Substring(0, levelWhere.Length - 5); // Trim de last AND inserted by foreach loop
                if (whereStatement.Count > 1)
                {
                    result += " (" + levelWhere + ") ";
                }
                else
                {
                    result += " " + levelWhere + " ";
                }
                result += " OR";
            }
            result = result.Substring(0, result.Length - 2); // Trim de last OR inserted by foreach loop

            return result;
        }

        public static string CreateComparisonClause(string fieldName, ComparisonOperator comparisonOperator, object value)
        {
            string output = string.Empty;
            if (value != null && value != System.DBNull.Value)
            {
                switch (comparisonOperator)
                {
                    case ComparisonOperator.EqualTo: output = fieldName + " = " + FormatSQLValue(value); break;
                    case ComparisonOperator.NotEqualTo: output = fieldName + " <> " + FormatSQLValue(value); break;
                    case ComparisonOperator.GreaterThan: output = fieldName + " > " + FormatSQLValue(value); break;
                    case ComparisonOperator.GreaterThanOrEqualTo: output = fieldName + " >= " + FormatSQLValue(value); break;
                    case ComparisonOperator.LessThan: output = fieldName + " < " + FormatSQLValue(value); break;
                    case ComparisonOperator.LessThanOrEqualTo: output = fieldName + " <= " + FormatSQLValue(value); break;
                    case ComparisonOperator.Like: output = fieldName + " LIKE " + FormatSQLValue(value); break;
                    case ComparisonOperator.NotLike: output = "NOT " + fieldName + " LIKE " + FormatSQLValue(value); break;
                    case ComparisonOperator.In: output = fieldName + " IN (" + FormatSQLValue(value) + ")"; break;
                    case ComparisonOperator.Contains: output = string.Format("{0} LIKE ({1})", fieldName, FormatSQLValue("%" + value + "%")); break;
                    case ComparisonOperator.NotContains: output = string.Format("NOT {0} LIKE {1}", fieldName, FormatSQLValue("%" + value + "%")); break;
                    case ComparisonOperator.StartsWith: output = string.Format("{0} LIKE ({1})", fieldName, FormatSQLValue(value + "%")); break;
                    case ComparisonOperator.EndsWith: output = string.Format("{0} LIKE ({1})", fieldName, FormatSQLValue("%" + value)); break;
                }
            }
            else // value==null	|| value==DBNull.Value
            {
                if ((comparisonOperator != ComparisonOperator.EqualTo) && (comparisonOperator != ComparisonOperator.NotEqualTo))
                {
                    throw new Exception("Cannot use comparison operator " + comparisonOperator.ToString() + " for NULL values.");
                }
                else
                {
                    switch (comparisonOperator)
                    {
                        case ComparisonOperator.EqualTo: output = fieldName + " IS NULL"; break;
                        case ComparisonOperator.NotEqualTo: output = "NOT " + fieldName + " IS NULL"; break;
                    }
                }
            }
            return output;
        }

        public static string FormatSQLValue(object someValue)
        {
            string formattedValue = string.Empty;
            // string StringType = Type.GetType("string").Name;
            // string DateTimeType = Type.GetType("DateTime").Name;

            if (someValue == null)
            {
                formattedValue = "NULL";
                return formattedValue;
            }

            var type = someValue.GetType();

            if (type.IsCollection())
            {
                var collection = (someValue as IEnumerable);
                Type collectionType;

                if (type.IsGenericType)
                {
                    collectionType = type.GetGenericArguments().Single();
                }
                else
                {
                    var firstItem = collection.OfType<object>().FirstOrDefault();
                    if (firstItem == null)
                    {
                        formattedValue = "NULL";
                        return formattedValue;
                    }
                    else
                    {
                        collectionType = firstItem.GetType();
                    }
                }

                switch (collectionType.Name)
                {
                    case "String":
                        formattedValue = string.Join("','", collection.OfType<string>().ToArray()).AddSingleQuotes();
                        break;

                    case "DateTime":
                        formattedValue = string.Join("','", collection.OfType<DateTime>().Select(x => x.ToString("yyyy/MM/dd HH:mm:ss")).ToArray()).AddSingleQuotes();
                        break;

                    case "Guid":
                        formattedValue = string.Join("','", collection.OfType<Guid>().Select(x => x.ToString().ToArray())).AddSingleQuotes();
                        break;

                    case "SqlLiteral":
                        formattedValue = string.Join(",", collection.OfType<SqlLiteral>().Select(x => x.Value).ToArray());
                        break;

                    case "DBNull": formattedValue = "NULL"; break;
                    default: formattedValue = string.Join(",", collection.OfType<object>().ToArray()); break;
                }
            }
            else
            {
                switch (type.Name)
                {
                    case "String": formattedValue = string.Format("'{0}'", ((string)someValue).Replace("'", "''")); break;
                    case "DateTime": formattedValue = string.Format("'{0:yyyy/MM/dd HH:mm:ss}'", (DateTime)someValue); break;
                    case "Guid": formattedValue = string.Format("'{0}'", (Guid)someValue); break;
                    case "Boolean": formattedValue = (bool)someValue ? "1" : "0"; break;
                    case "SqlLiteral": formattedValue = ((SqlLiteral)someValue).Value; break;
                    case "DBNull": formattedValue = "NULL"; break;
                    default: formattedValue = someValue.ToString(); break;
                }
            }
            return formattedValue;
        }

        /// <summary>
        /// This static method combines 2 where statements with eachother to form a new statement
        /// </summary>
        /// <param name="statement1"></param>
        /// <param name="statement2"></param>
        /// <returns></returns>
        public static WhereStatement CombineStatements(WhereStatement statement1, WhereStatement statement2)
        {
            // statement1: {Level1}((Age<15 OR Age>=20) AND (strEmail LIKE 'e%') OR {Level2}(Age BETWEEN 15 AND 20))
            // Statement2: {Level1}((Name = 'Peter'))
            // Return statement: {Level1}((Age<15 or Age>=20) AND (strEmail like 'e%') AND (Name = 'Peter'))

            // Make a copy of statement1
            var result = Copy(statement1);

            // Add all clauses of statement2 to result
            for (int i = 0; i < statement2.ClauseLevels; i++) // for each clause level in statement2
            {
                var level = statement2[i];
                foreach (var clause in level) // for each clause in level i
                {
                    for (int j = 0; j < result.ClauseLevels; j++)  // for each level in result, add the clause
                    {
                        result.AddWhereClauseToLevel(clause, j);
                    }
                }
            }

            return result;
        }

        public static WhereStatement Copy(WhereStatement statement)
        {
            var result = new WhereStatement();
            int currentLevel = 0;
            foreach (var level in statement)
            {
                currentLevel++;
                result.Add(new List<WhereClause>());
                foreach (var clause in statement[currentLevel - 1])
                {
                    var clauseCopy = new WhereClause(clause.FieldName, clause.ComparisonOperator, clause.Value);
                    foreach (var subClause in clause.SubClauses)
                    {
                        var subClauseCopy = new WhereClause.SubClause(subClause.LogicOperator, subClause.ComparisonOperator, subClause.Value);
                        clauseCopy.SubClauses.Add(subClauseCopy);
                    }
                    result[currentLevel - 1].Add(clauseCopy);
                }
            }
            return result;
        }
    }
}