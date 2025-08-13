using exercise.main.Inventory;
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
        private int _capacity;

        public Basket(int capacity = 4)
        {
            _capacity = capacity;
        }

        public int Capacity { get { return _capacity; } set { _capacity = value; } }

        public string AddProduct(IProduct product)
        {
            if(_items.Count >= _capacity)
            {
                return $"the basket is full";
            } else
            {
                _items.Add(product);
                return $"Item {product.Name} was successfully added to the basket.";
            }
        }

        // add by SKU with in-stock check 
        public string AddProduct(ICatalog catalog, string sku)
        {
            if (catalog == null) return "no catalog";
            if (!catalog.ProductExists(sku)) return "item not found";

            var item = catalog.Get(sku);
            if (!item.InStock) return $"{item.Name} - {item.Variant} is out of stock.";

            // create & add (capacity check + message reused)
            var product = catalog.CreateProduct(sku);
            return AddProduct(product);
        }

        public List<IProduct> Products => _items;

        public decimal TotalPrice => _items.Sum(p => p.Price);
    }
}
