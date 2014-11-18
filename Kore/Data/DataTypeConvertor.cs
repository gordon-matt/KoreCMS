using System;
using System.Data;
using System.Data.OleDb;
using Kore.Data.Common;

namespace Kore.Data
{
    public static class DataTypeConvertor
    {
        public static Type GetSystemType(DbType dbType)
        {
            return DbTypeConverter.ToSystemType(dbType);
        }

        public static Type GetSystemType(SqlDbType sqlDbType)
        {
            return SqlDbTypeConverter.ToSystemType(sqlDbType);
        }

        public static Type GetSystemType(OleDbType oleDbType)
        {
            return OleDbTypeConverter.ToSystemType(oleDbType);
        }

        public static DbType GetDbType(Type type)
        {
            return SystemTypeConverter.ToDbType(type);
        }

        public static DbType GetDbType(SqlDbType sqlDbType)
        {
            return SqlDbTypeConverter.ToDbType(sqlDbType);
        }

        public static DbType GetDbType(OleDbType oleDbType)
        {
            return OleDbTypeConverter.ToDbType(oleDbType);
        }

        public static SqlDbType GetSqlDbType(Type type)
        {
            return SystemTypeConverter.ToSqlDbType(type);
        }

        public static SqlDbType GetSqlDbType(DbType dbType)
        {
            return DbTypeConverter.ToSqlDbType(dbType);
        }

        public static SqlDbType GetSqlDbType(OleDbType oleDbType)
        {
            return OleDbTypeConverter.ToSqlDbType(oleDbType);
        }

        public static OleDbType GetOleDbType(Type type)
        {
            return SystemTypeConverter.ToOleDbType(type);
        }

        public static OleDbType GetOleDbType(DbType dbType)
        {
            return DbTypeConverter.ToOleDbType(dbType);
        }

        public static OleDbType GetOleDbType(SqlDbType sqlDbType)
        {
            return SqlDbTypeConverter.ToOleDbType(sqlDbType);
        }
    }
}