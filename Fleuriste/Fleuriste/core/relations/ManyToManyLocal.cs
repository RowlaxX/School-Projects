using System.Collections.Generic;
using System;
using System.Dynamic;

namespace BDD.Core.Relations
{
    public class ManyToManyLocal<T> : ManyToManyRelation<T> where T : Entity
    {
        public override ManyToManyService Service => service;
        
        private readonly Dictionary<T, int> d = new();
        private readonly Entity parent;
        private readonly ManyToManyService service;

        public ManyToManyLocal(ManyToManyService service, Entity parent)
        {
            this.service = service;
            this.parent = parent;
        }

        public override Entity Parent => parent;

        public override bool OnlineMode => false;

        public override void Clear() => d.Clear();

        public override bool Contains(T entity) => d.ContainsKey(entity);

        public override Dictionary<T, int> Describe() => new(d);

        public override List<T> Get() => new(d.Keys);

        public override int GetQuantity(T entity) => d.ContainsKey(entity) ? d[entity] : 0;

        public override bool IsQuantifiable() => Service.Quantifiable;

        public override void Put(T entity, int quantity)
        {
            if (d.ContainsKey(entity))
                d[entity] = quantity;
            else
                d.Add(entity, IsQuantifiable() ? quantity : 1);
        }

        public override void Remove(T entity) => d.Remove(entity);

        public override void SetQuantity(T entity, int quantity) => d[entity] = quantity;

        public override int Size() => d.Count;

        private T Get(int endKey)
        {
            return (T) (Service.End.Get(endKey) ?? throw new ArgumentException("The desired entity " + endKey + " is not online"));
        }

        public override void Put(int endKey, int quantity) => Put(Get(endKey), quantity);

        public override void Remove(int endKey) => Remove( Get(endKey));

        public override void SetQuantity(int endKey, int quantity) => SetQuantity(Get(endKey), quantity);

        public override int GetQuantity(int endKey) => GetQuantity(Get(endKey));

        public override List<int> GetId() => new(DescribeId().Keys);

        public override bool Contains(int endKey)
        {
            foreach (var k in d)
                if (k.Key.NullableId == endKey)
                    return true;
            return false;
        }

        public override Dictionary<int, int> DescribeId()
        {
            Dictionary<int, int> e = new(d.Count);

            foreach (var k in d)
                e.Add(k.Key.Id, k.Value);

            return e;
        }
    }
}
