using BDD.Core.Attributes;
using BDD.Core.Relations;

namespace BDD.Core.Entities
{
    [Table("item")]
    public class Item : Entity
    {
        private Item() { }

        [Column("name")]
        public string? Name { get; private set; }

        [ManyToMany("item_color", "idItem", "idColor")]
        public ManyToManyRelation<Color> Colors { get; private set; }
        [ManyToMany("product_content", "idItem", "idProduct", "quantity")]
        public ManyToManyRelation<Product> Products { get; private set; }
        [ManyToMany("stock", "idItem", "idStore", "quantity")]
        public ManyToManyRelation<Store> Stores { get; private set; }
    }
}
