using BDD.Core.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD.Core.Utils
{
    public class ObjectStringifier
    {
        private readonly string type;
        private readonly Dictionary<string, object?> param = new();

        public ObjectStringifier(string type)
        {
            this.type = type;
        }

        public ObjectStringifier Add(string column, object? value)
        {
            param.Add(column, value);
            return this;
        }

        public string Build()
        {
            StringBuilder sb = new();

            sb.Append(type);
            sb.Append(" [");

            foreach (var entry in param)
                sb.Append(entry.Key).Append('=').Append(entry.Value).Append(", ");

            sb.Remove(sb.Length - 2, 2);
            sb.Append(']');
            return sb.ToString();
        }
    }
}
