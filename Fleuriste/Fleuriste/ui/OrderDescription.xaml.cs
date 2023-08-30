using BDD.Core.Entities;
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
    public partial class OrderDescription : UserControl
    {
        public OrderDescription(Order order)
        {
            InitializeComponent();
            Store store = order.Store.Get() ?? throw new NullReferenceException();

            OShop.Text = store.Name + " (" + store.City + ")";
            ODate.Text = order.Date == null ? "" : order.Date.ToString();
            ODelivery.Text = order.DeliveryDate == null ? "" : order.DeliveryDate.ToString();
            OFinalPrice.Text = order.FinalPrice == null ? "" : order.FinalPrice.ToString();
            OFullPrice.Text = order.FullPrice == null ? "" : order.FullPrice.ToString();
            OMessage.Text = order.Message;
            OCard.Text = order.CreditCard;
        }
    }
}
