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
        private static Lazy<TupleHashSet<SqlDbType, Type>> netTypes;
        private static Lazy<TupleHashSet<SqlDbType, DbType>> dbTypes;
        private static Lazy<TupleHashSet<SqlDbType, OleDbType>> oleDbTypes;

        static SqlDbTypeConverter()
        {
            netTypes = new Lazy<TupleHashSet<SqlDbType, Type>>(() =>
            {
                var hashSet = new TupleHashSet<SqlDbType, Type>();
                hashSet.Add(SqlDbType.BigInt, typeof(long));
                hashSet.Add(SqlDbType.Binary, typeof(byte[]));
                hashSet.Add(SqlDbType.Bit, typeof(bool));
                hashSet.Add(SqlDbType.Char, typeof(string));
                hashSet.Add(SqlDbType.Date, typeof(DateTime));
                hashSet.Add(SqlDbType.DateTime, typeof(DateTime));
                hashSet.Add(SqlDbType.DateTime2, typeof(DateTime));
                hashSet.Add(SqlDbType.DateTimeOffset, typeof(DateTimeOffset));
                hashSet.Add(SqlDbType.Decimal, typeof(decimal));
                hashSet.Add(SqlDbType.Float, typeof(double));
                hashSet.Add(SqlDbType.Image, typeof(byte[]));
                hashSet.Add(SqlDbType.Int, typeof(int));
                hashSet.Add(SqlDbType.Money, typeof(decimal));
                hashSet.Add(SqlDbType.NChar, typeof(string));
                hashSet.Add(SqlDbType.NText, typeof(string));
                hashSet.Add(SqlDbType.NVarChar, typeof(string));
                hashSet.Add(SqlDbType.Real, typeof(float));
                hashSet.Add(SqlDbType.SmallDateTime, typeof(DateTime));
                hashSet.Add(SqlDbType.SmallInt, typeof(short));
                hashSet.Add(SqlDbType.SmallMoney, typeof(decimal));
                hashSet.Add(SqlDbType.Structured, typeof(object));
                hashSet.Add(SqlDbType.Text, typeof(string));
                hashSet.Add(SqlDbType.Time, typeof(TimeSpan));
                hashSet.Add(SqlDbType.Timestamp, typeof(byte[]));
                hashSet.Add(SqlDbType.TinyInt, typeof(byte));
                hashSet.Add(SqlDbType.Udt, typeof(object));
                hashSet.Add(SqlDbType.UniqueIdentifier, typeof(Guid));
                hashSet.Add(SqlDbType.VarBinary, typeof(byte[]));
                hashSet.Add(SqlDbType.VarChar, typeof(string));
                hashSet.Add(SqlDbType.Variant, typeof(object));
                hashSet.Add(SqlDbType.Xml, typeof(SqlXml));
                return hashSet;
            });
            dbTypes = new Lazy<TupleHashSet<SqlDbType, DbType>>(() =>
            {
                var hashSet = new TupleHashSet<SqlDbType, DbType>();
                hashSet.Add(SqlDbType.BigInt, DbType.Int64);
                hashSet.Add(SqlDbType.Binary, DbType.Binary);
                hashSet.Add(SqlDbType.Bit, DbType.Boolean);
                hashSet.Add(SqlDbType.Char, DbType.AnsiStringFixedLength);
                hashSet.Add(SqlDbType.Date, DbType.Date);
                hashSet.Add(SqlDbType.DateTime, DbType.DateTime);
                hashSet.Add(SqlDbType.DateTime2, DbType.DateTime2);
                hashSet.Add(SqlDbType.DateTimeOffset, DbType.DateTimeOffset);
                hashSet.Add(SqlDbType.Decimal, DbType.Decimal);
                hashSet.Add(SqlDbType.Float, DbType.Double);
                hashSet.Add(SqlDbType.Image, DbType.Binary);
                hashSet.Add(SqlDbType.Int, DbType.Int32);
                hashSet.Add(SqlDbType.Money, DbType.Currency);
                hashSet.Add(SqlDbType.NChar, DbType.StringFixedLength);
                hashSet.Add(SqlDbType.NText, DbType.String);
                hashSet.Add(SqlDbType.NVarChar, DbType.String);
                hashSet.Add(SqlDbType.Real, DbType.Single);
                hashSet.Add(SqlDbType.SmallDateTime, DbType.DateTime);
                hashSet.Add(SqlDbType.SmallInt, DbType.Int16);
                hashSet.Add(SqlDbType.SmallMoney, DbType.Currency);
                hashSet.Add(SqlDbType.Structured, DbType.Object);
                hashSet.Add(SqlDbType.Text, DbType.String);
                hashSet.Add(SqlDbType.Time, DbType.Time);
                hashSet.Add(SqlDbType.Timestamp, DbType.Binary);
                hashSet.Add(SqlDbType.TinyInt, DbType.Byte);
                hashSet.Add(SqlDbType.Udt, DbType.Object);
                hashSet.Add(SqlDbType.UniqueIdentifier, DbType.Guid);
                hashSet.Add(SqlDbType.VarBinary, DbType.Binary);
                hashSet.Add(SqlDbType.VarChar, DbType.AnsiString);
                hashSet.Add(SqlDbType.Variant, DbType.Object);
                hashSet.Add(SqlDbType.Xml, DbType.Xml);
                return hashSet;
            });
            oleDbTypes = new Lazy<TupleHashSet<SqlDbType, OleDbType>>(() =>
            {
                var hashSet = new TupleHashSet<SqlDbType, OleDbType>();
                hashSet.Add(SqlDbType.BigInt, OleDbType.BigInt);
                hashSet.Add(SqlDbType.Binary, OleDbType.Binary);
                hashSet.Add(SqlDbType.Bit, OleDbType.Boolean);
                hashSet.Add(SqlDbType.Char, OleDbType.Char);
                hashSet.Add(SqlDbType.Date, OleDbType.DBTimeStamp);
                hashSet.Add(SqlDbType.DateTime, OleDbType.DBTimeStamp);
                hashSet.Add(SqlDbType.DateTime2, OleDbType.DBTimeStamp);
                hashSet.Add(SqlDbType.DateTimeOffset, OleDbType.VarChar);
                hashSet.Add(SqlDbType.Decimal, OleDbType.Decimal);
                hashSet.Add(SqlDbType.Float, OleDbType.Double);
                hashSet.Add(SqlDbType.Image, OleDbType.Binary);
                hashSet.Add(SqlDbType.Int, OleDbType.Integer);
                hashSet.Add(SqlDbType.Money, OleDbType.Currency);
                hashSet.Add(SqlDbType.NChar, OleDbType.Char);
                hashSet.Add(SqlDbType.NText, OleDbType.VarChar);
                hashSet.Add(SqlDbType.NVarChar, OleDbType.VarChar);
                hashSet.Add(SqlDbType.Real, OleDbType.Single);
                hashSet.Add(SqlDbType.SmallDateTime, OleDbType.DBTimeStamp);
                hashSet.Add(SqlDbType.SmallInt, OleDbType.SmallInt);
                hashSet.Add(SqlDbType.SmallMoney, OleDbType.Currency);
                hashSet.Add(SqlDbType.Structured, OleDbType.Variant);
                hashSet.Add(SqlDbType.Text, OleDbType.VarChar);
                hashSet.Add(SqlDbType.Time, OleDbType.DBTime);
                hashSet.Add(SqlDbType.Timestamp, OleDbType.Binary);
                hashSet.Add(SqlDbType.TinyInt, OleDbType.TinyInt);
                hashSet.Add(SqlDbType.Udt, OleDbType.VarChar);
                hashSet.Add(SqlDbType.UniqueIdentifier, OleDbType.Guid);
                hashSet.Add(SqlDbType.VarBinary, OleDbType.VarBinary);
                hashSet.Add(SqlDbType.VarChar, OleDbType.VarChar);
                hashSet.Add(SqlDbType.Variant, OleDbType.Variant);
                hashSet.Add(SqlDbType.Xml, OleDbType.VarChar);
                return hashSet;
            });
        }

        public static Type ToSystemType(SqlDbType sqlDbType)
        {
            return netTypes.Value.First(x => x.Item1 == sqlDbType).Item2;
        }

        public static DbType ToDbType(SqlDbType sqlDbType)
        {
            return dbTypes.Value.First(x => x.Item1 == sqlDbType).Item2;
        }

        public static OleDbType ToOleDbType(SqlDbType sqlDbType)
        {
            return oleDbTypes.Value.First(x => x.Item1 == sqlDbType).Item2;
        }
    }
}