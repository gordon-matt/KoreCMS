using System;
using System.Data;
using System.Data.OleDb;

namespace Kore.Data.OleDb
{
    public static class OleDbConnectionExtensions
    {
        public static ColumnInfoCollection GetColumnData(this OleDbConnection connection, string tableName)
        {
            var restrictions = new object[] { null, null, tableName };
            var foreignKeyRestrictions = new object[] { null, null, null, null, null, tableName };

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            DataTable columnsSchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions);
            DataTable primaryKeySchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, restrictions);
            DataTable foreignKeySchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, foreignKeyRestrictions);

            if (!alreadyOpen)
            {
                connection.Close();
            }

            var columnData = new ColumnInfoCollection();
            foreach (DataRow row in columnsSchema.Rows)
            {
                var columnInfo = new ColumnInfo
                {
                    ColumnName = row.Field<string>("COLUMN_NAME"),
                    DataType = DataTypeConvertor.GetDbType((OleDbType)row.Field<int>("DATA_TYPE")),
                    IsNullable = row.Field<bool>("IS_NULLABLE")
                };

                if (row["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
                {
                    columnInfo.MaximumLength = row.Field<long>("CHARACTER_MAXIMUM_LENGTH");
                }
                columnInfo.OrdinalPosition = row.Field<long>("ORDINAL_POSITION");

                if (row.Field<bool>("COLUMN_HASDEFAULT"))
                {
                    columnInfo.DefaultValue = row.Field<string>("COLUMN_DEFAULT");
                }

                if (primaryKeySchema != null)
                {
                    foreach (DataRow pkRow in primaryKeySchema.Rows)
                    {
                        if (columnInfo.ColumnName == pkRow.Field<string>("COLUMN_NAME"))
                        {
                            columnInfo.KeyType = KeyType.PrimaryKey;
                            break;
                        }
                    }
                }

                if (columnInfo.KeyType == KeyType.None)
                {
                    if (foreignKeySchema != null)
                    {
                        foreach (DataRow fkRow in foreignKeySchema.Rows)
                        {
                            if (columnInfo.ColumnName == fkRow.Field<string>("FK_COLUMN_NAME"))
                            {
                                columnInfo.KeyType = KeyType.ForeignKey;
                                break;
                            }
                        }
                    }
                }

                if (row["NUMERIC_PRECISION"] != DBNull.Value)
                {
                    columnInfo.Precision = row.Field<int>("NUMERIC_PRECISION");
                }

                if (row["NUMERIC_SCALE"] != DBNull.Value)
                {
                    columnInfo.Scale = row.Field<int>("NUMERIC_SCALE");
                }

                columnData.Add(columnInfo);
            }

            columnsSchema.DisposeIfNotNull();
            primaryKeySchema.DisposeIfNotNull();
            foreignKeySchema.DisposeIfNotNull();

            return columnData;
        }

        public static ForeignKeyInfoCollection GetForeignKeyData(this OleDbConnection connection, string tableName)
        {
            var foreignKeyRestrictions = new object[] { null, null, null, null, null, tableName };

            bool alreadyOpen = (connection.State != ConnectionState.Closed);

            if (!alreadyOpen)
            {
                connection.Open();
            }

            DataTable foreignKeySchema = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, foreignKeyRestrictions);

            if (!alreadyOpen)
            {
                connection.Close();
            }

            var foreignKeyData = new ForeignKeyInfoCollection();
            foreach (DataRow row in foreignKeySchema.Rows)
            {
                var fkInfo = new ForeignKeyInfo
                {
                    ForeignKeyColumn = row.Field<string>("FK_COLUMN_NAME"),
                    ForeignKeyName = row.Field<string>("FK_NAME"),
                    ForeignKeyTable = row.Field<string>("FK_TABLE_NAME"),
                    PrimaryKeyColumn = row.Field<string>("PK_COLUMN_NAME"),
                    PrimaryKeyName = row.Field<string>("PK_NAME"),
                    PrimaryKeyTable = row.Field<string>("PK_TABLE_NAME")
                };
                foreignKeyData.Add(fkInfo);
            }
            return foreignKeyData;
        }
    }
}