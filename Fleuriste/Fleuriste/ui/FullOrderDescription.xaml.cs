using BDD.Core.Entities;
using MySqlX.XDevAPI;
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

namespace BDD.UI
{
    public partial class FullOrderDescription : UserControl
    {
        public Order Order { get; private set; }

        public FullOrderDescription(Order order, bool showCustomer)
        {
            InitializeComponent();
            Order = order;
            ButtonClient.Visibility = showCustomer ? Visibility.Visible : Visibility.Hidden;
            LeftContent.Children.Add(new OrderDescription(order));

            ButtonClient.Click += delegate { ShowCustomer(); };
            ButtonContent.Click += delegate { ShowContent(); };
            ButtonFacturation.Click += delegate { ShowFacturation(); };
            ButtonDelivery.Click += delegate { ShowDelivery(); };
            ShowContent();
        }

        private void Show(UIElement e)
        {
            RightContent.Children.Clear();
            RightContent.Children.Add(e);
        }

        private void ShowContent() => Show(new OrderContent(Order));
        private void ShowDelivery() => Show(new AddressDescription(Order.DeliveryAddress.Get()));
        private void ShowFacturation() => Show(new AddressDescription(Order.FacturationAddress.Get()));
        private void ShowCustomer() => Show(new CustomerDescription(Order.Customer.Get()));
    }
}
