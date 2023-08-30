using System;

namespace BDD.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Table : Attribute
    {
        public string Name { get; private set; }

        public Table(string name)
        {
            Name = name;
        }
    }
}
