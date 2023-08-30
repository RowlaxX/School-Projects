using BDD.Core.Attributes;
using BDD.Core.Relations;

namespace BDD.Core.Entities
{
    [Table("product")]
    public class Product : Entity
    {
        private Product() { }

        [Column("name")]
        public string? Name { get; private set; }
        [Column("price")]
        public double? Price { get; private set; }
        [Column("description")]
        public string? Description { get; private set; }
        [Column("profilePicture")]
        public string? ProfilePicture { get; private set; }
        [Column("visible")]
        public bool? Visible { get; private set; }

        [ManyToMany("product_event", "idProduct", "idEvent")]
        public ManyToManyRelation<Event> Events { get; private set; }
        [ManyToMany("product_tag", "idProduct", "idTag")]
        public ManyToManyRelation<Tag> Tags { get; private set; }
        [ManyToMany("product_picture", "idProduct", "idPicture")]
        public ManyToManyRelation<Picture> Pictures { get; private set; }

        [ManyToMany("product_content", "idProduct", "idItem", "quantity")]
        public ManyToManyRelation<Item> Items { get; private set; }
        [ManyToMany("order_content", "idProduct", "idOrder", "quantity")]
        public ManyToManyRelation<Order> Order { get; private set; }
    }
}
