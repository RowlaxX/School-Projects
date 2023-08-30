using System.Collections.Generic;

namespace BDD.Core.Relations
{
    public class OneToManyOnline<T> : OneToManyRelation<T> where T : Entity
    {
        public OneToManyService Service { get; private set; }
        private readonly Entity parent;

        public OneToManyOnline(OneToManyService service, Entity parent)
        {
            this.Service = service;
            this.parent = parent;
        }

        public override Entity Parent => parent;

        public override bool Contains(T entity)
        {
            if (!entity.IsPersisted)
                return false;
            return Contains(entity.Id);
        }

        public override bool Contains(int endKey) => Service.Contains(Parent.Id, endKey);

        public override int Size() => Service.Size(Parent.Id);

        public override List<int> GetId() => Service.GetId(Parent.Id);

        public override List<T> Get() => Service.Get<T>(Parent.Id);
    }
}
