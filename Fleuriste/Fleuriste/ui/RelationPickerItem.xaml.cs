using BDD.Core;
using BDD.Core.Relations;
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
    public partial class RelationPickerItem : UserControl
    {
        public RelationPicker Picker { get; private set; }
        public Entity Entity { get; private set; }
        public bool LeftSide { get; private set; }
        public int Quantity { get; private set; }

        public RelationPickerItem(RelationPicker parent, Entity entity, bool leftSide, int quantity=1)
        {
            InitializeComponent();
            Entity = entity;
            Picker = parent;
            LeftSide = leftSide;
            Quantity = quantity;

            if (!leftSide || !parent.Quantifiable)
                MyEdit.Visibility = Visibility.Hidden;
            else
            {
                MyEdit.LostFocus += delegate { Check(); };
                MyEdit.Text = quantity.ToString();
            }

            object? value = entity.Get(Picker.Column);
            MyText.Text = value == null ? "" : value.ToString();
        }

        private void Check()
        {
            try
            {
                Quantity = Convert.ToInt32(MyEdit.Text);
                Picker.Relation.Put(Entity.Id, Quantity);
            }
            catch (FormatException)
            {
                MessageBox.Show("La quantité n'est pas valide", "Erreur");
                MyEdit.Text = Quantity.ToString();
            }
        }
    }
}
