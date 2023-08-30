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
    public partial class CustomerPage : Page
    {
        public AdminContext Context { get; private set; }
        public Customer? Selection { get; private set; }

        public CustomerPage(AdminContext context)
        {
            InitializeComponent();
            Context = context;
            Context.Database.SelectAll<Customer>().ForEach(Add);
            ListUser.SelectionChanged += delegate { UpdateSelection(); };
        }

        private void Add(Customer customer) => ListUser.Items.Add(new CustomerElement(customer));

        private void UpdateSelection()
        {
            CustomerElement? ie = ListUser.SelectedItem as CustomerElement;

            if (Selection == ie?.Customer)
                return;

            Selection = ie?.Customer;

            MyContent.Children.Clear();
            if (Selection != null)
                MyContent.Children.Add(new CustomerDescription(Selection));
        }
    }

    public class CustomerElement : TextBlock
    {
        public Customer Customer { get; private set; }

        public CustomerElement(Customer customer)
        {
            Customer = customer;
            Text = customer.Email;
            Height = 30;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }
    }
}
