using System;
using System.Collections.Generic;
using System.Reflection;
using BDD.Core.Attributes;
using BDD.Core.Relations;
using MySql.Data.MySqlClient;
using System.Data;
using BDD.Core.SQL;
using BDD.Core.Utils;

namespace BDD.Core
{
    public class EntityService
    {
        public Database Database { get; private set; }
        public string Table { get; private set; }
        public Type Type { get; private set; }

        private readonly Dictionary<PropertyInfo, ManyToOneService> mtos = new();
        private readonly Dictionary<PropertyInfo, ManyToManyService> mtms = new();
        private readonly Dictionary<PropertyInfo, OneToManyService> otms = new();
        private readonly Dictionary<string, EntityField> fields = new();

        private readonly Dictionary<int, Entity> cache = new();

        internal EntityService(Database db, Type type)
        {
            if (type == typeof(Entity))
                throw new ApplicationException("the provided type must not be Entity");
            if (!type.IsSubclassOf(typeof(Entity)))
                throw new ApplicationException("can only provide a service for Entity class member");

            this.Database = db;
            this.Type = type;
            this.Database.services.Add(type, this);

            Table table = type.GetCustomAttribute<Table>()
                ?? throw new ApplicationException(GetType() + " class must have the attribute Table");
            this.Table = table.Name;

            foreach (PropertyInfo prop in Type.GetProperties())
            {
                if (prop.DeclaringType == typeof(Entity))
                    continue;

                MethodInfo method = prop.SetMethod ?? throw new ApplicationException("property " + prop + " must have a private setter");
                if (!method.IsPrivate)
                    throw new ApplicationException("property " + prop + " setter is not private");

                LoadProperty(prop);
            }
        }

        private void LoadProperty(PropertyInfo prop)
        {
            EntityField? ef = EntityField.From(this, prop);
            if (ef != null)
                fields.Add(ef.Column, ef);

            ManyToMany? mtm = prop.GetCustomAttribute<ManyToMany>();
            if (mtm != null)
                mtms.Add(prop, new ManyToManyService(Database, this, prop, mtm));
            
            ManyToOne? mto = prop.GetCustomAttribute<ManyToOne>();
            if (mto != null)
                mtos.Add(prop, new ManyToOneService(Database, this, prop, mto, ef ?? throw new NullReferenceException()));

            OneToMany? otm = prop.GetCustomAttribute<OneToMany>();
            if (otm != null)
                otms.Add(prop, new OneToManyService(Database, this, prop, otm));
        }

        public string Stringify(Entity e)
        {
            ObjectStringifier os = new(e.GetType().Name);

            os.Add("id", e.NullableId);
            foreach (EntityField field in fields.Values)
                os.Add(field.Property.Name, field.Get(e));

            return os.Build();
        }

        public object? GetValue(Entity e, string column)
        {
            return fields[column].Get(e);
        }

        private Entity CreateBlank()
        {
            Entity e = (Entity)(Activator.CreateInstance(Type, true) ?? throw new NullReferenceException());
            e.Service = this;
            return e;
        }

        public Entity CreateLocal()
        {
            Entity e = CreateBlank();
            ApplyLocalRelations(e);
            return e;
        }

        private void ApplyLocalRelations(Entity e)
        {
            foreach (var k in mtos)
                k.Key.SetValue(e, k.Value.NewLocalRelation(e));
            foreach (var k in mtms)
                k.Key.SetValue(e, k.Value.NewLocalRelation(e));
            foreach (var k in otms)
                k.Key.SetValue(e, k.Value.NewLocalRelation(e));
        }

        private void ApplyOnlineRelations(Entity e, MySqlDataReader reader)
        {
            foreach (var k in mtos) 
            {
                object o = reader.GetValue(k.Value.SpecialKey);
                int? target = null;

                if (o != null && o is not DBNull)
                    target = System.Convert.ToInt32(o);

                k.Key.SetValue(e, k.Value.NewOnlineRelation(e, target));
            }
            foreach (var k in mtms)
                k.Key.SetValue(e, k.Value.NewOnlineRelation(e));
            foreach (var k in otms)
                k.Key.SetValue(e, k.Value.NewOnlineRelation(e));
        }

        private Entity CreateOnline(MySqlDataReader reader)
        {
            int id = reader.GetInt32("id");
            if (cache.ContainsKey(id))
                return cache[id];

            Entity entity = CreateBlank();
            entity.NullableId = id;

            foreach(var field in fields)
            {
                if (!field.Value.IsColumn)
                    continue;

                object value = reader.GetValue(field.Key);
                if (value is DBNull)
                    continue;

                field.Value.Set(entity, value);
            }

            ApplyOnlineRelations(entity, reader);
            cache.Add(id, entity);
            return entity;
        }




        private List<Entity> ExecuteAndGet(string sql)
        {
            List<Entity> list = new();

            Database.Connection.RunInTransaction(sql, r =>
            {
                while (r.Read())
                    list.Add(CreateOnline(r));
            });

            return list;
        }

        public int Count()
        {
            return Database.Connection.ExecuteScalar("SELECT COUNT(id) FROM `" + Table + "`");
        }

        public int PageCount(int pageSize)
        {
            return 1 + (Count() - 1) / pageSize;
        }

        public List<Entity> Select(int page, int pageSize)
        {
            int offset = page * pageSize;
            
            return ExecuteAndGet("SELECT * FROM `" + Table + "` ORDER BY id DESC LIMIT " + pageSize + " OFFSET " + offset);
        }

        public List<Entity> FindAll(string column, object value)
        {
            return ExecuteAndGet("SELECT * FROM `" + Table + "` WHERE " + column + " = " + SqlUtils.Format(value) );
        }

        public List<Entity> SelectAll()
        {
            return ExecuteAndGet("SELECT * FROM `" + Table + "`");
        }

        public List<Entity> Search(string column, string str)
        {
            return ExecuteAndGet("SELECT * FROM `" + Table + "` WHERE " + column + " LIKE '%" + str + "%'");
        }

        public Entity? Find(string column, object value)
        {
            List<Entity> list = ExecuteAndGet("SELECT * FROM `" + Table + "` WHERE " + column + " = " + SqlUtils.Format(value) + " LIMIT 1");

            if (list.Count == 0)
                return null;
            return list[0];
        }

        public void Edit(Entity entity, string column, object? value)
        {
            if (entity.GetType() != Type)
                throw new ArgumentException("This service cannot persist this entity");

            EntityField field = fields[column];

            object? current = field.Get(entity);

            Type t = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;
            value = Convert.ChangeType(value, t);

            if (Equals(current, value))
                return;

            if (entity.IsPersisted)    
                Database.Connection.Execute("UPDATE `" + Table + "` SET " + column + " = " + SqlUtils.Format(value) + " WHERE id = " + entity.Id);
            
            field.Set(entity, value);
        }

        public Entity? Get(int id)
        {
            if (cache.ContainsKey(id))
                return cache[id];
            return Find("id", id);
        }

        public void Persist(Entity entity)
        {
            if (entity.GetType() != Type)
                throw new ArgumentException("This service cannot persist this entity");
            if (entity.IsPersisted)
                return;

            PersistMTORelations(entity);
            InsertCommand ic = new(Table);
            foreach (var field in fields)
                ic.Add(field.Key, field.Value.Get(entity));
            Database.Connection.Execute(ic.Build());

            int id = Database.Connection.ExecuteScalar("SELECT last_insert_id()");
            
            entity.NullableId = id;            
            PersistMTMRelations(entity);
            PersistOTMRelations(entity);
            cache.Add(id, entity);
        }

        public void Delete(Entity entity)
        {
            if (entity.GetType() != Type)
                throw new ArgumentException("This service cannot persist this entity");
            if (!entity.IsPersisted)
                return;

            cache.Remove(entity.Id);

            foreach (var k in mtms)
                k.Value.Clear(entity.Id);
            foreach (var k in otms)
                if (k.Value.Size(entity.Id) > 0)
                    throw new ArgumentException("The entity is referenced in table " + k.Value.Table);

            Database.Connection.Execute("DELETE FROM `" + Table + "` WHERE id = " + entity.Id);
            entity.NullableId = null;
            ApplyLocalRelations(entity);//TODO Unpersist pour conserver les relations de l'object
        }

        private void PersistMTORelations(Entity e)
        {
            foreach (var k in mtos)
            {
                ManyToOneService mtos = k.Value;
                ManyToOneRelation? mtor = (ManyToOneRelation?)k.Key.GetValue(e);
                k.Key.SetValue(e, mtor == null ? mtos.NewOnlineRelation(e, null) : mtos.Persist(mtor));
            }
        }

        private void PersistMTMRelations(Entity e)
        {
            foreach (var k in mtms) 
            {
                ManyToManyService mtms = k.Value;
                ManyToManyRelation? mtmr = (ManyToManyRelation?)k.Key.GetValue(e);
                k.Key.SetValue(e, mtmr == null ? mtms.NewOnlineRelation(e) : mtms.Persist(mtmr));
            }
        }

        private void PersistOTMRelations(Entity e)
        {
            foreach (var k in otms)
                k.Key.SetValue(e, k.Value.NewOnlineRelation(e));
        }
    }
}
