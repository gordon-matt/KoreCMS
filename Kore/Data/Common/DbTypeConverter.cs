using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Linq;
using Kore.Collections.Generic;

namespace Kore.Data.Common
{
    internal static class DbTypeConverter
    {
        private static PairList<DbType, Type> netTypes = new PairList<DbType, Type>();
        private static PairList<DbType, SqlDbType> sqlDbTypes = new PairList<DbType, SqlDbType>();
        private static PairList<DbType, OleDbType> oleDbTypes = new PairList<DbType, OleDbType>();

        static DbTypeConverter()
        {
            netTypes.Add(DbType.AnsiString, typeof(String));
            netTypes.Add(DbType.AnsiStringFixedLength, typeof(Char));
            netTypes.Add(DbType.Binary, typeof(Byte[]));
            netTypes.Add(DbType.Boolean, typeof(Boolean));
            netTypes.Add(DbType.Byte, typeof(Byte));
            netTypes.Add(DbType.Currency, typeof(Decimal));
            netTypes.Add(DbType.Date, typeof(DateTime));
            netTypes.Add(DbType.DateTime, typeof(DateTime));
            netTypes.Add(DbType.DateTime2, typeof(DateTime));
            netTypes.Add(DbType.DateTimeOffset, typeof(DateTimeOffset));
            netTypes.Add(DbType.Decimal, typeof(Decimal));
            netTypes.Add(DbType.Double, typeof(Double));
            netTypes.Add(DbType.Guid, typeof(Guid));
            netTypes.Add(DbType.Int16, typeof(Int16));
            netTypes.Add(DbType.Int32, typeof(Int32));
            netTypes.Add(DbType.Int64, typeof(Int64));
            netTypes.Add(DbType.Object, typeof(Object));
            netTypes.Add(DbType.SByte, typeof(SByte));
            netTypes.Add(DbType.Single, typeof(Single));
            netTypes.Add(DbType.String, typeof(String));
            netTypes.Add(DbType.StringFixedLength, typeof(Char));
            netTypes.Add(DbType.Time, typeof(TimeSpan));
            netTypes.Add(DbType.UInt16, typeof(UInt16));
            netTypes.Add(DbType.UInt32, typeof(UInt32));
            netTypes.Add(DbType.UInt64, typeof(UInt64));
            netTypes.Add(DbType.VarNumeric, typeof(Int32));
            netTypes.Add(DbType.Xml, typeof(SqlXml));

            sqlDbTypes.Add(DbType.AnsiString, SqlDbType.VarChar);
            sqlDbTypes.Add(DbType.AnsiStringFixedLength, SqlDbType.Char);
            sqlDbTypes.Add(DbType.Binary, SqlDbType.Binary);
            sqlDbTypes.Add(DbType.Boolean, SqlDbType.Bit);
            sqlDbTypes.Add(DbType.Byte, SqlDbType.TinyInt);
            sqlDbTypes.Add(DbType.Currency, SqlDbType.Money);
            sqlDbTypes.Add(DbType.Date, SqlDbType.Date);
            sqlDbTypes.Add(DbType.DateTime, SqlDbType.DateTime);
            sqlDbTypes.Add(DbType.DateTime2, SqlDbType.DateTime2);
            sqlDbTypes.Add(DbType.DateTimeOffset, SqlDbType.DateTimeOffset);
            sqlDbTypes.Add(DbType.Decimal, SqlDbType.Decimal);
            sqlDbTypes.Add(DbType.Double, SqlDbType.Float);
            sqlDbTypes.Add(DbType.Guid, SqlDbType.UniqueIdentifier);
            sqlDbTypes.Add(DbType.Int16, SqlDbType.SmallInt);
            sqlDbTypes.Add(DbType.Int32, SqlDbType.Int);
            sqlDbTypes.Add(DbType.Int64, SqlDbType.BigInt);
            sqlDbTypes.Add(DbType.Object, SqlDbType.Variant);
            sqlDbTypes.Add(DbType.SByte, SqlDbType.TinyInt);
            sqlDbTypes.Add(DbType.Single, SqlDbType.Real);
            sqlDbTypes.Add(DbType.String, SqlDbType.NVarChar);
            sqlDbTypes.Add(DbType.StringFixedLength, SqlDbType.NChar);
            sqlDbTypes.Add(DbType.Time, SqlDbType.Time);
            sqlDbTypes.Add(DbType.UInt16, SqlDbType.SmallInt);
            sqlDbTypes.Add(DbType.UInt32, SqlDbType.Int);
            sqlDbTypes.Add(DbType.UInt64, SqlDbType.BigInt);
            sqlDbTypes.Add(DbType.VarNumeric, SqlDbType.Int);
            sqlDbTypes.Add(DbType.Xml, SqlDbType.Xml);

            oleDbTypes.Add(DbType.AnsiString, OleDbType.VarChar);
            oleDbTypes.Add(DbType.AnsiStringFixedLength, OleDbType.Char);
            oleDbTypes.Add(DbType.Binary, OleDbType.Binary);
            oleDbTypes.Add(DbType.Boolean, OleDbType.Boolean);
            oleDbTypes.Add(DbType.Byte, OleDbType.UnsignedTinyInt);
            oleDbTypes.Add(DbType.Currency, OleDbType.Currency);
            oleDbTypes.Add(DbType.Date, OleDbType.Date);
            oleDbTypes.Add(DbType.DateTime, OleDbType.DBTimeStamp);
            oleDbTypes.Add(DbType.DateTime2, OleDbType.DBTimeStamp);
            oleDbTypes.Add(DbType.DateTimeOffset, OleDbType.VarChar);
            oleDbTypes.Add(DbType.Decimal, OleDbType.Decimal);
            oleDbTypes.Add(DbType.Double, OleDbType.Double);
            oleDbTypes.Add(DbType.Guid, OleDbType.Guid);
            oleDbTypes.Add(DbType.Int16, OleDbType.SmallInt);
            oleDbTypes.Add(DbType.Int32, OleDbType.Integer);
            oleDbTypes.Add(DbType.Int64, OleDbType.BigInt);
            oleDbTypes.Add(DbType.Object, OleDbType.Variant);
            oleDbTypes.Add(DbType.SByte, OleDbType.TinyInt);
            oleDbTypes.Add(DbType.Single, OleDbType.Single);
            oleDbTypes.Add(DbType.String, OleDbType.VarChar);
            oleDbTypes.Add(DbType.StringFixedLength, OleDbType.Char);
            oleDbTypes.Add(DbType.Time, OleDbType.DBTime);
            oleDbTypes.Add(DbType.UInt16, OleDbType.UnsignedSmallInt);
            oleDbTypes.Add(DbType.UInt32, OleDbType.UnsignedInt);
            oleDbTypes.Add(DbType.UInt64, OleDbType.UnsignedBigInt);
            oleDbTypes.Add(DbType.VarNumeric, OleDbType.VarNumeric);
            oleDbTypes.Add(DbType.Xml, OleDbType.VarChar);
        }

        public static Type ToSystemType(DbType dbType)
        {
            return netTypes.SingleOrDefault(x => x.First == dbType).Second;
        }

        public static SqlDbType ToSqlDbType(DbType dbType)
        {
            return sqlDbTypes.SingleOrDefault(x => x.First == dbType).Second;
        }

        public static OleDbType ToOleDbType(DbType dbType)
        {
            return oleDbTypes.SingleOrDefault(x => x.First == dbType).Second;
        }
    }
}