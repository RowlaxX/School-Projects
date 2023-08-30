using BDD.Core.Attributes;
using BDD.Core.Relations;

namespace BDD.Core.Entities
{
    [Table("pictures")]
    public class Picture : Entity
    {
        private Picture() { }

        [Column("name")]
        public string Name { get; private set; }
        [Column("url")]
        public string? URL { get; private set; }

        [ManyToMany("product_picture", "idPicture", "idProduct")]
        public ManyToManyRelation<Product> Products { get; private set; }
    }
}
