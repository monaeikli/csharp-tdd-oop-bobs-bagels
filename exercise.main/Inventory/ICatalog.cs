using exercise.main.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exercise.main.Inventory
{
    public interface ICatalog
    {
        bool Has(string sku);
        CatalogItem Get(string sku);
        IEnumerable<CatalogItem> GetByType(ProductType type, bool onlyInStock = true);
        IEnumerable<CatalogItem> GetSoldOutByType(ProductType type);
        IProduct CreateProduct(string sku); // makes a concrete Bagel/Coffee/Filling
        decimal GetPrice(string sku);
    }
}
