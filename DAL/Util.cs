using MySql.Data.MySqlClient;
using System.Data;

namespace ACBWeb.DAL
{
    public static class Util
    {
        public static string? GetNullableString(this MySqlDataReader reader, string column)
            => reader.IsDBNull(column) ? null : reader.GetString(column);

        public static int? GetNullableInt(this MySqlDataReader reader, string column)
            => reader.IsDBNull(column) ? (int?)null : reader.GetInt32(column);

        public static decimal? GetNullableDecimal(this MySqlDataReader reader, string column)
            => reader.IsDBNull(column) ? (decimal?)null : reader.GetDecimal(column);

        public static DateTime? GetNullableDateTime(this MySqlDataReader reader, string column)
            => reader.IsDBNull(column) ? (DateTime?)null : reader.GetDateTime(column);
    }
}
