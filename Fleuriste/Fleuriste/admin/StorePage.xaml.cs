using BDD.Core;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BDD.Admin
{
    public partial class StorePage : Page
    {
        public AdminContext Context { get; private set; }
        public Store? Selected { get; private set; }

        public StorePage(AdminContext context)
        {
            InitializeComponent();
            Context = context;

            AddItem.Click += delegate { GenItem(); };
            AddStore.Click += delegate { GenStore(); };
            ListStore.SelectionChanged += delegate { UpdateSelected(); };

            Context.Database.SelectAll<Store>().ForEach(AddStoreToList);
            Context.Database.SelectAll<Item>().ForEach(AddItemToList);
        }

        private void UpdateSelected()
        {
            StoreChoice? sc = ListStore.SelectedItem as StoreChoice;
            Selected = sc?.Store;
            Dictionary<int, int>? stock = Selected?.Stock.DescribeId();

            foreach(TextQuantityElement e in ListItem.Items)
                if (Selected == null)
                    e.Reset();
                else if (stock != null)
                    e.SetQuantity(stock.ContainsKey(e.Entity.Id) ? stock[e.Entity.Id] : 0);
        }

        private void OnItemQuantityChanged(TextQuantityElement tqe) => Selected?.Stock.Put(tqe.Entity.Id, tqe.Quantity);
        
        private void AddItemToList(Item item) => ListItem.Items.Add(new TextQuantityElement(item.Name ?? "null", item, OnItemQuantityChanged));
        private void AddStoreToList(Store store) => ListStore.Items.Add(new StoreChoice(store, 30));

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
        private void GenStore()
        {
            DoublePromptWindow window = new("Quel est le nom de la ville ?", "Quel est le nom du magasin ?");
            window.ShowDialog();

            if (window.Result1 == null || window.Result2 == null)
                return;

            Store e = Context.Database.Create<Store>();
            e.Edit("name", window.Result1);
            e.Edit("city", window.Result2);

            try
            {
                e.Persist();
                AddStoreToList(e);
            }
            catch (MySqlException f)
            {
                MessageBox.Show(f.Message, "Erreur");
            }
        }
    }
}
