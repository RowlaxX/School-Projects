using System.Collections.Generic;

namespace BDD.Core.Relations
{
    public class OneToManyLocal<S> : OneToManyRelation<S> where S : Entity
    {
        public OneToManyService Service { get; private set; }
        private readonly Entity parent;

        public OneToManyLocal(OneToManyService service, Entity parent)
        {
            this.parent = parent;
            this.Service = service;
        }

        public override Entity Parent => parent;

        public override bool Contains(S entity) => false;

        public override bool Contains(int endKey) => false;

        public override List<S> Get() => new();

        public override List<int> GetId() => new();

        public override int Size() => 0;
    }
}
