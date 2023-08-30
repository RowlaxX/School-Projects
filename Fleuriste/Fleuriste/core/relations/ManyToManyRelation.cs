using System.Collections.Generic;

namespace BDD.Core.Relations
{
    public abstract class ManyToManyRelation
    {
        public abstract ManyToManyService Service { get; }
        public abstract bool OnlineMode { get; }
        public abstract Entity Parent { get; }

        public bool IsEmpty()
        {
            return Size() == 0;
        }

        public abstract bool Contains(int endKey);
        public abstract int Size();

        public abstract List<int> GetId();
        public abstract List<Entity> GetRaw();

        public abstract void Clear();
        public abstract bool IsQuantifiable();

        public abstract Dictionary<int, int> DescribeId();
        public abstract Dictionary<Entity, int> DescribeRaw();

        public void Put(int endKey) => Put(endKey, 1);
        public abstract void Put(int endKey, int quantity);

        public void PutAll(Dictionary<int, int> map)
        {
            foreach (var e in map)
                Put(e.Key, e.Value);
        }

        public abstract void PutAll(Dictionary<Entity, int> map);
        public abstract void PutAll(List<Entity> list);
        public void PutAll(List<int> list) => list.ForEach(Put);

        public abstract void Remove(int id);

        public abstract void SetQuantity(int endKey, int quantity);
        public abstract int GetQuantity(int endKey);
    }

    public abstract class ManyToManyRelation<T> : ManyToManyRelation where T : Entity
    {
        public abstract bool Contains(T entity);
        public abstract List<T> Get();
        public abstract Dictionary<T, int> Describe();
        public void Put(T entity) => Put(entity, 1);
        public abstract void Put(T entity, int quantity);

        public void PutAll(Dictionary<T, int> map) 
        {
            foreach (var e in map)
                Put(e.Key, e.Value);
        }

        public abstract void Remove(T entity);

        public abstract void SetQuantity(T entity, int quantity);
        public abstract int GetQuantity(T entity);

        public override List<Entity> GetRaw()
        {
            List<T> l = Get();
            List<Entity> j = new(l.Count);
            l.ForEach(j.Add);
            return j;
        }

        public override Dictionary<Entity, int> DescribeRaw()
        {
            Dictionary<T, int> d = Describe();
            Dictionary<Entity, int> e = new(d.Count);

            foreach (var k in d)
                e.Add(k.Key, k.Value);

            return e;
        }

        public override void PutAll(Dictionary<Entity, int> map)
        {
            foreach (var e in map)
                Put( (T)e.Key, e.Value);
        }

        public override void PutAll(List<Entity> list)
        {
            foreach (var e in list)
                Put((T)e);
        }
    }
}
