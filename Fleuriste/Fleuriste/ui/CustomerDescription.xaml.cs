using BDD.Core.Entities;
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

namespace BDD.UI
{
    public partial class CustomerDescription : UserControl
    {
        public CustomerDescription(Customer customer)
        {
            InitializeComponent();
            CEmail.Text = customer.Email;
            CFirstName.Text = customer.FirstName;
            CName.Text = customer.Name;
            CPhone.Text = customer.Phone;
        }
    }
}
