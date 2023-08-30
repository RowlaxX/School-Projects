using System.Collections.Generic;

namespace BDD.Core.Relations
{
    public class ManyToManyOnline<T> : ManyToManyRelation<T> where T : Entity
    {
        public override ManyToManyService Service => service;

        private readonly Entity parent;
        private readonly ManyToManyService service;

        public ManyToManyOnline(ManyToManyService service, Entity parent)
        {
            this.service = service;
            this.parent = parent;
        }

        public override bool OnlineMode => true;

        public override Entity Parent => parent;

        public override bool Contains(T entity)
        {
            if (!entity.IsPersisted)
                return false;
            return Contains(entity.Id);
        }

        public override void Put(T entity, int quantity) => Put(entity.Id, quantity);

        public override bool Contains(int endKey) => Service.Contains(parent.Id, endKey);

        public override int Size() => Service.Size(parent.Id);

        public override List<int> GetId() => Service.GetId(parent.Id);

        public override List<T> Get() => Service.Get<T>(parent.Id);

        public override void Clear() => Service.Clear(parent.Id);

        public override bool IsQuantifiable() => Service.Quantifiable;

        public override Dictionary<int, int> DescribeId() => Service.DescribeId(parent.Id);

        public override Dictionary<T, int> Describe() => Service.Describe<T>(parent.Id);

        public override void Put(int endKey, int quantity) => Service.Put(parent.Id, endKey, quantity);

        public override void Remove(int endKey) => Service.Remove(parent.Id, endKey);

        public override void Remove(T entity) => Remove(entity.Id);

        public override void SetQuantity(int endKey, int quantity) => Service.SetQuantity(parent.Id, endKey, quantity);

        public override void SetQuantity(T entity, int quantity) => SetQuantity(entity.Id, quantity);

        public override int GetQuantity(int endKey) => Service.GetQuantity(parent.Id, endKey);

        public override int GetQuantity(T entity) => GetQuantity(entity.Id);
    }
}
