using BDD.core;
using Microsoft.Windows.Themes;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD.Core
{
    public class Database
    {
        internal readonly Dictionary<Type, EntityService> services = new();
        
        public DatabaseConnection Connection { get; private set; }

        public Database(string address="localhost", int port=3306, string database="fleurs", string user="root", string password="root")
        {
            this.Connection = new DatabaseConnection(address, port, database, user, password);
        }

        public EntityService GetService<T>() where T : Entity
        {
            if (services.ContainsKey(typeof(T)))
                return services[typeof(T)];

            return new EntityService(this, typeof(T));
        }

        public EntityService GetService(Type type)
        {
            if (services.ContainsKey(type))
                return services[type];

            return new EntityService(this, type);
        }

        public T Create<T>() where T: Entity
        {
            return (T)GetService<T>().CreateLocal();
        }

        public void Persist(Entity e)
        {
            GetService(e.GetType()).Persist(e);
        }

        public void Delete(Entity e)
        {
            GetService(e.GetType()).Delete(e);
        }

        public int Count<E>() where E : Entity
        {
            return GetService<E>().Count();
        }

        public int PageCount<E>(int pageSize) where E : Entity
        {
            return GetService<E>().PageCount(pageSize);
        }

        private static List<E> Convert<E>(List<Entity> list) where E : Entity
        {
            List<E> j = new(list.Count);

            foreach (Entity e in list)
                j.Add((E)e);

            return j;
        }

        public List<E> Select<E>(int page, int pageSize) where E : Entity
        {
            return Convert<E>(GetService<E>().Select(page, pageSize));
        }

        public List<E> FindAll<E>(string column, object value) where E : Entity
        {
            return Convert<E>(GetService<E>().FindAll(column, value));
        }

        public E? Find<E>(string column, object value) where E : Entity
        {
            return (E?) GetService<E>().Find(column, value);
        }

        public E? Get<E>(int id) where E : Entity
        {
            return (E?)GetService<E>().Get(id);
        }

        public void Edit(Entity entity, string column, object? value)
        {
            GetService(entity.GetType()).Edit(entity, column, value);
        }

        public List<E> SelectAll<E>() where E : Entity
        {
            return Convert<E>(GetService<E>().SelectAll());
        }

        public List<E> Search<E>(string column, string str) where E : Entity
        {
            return Convert<E>(GetService<E>().Search(column, str));
        }
    }
}
