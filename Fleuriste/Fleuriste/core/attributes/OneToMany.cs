using System;

namespace BDD.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OneToMany : Attribute
    {
        public string Key { get; private set; }

        public OneToMany(string key) 
        {
            this.Key = key;
        }
    }
}
