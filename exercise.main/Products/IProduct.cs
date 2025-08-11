using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main.Products
{
    public interface IProduct
    {
        int Id { get; }
        string Sku { get; }
        string Name { get; }
        string Variant { get; }
        decimal Price { get; }
        ProductType Type { get; }
    }
}

