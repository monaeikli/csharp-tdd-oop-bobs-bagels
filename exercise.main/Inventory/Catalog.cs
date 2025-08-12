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

        public bool ProductExists(string sku)
        {
            return _items.ContainsKey(sku);
        }

        public CatalogItem Get(string sku)
        {
            CatalogItem item;
            if (_items.TryGetValue(sku, out item))
            {
                return item;
            }
            return null;
        }

        // Returns all items of a specific type, optionally filtering out those that are not in stock.
        public IEnumerable<CatalogItem> GetByType(ProductType type, bool onlyInStock = true)
        {
            var result = new List<CatalogItem>();

            foreach (var i in _items.Values)
            {
                if (i.Type == type)
                {
                    if (onlyInStock && !i.InStock)
                    {
                        continue;
                    }
                    result.Add(i);
                }
            }

            return result;
        }

        // Returns all sold-out items of a specific type.
        public IEnumerable<CatalogItem> GetSoldOutByType(ProductType type)
        {
            var result = new List<CatalogItem>();

            foreach (var i in _items.Values)
            {
                if (i.Type == type && !i.InStock)
                {
                    result.Add(i);
                }
            }

            return result;
        }

        // Creates a product instance based on the SKU.
        // Returns null if the SKU doesn't exist, the item is out of stock, or the type is unsupported.
        public IProduct CreateProduct(string sku)
        {
            if (!_items.TryGetValue(sku, out var item))
            {
                // SKU not found
                return null;
            }

            // Not in stock
            if (!item.InStock)
            {
                return null;
            }

            // Unique id in case of multible of the same sku 
            var id = _nextId++;

            // Create the correct product type
            if (item.Type == ProductType.Bagel)
                return new Bagel(id, item.Sku, item.Variant, item.Price);
            if (item.Type == ProductType.Coffee)
                return new Coffee(id, item.Sku, item.Variant, item.Price);
            if (item.Type == ProductType.Filling)
                return new Filling(id, item.Sku, item.Variant, item.Price);

            return null;
        }


        public decimal GetPrice(string sku) => Get(sku).Price;
    }
}
