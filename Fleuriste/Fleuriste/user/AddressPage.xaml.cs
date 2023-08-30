using BDD.Core.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace BDD.User
{
    public partial class AddressPage : Page
    {
        public UserContext Context { get; private set; }
        public Address? Selection { get; private set; }

        public AddressPage(UserContext context)
        {
            InitializeComponent();
            Context = context;

            CreateAddress.Click += delegate { Create(); };
            AddAddress.Click += delegate { SetupCreateNewAddress(); };
            ListAddress.SelectionChanged += delegate { ShowSelection(); };

            AddressName.LostFocus += delegate { Edit("name", typeof(string), AddressName); };
            AddressCountry.LostFocus += delegate { Edit("country", typeof(string), AddressCountry); };
            AddressCity.LostFocus += delegate { Edit("city", typeof(string), AddressCity); };
            AddressZip.LostFocus += delegate { Edit("zip", typeof(int), AddressZip); };
            AddressStreet.LostFocus += delegate { Edit("street", typeof(string), AddressStreet); };
            AddressNumber.LostFocus += delegate { Edit("number", typeof(int), AddressNumber); };
            AddressHint.LostFocus += delegate { Edit("hint", typeof(string), AddressHint); };
            AddressComment.LostFocus += delegate { Edit("comment", typeof(string), AddressComment); };
            
            foreach (Address address in context.Customer.Addresses.Get() )
                ListAddress.Items.Add( new AddressElement(address) );

            SetEnable(false);
        }

        private void SetTexts(Address? address)
        {
            AddressName.Text = address == null ? "" : address.Name;
            AddressCountry.Text = address == null ? "" : address.Country;
            AddressCity.Text = address == null ? "" : address.City;
            AddressZip.Text = address == null ? "" : address.Zip.ToString();
            AddressStreet.Text = address == null ? "" : address.Street;
            AddressNumber.Text = address == null ? "" : address.Number.ToString();
            AddressHint.Text = address == null ? "" : address.Hint;
            AddressComment.Text = address == null ? "" : address.Comment;
        }

        private void SetEnable(bool enable)
        {
            CreateAddress.IsEnabled = enable;
            AddressName.IsEnabled = enable;
            AddressCountry.IsEnabled = enable;
            AddressCity.IsEnabled = enable;
            AddressZip.IsEnabled = enable;
            AddressStreet.IsEnabled = enable;
            AddressNumber.IsEnabled = enable;
            AddressHint.IsEnabled = enable;
            AddressComment.IsEnabled = enable;
        }


        private void ShowSelection()
        {
            if (ListAddress.SelectedItem is not AddressElement element)
                return;

            SetEnable(true);
            CreateAddress.IsEnabled = false;
            CreateAddress.Visibility = Visibility.Hidden;
            AddressName.IsEnabled = false;
            Selection = element.Address;
            SetTexts(Selection);
        }

        private void SetupCreateNewAddress()
        {
            if (Selection != null && !Selection.IsPersisted)
                return;

            SetEnable(true);
            ListAddress.SelectedItem = null;
            CreateAddress.Visibility = Visibility.Visible;
            Selection = Context.Database.Create<Address>();
            Selection.Customer.Set(Context.Customer);
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
                AddressElement ae = new(Selection);
                ListAddress.Items.Add(ae);
                ListAddress.SelectedItem = ae;
                ShowSelection();
            }
            catch(MySqlException e)
            {
                MessageBox.Show(e.Message, "Erreur");
            }
        }
    }

    public class AddressElement : TextBlock
    {
        public Address Address { get; private set; }

        public AddressElement(Address address) 
        {
            Address = address;
            Text = address.Name;
            Height = 30;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
