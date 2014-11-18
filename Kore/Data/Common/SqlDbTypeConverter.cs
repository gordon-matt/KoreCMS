using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Linq;
using Kore.Collections.Generic;

namespace Kore.Data.Common
{
    internal static class SqlDbTypeConverter
    {
        private static PairList<SqlDbType, Type> netTypes = new PairList<SqlDbType, Type>();
        private static PairList<SqlDbType, DbType> dbTypes = new PairList<SqlDbType, DbType>();
        private static PairList<SqlDbType, OleDbType> oleDbTypes = new PairList<SqlDbType, OleDbType>();

        static SqlDbTypeConverter()
        {
            netTypes.Add(SqlDbType.BigInt, typeof(Int64));
            netTypes.Add(SqlDbType.Binary, typeof(Byte[]));
            netTypes.Add(SqlDbType.Bit, typeof(Boolean));
            netTypes.Add(SqlDbType.Char, typeof(String));
            netTypes.Add(SqlDbType.Date, typeof(DateTime));
            netTypes.Add(SqlDbType.DateTime, typeof(DateTime));
            netTypes.Add(SqlDbType.DateTime2, typeof(DateTime));
            netTypes.Add(SqlDbType.DateTimeOffset, typeof(DateTimeOffset));
            netTypes.Add(SqlDbType.Decimal, typeof(Decimal));
            netTypes.Add(SqlDbType.Float, typeof(Double));
            netTypes.Add(SqlDbType.Int, typeof(Int32));
            netTypes.Add(SqlDbType.Money, typeof(Decimal));
            netTypes.Add(SqlDbType.NChar, typeof(String));
            netTypes.Add(SqlDbType.NText, typeof(String));
            netTypes.Add(SqlDbType.NVarChar, typeof(String));
            netTypes.Add(SqlDbType.Real, typeof(Single));
            netTypes.Add(SqlDbType.SmallDateTime, typeof(DateTime));
            netTypes.Add(SqlDbType.SmallInt, typeof(Int16));
            netTypes.Add(SqlDbType.SmallMoney, typeof(Decimal));
            netTypes.Add(SqlDbType.Structured, typeof(Object));
            netTypes.Add(SqlDbType.Text, typeof(String));
            netTypes.Add(SqlDbType.Time, typeof(TimeSpan));
            netTypes.Add(SqlDbType.Timestamp, typeof(Byte[]));
            netTypes.Add(SqlDbType.TinyInt, typeof(Byte));
            netTypes.Add(SqlDbType.Udt, typeof(Object));
            netTypes.Add(SqlDbType.UniqueIdentifier, typeof(Guid));
            netTypes.Add(SqlDbType.VarBinary, typeof(Byte[]));
            netTypes.Add(SqlDbType.VarChar, typeof(String));
            netTypes.Add(SqlDbType.Variant, typeof(Object));
            netTypes.Add(SqlDbType.Xml, typeof(SqlXml));

            dbTypes.Add(SqlDbType.BigInt, DbType.Int64);
            dbTypes.Add(SqlDbType.Binary, DbType.Binary);
            dbTypes.Add(SqlDbType.Bit, DbType.Boolean);
            dbTypes.Add(SqlDbType.Char, DbType.StringFixedLength);
            dbTypes.Add(SqlDbType.Date, DbType.Date);
            dbTypes.Add(SqlDbType.DateTime, DbType.DateTime);
            dbTypes.Add(SqlDbType.DateTime2, DbType.DateTime2);
            dbTypes.Add(SqlDbType.DateTimeOffset, DbType.DateTimeOffset);
            dbTypes.Add(SqlDbType.Decimal, DbType.Decimal);
            dbTypes.Add(SqlDbType.Float, DbType.Double);
            dbTypes.Add(SqlDbType.Int, DbType.Int32);
            dbTypes.Add(SqlDbType.Money, DbType.Currency);
            dbTypes.Add(SqlDbType.NChar, DbType.String);
            dbTypes.Add(SqlDbType.NText, DbType.String);
            dbTypes.Add(SqlDbType.NVarChar, DbType.String);
            dbTypes.Add(SqlDbType.Real, DbType.Single);
            dbTypes.Add(SqlDbType.SmallDateTime, DbType.DateTime);
            dbTypes.Add(SqlDbType.SmallInt, DbType.Int16);
            dbTypes.Add(SqlDbType.SmallMoney, DbType.Currency);
            dbTypes.Add(SqlDbType.Structured, DbType.Object);
            dbTypes.Add(SqlDbType.Text, DbType.String);
            dbTypes.Add(SqlDbType.Time, DbType.Time);
            dbTypes.Add(SqlDbType.Timestamp, DbType.Binary);
            dbTypes.Add(SqlDbType.TinyInt, DbType.Byte);
            dbTypes.Add(SqlDbType.Udt, DbType.Object);
            dbTypes.Add(SqlDbType.UniqueIdentifier, DbType.Guid);
            dbTypes.Add(SqlDbType.VarBinary, DbType.Binary);
            dbTypes.Add(SqlDbType.VarChar, DbType.String);
            dbTypes.Add(SqlDbType.Variant, DbType.Object);
            dbTypes.Add(SqlDbType.Xml, DbType.Xml);

            oleDbTypes.Add(SqlDbType.BigInt, OleDbType.BigInt);
            oleDbTypes.Add(SqlDbType.Binary, OleDbType.Binary);
            oleDbTypes.Add(SqlDbType.Bit, OleDbType.Boolean);
            oleDbTypes.Add(SqlDbType.Char, OleDbType.Char);
            oleDbTypes.Add(SqlDbType.Date, OleDbType.DBTimeStamp);
            oleDbTypes.Add(SqlDbType.DateTime, OleDbType.DBTimeStamp);
            oleDbTypes.Add(SqlDbType.DateTime2, OleDbType.DBTimeStamp);
            oleDbTypes.Add(SqlDbType.DateTimeOffset, OleDbType.VarChar);
            oleDbTypes.Add(SqlDbType.Decimal, OleDbType.Decimal);
            oleDbTypes.Add(SqlDbType.Float, OleDbType.Double);
            oleDbTypes.Add(SqlDbType.Int, OleDbType.Integer);
            oleDbTypes.Add(SqlDbType.Money, OleDbType.Currency);
            oleDbTypes.Add(SqlDbType.NChar, OleDbType.Char);
            oleDbTypes.Add(SqlDbType.NText, OleDbType.VarChar);
            oleDbTypes.Add(SqlDbType.NVarChar, OleDbType.VarChar);
            oleDbTypes.Add(SqlDbType.Real, OleDbType.Single);
            oleDbTypes.Add(SqlDbType.SmallDateTime, OleDbType.DBTimeStamp);
            oleDbTypes.Add(SqlDbType.SmallInt, OleDbType.SmallInt);
            oleDbTypes.Add(SqlDbType.SmallMoney, OleDbType.Currency);
            oleDbTypes.Add(SqlDbType.Structured, OleDbType.Variant);
            oleDbTypes.Add(SqlDbType.Text, OleDbType.VarChar);
            oleDbTypes.Add(SqlDbType.Time, OleDbType.DBTime);
            oleDbTypes.Add(SqlDbType.Timestamp, OleDbType.Binary);
            oleDbTypes.Add(SqlDbType.TinyInt, OleDbType.TinyInt);
            oleDbTypes.Add(SqlDbType.Udt, OleDbType.VarChar);
            oleDbTypes.Add(SqlDbType.UniqueIdentifier, OleDbType.Guid);
            oleDbTypes.Add(SqlDbType.VarBinary, OleDbType.VarBinary);
            oleDbTypes.Add(SqlDbType.VarChar, OleDbType.VarChar);
            oleDbTypes.Add(SqlDbType.Variant, OleDbType.Variant);
            oleDbTypes.Add(SqlDbType.Xml, OleDbType.VarChar);
        }

        public static Type ToSystemType(SqlDbType sqlDbType)
        {
            return netTypes.SingleOrDefault(x => x.First == sqlDbType).Second;
        }

        public static DbType ToDbType(SqlDbType sqlDbType)
        {
            return dbTypes.SingleOrDefault(x => x.First == sqlDbType).Second;
        }

        public static OleDbType ToOleDbType(SqlDbType sqlDbType)
        {
            return oleDbTypes.SingleOrDefault(x => x.First == sqlDbType).Second;
        }
    }
}