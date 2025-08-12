using static NUnit.Framework.Internal.OSPlatform;
using exercise.main;               
using exercise.main.Inventory;    
using ProductType = exercise.main.Products.ProductType;
using exercise.main.Products;


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
    public void RemoveFromBasket()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        basket.AddProduct(catalog.CreateProduct("BGLP")); 
        basket.AddProduct(catalog.CreateProduct("COFB")); 

        basket.Products.RemoveAt(0); 

        Assert.That(basket.Products.Count, Is.EqualTo(1));
    }
 
    [Test]
    public void TotalCost()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        basket.AddProduct(catalog.CreateProduct("BGLP")); // 0.39
        basket.AddProduct(catalog.CreateProduct("COFB")); // 0.99
        basket.AddProduct(catalog.CreateProduct("FILC")); // 0.12

        Assert.That(basket.TotalPrice, Is.EqualTo(0.39m + 0.99m + 0.12m)); // 1.50
    }

    [Test]
    public void IfBasketIsFullMessageShouldBe() 
    {
        var catalog = new Catalog();
        var basket = new Basket();

        
        basket.AddProduct(catalog.CreateProduct("BGLP"));
        basket.AddProduct(catalog.CreateProduct("BGLP"));
        basket.AddProduct(catalog.CreateProduct("BGLP"));
        basket.AddProduct(catalog.CreateProduct("BGLP"));

        string resultat = basket.AddProduct(catalog.CreateProduct("BGLO"));

        string expected = $"the basket is full";

        Assert.That(resultat, Is.EqualTo(expected));
    }

    [Test]
    public void ChangeBasketCapacity()
    {
        var catalog = new Catalog();
        var basket = new Basket(2);
      
        Assert.That(basket.Capacity, Is.EqualTo(2));

        basket.AddProduct(catalog.CreateProduct("BGLP"));
        basket.AddProduct(catalog.CreateProduct("COFB")); 
        basket.AddProduct(catalog.CreateProduct("FILC")); 

        string resultat = basket.AddProduct(catalog.CreateProduct("BGLO"));

        string expected = $"the basket is full";
        Assert.That(resultat, Is.EqualTo(expected));

        basket.Capacity = 8; // change capacity to 8

        string result = basket.AddProduct(catalog.CreateProduct("BGLO"));

        string expected2 = $"Item {catalog.CreateProduct("BGLO").Name} was successfully added to the basket.";

        Assert.That(basket.Capacity, Is.EqualTo(8));
        Assert.That(result, Is.EqualTo(expected2));
    }

    [Test]
    public void ViewPriceOfItem()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        basket.AddProduct(catalog.CreateProduct("BGLP")); // 0.39
        basket.AddProduct(catalog.CreateProduct("COFB")); // 0.99
        basket.AddProduct(catalog.CreateProduct("FILC")); // 0.12

        Assert.That(basket.Products[0].Price, Is.EqualTo(0.39m));
        Assert.That(basket.Products[1].Price, Is.EqualTo(0.99m));
        Assert.That(basket.Products[2].Price, Is.EqualTo(0.12m));
    }

    [Test]
    public void OnlyOrderItemsInStock()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        // in-stock -> added
        string expected = "Item Bagel was successfully added to the basket.";
        string result = basket.AddProduct(catalog, "BGLP");
        Assert.That(result, Is.EqualTo(expected));
        Assert.That(basket.Products.Count, Is.EqualTo(1));

        // try add out-of-stock item
        var outOfStockItem = catalog.Get("COFC");
        expected = $"{outOfStockItem.Name} - {outOfStockItem.Variant} is out of stock.";
        result = basket.AddProduct(catalog, "COFC");

        Assert.That(result, Is.EqualTo(expected));
        Assert.That(basket.Products.Count, Is.EqualTo(1)); 
    }

    [Test]
    public void AddExtraFillingsToBagel()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        basket.AddProduct(catalog.CreateProduct("BGLP")); // bagel

        string expected = "Item Filling was successfully added to the basket.";

        string result = basket.AddProduct(catalog.CreateProduct("FILC")); // cheese
        Assert.That(result, Is.EqualTo(expected));

        result = basket.AddProduct(catalog.CreateProduct("FILB")); // bacon
        Assert.That(result, Is.EqualTo(expected));

        Assert.That(basket.Products.Count, Is.EqualTo(3));
        Assert.That(basket.Products[1].Type, Is.EqualTo(ProductType.Filling));
        Assert.That(basket.Products[2].Type, Is.EqualTo(ProductType.Filling));
    }

    [Test]
    public void AddFillings()
    {
        var catalog = new Catalog();
        var basket = new Basket();

        basket.AddProduct(catalog.CreateProduct("BGLP")); // bagel

        string expected = "Item Filling was successfully added to the basket.";
        string result = basket.AddProduct(catalog.CreateProduct("FILC")); 
        Assert.That(result, Is.EqualTo(expected));

        result = basket.AddProduct(catalog.CreateProduct("FILB")); // bacon
        Assert.That(result, Is.EqualTo(expected));

        Assert.That(basket.Products.Count, Is.EqualTo(3));
        Assert.That(basket.Products.Count(p => p.Type == ProductType.Filling), Is.EqualTo(2));
    }

    [Test]
    public void ViewPriceOfEachFilling()
    {
        var catalog = new Catalog();

        var expected = new Dictionary<string, decimal>
        {
            ["FILB"] = 0.12m, // Bacon
            ["FILE"] = 0.12m, // Egg
            ["FILC"] = 0.12m, // Cheese
            ["FILX"] = 0.12m, // Cream Cheese
            ["FILS"] = 0.12m, // Smoked Salmon
            ["FILH"] = 0.12m, // Ham
        };

        foreach (var (sku, expectedPrice) in expected)
        {
            var result = catalog.GetPrice(sku);
            Assert.That(result, Is.EqualTo(expectedPrice));
        }
    }

    [Test]
    public void ViewPriceOfEachBagel()
    {
        var catalog = new Catalog();

        var expected = new Dictionary<string, decimal>
        {
            ["BGLO"] = 0.49m, // Onion
            ["BGLP"] = 0.39m, // Plain
            ["BGLE"] = 0.49m, // Everything
            ["BGLS"] = 0.49m, // Sesame
        };

        foreach (var entry in expected)
        {
            decimal expectedPrice = entry.Value;
            decimal result = catalog.GetPrice(entry.Key);
            Assert.That(result, Is.EqualTo(expectedPrice));
        }
    }




}
