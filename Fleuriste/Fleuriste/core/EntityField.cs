using BDD.Core.Attributes;
using BDD.Core.Relations;
using System;
using System.Reflection;
using System.Windows.Navigation;

namespace BDD.Core
{
    public abstract class EntityField
    {
        public static EntityField? From(EntityService service, PropertyInfo prop)
        {
            Column? column = prop.GetCustomAttribute<Column>();
            if (column != null)
                return new ColumnEntityField(service, column.Name, prop);

            ManyToOne? mto = prop.GetCustomAttribute<ManyToOne>();

            if (mto != null)
                return new ManyToOneEntityField(service, mto.Column, prop);

            return null;
        }

        public abstract Type PropertyType { get; }
        public PropertyInfo Property { get; private set; }
        public EntityService Service { get; private set; }
        public bool IsColumn => this is ColumnEntityField;
        public bool IsManyToOne => this is ManyToOneEntityField;
        public string Column { get; private set; }
        
        protected EntityField(EntityService service, string column, PropertyInfo prop)
        {
            this.Column = column;
            this.Service = service;
            this.Property = prop;
        }

        public abstract void Set(Entity entity, object? value);
        public abstract object? Get(Entity entity);
    }

    public class ColumnEntityField : EntityField
    {
        public ColumnEntityField(EntityService service, string column, PropertyInfo property) : base(service, column, property) {}

        public override Type PropertyType => Property.PropertyType;

        public override object? Get(Entity entity) => Property.GetValue(entity);

        public override void Set(Entity entity, object? value) => Property.SetValue(entity, value);
    }

    public class ManyToOneEntityField : EntityField
    {
        public ManyToOneEntityField(EntityService service, string column, PropertyInfo property) : base(service, column, property) {}

        private ManyToOneRelation MTM(Entity entity)
        {
            return (ManyToOneRelation)( Property.GetValue(entity) ?? throw new NullReferenceException());
        }

        public override Type PropertyType => typeof(int?);

        public override void Set(Entity entity, object? value) => MTM(entity).Set((int?)value);

        public override object? Get(Entity entity)
        {
            ManyToOneRelation mtm = MTM(entity);

            Entity? raw = mtm.GetRaw();
            if (raw == null)
                return null;
            
            if (raw.IsPersisted)
                return raw.Id;

            return null;
        }
    }
}
