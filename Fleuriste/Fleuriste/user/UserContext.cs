using BDD.Core.Entities;
using BDD.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDD.User
{
    public class UserContext : Context
    {
        public Customer Customer { get; private set; }
        public Store Store { get; private set; }

        public AddressPage AddressPage { get; private set; }
        public Basket Basket { get; private set; }
        public AccountPage AccountPage { get; private set; }
        public ShoppingPage ShoppingPage { get; private set; }

        public UserContext(MainWindow main, Customer customer, Store store) : base(main)
        {
            Customer = customer;
            Store = store;

            Basket = new Basket(this);
            AddressPage = new(this);
            AccountPage = new(this);
            ShoppingPage = new(this);

            AddMenuButton("Shopping", ShoppingPage);
            AddMenuButton("Basket", () => SetContent(new BasketPage(this)));
            AddMenuButton("Address", AddressPage);
            AddMenuButton("Orders", () => SetContent(new OrderPage(this)));
            AddMenuButton("Account", AccountPage);
            AddMenuButton("Logout", Logout);
            SetContent(ShoppingPage);
        }

        private void Logout() => MainWindow.Context = new MainContext(MainWindow);
    }
}
