using System;

namespace BDD.Core.SQL
{
    public static class SqlUtils
    {
        public static string Format(object? value)
        {
            if (value == null)
                return "null";

            if (value is double d)
                return "'" + string.Format("{0:0.0000}", d).Replace(',','.') + "'";
            if (value is bool b)
                return b ? "'1'" : "'0'";
            if (value is DateTime dt)
                return "'" + dt.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            if (value is byte[] bytes)
                return "0x" + BitConverter.ToString(bytes).Replace("-", "");

            return "'" + value.ToString() + "'";
        }
    }
}
