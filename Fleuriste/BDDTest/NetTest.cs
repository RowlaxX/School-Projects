using BDD.Core;
using BDD.Core.Entities;
using BDD.Core.SQL;

namespace BDDTest
{
    [TestClass]
    public class NetTest
    {
        [TestMethod]
        public void Persist()
        {
            Database s = new();

            Store store = s.Create<Store>();
            store.Edit("name", "auchan");
            store.Edit("city", "Metz");
            Console.WriteLine(store);

            Customer customer = s.Create<Customer>();
            customer.Edit("name", "LINDER");
            customer.Edit("firstname", "Theo");
            customer.Edit("email", "theo.linder4@gmail.com");
            customer.Edit("phone", "0781713040");
            customer.SetPassword("password123");
            Console.WriteLine(customer);

            Address address = s.Create<Address>();
            address.Edit("name", "maison");
            address.Edit("country", "France");
            address.Edit("zip", 65000);
            address.Edit("city", "Paris");
            address.Edit("street", "Rue laugier");
            address.Edit("number", 54);
            address.Edit("comment", "aucnu");
            address.Edit("hint", "aucun");
            address.Customer.Set(customer);
            Console.WriteLine(address);

            Order order = s.Create<Order>();
            order.Edit("creditCard", "4388672189");
            order.Edit("date", DateTime.Now);
            order.Edit("deliveryDate", DateTime.Now);
            order.Edit("message", "coucou");
            order.Edit("finalPrice", 100);
            order.Edit("fullPrice", 100);
            order.Customer.Set(customer);
            order.Store.Set(store);
            order.FacturationAddress.Set(address);
            order.DeliveryAddress.Set(address);
            Console.WriteLine(order);

            Item item1 = s.Create<Item>();
            item1.Edit("name", "rose");
            Console.WriteLine(item1);

            Item item2 = s.Create<Item>();
            item2.Edit("name", "tulippe");
            Console.WriteLine(item2);

            Item item3 = s.Create<Item>();
            item3.Edit("name", "margueritte");
            Console.WriteLine(item3);

            Product product = s.Create<Product>();
            product.Items.Put(item1, 2);
            product.Items.Put(item2, 2);
            product.Items.Put(item3, 3);
            product.Edit("name", "bouquet 1");
            product.Edit("description", "un merveilleux bouquet");
            product.Edit("profilePicture", "https://");
            product.Edit("price", 56);
            product.Edit("visible", true);
            Console.WriteLine(product);

            s.Persist(order);
            s.Persist(product);
        }

        [TestMethod]
        public void Retrieve()
        {
            Database s = new();

            Order order = s.Select<Order>(0, 10)[0];
            Console.WriteLine(order);
            Console.WriteLine(order.Customer.Get());
            Console.WriteLine(order.DeliveryAddress.Get());
            Console.WriteLine(order.FacturationAddress.Get());
            Console.WriteLine(order.Store.Get());

            Product product = s.Select<Product>(0, 10)[0];
            Console.WriteLine(product);

            foreach (var k in product.Items.Describe())
                Console.WriteLine("\t" + k.Value + " : " + k.Key);

            Customer customer = s.Select<Customer>(0, 10)[0];
            Console.WriteLine(SqlUtils.Format(customer.Password));
            Console.WriteLine(SqlUtils.Format(customer.Salt));
            Console.WriteLine(customer.TestPassword("password123"));
            Console.WriteLine(customer.TestPassword("wrong"));

        }

        [TestMethod]
        public void Delete()
        {
            Database s = new();

            Order order = s.Select<Order>(0, 10)[0];
            Product product = s.Select<Product>(0, 10)[0];

            order.Delete();
            product.Delete();
        }

        [TestMethod]
        public void Test()
        {
            Database s = new();

            Item item1 = s.Create<Item>();
            item1.Edit("name", "rose5");

            Item item2 = s.Create<Item>();
            item2.Edit("name", "tulippe5");

            Item item3 = s.Create<Item>();
            item3.Edit("name", "margueritte5");

            Product product = s.Create<Product>();
            product.Items.Put(item1, 2);
            product.Items.Put(item2, 5);
            product.Items.Put(item3, 3);
            product.Edit("name", "bouquet 1j");
            product.Edit("description", "un merveilleux bouquet");
            product.Edit("profilePicture", "https://");
            product.Edit("price", 56);
            product.Edit("visible", true);
            Console.WriteLine(product);

            product.Persist();

            Console.WriteLine();
            product.Delete();
        }
    }
}