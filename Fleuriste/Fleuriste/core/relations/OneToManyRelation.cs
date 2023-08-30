using System.Collections.Generic;

namespace BDD.Core.Relations
{
    public abstract class OneToManyRelation<T> where T : Entity
    {
        public abstract Entity Parent { get; }

        public bool IsEmpty() => Size() == 0;
        public abstract bool Contains(T entity);
        public abstract bool Contains(int endKey);
        public abstract int Size();
        public abstract List<int> GetId();
        public abstract List<T> Get();
    }
}
