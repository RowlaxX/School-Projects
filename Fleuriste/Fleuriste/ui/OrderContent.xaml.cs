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
    public partial class OrderContent : UserControl
    {
        public OrderContent(Order order)
        {
            InitializeComponent();

            foreach(var k in order.Products.Describe())
            {

                TextQuantityElement tqe = new(k.Key.Name);
                tqe.SetQuantity(k.Value, false);
                MyList.Items.Add(tqe);
            }
        }
    }
}
