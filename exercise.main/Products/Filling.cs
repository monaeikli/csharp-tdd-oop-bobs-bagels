using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main.Products
{
    public class Filling : Product
    {
        public Filling(int id, string sku, string variant, decimal price)
            : base(id, sku, "Filling", variant, price, ProductType.Filling) { }
    }
}

