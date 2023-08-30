using BDD.Core.Attributes;
using System;
using System.Reflection;

namespace BDD.Core.Relations
{
    public class ManyToOneService : RelationService
    {
        public string Table { get; private set; }
        public string SpecialKey { get; private set; }
        public EntityField Field { get; private set; }

        public ManyToOneService(Database db, EntityService start, PropertyInfo prop, ManyToOne mto, EntityField ef)
            : base(db, Cardinality.Many, Cardinality.One, typeof(ManyToOneRelation<>), start, prop)
        {
            Table = Start.Table;
            SpecialKey = mto.Column;
            Field = ef;
        }

        public ManyToOneRelation NewLocalRelation(Entity parent)
        {
            return (ManyToOneRelation) (Activator.CreateInstance(typeof(ManyToOneLocal<>)
                .MakeGenericType(End.Type), new object?[] { this, parent })
                ?? throw new NullReferenceException());
        }

        public ManyToOneRelation NewOnlineRelation(Entity parent, int? targetId)
        {
            return (ManyToOneRelation) (Activator.CreateInstance(typeof(ManyToOneOnline<>)
                .MakeGenericType(End.Type), new object?[] { this, parent, targetId })
                ?? throw new NullReferenceException());
        }

        public ManyToOneRelation Persist(ManyToOneRelation mto)
        {
            if (mto.OnlineMode)
                return mto;

            Entity? entity = mto.GetRaw();

            if (entity == null)
                return NewOnlineRelation(mto.Parent, null);

            if (entity.IsPersisted)
                return NewOnlineRelation(mto.Parent, mto.GetId());

            Database.Persist(entity);
            return NewOnlineRelation(mto.Parent, entity.Id);
        }

        public void Edit(int id, int? targetId)
        {
            Database.Connection.Execute("UPDATE `" + Table + "` SET " + SpecialKey + " = " + (targetId == null ? "null" : targetId) + " WHERE id = " + id);
        }
    }
}
