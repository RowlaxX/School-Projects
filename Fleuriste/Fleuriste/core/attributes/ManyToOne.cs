using System;

namespace BDD.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ManyToOne : Attribute
    {
        public string Column { get; private set; }

        public ManyToOne(string column) 
        {
            this.Column = column;
        }
    }
}
