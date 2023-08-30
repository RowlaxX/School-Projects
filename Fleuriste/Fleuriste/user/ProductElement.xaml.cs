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
    public partial class ProductElement : UserControl
    {
        public UserContext Context { get; private set; }
        public Product Product { get; private set; }
        public int Stock { get; private set; }

        public ProductElement(UserContext context, Product product)
        {
            InitializeComponent();
            Context = context;
            Product = product;
            PreviewGrid.Children.Add(new ProductPreview(product));
            PreviewBuy.Click += delegate { Buy(); };
            UpdateStock();
        }

        public void UpdateStock()
        {
            Stock = Context.Basket.Remaining(Product);
            PreviewStock.Text = "Stock : " + Stock;
        }

        public void Buy()
        {
            if (Stock == 0)
            {
                MessageBox.Show("Rupture de stock", "Erreur");
                return;
            }

            PromptWindow w = new("Combien voulez-vous en acheter ?");
            w.ShowDialog();

            try
            {
                int qty = Convert.ToInt32(w.Result);
                if (qty < 0)
                    throw new FormatException();

                Context.Basket.Add(Product, qty);
            }
            catch (FormatException)
            {
                MessageBox.Show("Saisie incorrect", "Erreur");
            }
            catch (ApplicationException)
            {
                MessageBox.Show("Le stock est insufisant", "Erreur");
            }
        }
    }
}
