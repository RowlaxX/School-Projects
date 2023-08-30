using BDD.Core.Entities;
using BDD.Core.Relations;
using BDD.User;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace BDD.User
{
    public class Basket
    {
        public UserContext Context { get; private set; }
        public Dictionary<Product, int> Content { get; } = new();
        
        public Basket(UserContext context) => Context = context;

        private Dictionary<int, int> AsItems()
        {
            Dictionary<int, int> d = new();

            foreach(var v in Content)
                foreach(var u in v.Key.Items.DescribeId())
                    if (d.ContainsKey(u.Key))
                        d[u.Key] += u.Value * v.Value;
                    else
                        d.Add(u.Key, u.Value * v.Value);

            return d;
        }

        public void Set(Product product, int quantity)
        {
            if (quantity < 0)
                quantity = 0;
            if (quantity == 0)
            {
                Content.Remove(product);
                return;
            }

            int diff = quantity - Get(product);
            if (diff == 0)
                return;

            int remains = Remaining(product);

            if (diff > remains)
                throw new ApplicationException();

            if (Content.ContainsKey(product))
                Content[product] += diff;
            else
                Content.Add(product, diff);

            Context.ShoppingPage.Refresh();
        }

        public int Get(Product product)
        {
            if (Content.ContainsKey(product))
                return Content[product];
            return 0;
        }

        public double GetPrice()
        {
            double price = 0;
            foreach (var u in Content)
                price += (u.Key.Price ?? 0) * u.Value;
            return price;
        }

        public void Add(Product product, int quantity)
        {
            Set(product, Get(product) + quantity);
        }

        public void Del(Product product, int quantity)
        {
            Set(product, Get(product) - quantity);
        }

        public int Remaining(Product product)
        {
            Dictionary<int, int> basket = AsItems();
            Dictionary<int, int> stock = Context.Store.Stock.DescribeId();

            int min = 1000;
            
            foreach (var v in product.Items.DescribeId())
            {
                if (v.Value <= 0)
                    continue;

                int localCount = basket.ContainsKey(v.Key) ? basket[v.Key] : 0;
                int onlineStock = stock.ContainsKey(v.Key) ? stock[v.Key] : 0;
                int available = onlineStock - localCount;
                int remaining = available / v.Value;

                if (remaining < min)
                    min = remaining;
            }

            return min;
        }

        public void Empty()
        {
            Content.Clear();
            Context.ShoppingPage.Refresh();
        }

        public void Validate()
        {
            ManyToManyRelation mtm;

            foreach(var k in AsItems())
            {
                mtm = Context.Store.Stock;
                int current = mtm.GetQuantity(k.Key);
                mtm.SetQuantity(k.Key, current - k.Value);
            }

            Empty();
        }
    }
}
