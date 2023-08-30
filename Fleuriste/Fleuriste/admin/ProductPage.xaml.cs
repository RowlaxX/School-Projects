using BDD.Core;
using BDD.Core.Entities;
using BDD.Core.Relations;
using BDD.UI;
using BDD.User;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
    public partial class ProductPage : Page
    {
        public AdminContext Context { get; private set; }
        public Product? Selection { get; private set; }

        public ProductPage(AdminContext context)
        {
            InitializeComponent();
            Context = context;

            MyCreate.Click += delegate { Create(); };
            MyAddProduct.Click += delegate { SetupCreateNewAddress(); };
            MyListProduct.SelectionChanged += delegate { ShowSelection(); };

            MyProductName.LostFocus += delegate { Edit("name", typeof(string), MyProductName); };
            MyProductDescription.LostFocus += delegate { Edit("description", typeof(string), MyProductDescription); };
            MyProductPrice.LostFocus += delegate { Edit("price", typeof(double), MyProductPrice); };
            MyProductVisible.Click += delegate { Selection?.Edit("visible", MyProductVisible.IsChecked); };

            MyItems.Click += delegate { PickItems(); };
            MyTags.Click += delegate { PickTags(); };
            MyEvents.Click += delegate { PickEvents(); };
            MyPictures.Click += delegate { PickPictures(); };
            MyProfile.Click += delegate { EditProfilePicture(); };

            foreach (Product product in context.Database.SelectAll<Product>())
                MyListProduct.Items.Add(new ProductElement(product));

            SetEnable(false);
        }

        private void Pick(ManyToManyRelation? relation, string column, Func<Entity?>? create=null)
        {
            if (relation == null)
            {
                MyView.Children.Clear();
                return;
            }

            RelationPicker? picker = MyView.Children.Count > 0 ? MyView.Children[0] as RelationPicker : null;
            if (picker != null && relation == picker.Relation)
                return;

            picker = new(relation, column, create);
            MyPickerDescription.Text = relation.Service.End.Type.Name;
            MyView.Children.Clear();
            MyView.Children.Add(picker);
        }

        private void PickItems() => Pick(Selection?.Items, "name", CreateItem);
        private void PickTags() => Pick(Selection?.Tags, "name", CreateTag);
        private void PickEvents() => Pick(Selection?.Events, "name", CreateEvent);
        private void PickPictures() => Pick(Selection?.Pictures, "name", CreatePicture);

        private void EditProfilePicture()
        {
            if (Selection == null)
                return;

            MyPickerDescription.Text = "Image";
            MyView.Children.Clear();
            MyView.Children.Add(new ProductPageProfilePicture(Selection));
        }

        private E? CreateSimpleEntity<E>(string question) where E : Entity
        {
            PromptWindow window = new(question);
            window.ShowDialog();

            if (window.Result == null)
                return null;

            E e = Context.Database.Create<E>();
            e.Edit("name", window.Result);
            return e;
        }

        private Item? CreateItem() => CreateSimpleEntity<Item>("Quel est le nom de l'item ?");
        private Tag? CreateTag() => CreateSimpleEntity<Tag>("Quel est le nom du tag ?");
        private Event? CreateEvent() => CreateSimpleEntity<Event>("Quel est le nom de l'evenement ?");
        
        private Picture? CreatePicture()
        {
            DoublePromptWindow window = new("Quel est le nom de l'image ?", "Quel est l'URL de l'image ?");
            window.ShowDialog();

            if (window.Result1 == null || window.Result2 == null)
                return null;

            Picture e = Context.Database.Create<Picture>();
            e.Edit("name", window.Result1);
            e.Edit("url", window.Result2);
            return e;
        }

        private void SetTexts(Product? product)
        {
            MyProductName.Text = product == null ? "" : product.Name;
            MyProductDescription.Text = product == null ? "" : product.Description;
            MyProductPrice.Text = product == null ? "" : (product.Price == null ? "" : product.Price.ToString());
            MyProductVisible.IsChecked = product == null ? true : product.Visible;
        }

        private void SetEnable(bool enable)
        {
            MyProductName.IsEnabled = enable;
            MyProductDescription.IsEnabled = enable;
            MyProductPrice.IsEnabled = enable;
            MyProductVisible.IsEnabled = enable;
            MyPictures.IsEnabled = enable;
            MyItems.IsEnabled = enable;
            MyEvents.IsEnabled = enable;
            MyProfile.IsEnabled = enable;
            MyTags.IsEnabled = enable;
            MyCreate.IsEnabled = enable;
        }

        private void ShowSelection()
        {
            if (MyListProduct.SelectedItem is not ProductElement element)
                return;

            SetEnable(true);
            MyCreate.IsEnabled = false;
            MyCreate.Visibility = Visibility.Hidden;
            MyProductName.IsEnabled = false;
            Selection = element.Product;
            PickItems();
            SetTexts(Selection);
        }

        private void SetupCreateNewAddress()
        {
            if (Selection != null && !Selection.IsPersisted)
                return;

            SetEnable(true);
            MyListProduct.SelectedItem = null;
            MyCreate.Visibility = Visibility.Visible;
            Selection = Context.Database.Create<Product>();
            Selection.Edit("visible", true);
            PickItems();
            SetTexts(null);
        }

        private void Edit(string column, Type type, TextBox tb)
        {
            if (Selection == null)
                return;

            try
            {
                object value = Convert.ChangeType(tb.Text, type);
                Selection.Edit(column, value);
            }
            catch (FormatException e)
            {
                MessageBox.Show(e.Message, "Erreur");
                tb.Text = Selection.Get(column)?.ToString();
            }
        }

        private void Create()
        {
            if (Selection == null)
                return;

            if (Selection.IsPersisted)
                return;

            try
            {
                Selection.Persist();
                ProductElement ae = new(Selection);
                MyListProduct.Items.Add(ae);
                MyListProduct.SelectedItem = ae;
                ShowSelection();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message, "Erreur");
            }
        }
    }

    public class ProductElement : TextBlock
    {
        public Product Product { get; private set; }

        public ProductElement(Product product)
        {
            Product = product;
            Text = product.Name;
            Height = 30;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }
    }
}
