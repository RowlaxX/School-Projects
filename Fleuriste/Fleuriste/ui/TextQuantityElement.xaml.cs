using BDD.Core;
using Org.BouncyCastle.Bcpg;
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
    public partial class TextQuantityElement : UserControl
    {
        public Entity Entity { get; private set; }
        public int Quantity { get; private set; }
        public Action<TextQuantityElement>? OnQuantityChanged { get; private set; }

        public TextQuantityElement(string text, Entity entity=null, Action<TextQuantityElement>? onquantitychanged=null)
        {
            InitializeComponent();
            MyText.Text = text;
            MyEdit.IsEnabled = false;
            MyEdit.LostFocus += delegate { Check(); };
            OnQuantityChanged = onquantitychanged;
            Quantity = 0;
            Entity = entity;
        }

        public void Reset()
        {
            MyEdit.Text = "";
            MyEdit.IsEnabled = false;
            Quantity = 0;
        }

        public void SetQuantity(int quantity, bool enabled=true)
        {
            Quantity = quantity;
            MyEdit.IsEnabled = enabled;
            MyEdit.Text = quantity.ToString();
        }

        private void Check()
        {
            try
            {
                Quantity = Convert.ToInt32(MyEdit.Text);
                OnQuantityChanged?.Invoke(this);
            }
            catch (FormatException)
            {
                MessageBox.Show("La quantité n'est pas valide", "Erreur");
                MyEdit.Text = Quantity.ToString();
            }
        }
    }
}
