using System;

namespace BDD.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ManyToMany : Attribute
    {
        public string Table { get; private set; }
        public string PrimaryKey { get; private set; }
        public string ForeignKey { get; private set; }
        public string ?Quantity { get; private set; }

        public bool IsQuantifiable 
        {
            get
            {
                return this.Quantity != null;
            }
        }

        public ManyToMany(string table, string primaryKey, string foreignKey, string ?quantity=null) 
        {
            this.Table = table;
            this.PrimaryKey = primaryKey;
            this.ForeignKey = foreignKey;
            this.Quantity = quantity;
        }
    }
}
