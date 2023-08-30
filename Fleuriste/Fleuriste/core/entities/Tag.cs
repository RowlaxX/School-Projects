using BDD.Core.Attributes;
using BDD.Core.Relations;

namespace BDD.Core.Entities
{
    [Table("tags")]
    public class Tag : Entity
    {
        private Tag() { }

        [Column("name")]
        public string? Name { get; private set; }

        [ManyToMany("product_tag", "idTag", "idProduct")]
        public ManyToManyRelation<Product> Products { get; private set; }
    }
}
