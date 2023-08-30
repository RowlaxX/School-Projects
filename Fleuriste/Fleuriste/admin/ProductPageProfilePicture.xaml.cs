using BDD.Core;
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

namespace BDD.Admin
{
    public partial class ProductPageProfilePicture : UserControl
    {
        public Product Product { get; private set; }
        public ImageUrlElement ImageUrl { get; private set; }

        public ProductPageProfilePicture(Product product)
        {
            InitializeComponent();
            Product = product;
            PromptURL.Click += delegate { Prompt(); };
            ImageUrl = new();
            Room.Children.Add(ImageUrl);

            if (Product.ProfilePicture == null || Product.ProfilePicture.Length == 0)
                return;

            ImageUrl.SetURL(Product.ProfilePicture);
        }

        private void Prompt()
        {
            PromptWindow prompt = new("Quelle est l'URL de l'image ?");
            prompt.ShowDialog();

            string? answer = prompt.Result;
            if (answer == null)
                return;

            Product.Edit("profilePicture", answer);
            ImageUrl.SetURL(answer);
        }
    }
}
