using BDD.Core.Entities;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace BDD.Main
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public MainContext Context { get; private set; }

        public RegisterPage(MainContext context)
        {
            InitializeComponent();
            Context = context;
            Creation.Click += delegate { Register(); } ;
        }

        private void Register()
        {
            string? error = Check();
            if (error != null)
            {
                MessageBox.Show(error, "Erreur");
                return;
            }

            Customer customer = Context.Database.Create<Customer>();
            customer.SetPassword(PasswordInput.Password);
            customer.Edit("email", EmailInput.Text);
            customer.Edit("phone", TelephoneInput.Text);
            customer.Edit("firstname", FirstNameInput.Text);
            customer.Edit("name", NameInput.Text);

            try
            {
                customer.Persist();

                MessageBox.Show("Utilisateur crée", "Info");
                Context.ShowLogin();
                Context.LoginPage.EmailInput.Text = EmailInput.Text;
                Context.LoginPage.PasswordInput.Password = PasswordInput.Password;
            }
            catch(MySqlException e)
            {
                MessageBox.Show(e.Message, "Erreur");
                return;
            }
        }

        private string? Check()
        {
            string? error = null;

            if (!MainContext.IsEmailValid(EmailInput.Text))
                error = "L'e-mail donnée est incorrect";
            else if (PasswordInput.Password.Length < 8)
                error = "Le mot de passe doit faire au moins 8 charactères";
            else if (!PasswordInput.Password.Equals(PasswordVerification.Password))
                error = "Les mots de passe ne correspondent pas";
            else if (!MainContext.IsNameValid(NameInput.Text))
                error = "Le nom n'est pas valide";
            else if (!MainContext.IsNameValid(FirstNameInput.Text))
                error = "Le prenom n'est pas valide";
            else if (!MainContext.IsPhoneValid(TelephoneInput.Text))
                error = "Le telephone n'est pas valide";

            return error;
        }
    }
}
