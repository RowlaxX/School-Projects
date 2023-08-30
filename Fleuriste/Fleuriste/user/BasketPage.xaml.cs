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
    public partial class BasketPage : Page
    {
        public UserContext Context { get; private set; }
        
        public BasketPage(UserContext context)
        {
            InitializeComponent();
            Context = context;
            MyList.SelectionChanged += delegate { MyList.SelectedItem = null; };
            ButtonClear.Click += delegate { Clear(); };
            ButtonOrder.Click += delegate { Order(); };

            foreach (var k in Context.Basket.Content.Keys)
                MyList.Items.Add( new BasketElement(this, k) );
        }

        private void Clear()
        {
            MyList.Items.Clear();
            Context.Basket.Empty();
        }

        private void Order()
        {
            if (Context.Basket.Content.Count == 0)
            {
                MessageBox.Show("Le panier est vide", "Erreur");
                return;
            }
            if (Context.Customer.Addresses.Size() == 0)
            {
                MessageBox.Show("Veuillez renseigner au moins une addresse avant de commander");
                return;
            }

            Context.SetContent(new PayPage(Context));
        }
    }
}
