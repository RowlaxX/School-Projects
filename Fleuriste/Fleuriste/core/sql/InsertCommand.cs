using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD.Core.SQL
{
    public class InsertCommand
    {
        private readonly string table;
        private readonly Dictionary<string, object?> param = new();

        public InsertCommand(string table) 
        {
            this.table = table;
        }

        public InsertCommand Add(string column, object? value)
        {
            param.Add(column, value);
            return this;
        }

        public string Build()
        {
            StringBuilder columns = new();
            StringBuilder values = new();

            foreach (var entry in param)
            {
                columns.Append(entry.Key);
                columns.Append(", ");

                values.Append(SqlUtils.Format(entry.Value));
                values.Append(", ");
            }

            columns.Remove(columns.Length - 2, 2);
            values.Remove(values.Length - 2, 2);

            return "INSERT INTO `" + table + "` (" + columns.ToString() + ") VALUES (" + values.ToString() + ")"; 
        }
    }
}
