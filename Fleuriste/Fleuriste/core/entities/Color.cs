using BDD.Core.Attributes;
using BDD.Core.Relations;

namespace BDD.Core.Entities
{
    [Table("colors")]
    public class Color : Entity
    {
        private Color() { }

        [Column("name")]
        public string? Name { get; private set; }

        [ManyToMany("item_color", "idColor", "idItem")]
        public ManyToManyRelation<Item> Items { get; private set; }
    }
}
