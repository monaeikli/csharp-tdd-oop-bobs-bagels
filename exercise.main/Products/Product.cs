using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main.Products
{
    public abstract class Product : IProduct
    {
        public int Id { get; }
        public string Sku { get; }
        public string Name { get; }
        public string Variant { get; }
        public decimal Price { get; }
        public ProductType Type { get; }

        protected Product(int id, string sku, string name, string variant, decimal price, ProductType type)
        {
            Id = id;
            Sku = sku;
            Name = name;
            Variant = variant;
            Price = price;
            Type = type;
        }

        public override string ToString() => $"{Name} - {Variant} ({Sku}) ${Price:0.00}";
    }
}

