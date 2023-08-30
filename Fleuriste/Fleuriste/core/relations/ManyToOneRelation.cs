namespace BDD.Core.Relations
{
    public abstract class ManyToOneRelation
    {
        public abstract Entity Parent { get; }
        public abstract bool OnlineMode { get; }

        public abstract Entity? GetRaw();
        public abstract int? GetId();
        public abstract void Set(int? id);
        public abstract bool Exists();
    }

    public abstract class ManyToOneRelation<S> : ManyToOneRelation where S : Entity
    {
        public abstract S? Get();
        public abstract void Set(S? entity);
    }
}
