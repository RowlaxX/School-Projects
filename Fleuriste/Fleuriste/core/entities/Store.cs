using BDD.Core.Attributes;
using BDD.Core.Relations;

namespace BDD.Core.Entities
{
    [Table("store")]
    public class Store : Entity
    {
        private Store() { }

        [Column("name")]
        public string? Name { get; private set; }
        [Column("city")]
        public string? City { get; private set; }

        [ManyToMany("stock", "idStore", "idItem", "quantity")]
        public ManyToManyRelation<Item> Stock { get; private set; }
        [OneToMany("idStore")]
        public OneToManyRelation<Order> Orders { get; private set; }

    }
}
