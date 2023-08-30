using BDD.Core.Attributes;
using BDD.Core.Relations;

namespace BDD.Core.Entities
{
    [Table("address")]
    public class Address : Entity
    {
        private Address() { }

        [Column("name")]
        public string? Name { get; private set; }
        [Column("country")]
        public string? Country { get; private set; }
        [Column("zip")]
        public int? Zip { get; private set; }
        [Column("city")]
        public string? City { get; private set; }
        [Column("street")]
        public string? Street { get; private set; }
        [Column("number")]
        public int? Number { get; private set; }
        [Column("comment")]
        public string? Comment { get; private set; }
        [Column("hint")]
        public string? Hint { get; private set; }

        [ManyToOne("idCustomer")]
        public ManyToOneRelation<Customer> Customer { get; private set; }
        [OneToMany("idAddress")]
        public OneToManyRelation<Order> Addresses { get; private set; }
        [OneToMany("idFacturationAddress")]
        public OneToManyRelation<Order> FacturationAddresses { get; private set; }
    }
}
