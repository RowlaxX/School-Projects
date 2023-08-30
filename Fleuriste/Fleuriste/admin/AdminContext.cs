using BDD.Main;
using Org.BouncyCastle.Crypto.Agreement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD.Admin
{
    public class AdminContext : Context
    {

        public AdminContext(MainWindow main) : base(main)
        {
            AddMenuButton("Products", () => SetContent(new ProductPage(this) ));
            AddMenuButton("Stores", () => SetContent(new StorePage(this)));
            AddMenuButton("Items", () => SetContent(new ItemPage(this)));
            AddMenuButton("Orders", () => SetContent(new OrderPage(this)));
            AddMenuButton("Users", () => SetContent(new CustomerPage(this)));
            AddMenuButton("Logout", Logout);
            SetContent(new ProductPage(this));
        }

        private void Logout() => MainWindow.Context = new MainContext(MainWindow);
    }
}
