using System;
using System.Collections.Generic;
using System.Reflection;

namespace BDD.Core.Relations
{
    public abstract class RelationService
    {
        public enum Cardinality { Many, One };

        public Database Database { get; private set; }
        public Cardinality StartCardinality { get; private set; }
        public Cardinality EndCardinality { get; private set; }
        public EntityService Start { get; private set; }
        public EntityService End { get; private set; }
        public Type AcceptedType { get; private set; }

        protected RelationService(Database db, Cardinality startCardinality, Cardinality endCardinality, Type accepted, EntityService start, PropertyInfo prop)
        {
            this.Database = db;
            this.StartCardinality = startCardinality;
            this.EndCardinality = endCardinality;
            this.Start = start;
            this.AcceptedType = accepted;
            
            Type type = prop.PropertyType;
            
            if (accepted != type.GetGenericTypeDefinition())
                throw new ApplicationException("The property " + prop + " in " + Start.Type + " is not a valid type container");

            type = type.GenericTypeArguments[0];
            this.End = Database.GetService(type);

            Console.WriteLine("Relation " + End.Type + " (" + EndCardinality + ") " + " -> " + Start.Type + " (" + StartCardinality + ")");
        }
    }
}
