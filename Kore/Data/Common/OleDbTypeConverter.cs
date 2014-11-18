using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Kore.Collections.Generic;

namespace Kore.Data.Common
{
    internal static class OleDbTypeConverter
    {
        private static PairList<OleDbType, Type> netTypes = new PairList<OleDbType, Type>();
        private static PairList<OleDbType, SqlDbType> sqlDbTypes = new PairList<OleDbType, SqlDbType>();
        private static PairList<OleDbType, DbType> dbTypes = new PairList<OleDbType, DbType>();

        static OleDbTypeConverter()
        {
            netTypes.Add(OleDbType.BigInt, typeof(Int64));
            netTypes.Add(OleDbType.Binary, typeof(Byte[]));
            netTypes.Add(OleDbType.Boolean, typeof(Boolean));
            netTypes.Add(OleDbType.BSTR, typeof(String));
            netTypes.Add(OleDbType.Char, typeof(String));
            netTypes.Add(OleDbType.Currency, typeof(Decimal));
            netTypes.Add(OleDbType.Date, typeof(DateTime));
            netTypes.Add(OleDbType.DBDate, typeof(DateTime));
            netTypes.Add(OleDbType.DBTime, typeof(TimeSpan));
            netTypes.Add(OleDbType.DBTimeStamp, typeof(DateTime));
            netTypes.Add(OleDbType.Decimal, typeof(Decimal));
            netTypes.Add(OleDbType.Double, typeof(Double));
            netTypes.Add(OleDbType.Empty, null);
            netTypes.Add(OleDbType.Error, typeof(Exception));
            netTypes.Add(OleDbType.Filetime, typeof(DateTime));
            netTypes.Add(OleDbType.Guid, typeof(Guid));
            netTypes.Add(OleDbType.IDispatch, typeof(Object));
            netTypes.Add(OleDbType.Integer, typeof(Int32));
            netTypes.Add(OleDbType.IUnknown, typeof(Object));
            netTypes.Add(OleDbType.LongVarBinary, typeof(Byte[]));
            netTypes.Add(OleDbType.LongVarChar, typeof(String));
            netTypes.Add(OleDbType.LongVarWChar, typeof(String));
            netTypes.Add(OleDbType.Numeric, typeof(Decimal));
            netTypes.Add(OleDbType.PropVariant, typeof(Object));
            netTypes.Add(OleDbType.Single, typeof(Single));
            netTypes.Add(OleDbType.SmallInt, typeof(Int16));
            netTypes.Add(OleDbType.TinyInt, typeof(SByte));
            netTypes.Add(OleDbType.UnsignedBigInt, typeof(UInt64));
            netTypes.Add(OleDbType.UnsignedInt, typeof(UInt32));
            netTypes.Add(OleDbType.UnsignedSmallInt, typeof(UInt16));
            netTypes.Add(OleDbType.UnsignedTinyInt, typeof(Byte));
            netTypes.Add(OleDbType.VarBinary, typeof(Byte[]));
            netTypes.Add(OleDbType.VarChar, typeof(String));
            netTypes.Add(OleDbType.Variant, typeof(Object));
            netTypes.Add(OleDbType.VarNumeric, typeof(Decimal));
            netTypes.Add(OleDbType.VarWChar, typeof(String));
            netTypes.Add(OleDbType.WChar, typeof(String));

            sqlDbTypes.Add(OleDbType.BigInt, SqlDbType.BigInt);
            sqlDbTypes.Add(OleDbType.Binary, SqlDbType.Binary);
            sqlDbTypes.Add(OleDbType.Boolean, SqlDbType.Bit);
            sqlDbTypes.Add(OleDbType.BSTR, SqlDbType.NVarChar);
            sqlDbTypes.Add(OleDbType.Char, SqlDbType.VarChar);
            sqlDbTypes.Add(OleDbType.Currency, SqlDbType.Money);
            sqlDbTypes.Add(OleDbType.Date, SqlDbType.Date);
            sqlDbTypes.Add(OleDbType.DBDate, SqlDbType.Date);
            sqlDbTypes.Add(OleDbType.DBTime, SqlDbType.Time);
            sqlDbTypes.Add(OleDbType.DBTimeStamp, SqlDbType.DateTime);
            sqlDbTypes.Add(OleDbType.Decimal, SqlDbType.Decimal);
            sqlDbTypes.Add(OleDbType.Double, SqlDbType.Float);
            sqlDbTypes.Add(OleDbType.Empty, SqlDbType.Variant);//correct?
            sqlDbTypes.Add(OleDbType.Error, SqlDbType.Variant);//correct?
            sqlDbTypes.Add(OleDbType.Filetime, SqlDbType.VarChar);
            sqlDbTypes.Add(OleDbType.Guid, SqlDbType.UniqueIdentifier);
            sqlDbTypes.Add(OleDbType.IDispatch, SqlDbType.Variant);
            sqlDbTypes.Add(OleDbType.Integer, SqlDbType.Int);
            sqlDbTypes.Add(OleDbType.IUnknown, SqlDbType.Variant);
            sqlDbTypes.Add(OleDbType.LongVarBinary, SqlDbType.VarBinary);
            sqlDbTypes.Add(OleDbType.LongVarChar, SqlDbType.VarChar);
            sqlDbTypes.Add(OleDbType.LongVarWChar, SqlDbType.NVarChar);
            sqlDbTypes.Add(OleDbType.Numeric, SqlDbType.Decimal);
            sqlDbTypes.Add(OleDbType.PropVariant, SqlDbType.Variant);
            sqlDbTypes.Add(OleDbType.Single, SqlDbType.Float);
            sqlDbTypes.Add(OleDbType.SmallInt, SqlDbType.SmallInt);
            sqlDbTypes.Add(OleDbType.TinyInt, SqlDbType.TinyInt);//cannot map exactly
            sqlDbTypes.Add(OleDbType.UnsignedBigInt, SqlDbType.BigInt);//cannot map exactly
            sqlDbTypes.Add(OleDbType.UnsignedInt, SqlDbType.Int);//cannot map exactly
            sqlDbTypes.Add(OleDbType.UnsignedSmallInt, SqlDbType.SmallInt);//cannot map exactly
            sqlDbTypes.Add(OleDbType.UnsignedTinyInt, SqlDbType.TinyInt);
            sqlDbTypes.Add(OleDbType.VarBinary, SqlDbType.VarBinary);
            sqlDbTypes.Add(OleDbType.VarChar, SqlDbType.VarChar);
            sqlDbTypes.Add(OleDbType.Variant, SqlDbType.Variant);
            sqlDbTypes.Add(OleDbType.VarNumeric, SqlDbType.Decimal);
            sqlDbTypes.Add(OleDbType.VarWChar, SqlDbType.NVarChar);
            sqlDbTypes.Add(OleDbType.WChar, SqlDbType.NVarChar);

            dbTypes.Add(OleDbType.BigInt, DbType.Int64);
            dbTypes.Add(OleDbType.Binary, DbType.Binary);
            dbTypes.Add(OleDbType.Boolean, DbType.Boolean);
            dbTypes.Add(OleDbType.BSTR, DbType.String);
            dbTypes.Add(OleDbType.Char, DbType.String);
            dbTypes.Add(OleDbType.Currency, DbType.Currency);
            dbTypes.Add(OleDbType.Date, DbType.Date);
            dbTypes.Add(OleDbType.DBDate, DbType.Date);
            dbTypes.Add(OleDbType.DBTime, DbType.Time);
            dbTypes.Add(OleDbType.DBTimeStamp, DbType.DateTime);
            dbTypes.Add(OleDbType.Decimal, DbType.Decimal);
            dbTypes.Add(OleDbType.Double, DbType.Double);
            dbTypes.Add(OleDbType.Empty, DbType.Object);//correct?
            dbTypes.Add(OleDbType.Error, DbType.Object);//correct?
            dbTypes.Add(OleDbType.Filetime, DbType.UInt64);
            dbTypes.Add(OleDbType.Guid, DbType.Guid);
            dbTypes.Add(OleDbType.IDispatch, DbType.Object);
            dbTypes.Add(OleDbType.Integer, DbType.Int32);
            dbTypes.Add(OleDbType.IUnknown, DbType.Object);
            dbTypes.Add(OleDbType.LongVarBinary, DbType.Binary);
            dbTypes.Add(OleDbType.LongVarChar, DbType.String);
            dbTypes.Add(OleDbType.LongVarWChar, DbType.String);
            dbTypes.Add(OleDbType.Numeric, DbType.Decimal);
            dbTypes.Add(OleDbType.PropVariant, DbType.Object);
            dbTypes.Add(OleDbType.Single, DbType.Single);
            dbTypes.Add(OleDbType.SmallInt, DbType.Int16);
            dbTypes.Add(OleDbType.TinyInt, DbType.SByte);
            dbTypes.Add(OleDbType.UnsignedBigInt, DbType.UInt64);
            dbTypes.Add(OleDbType.UnsignedInt, DbType.UInt32);
            dbTypes.Add(OleDbType.UnsignedSmallInt, DbType.UInt16);
            dbTypes.Add(OleDbType.UnsignedTinyInt, DbType.Byte);
            dbTypes.Add(OleDbType.VarBinary, DbType.Binary);
            dbTypes.Add(OleDbType.VarChar, DbType.String);
            dbTypes.Add(OleDbType.Variant, DbType.Object);
            dbTypes.Add(OleDbType.VarNumeric, DbType.VarNumeric);
            dbTypes.Add(OleDbType.VarWChar, DbType.String);
            dbTypes.Add(OleDbType.WChar, DbType.String);
        }

        public static Type ToSystemType(OleDbType oleDbType)
        {
            return netTypes.SingleOrDefault(x => x.First == oleDbType).Second;
        }

        public static DbType ToDbType(OleDbType oleDbType)
        {
            return dbTypes.SingleOrDefault(x => x.First == oleDbType).Second;
        }

        public static SqlDbType ToSqlDbType(OleDbType oleDbType)
        {
            return sqlDbTypes.SingleOrDefault(x => x.First == oleDbType).Second;
        }
    }
}