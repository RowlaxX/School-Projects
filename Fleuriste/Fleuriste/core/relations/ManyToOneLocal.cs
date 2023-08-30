namespace BDD.Core.Relations
{
    public class ManyToOneLocal<S> : ManyToOneRelation<S> where S : Entity
    {
        public ManyToOneService Service { get; private set; }
        private readonly Entity parent;
        private S? reference;

        public ManyToOneLocal(ManyToOneService service, Entity parent)
        {
            this.Service = service;
            this.parent = parent;
        }

        public override bool OnlineMode => false;

        public override Entity Parent => parent;

        public override Entity? GetRaw() => reference;

        public override bool Exists() => reference != null;

        public override S? Get() => reference;

        public override int? GetId() => reference?.Id;

        public override void Set(S? entity) => reference = entity;

        public override void Set(int? id) => reference = id == null ? null : (S?) Service.End.Get(id.Value);
    }
}
