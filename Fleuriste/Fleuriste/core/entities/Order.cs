using BDD.Core.Attributes;
using BDD.Core.Relations;
using System;

namespace BDD.Core.Entities
{
    [Table("order")]
    public class Order : Entity
    {
        private Order() { }

        [Column("creditCard")]
        public string? CreditCard { get; private set; }
        [Column("date")]
        public DateTime? Date { get; private set; }
        [Column("deliveryDate")]
        public DateTime? DeliveryDate { get; private set; }
        [Column("message")]
        public string? Message { get; private set; }
        [Column("fullPrice")]
        public double? FullPrice { get; private set; }
        [Column("finalPrice")]
        public double? FinalPrice { get; private set; }

        [ManyToOne("idCustomer")]
        public ManyToOneRelation<Customer> Customer { get; private set; }
        [ManyToOne("idStore")]
        public ManyToOneRelation<Store> Store { get; private set; }
        [ManyToOne("idAddress")] 
        public ManyToOneRelation<Address> DeliveryAddress { get; private set; }
        [ManyToOne("idFacturationAddress")] 
        public ManyToOneRelation<Address> FacturationAddress { get; private set; }

        [ManyToMany("order_content", "idOrder", "idProduct", "quantity")]
        public ManyToManyRelation<Product> Products { get; private set; }
    }
}
