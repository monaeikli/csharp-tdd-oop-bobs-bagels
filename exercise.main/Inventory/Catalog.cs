using exercise.main.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main.Inventory
{
    public class Catalog : ICatalog
    {
        private static int _nextId = 1;
        private readonly Dictionary<string, CatalogItem> _items;

        public Catalog()
        {
            _items = new(StringComparer.OrdinalIgnoreCase)
            {
                ["BGLO"] = new("BGLO", 0.49m, "Bagel", "Onion", ProductType.Bagel, true),
                ["BGLP"] = new("BGLP", 0.39m, "Bagel", "Plain", ProductType.Bagel, true),
                ["BGLE"] = new("BGLE", 0.49m, "Bagel", "Everything", ProductType.Bagel, true),
                ["BGLS"] = new("BGLS", 0.49m, "Bagel", "Sesame", ProductType.Bagel, true), 

                ["COFB"] = new("COFB", 0.99m, "Coffee", "Black", ProductType.Coffee, true),
                ["COFW"] = new("COFW", 1.19m, "Coffee", "White", ProductType.Coffee, true),
                ["COFC"] = new("COFC", 1.29m, "Coffee", "Cappuccino", ProductType.Coffee, false),
                ["COFL"] = new("COFL", 1.29m, "Coffee", "Latte", ProductType.Coffee, false), 

                ["FILB"] = new("FILB", 0.12m, "Filling", "Bacon", ProductType.Filling, true),
                ["FILE"] = new("FILE", 0.12m, "Filling", "Egg", ProductType.Filling, true),
                ["FILC"] = new("FILC", 0.12m, "Filling", "Cheese", ProductType.Filling, true),
                ["FILX"] = new("FILX", 0.12m, "Filling", "Cream Cheese", ProductType.Filling, true),
                ["FILS"] = new("FILS", 0.12m, "Filling", "Smoked Salmon", ProductType.Filling, true),
                ["FILH"] = new("FILH", 0.12m, "Filling", "Ham", ProductType.Filling, true),
            };
        }

        public bool Has(string sku) => _items.ContainsKey(sku);

        public CatalogItem Get(string sku) =>
            _items.TryGetValue(sku, out var item) ? item
            : throw new KeyNotFoundException($"SKU '{sku}' not found.");

        public IEnumerable<CatalogItem> GetByType(ProductType type, bool onlyInStock = true) =>
            _items.Values.Where(i => i.Type == type && (!onlyInStock || i.InStock));

        public IEnumerable<CatalogItem> GetSoldOutByType(ProductType type) =>
            _items.Values.Where(i => i.Type == type && !i.InStock);

        public IProduct CreateProduct(string sku)
        {
            var i = Get(sku);
            if (!i.InStock) throw new InvalidOperationException($"{i.Name} - {i.Variant} is out of stock.");
            var id = _nextId++;

            return i.Type switch
            {
                ProductType.Bagel => new Bagel(id, i.Sku, i.Variant, i.Price),
                ProductType.Coffee => new Coffee(id, i.Sku, i.Variant, i.Price),
                ProductType.Filling => new Filling(id, i.Sku, i.Variant, i.Price),
                _ => throw new NotSupportedException($"Unsupported type for SKU '{i.Sku}'.")
            };
        }

        public decimal GetPrice(string sku) => Get(sku).Price;
    }
}
