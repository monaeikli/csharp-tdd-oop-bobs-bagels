using exercise.main;
using exercise.main.Inventory;
using exercise.main.Products;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace exercise.main
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var basket = new Basket();
            var catalog = new Catalog();

            while (true)
            {
                Console.WriteLine("Welcome to Bob's Bagels! What would you like to order?");
                Console.WriteLine("1. Bagel");
                Console.WriteLine("2. Coffee");
                Console.WriteLine("(q = quit)");
                Console.Write("Choice: ");
                var first = (Console.ReadLine() ?? "").Trim();
                if (first.Equals("q", StringComparison.OrdinalIgnoreCase)) return;

                if (first == "1")
                {
                    AddBagelWithFillings(catalog, basket);
                }
                else if (first == "2")
                {
                    AddCoffee(catalog, basket);
                }
                else
                {
                    Console.WriteLine("Invalid choice.\n");
                    continue;
                }

                // change menu
                while (true)
                {
                    Console.WriteLine("\nDo you want to change anything in your basket?");
                    Console.WriteLine("1. Add something");
                    Console.WriteLine("2. Remove something");
                    Console.WriteLine("3. No, I'm good");
                    Console.Write("Choice: ");
                    var change = (Console.ReadLine() ?? "").Trim();

                    if (change == "1")
                    {
                        Console.WriteLine("\nWhat would you like to add?");
                        Console.WriteLine("1. Bagel");
                        Console.WriteLine("2. Coffee");
                        Console.WriteLine("3. Cancel");
                        Console.Write("Choice: ");
                        var add = (Console.ReadLine() ?? "").Trim();

                        if (add == "1") AddBagelWithFillings(catalog, basket);
                        else if (add == "2") AddCoffee(catalog, basket);
                    }
                    else if (change == "2")
                    {
                        if (!RemoveByPrintedNumber(basket))
                            continue; // invalid number -> message already shown
                    }
                    else if (change == "3") break;
                    else Console.WriteLine("Invalid choice.");
                }

                // summary and exit
                PrintBasketIndented(basket);
                Console.WriteLine($"\nYour total is: ${basket.TotalPrice:0.00}\n");
                Console.WriteLine("Thank you for your order!");
                return;
            }
        }

        // ---------- flows ----------
        private static void AddBagelWithFillings(ICatalog catalog, Basket basket)
        {
            var bagel = PickOne(catalog, ProductType.Bagel, "Which bagel would you like?");
            if (bagel == null) return;

            basket.AddProduct(catalog.CreateProduct(bagel.Sku));

            var chosenFillings = PickFillings(catalog, bagel.Variant);
            foreach (var f in chosenFillings) basket.AddProduct(catalog.CreateProduct(f.Sku));

            Console.WriteLine(chosenFillings.Count == 0
                ? $"\nYou have ordered a {bagel.Variant.ToLower()} bagel with no fillings."
                : $"\nYou have ordered a {bagel.Variant.ToLower()} bagel with {FormatList(chosenFillings.Select(x => x.Variant))}.");
        }

        private static void AddCoffee(ICatalog catalog, Basket basket)
        {
            var coffee = PickOne(catalog, ProductType.Coffee, "Which coffee would you like?");
            if (coffee == null) return;

            basket.AddProduct(catalog.CreateProduct(coffee.Sku));
            Console.WriteLine($"\nYou have added {coffee.Variant.ToLower()} coffee.");
        }

        // ---------- pickers ----------
        private static CatalogItem? PickOne(ICatalog catalog, ProductType type, string title)
        {
            var avail = catalog.GetByType(type, onlyInStock: true).ToList();
            var sold = catalog.GetSoldOutByType(type).ToList();

            Console.WriteLine($"\n{title}");
            for (int i = 0; i < avail.Count; i++) Console.WriteLine($"{i + 1}. {avail[i].Variant} — ${avail[i].Price:0.00}");
            if (sold.Count > 0) Console.WriteLine($"\nWe are unfortunately out of stock for: {string.Join(", ", sold.Select(x => x.Variant))}");
            Console.Write("Enter number: ");

            int idx = ReadIndex(avail.Count);
            return avail[idx];
        }

        private static List<CatalogItem> PickFillings(ICatalog catalog, string bagelVariant)
        {
            var fills = catalog.GetByType(ProductType.Filling, onlyInStock: true).ToList();
            Console.WriteLine($"\nWhat would you like on your {bagelVariant.ToLower()} bagel?");
            for (int i = 0; i < fills.Count; i++) Console.WriteLine($"{i + 1}. {fills[i].Variant} — ${fills[i].Price:0.00}");
            Console.WriteLine($"{fills.Count + 1}. No fillings");
            Console.Write($"Pick one or more (e.g., 1,3) or {fills.Count + 1} for none: ");

            var input = (Console.ReadLine() ?? "").Trim();
            if (int.TryParse(input, out var single) && single == fills.Count + 1) return new();

            var chosen = new List<CatalogItem>();
            foreach (var p in input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                if (int.TryParse(p, out var n) && n >= 1 && n <= fills.Count)
                    if (!chosen.Contains(fills[n - 1])) chosen.Add(fills[n - 1]);

            return chosen;
        }

        // ---------- basket UI ----------
        private static void PrintBasketIndented(Basket basket)
        {
            Console.WriteLine("\nYour basket:");
            for (int i = 0; i < basket.Products.Count; i++)
            {
                var p = basket.Products[i];
                if (p.Type == ProductType.Bagel)
                {
                    Console.WriteLine($"- {p.Name} - {p.Variant} … ${p.Price:0.00}");
                    int j = i + 1;
                    while (j < basket.Products.Count && basket.Products[j].Type == ProductType.Filling)
                    {
                        var f = basket.Products[j];
                        Console.WriteLine($"  - {f.Name} - {f.Variant} … ${f.Price:0.00}");
                        j++;
                    }
                    i = j - 1;
                }
                else
                {
                    Console.WriteLine($"- {p.Name} - {p.Variant} … ${p.Price:0.00}");
                }
            }
        }

        private static bool RemoveByPrintedNumber(Basket basket)
        {
            if (basket.Products.Count == 0) { Console.WriteLine("\nYour basket is empty."); return false; }

            Console.WriteLine("\nYour basket (choose a number to remove):");
            for (int i = 0; i < basket.Products.Count; i++)
            {
                var p = basket.Products[i];
                var indent = p.Type == ProductType.Filling ? "  " : "";
                Console.WriteLine($"{i + 1}. {indent}{p.Name} - {p.Variant} … ${p.Price:0.00}");
            }
            Console.Write("Enter number to remove: ");
            var raw = (Console.ReadLine() ?? "").Trim();

            // foreach-based validation (ok per your preference)
            var found = false; int choice;
            if (int.TryParse(raw, out choice))
            {
                int pos = 1;
                foreach (var _ in basket.Products) { if (pos == choice) { found = true; break; } pos++; }
            }
            if (!found) { Console.WriteLine("That item is not in your basket."); return false; }

            int idx = choice - 1;
            var item = basket.Products[idx];

            if (item.Type == ProductType.Bagel)
            {
                basket.Products.RemoveAt(idx);
                while (idx < basket.Products.Count && basket.Products[idx].Type == ProductType.Filling)
                    basket.Products.RemoveAt(idx);
                Console.WriteLine("\nRemoved bagel (and its fillings).");
            }
            else
            {
                basket.Products.RemoveAt(idx);
                Console.WriteLine("\nRemoved item.");
            }
            return true;
        }

        private static int ReadIndex(int max)
        {
            while (true)
            {
                var input = (Console.ReadLine() ?? "").Trim();
                if (int.TryParse(input, out var n) && n >= 1 && n <= max) return n - 1;
                Console.Write("Invalid choice. Try again: ");
            }
        }

        private static string FormatList(IEnumerable<string> items)
        {
            var list = items.Select(s => s.ToLower()).ToList();
            if (list.Count == 0) return "";
            if (list.Count == 1) return list[0];
            if (list.Count == 2) return $"{list[0]} and {list[1]}";
            return string.Join(", ", list.Take(list.Count - 1)) + " and " + list[^1];
        }
    }
}
