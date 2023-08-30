using BDD.Core.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDD.User
{
    public partial class PayPage : Page
    {
        public UserContext Context { get; private set; }
        public int Reduction { get; private set; }
        public double FullPrice { get; private set; }
        public double FinalPrice { get; private set; }

        private readonly Order order;

        public PayPage(UserContext context)
        {
            InitializeComponent();
            Context = context;
        
            foreach(Address e in Context.Customer.Addresses.Get())
            {
                MyAddress.Items.Add(new AddressElement(e));
                MyFacturation.Items.Add(new AddressElement(e));
            }

            PayButton.Click += delegate { Pay(); };
            Reduction = GetReduction(Context.Customer);
            FullPrice = Context.Basket.GetPrice();
            FinalPrice = FullPrice * (1.0 -  Reduction / 100.0);

            LabelDiscount.Content = Reduction + "%";
            LabelPrice.Content = string.Format("{0:0.00}", FullPrice) + "€";
            LabelFinalPrice.Content = string.Format("{0:0.00}", FinalPrice) + "€";

            order = Context.Database.Create<Order>();
            order.Customer.Set(Context.Customer);
            order.Store.Set(Context.Store);
            order.Edit("fullPrice", FullPrice);
            order.Edit("finalPrice", FinalPrice);
            order.Edit("date", DateTime.Now);

            foreach (var e in Context.Basket.Content)
                order.Products.Put(e.Key, e.Value);
        }

        private static int GetReduction(Customer customer)
        {
            int count = 0;
            foreach(Order order in customer.Orders.Get())
            {
                if (order.Date == null)
                    continue;

                TimeSpan? s = DateTime.Now - order.Date;
                if (!s.HasValue)
                    continue;

                if (s.Value.Days < 30)
                    count++;
            }

            if (count >= 5)
                return 15;
            if (count >= 1)
                return 5;
            return 0;
        }

        private void Pay()
        {
            if (MyDate.SelectedDate == null)
            {
                MessageBox.Show("La date de livraison choisie est invalide", "Erreur");
                return;
            }
            if (MyFacturation.SelectedItem == null)
            {
                MessageBox.Show("Veuillez choisir une adresse de facturation", "Erreur");
                return;
            }
            if (MyAddress.SelectedItem == null)
            {
                MessageBox.Show("Veuillez choisir une adresse de livraison", "Erreur");
                return;
            }


            order.Edit("message", MyMessage.Text);
            order.Edit("creditCard", MyCard.Text);
            order.Edit("deliveryDate", MyDate.SelectedDate);
            order.DeliveryAddress.Set( (MyAddress.SelectedValue as AddressElement)?.Address );
            order.FacturationAddress.Set( (MyFacturation.SelectedValue as AddressElement)?.Address );

            try
            {
                order.Persist();
                MessageBox.Show("La commande a été effectuée", "Info");
                Context.Basket.Validate();
                Context.SetContent(Context.ShoppingPage);
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message, "Erreur");
            }
        }

        
    }
}
