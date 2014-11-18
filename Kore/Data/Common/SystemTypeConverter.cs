using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Kore.Collections.Generic;

namespace Kore.Data.Common
{
    internal static class SystemTypeConverter
    {
        private static PairList<Type, DbType> dbTypes = new PairList<Type, DbType>();
        private static PairList<Type, SqlDbType> sqlDbTypes = new PairList<Type, SqlDbType>();
        private static PairList<Type, OleDbType> oleDbTypes = new PairList<Type, OleDbType>();

        static SystemTypeConverter()
        {
            dbTypes.Add(typeof(Boolean), DbType.Boolean);
            dbTypes.Add(typeof(Byte), DbType.Byte);
            dbTypes.Add(typeof(Char), DbType.StringFixedLength);
            dbTypes.Add(typeof(Int16), DbType.Int16);
            dbTypes.Add(typeof(Int32), DbType.Int32);
            dbTypes.Add(typeof(Int64), DbType.Int64);
            dbTypes.Add(typeof(Decimal), DbType.Decimal);
            dbTypes.Add(typeof(Double), DbType.Double);
            dbTypes.Add(typeof(DateTime), DbType.DateTime);
            dbTypes.Add(typeof(DateTimeOffset), DbType.DateTimeOffset);
            dbTypes.Add(typeof(Guid), DbType.Guid);
            dbTypes.Add(typeof(Single), DbType.Single);
            dbTypes.Add(typeof(String), DbType.String);
            dbTypes.Add(typeof(SByte), DbType.SByte);
            dbTypes.Add(typeof(TimeSpan), DbType.Time);
            dbTypes.Add(typeof(UInt16), DbType.UInt16);
            dbTypes.Add(typeof(UInt32), DbType.UInt32);
            dbTypes.Add(typeof(UInt64), DbType.UInt64);
            dbTypes.Add(typeof(Uri), DbType.String);

            sqlDbTypes.Add(typeof(Boolean), SqlDbType.Bit);
            sqlDbTypes.Add(typeof(Byte), SqlDbType.TinyInt);
            sqlDbTypes.Add(typeof(Char), SqlDbType.NChar);
            sqlDbTypes.Add(typeof(Int16), SqlDbType.SmallInt);
            sqlDbTypes.Add(typeof(Int32), SqlDbType.Int);
            sqlDbTypes.Add(typeof(Int64), SqlDbType.BigInt);
            sqlDbTypes.Add(typeof(Decimal), SqlDbType.Decimal);
            sqlDbTypes.Add(typeof(Double), SqlDbType.Float);
            sqlDbTypes.Add(typeof(DateTime), SqlDbType.DateTime);
            sqlDbTypes.Add(typeof(DateTimeOffset), SqlDbType.DateTimeOffset);
            sqlDbTypes.Add(typeof(Guid), SqlDbType.UniqueIdentifier);
            sqlDbTypes.Add(typeof(Single), SqlDbType.Real);
            sqlDbTypes.Add(typeof(String), SqlDbType.NVarChar);
            sqlDbTypes.Add(typeof(SByte), SqlDbType.TinyInt);
            sqlDbTypes.Add(typeof(TimeSpan), SqlDbType.Time);
            sqlDbTypes.Add(typeof(UInt16), SqlDbType.SmallInt);
            sqlDbTypes.Add(typeof(UInt32), SqlDbType.Int);
            sqlDbTypes.Add(typeof(UInt64), SqlDbType.BigInt);
            sqlDbTypes.Add(typeof(Uri), SqlDbType.NVarChar);

            oleDbTypes.Add(typeof(Boolean), OleDbType.Boolean);
            oleDbTypes.Add(typeof(Byte), OleDbType.UnsignedTinyInt);
            oleDbTypes.Add(typeof(Char), OleDbType.Char);//check
            oleDbTypes.Add(typeof(Int16), OleDbType.SmallInt);
            oleDbTypes.Add(typeof(Int32), OleDbType.Integer);
            oleDbTypes.Add(typeof(Int64), OleDbType.BigInt);
            oleDbTypes.Add(typeof(Decimal), OleDbType.Decimal);
            oleDbTypes.Add(typeof(Double), OleDbType.Double);
            oleDbTypes.Add(typeof(DateTime), OleDbType.DBTimeStamp);
            oleDbTypes.Add(typeof(DateTimeOffset), OleDbType.VarChar);
            oleDbTypes.Add(typeof(Guid), OleDbType.Guid);
            oleDbTypes.Add(typeof(Single), OleDbType.Single);
            oleDbTypes.Add(typeof(String), OleDbType.VarChar);
            oleDbTypes.Add(typeof(SByte), OleDbType.TinyInt);
            oleDbTypes.Add(typeof(TimeSpan), OleDbType.DBTime);
            oleDbTypes.Add(typeof(UInt16), OleDbType.UnsignedSmallInt);
            oleDbTypes.Add(typeof(UInt32), OleDbType.UnsignedInt);
            oleDbTypes.Add(typeof(UInt64), OleDbType.UnsignedBigInt);
            oleDbTypes.Add(typeof(Uri), OleDbType.VarChar);
        }

        public static DbType ToDbType(Type systemType)
        {
            return dbTypes.SingleOrDefault(x => x.First == systemType).Second;
        }

        public static SqlDbType ToSqlDbType(Type systemType)
        {
            return sqlDbTypes.SingleOrDefault(x => x.First == systemType).Second;
        }

        public static OleDbType ToOleDbType(Type systemType)
        {
            return oleDbTypes.SingleOrDefault(x => x.First == systemType).Second;
        }
    }
}