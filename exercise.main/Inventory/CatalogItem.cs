using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using exercise.main.Products;

namespace exercise.main.Inventory
{
    public class CatalogItem
    {
        public string Sku { get; }
        public decimal Price { get; }
        public string Name { get; }
        public string Variant { get; }
        public ProductType Type { get; }
        public bool InStock { get; set; }

        public CatalogItem(string sku, decimal price, string name, string variant, ProductType type, bool inStock = true)
        {
            Sku = sku;
            Price = price;
            Name = name;
            Variant = variant;
            Type = type;
            InStock = inStock;
        }
    }
}

