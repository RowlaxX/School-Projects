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

namespace BDD.Main
{
    public partial class StoreChoice : UserControl
    {
        public Store Store { get; private set; }

        public StoreChoice(Store store, int? heigth=null)
        {
            InitializeComponent();
            Store = store;
            TextCity.Text = Store.City;
            TextName.Text = Store.Name;

            if (heigth.HasValue)
                MyGrid.Height = (double)heigth;
        }
    }
}
