using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main.Products
{
    public class Bagel : Product
    {
        public Bagel(int id, string sku, string variant, decimal price)
            : base(id, sku, "Bagel", variant, price, ProductType.Bagel) { }
    }
}

