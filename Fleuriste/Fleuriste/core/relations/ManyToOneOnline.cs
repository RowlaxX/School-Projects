namespace BDD.Core.Relations
{
    public class ManyToOneOnline<T> : ManyToOneRelation<T> where T : Entity
    {
        public ManyToOneService Service { get; private set; }
        private int? referenceId;
        private readonly Entity parent;

        public ManyToOneOnline(ManyToOneService service, Entity parent, int? referenceId)
        {
            Service = service;
            this.parent = parent;
            this.referenceId = referenceId;
        }

        public override bool OnlineMode => true;

        public override Entity Parent => parent;

        public override T? Get()
        {
            if (referenceId == null)
                return null;
            return (T?) Service.End.Get(referenceId.Value);
        }

        public override Entity? GetRaw() => Get();

        public override void Set(T? entity)
        {
            if (entity == null)
                return;
            Set(entity.Id);
        }

        public override int? GetId() => referenceId;

        public override void Set(int? targetId)
        {
            if (referenceId == targetId)
                return;

            Service.Edit(Parent.Id, targetId);
            this.referenceId = targetId;
        }

        public override bool Exists() => referenceId.HasValue;
    }
}
