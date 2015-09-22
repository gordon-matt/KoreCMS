using System.Data.Common;

namespace Kore.Data
{
    public interface IKoreDbHelper
    {
        string Escape(string s);

        bool CheckIfTableExists(DbConnection connection, string tableName);
    }
}