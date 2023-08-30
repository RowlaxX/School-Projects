using BDD.Core.Entities;
using BDD.Main;
using BDD.UI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDD.Admin
{
    public partial class ItemPage : Page
    {
        public AdminContext Context { get; private set; }
        public Item? Selected { get; private set; }

        public ItemPage(AdminContext context)
        {
            InitializeComponent();
            Context = context;
            MyAddItem.Click += delegate { GenItem(); };
            MyListItem.SelectionChanged += delegate { UpdateSelected(); };
            Context.Database.SelectAll<Item>().ForEach(AddItemToList);
        }

        private void UpdateSelected()
        {
            ItemElement? ie = MyListItem.SelectedItem as ItemElement;

            if (Selected == ie?.Item)
                return;

            Selected = ie?.Item;
            MyGrid.Children.Clear();
            if (Selected != null)
                MyGrid.Children.Add(new RelationPicker(Selected.Colors, "name", GenColor));
        }

        private void AddItemToList(Item item) => MyListItem.Items.Add(new ItemElement(item));

        private Core.Entities.Color? GenColor()
        {
            PromptWindow window = new("Quel est le nom de la couleur ?");
            window.ShowDialog();

            if (window.Result == null)
                return null;

            Core.Entities.Color e = Context.Database.Create<Core.Entities.Color>();
            e.Edit("name", window.Result);

            try
            {
                e.Persist();
                return e;
            }
            catch (MySqlException f)
            {
                MessageBox.Show(f.Message, "Erreur");
                return null;
            }
        }

        private void GenItem()
        {
            PromptWindow window = new("Quel est le nom de l'item ?");
            window.ShowDialog();

            if (window.Result == null)
                return;

            Item e = Context.Database.Create<Item>();
            e.Edit("name", window.Result);

            try
            {
                e.Persist();
                AddItemToList(e);
            }
            catch (MySqlException f)
            {
                MessageBox.Show(f.Message, "Erreur");
            }
        }
    }

    public class ItemElement : TextBlock
    {
        public Item Item { get; private set; }

        public ItemElement(Item item)
        {
            Item = item;
            Text = item.Name;
            Height = 30;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }
    }
}
