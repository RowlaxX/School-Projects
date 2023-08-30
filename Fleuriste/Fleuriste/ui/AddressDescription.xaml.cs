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
    public partial class AddressDescription : UserControl
    {
        public AddressDescription(Address address)
        {
            InitializeComponent();
            AddressName.Text = address.Name ?? "";
            AddressCountry.Text = address.Country ?? "";
            AddressCity.Text = address.City ?? "";
            AddressHint.Text = address.Hint ?? "";
            AddressComment.Text = address.Comment ?? "";
            AddressStreet.Text = address.Street ?? "";
            AddressNumber.Text = address.Number == null ? "" : address.Number.ToString();
            AddressZip.Text = address.Zip == null ? "" : address.Zip.ToString();
        }
    }
}
