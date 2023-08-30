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

namespace BDD.User
{
    public partial class ShoppingPage : Page
    {
        public UserContext Context { get; private set; }

        public ShoppingPage(UserContext context)
        {
            InitializeComponent();
            Context = context;
            MyList.SelectionChanged += delegate { MyList.SelectedItem = null; };

            foreach (Product product in Context.Database.FindAll<Product>("visible", "1"))
                MyList.Items.Add(new ProductElement(Context, product));
        }

        public void Refresh()
        {
            foreach (object pe in MyList.Items)
                if (pe is ProductElement p)
                    p.UpdateStock();
        }
    }
}
