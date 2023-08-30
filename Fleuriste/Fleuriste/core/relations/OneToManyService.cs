using BDD.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BDD.Core.Relations
{
    public class OneToManyService : RelationService
    {
        public string Table { get; private set; }
        public string SpecialKey { get; private set; }

        public OneToManyService(Database db, EntityService start, PropertyInfo prop, OneToMany otm) 
            : base(db, Cardinality.One, Cardinality.Many, typeof(OneToManyRelation<>), start, prop)
        {
            this.Table = End.Table;
            this.SpecialKey = otm.Key;
        }

        public object NewLocalRelation(Entity parent)
        {
            return Activator.CreateInstance(typeof(OneToManyLocal<>)
                .MakeGenericType(End.Type), new object[] { this, parent })
                ?? throw new NullReferenceException();
        }

        public object NewOnlineRelation(Entity parent)
        {
            return Activator.CreateInstance(typeof(OneToManyOnline<>)
                .MakeGenericType(End.Type), new object[] { this, parent })
                ?? throw new NullReferenceException();
        }

        public bool Contains(int startKey, int endKey)
        {
            return Database.Connection.ExecuteScalar("SELECT COUNT(*) FROM `" + Table + "` WHERE " + SpecialKey + " = " + startKey + " AND id = " + endKey + " LIMIT 1") > 0;
        }

        public int Size(int startKey)
        {
            return Database.Connection.ExecuteScalar("SELECT COUNT(*) FROM `" + Table + "` WHERE " + SpecialKey + " = " + startKey);
        }

        public List<int> GetId(int startKey)
        {
            List<int> list = new();

            Database.Connection.RunInTransaction("SELECT id FROM `" + Table + "` WHERE " + SpecialKey + " = " + startKey, r =>
            {
                while (r.Read())
                    list.Add(r.GetInt32(""));
            });

            return list;
        }

        public List<T> Get<T>(int startKey) where T : Entity
        {
            List<Entity> l = End.FindAll(SpecialKey, startKey);
            List<T> j = new(l.Count);

            foreach (var e in l)
                j.Add((T)e);

            return j;
        }
    }
}
