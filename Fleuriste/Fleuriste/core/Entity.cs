using BDD.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD.Core
{

    public abstract class Entity
    {
        public EntityService? Service { get; internal set; }

        public int? NullableId { get; internal set; }

        public bool IsPersisted { get { return NullableId != null; } }
        public int Id { get { return NullableId ?? throw new ApplicationException("Not persisted"); } }

        public object? Get(string column)
        {
            return Service.GetValue(this, column);
        }

        public void Edit(string column, object? value)
        {
            Service.Edit(this, column, value);
        }

        public void Delete()
        {
            Service.Delete(this);
        }

        public override string ToString()
        {
            return Service.Stringify(this);
        }

        public void Persist()
        {
            Service.Persist(this);
        }
    }
}
