using exercise.main.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace exercise.main
{
    public class Basket
    {
        private readonly List<IProduct> _items = new();

        public void AddProduct(IProduct product) => _items.Add(product);

        public List<IProduct> Products => _items;

        public decimal TotalPrice => _items.Sum(p => p.Price);
    }
}
