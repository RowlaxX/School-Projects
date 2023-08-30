using BDD.Core.Entities;
using BDD.User;
using System.Windows;
using System.Windows.Controls;

namespace BDD.Main
{
    public partial class LoginPage : Page
    {
        public MainContext Context { get; private set; }

        public LoginPage(MainContext context)
        {
            InitializeComponent();

            Context = context;
            LoginButton.Click += delegate { Login(); };

            foreach (Store store in Context.Database.SelectAll<Store>())
                StoreChoice.Items.Add(new StoreChoice(store));
        }

        public void Login()
        {
            string email = EmailInput.Text;
            string password = PasswordInput.Password;

            if (StoreChoice.SelectedItem is not StoreChoice choice)
            {
                MessageBox.Show("Veuillez choisir un magasin", "Erreur");
                return;
            }
            if (!Check(email, password))
                return;

            Customer? customer = Login(email, password);
            if (customer == null)
                return;

            MainWindow main = Context.MainWindow;
            main.Context = new UserContext(main, customer, choice.Store);
        }

        private static bool Check(string email, string password)
        {
            string? error = null;

            if (!MainContext.IsEmailValid(email))
                error = "L'e-mail donnée est incorrect";
            else if (password.Length < 8)
                error = "Le mot de passe doit faire au moins 8 charactères";

            if (error != null)
            {
                MessageBox.Show(error, "Erreur");
                return false;
            }
            return true;
        }

        public Customer? Login(string email, string password)
        {
            Customer? customer = Context.Database.Find<Customer>("email", email);

            if (customer == null)
                MessageBox.Show("Email inexistante", "Erreur");
            else if (!customer.TestPassword(password))
            {
                MessageBox.Show("Mot de passe incorrect", "Erreur");
                return null;
            }

            return customer;
        }
    }
}
