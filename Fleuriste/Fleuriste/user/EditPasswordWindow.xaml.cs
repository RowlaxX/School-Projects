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
using System.Windows.Shapes;

namespace BDD.User
{
    public partial class EditPasswordWindow : Window
    {
        public Customer Customer { get; private set; }

        public EditPasswordWindow(Customer customer)
        {
            InitializeComponent();
            Customer = customer;

            Cancel.Click += delegate { Close(); };
            Confirm.Click += delegate { Check(); };
        }

        private void Check()
        {
            string oldP = InputOld.Password;
            string newP = InputNew.Password;
            string confirmP = InputConfirm.Password;

            string? error = null;
            if (!Customer.TestPassword(oldP))
                error = "L'ancien mot de passe ne correspond pas";
            else if (newP.Length < 8)
                error = "Le mot de passe doit faire au moins 8 charactères";
            else if (!newP.Equals(confirmP))
                error = "Les mots de passe ne correspondent pas";
        
            if (error != null)
            {
                MessageBox.Show(error, "Erreur");
                return;
            }

            Customer.SetPassword(newP);
            MessageBox.Show("Mot de passe modifié", "Info");
            Close();
        }
    }
}
