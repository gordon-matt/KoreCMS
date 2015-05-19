using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Kore.Collections.Generic;

namespace Kore.Data.Common
{
    internal static class OleDbTypeConverter
    {
        private static Lazy<TupleHashSet<OleDbType, Type>> netTypes;
        private static Lazy<TupleHashSet<OleDbType, SqlDbType>> sqlDbTypes;
        private static Lazy<TupleHashSet<OleDbType, DbType>> dbTypes;

        static OleDbTypeConverter()
        {
            netTypes = new Lazy<TupleHashSet<OleDbType, Type>>(() =>
            {
                var hashSet = new TupleHashSet<OleDbType, Type>();
                hashSet.Add(OleDbType.BigInt, typeof(Int64));
                hashSet.Add(OleDbType.Binary, typeof(Byte[]));
                hashSet.Add(OleDbType.Boolean, typeof(Boolean));
                hashSet.Add(OleDbType.BSTR, typeof(String));
                hashSet.Add(OleDbType.Char, typeof(String));
                hashSet.Add(OleDbType.Currency, typeof(Decimal));
                hashSet.Add(OleDbType.Date, typeof(DateTime));
                hashSet.Add(OleDbType.DBDate, typeof(DateTime));
                hashSet.Add(OleDbType.DBTime, typeof(TimeSpan));
                hashSet.Add(OleDbType.DBTimeStamp, typeof(DateTime));
                hashSet.Add(OleDbType.Decimal, typeof(Decimal));
                hashSet.Add(OleDbType.Double, typeof(Double));
                hashSet.Add(OleDbType.Empty, null);
                hashSet.Add(OleDbType.Error, typeof(Exception));
                hashSet.Add(OleDbType.Filetime, typeof(DateTime));
                hashSet.Add(OleDbType.Guid, typeof(Guid));
                hashSet.Add(OleDbType.IDispatch, typeof(Object));
                hashSet.Add(OleDbType.Integer, typeof(Int32));
                hashSet.Add(OleDbType.IUnknown, typeof(Object));
                hashSet.Add(OleDbType.LongVarBinary, typeof(Byte[]));
                hashSet.Add(OleDbType.LongVarChar, typeof(String));
                hashSet.Add(OleDbType.LongVarWChar, typeof(String));
                hashSet.Add(OleDbType.Numeric, typeof(Decimal));
                hashSet.Add(OleDbType.PropVariant, typeof(Object));
                hashSet.Add(OleDbType.Single, typeof(Single));
                hashSet.Add(OleDbType.SmallInt, typeof(Int16));
                hashSet.Add(OleDbType.TinyInt, typeof(SByte));
                hashSet.Add(OleDbType.UnsignedBigInt, typeof(UInt64));
                hashSet.Add(OleDbType.UnsignedInt, typeof(UInt32));
                hashSet.Add(OleDbType.UnsignedSmallInt, typeof(UInt16));
                hashSet.Add(OleDbType.UnsignedTinyInt, typeof(Byte));
                hashSet.Add(OleDbType.VarBinary, typeof(Byte[]));
                hashSet.Add(OleDbType.VarChar, typeof(String));
                hashSet.Add(OleDbType.Variant, typeof(Object));
                hashSet.Add(OleDbType.VarNumeric, typeof(Decimal));
                hashSet.Add(OleDbType.VarWChar, typeof(String));
                hashSet.Add(OleDbType.WChar, typeof(String));
                return hashSet;
            });
            sqlDbTypes = new Lazy<TupleHashSet<OleDbType, SqlDbType>>(() =>
            {
                var hashSet = new TupleHashSet<OleDbType, SqlDbType>();
                hashSet.Add(OleDbType.BigInt, SqlDbType.BigInt);
                hashSet.Add(OleDbType.Binary, SqlDbType.Binary);
                hashSet.Add(OleDbType.Boolean, SqlDbType.Bit);
                hashSet.Add(OleDbType.BSTR, SqlDbType.NVarChar);
                hashSet.Add(OleDbType.Char, SqlDbType.VarChar);
                hashSet.Add(OleDbType.Currency, SqlDbType.Money);
                hashSet.Add(OleDbType.Date, SqlDbType.Date);
                hashSet.Add(OleDbType.DBDate, SqlDbType.Date);
                hashSet.Add(OleDbType.DBTime, SqlDbType.Time);
                hashSet.Add(OleDbType.DBTimeStamp, SqlDbType.DateTime);
                hashSet.Add(OleDbType.Decimal, SqlDbType.Decimal);
                hashSet.Add(OleDbType.Double, SqlDbType.Float);
                hashSet.Add(OleDbType.Empty, SqlDbType.Variant);//correct?
                hashSet.Add(OleDbType.Error, SqlDbType.Variant);//correct?
                hashSet.Add(OleDbType.Filetime, SqlDbType.VarChar);
                hashSet.Add(OleDbType.Guid, SqlDbType.UniqueIdentifier);
                hashSet.Add(OleDbType.IDispatch, SqlDbType.Variant);
                hashSet.Add(OleDbType.Integer, SqlDbType.Int);
                hashSet.Add(OleDbType.IUnknown, SqlDbType.Variant);
                hashSet.Add(OleDbType.LongVarBinary, SqlDbType.VarBinary);
                hashSet.Add(OleDbType.LongVarChar, SqlDbType.VarChar);
                hashSet.Add(OleDbType.LongVarWChar, SqlDbType.NVarChar);
                hashSet.Add(OleDbType.Numeric, SqlDbType.Decimal);
                hashSet.Add(OleDbType.PropVariant, SqlDbType.Variant);
                hashSet.Add(OleDbType.Single, SqlDbType.Float);
                hashSet.Add(OleDbType.SmallInt, SqlDbType.SmallInt);
                hashSet.Add(OleDbType.TinyInt, SqlDbType.TinyInt);//cannot map exactly
                hashSet.Add(OleDbType.UnsignedBigInt, SqlDbType.BigInt);//cannot map exactly
                hashSet.Add(OleDbType.UnsignedInt, SqlDbType.Int);//cannot map exactly
                hashSet.Add(OleDbType.UnsignedSmallInt, SqlDbType.SmallInt);//cannot map exactly
                hashSet.Add(OleDbType.UnsignedTinyInt, SqlDbType.TinyInt);
                hashSet.Add(OleDbType.VarBinary, SqlDbType.VarBinary);
                hashSet.Add(OleDbType.VarChar, SqlDbType.VarChar);
                hashSet.Add(OleDbType.Variant, SqlDbType.Variant);
                hashSet.Add(OleDbType.VarNumeric, SqlDbType.Decimal);
                hashSet.Add(OleDbType.VarWChar, SqlDbType.NVarChar);
                hashSet.Add(OleDbType.WChar, SqlDbType.NVarChar);
                return hashSet;
            });
            dbTypes = new Lazy<TupleHashSet<OleDbType, DbType>>(() =>
            {
                var hashSet = new TupleHashSet<OleDbType, DbType>();
                hashSet.Add(OleDbType.BigInt, DbType.Int64);
                hashSet.Add(OleDbType.Binary, DbType.Binary);
                hashSet.Add(OleDbType.Boolean, DbType.Boolean);
                hashSet.Add(OleDbType.BSTR, DbType.String);
                hashSet.Add(OleDbType.Char, DbType.String);
                hashSet.Add(OleDbType.Currency, DbType.Currency);
                hashSet.Add(OleDbType.Date, DbType.Date);
                hashSet.Add(OleDbType.DBDate, DbType.Date);
                hashSet.Add(OleDbType.DBTime, DbType.Time);
                hashSet.Add(OleDbType.DBTimeStamp, DbType.DateTime);
                hashSet.Add(OleDbType.Decimal, DbType.Decimal);
                hashSet.Add(OleDbType.Double, DbType.Double);
                hashSet.Add(OleDbType.Empty, DbType.Object);//correct?
                hashSet.Add(OleDbType.Error, DbType.Object);//correct?
                hashSet.Add(OleDbType.Filetime, DbType.UInt64);
                hashSet.Add(OleDbType.Guid, DbType.Guid);
                hashSet.Add(OleDbType.IDispatch, DbType.Object);
                hashSet.Add(OleDbType.Integer, DbType.Int32);
                hashSet.Add(OleDbType.IUnknown, DbType.Object);
                hashSet.Add(OleDbType.LongVarBinary, DbType.Binary);
                hashSet.Add(OleDbType.LongVarChar, DbType.String);
                hashSet.Add(OleDbType.LongVarWChar, DbType.String);
                hashSet.Add(OleDbType.Numeric, DbType.Decimal);
                hashSet.Add(OleDbType.PropVariant, DbType.Object);
                hashSet.Add(OleDbType.Single, DbType.Single);
                hashSet.Add(OleDbType.SmallInt, DbType.Int16);
                hashSet.Add(OleDbType.TinyInt, DbType.SByte);
                hashSet.Add(OleDbType.UnsignedBigInt, DbType.UInt64);
                hashSet.Add(OleDbType.UnsignedInt, DbType.UInt32);
                hashSet.Add(OleDbType.UnsignedSmallInt, DbType.UInt16);
                hashSet.Add(OleDbType.UnsignedTinyInt, DbType.Byte);
                hashSet.Add(OleDbType.VarBinary, DbType.Binary);
                hashSet.Add(OleDbType.VarChar, DbType.String);
                hashSet.Add(OleDbType.Variant, DbType.Object);
                hashSet.Add(OleDbType.VarNumeric, DbType.VarNumeric);
                hashSet.Add(OleDbType.VarWChar, DbType.String);
                hashSet.Add(OleDbType.WChar, DbType.String);
                return hashSet;
            });
        }

        public static Type ToSystemType(OleDbType oleDbType)
        {
            return netTypes.Value.First(x => x.Item1 == oleDbType).Item2;
        }

        public static DbType ToDbType(OleDbType oleDbType)
        {
            return dbTypes.Value.First(x => x.Item1 == oleDbType).Item2;
        }

        public static SqlDbType ToSqlDbType(OleDbType oleDbType)
        {
            return sqlDbTypes.Value.First(x => x.Item1 == oleDbType).Item2;
        }
    }
}