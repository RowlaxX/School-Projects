using BDD.Admin;
using System.Text.RegularExpressions;

namespace BDD.Main
{
    public class MainContext : Context
    {
        public LoginPage LoginPage { get; private set; }
        public RegisterPage RegisterPage { get; private set; }

        public MainContext(MainWindow main) : base(main)
        {
            LoginPage = new(this);
            RegisterPage = new(this);

            AddMenuButton("Admin", () => MainWindow.Context = new AdminContext(MainWindow));
            AddMenuButton("Login", LoginPage);
            AddMenuButton("Register", RegisterPage);

            SetContent(LoginPage);
        }

        private static readonly string REG_PHONE = @"^0[1-9]([-. ]?[0-9]{2}){4}$";
        private static readonly string REG_NAME = @"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$";
        private static readonly string REG_EMAIL = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        private static bool Match(string? str, string reg)
        {
            if (str == null)
                return false;

            return Regex.IsMatch(str, reg);
        }

        public static bool IsEmailValid(string? email) => Match(email, REG_EMAIL);

        public static bool IsPhoneValid(string? phone) => Match(phone, REG_PHONE);

        public static bool IsNameValid(string? name) => Match(name, REG_NAME);

        public void ShowLogin() => SetContent(LoginPage);
    }
}
