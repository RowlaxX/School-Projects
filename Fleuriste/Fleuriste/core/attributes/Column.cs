using System;

namespace BDD.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Column : Attribute
    {
        public string Name { get; private set; }

        public bool IsPrimaryKey
        {
            get
            {
                return Name.Equals("id", StringComparison.OrdinalIgnoreCase);
            }
        }

        public Column(string name)
        {
            Name = name;
        }
    }
}
