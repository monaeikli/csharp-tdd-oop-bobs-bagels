using static NUnit.Framework.Internal.OSPlatform;
using exercise.main;               
using exercise.main.Inventory;    
using ProductType = exercise.main.Products.ProductType;


namespace exercise.tests;

public class Tests
{
    [Test]
    public void AddToBasket_Bagel()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        var bagel = catalog.CreateProduct("BGLP");
        basket.AddProduct(bagel);

        Assert.That(basket.Products.Count, Is.EqualTo(1));
        Assert.That(basket.Products[0].Type, Is.EqualTo(ProductType.Bagel));
        Assert.That(basket.Products[0].Variant, Is.EqualTo("Plain"));
    }

    [Test]
    public void AddToBasket_OutOfStock()
    {
        var catalog = new Catalog();  
        var basket = new Basket();

        Assert.That(catalog.Has("COFC"), Is.True);

        // trying to create the product should fail
        Assert.Throws<System.InvalidOperationException>(() => catalog.CreateProduct("COFC"));

        // nothing was added
        Assert.That(basket.Products, Is.Empty);
    }

    [Test]
    public void RemoveFromBasket()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        basket.AddProduct(catalog.CreateProduct("BGLP")); 
        basket.AddProduct(catalog.CreateProduct("COFB")); 

        basket.Products.RemoveAt(0); 

        Assert.That(basket.Products.Count, Is.EqualTo(1));
    }
    /*
    [Test]
    public void RemoveItemsNotInBasket()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        basket.AddProduct(catalog.CreateProduct("BGLP")); 

  
    }
    */
    [Test]
    public void TotalCost()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        basket.AddProduct(catalog.CreateProduct("BGLP")); // 0.39
        basket.AddProduct(catalog.CreateProduct("COFB")); // 0.99 (in stock)
        basket.AddProduct(catalog.CreateProduct("FILC")); // 0.12

        Assert.That(basket.TotalPrice, Is.EqualTo(0.39m + 0.99m + 0.12m)); // 1.50
    }
}
