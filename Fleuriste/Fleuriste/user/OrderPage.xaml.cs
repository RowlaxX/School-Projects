using BDD.Admin;
using BDD.Core.Entities;
using BDD.UI;
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
    public partial class OrderPage : Page
    {
        public UserContext Context { get; private set; }
        public Order? Selection { get; private set; }

        public OrderPage(UserContext context)
        {
            InitializeComponent();
            Context = context;
            Context.Database.FindAll<Order>("idCustomer", Context.Customer.Id).ForEach(Add);
            ListOrder.SelectionChanged += delegate { UpdateSelection(); };
        }

        private void Add(Order order) => ListOrder.Items.Add(new OrderElement(order));

        private void UpdateSelection()
        {
            OrderElement? ie = ListOrder.SelectedItem as OrderElement;

            if (Selection == ie?.Order)
                return;

            Selection = ie?.Order;

            MyContent.Children.Clear();
            if (Selection != null)
                MyContent.Children.Add(new FullOrderDescription(Selection, false));
        }

        public class OrderElement : TextBlock
        {
            public Order Order { get; private set; }

            public OrderElement(Order order)
            {
                Order = order;
                Text = "Commande du " + order.DeliveryDate;
                Height = 30;
                VerticalAlignment = VerticalAlignment.Center;
                HorizontalAlignment = HorizontalAlignment.Stretch;
            }
        }
    }
}
