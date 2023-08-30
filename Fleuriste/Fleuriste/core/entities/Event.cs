using BDD.Core.Attributes;
using BDD.Core.Relations;

namespace BDD.Core.Entities
{
    [Table("events")]
    public class Event : Entity
    {
        private Event() { }

        [Column("name")]
        public string? Name { get; private set; }

        [ManyToMany("product_event", "idEvent", "idProduct")]
        public ManyToManyRelation<Product> Products { get; private set; }
    }
}
