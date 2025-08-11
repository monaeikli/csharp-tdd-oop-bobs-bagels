using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main.Products
{
    public class Coffee : Product
    {
        public Coffee(int id, string sku, string variant, decimal price)
            : base(id, sku, "Coffee", variant, price, ProductType.Coffee) { }
    }
}

