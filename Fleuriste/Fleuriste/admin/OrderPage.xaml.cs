using BDD.Core.Entities;
using BDD.UI;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDD.Admin
{
    public partial class OrderPage : Page
    {
        public AdminContext Context { get; private set; }
        public Order? Selection { get; private set; }

        public OrderPage(AdminContext context)
        {
            InitializeComponent();
            Context = context;
            Context.Database.SelectAll<Order>().ForEach(Add);
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
                MyContent.Children.Add(new FullOrderDescription(Selection, true));
        }
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
