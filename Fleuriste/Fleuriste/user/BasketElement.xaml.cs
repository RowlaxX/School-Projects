using BDD.Core.Entities;
using BDD.UI;
using BDD.User;
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
    /// <summary>
    /// Logique d'interaction pour BasketElement.xaml
    /// </summary>
    public partial class BasketElement : UserControl
    {
        public BasketPage ParentPage { get; private set; }
        public Product Product { get; private set; }
        public int Quantity { get; private set; }

        public BasketElement(BasketPage parent, Product product)
        {
            InitializeComponent();
            Product = product;
            ParentPage = parent;
            PreviewGrid.Children.Add(new ProductPreview(product));

            BasketEdit.Click += delegate { Edit(); };
            UpdateQuantity();
        }

        private void UpdateQuantity()
        {
            Basket basket = ParentPage.Context.Basket;
            Quantity = basket.Get(Product);
            BasketQuantity.Text = "Quantite : " + Quantity; 
        }

        private void UpdateQuantity(int qty)
        {
            try
            {
                ParentPage.Context.Basket.Set(Product, qty);
                Quantity = qty;
                BasketQuantity.Text = "Quantite : " + qty;
            }
            catch (ApplicationException)
            {
                MessageBox.Show("Le stock du magasin est insufisant", "Erreur");
            }
            
        }

        private void Edit()
        {
            PromptWindow w = new("Quelle est la nouvelle quantité ?");
            w.ShowDialog();

            if (w.Result == null)
                return;

            try
            {
                int qty = Convert.ToInt32(w.Result);
                UpdateQuantity(qty);

                if (Quantity <= 0)
                    ParentPage.MyList.Items.Remove(this);
            }
            catch (FormatException)
            {
                MessageBox.Show("Saisie incorrect", "Erreur");
            }
        }
    }
}
