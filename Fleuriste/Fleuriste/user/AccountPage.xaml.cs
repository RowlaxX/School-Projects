using BDD.Core.Entities;
using BDD.Main;
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

namespace BDD.User
{
    public partial class AccountPage : Page
    {
        public UserContext Context { get; private set; }

        public AccountPage(UserContext context)
        {
            InitializeComponent();
            Context = context;

            EditFirstName.LostFocus += delegate { UpdateFirstName(); };
            EditEmail.LostFocus += delegate { UpdateEmail(); };
            EditName.LostFocus += delegate { UpdateName(); };
            EditPhone.LostFocus += delegate { UpdatePhone(); };
            EditPassword.Click += delegate { UpdatePassword(); };

            Display(Context.Customer);
        }

        private void Edit(string column, TextBox tb, string? msg)
        {
            bool success = false;

            if (msg == null)
                try
                {


                    Context.Customer.Edit(column, tb.Text);
                    success = true;
                }
                catch (MySqlException e)
                {
                    MessageBox.Show(e.Message, "Erreur");
                }
            else
                MessageBox.Show(msg, "Erreur");

            if (!success)
                tb.Text = (string)(Context.Customer.Get(column) ?? "");
        }

        private void Display(Customer c)
        {
            EditName.Text = c.Name;
            EditFirstName.Text = c.FirstName;
            EditPhone.Text = c.Phone;
            EditEmail.Text = c.Email;
        }

        private void UpdatePassword()
        {
            new EditPasswordWindow(Context.Customer).ShowDialog();
        }

        private void UpdateName() => Edit("name", EditName, MainContext.IsNameValid(EditName.Text) ? null : "Le nom n'est pas valide");
        private void UpdateFirstName() => Edit("firstname", EditFirstName, MainContext.IsNameValid(EditFirstName.Text)? null : "Le prenom n'est pas valide");
        private void UpdateEmail() => Edit("email", EditEmail, MainContext.IsEmailValid(EditEmail.Text) ? null : "L'e-mail donnée est incorrect");
        private void UpdatePhone() => Edit("phone", EditPhone, MainContext.IsPhoneValid(EditPhone.Text) ? null : "Le telephone n'est pas valide");
    }
}
