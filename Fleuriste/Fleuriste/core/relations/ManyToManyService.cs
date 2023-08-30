using BDD.Core.Attributes;
using BDD.Core.SQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace BDD.Core.Relations
{
    public class ManyToManyService : RelationService
    {
        public string Table { get; private set; }
        public string? Quantity { get; private set; }
        public string StartKey { get; private set; }
        public string EndKey { get; private set; }

        public bool Quantifiable => Quantity != null;

        public ManyToManyService(Database db, EntityService start, PropertyInfo info, ManyToMany mtm) 
            : base(db, Cardinality.Many, Cardinality.Many, typeof(ManyToManyRelation<>), start, info)
        {
            Table = mtm.Table;
            Quantity = mtm.Quantity;
            StartKey = mtm.PrimaryKey;
            EndKey = mtm.ForeignKey;
        }

        public ManyToManyRelation NewLocalRelation(Entity parent)
        {
            return (ManyToManyRelation) (Activator.CreateInstance(typeof(ManyToManyLocal<>)
                .MakeGenericType(End.Type), new object[] { this, parent })
                ?? throw new NullReferenceException());
        }

        public ManyToManyRelation NewOnlineRelation(Entity parent)
        {
            return (ManyToManyRelation) (Activator.CreateInstance(typeof(ManyToManyOnline<>)
                .MakeGenericType(End.Type), new object[] { this, parent })
                ?? throw new NullReferenceException());
        }

        public ManyToManyRelation Persist(ManyToManyRelation mtm)
        {
            if (mtm.OnlineMode)
                return mtm;

            Dictionary<Entity, int> d = mtm.DescribeRaw();

            foreach (Entity e in d.Keys)
                if (!e.IsPersisted)
                    Database.Persist(e);

            ManyToManyRelation online = NewOnlineRelation(mtm.Parent);
            online.PutAll(d);
            return online;
        }

        public bool Contains(int startKey) => Size(startKey) > 0;
       
        public void Put(int startKey, int endKey) => Put(startKey, endKey, 1);

        public bool Contains(int startKey, int endKey)
        {
            return Database.Connection.ExecuteScalar("SELECT COUNT(*) FROM `" + Table + "` WHERE " + StartKey + " = " + startKey + " AND " + EndKey + " = " + endKey + " LIMIT 1") > 0;
        }

        public int Size(int startKey)
        {
            return Database.Connection.ExecuteScalar("SELECT COUNT(*) FROM `" + Table + "` WHERE " + StartKey + " = " + startKey);
        }

        public void Put(int startKey, int endKey, int quantity)
        {
            InsertCommand ic = new(Table);

            ic.Add(StartKey, startKey);
            ic.Add(EndKey, endKey);

            if (Quantity != null)
                ic.Add(Quantity, quantity);

            try
            {
                Database.Connection.Execute(ic.Build());
            }
            catch (MySqlException)
            {
                if (Quantifiable)
                    SetQuantity(startKey, endKey, quantity);
            }
        }

        public void SetQuantity(int startKey, int endKey, int quantity)
        {
            Database.Connection.Execute("UPDATE `" + Table + "` SET " + Quantity + " = " + quantity + " WHERE " + StartKey + " = " + startKey + " AND " + EndKey + " = " + endKey);
        }

        public void Clear(int startKey)
        {
            Database.Connection.Execute("DELETE FROM `" + Table + "` WHERE " + StartKey + " = " + startKey);
        }

        public void Remove(int startKey, int endKey)
        {
            Database.Connection.Execute("DELETE FROM `" + Table + "` WHERE " + StartKey + " = " + startKey + " AND " + EndKey + " = " + endKey);
        }

        public int GetQuantity(int startKey, int endKey)
        {
            if (!Quantifiable)
                return 1;
            return Database.Connection.ExecuteScalar("SELECT " + Quantity + " FROM `" + Table + "` WHERE " + StartKey + " = " + startKey + " AND " + EndKey + " = " + endKey);
        }

        public List<int> GetId(int startKey)
        {
            List<int> list = new();

            Database.Connection.RunInTransaction("SELECT " + EndKey + " FROM `" + Table + "` WHERE " + StartKey + " = " + startKey, r =>
            {
                while (r.Read())
                    list.Add(r.GetInt32(EndKey));
            });

            return list;
        }

        public List<T> Get<T>(int startKey) where T : Entity
        {
            List<int> l = GetId(startKey);
            List<T> j = new(l.Count);

            foreach (int i in l)
                j.Add( (T) (End.Get(i) ?? throw new NullReferenceException()) );

            return j;
        }

        public Dictionary<int, int> DescribeId(int startKey)
        {
            Dictionary<int, int> dict = new();

            Database.Connection.RunInTransaction("SELECT * FROM `" + Table + "` WHERE " + StartKey + " = " + startKey, r =>
            {
                while (r.Read())
                    dict.Add(r.GetInt32(EndKey), Quantifiable ? r.GetInt32(Quantity) : 1);
            });

            return dict;
        }

        public Dictionary<T, int> Describe<T>(int startKey) where T : Entity
        {
            Dictionary<int, int> d = DescribeId(startKey);
            Dictionary<T, int> e = new(d.Count);

            foreach (var i in d)
                e.Add( (T) (End.Get(i.Key) ?? throw new NullReferenceException()), i.Value );

            return e;
        }
    }
}
