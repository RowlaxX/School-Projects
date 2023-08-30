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
    public partial class ProductPreview : UserControl
    {
        public ProductPreview(Product product)
        {
            InitializeComponent();
            ImageUrlElement image = new();
            IMG.Children.Add(image);
            ProductDescription.Text = product.Description ?? "";
            ProductPrice.Text = (product.Price == null ? "null" : product.Price) + "€";
            ProductName.Text = product.Name ?? "null";
            image.SetURL(product.ProfilePicture ?? "");
        }
    }
}
