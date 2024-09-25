using Estore.Core.Entities;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Helpers;
public class EstoreContextSeed
{
    const int MAX_PRODUCT = 10;
    const int MAX_CATS = 10;
    public static async Task Seed(EStoreDbContext context)
    {

        if (!context.Database.CanConnect())
            throw new Exception("Database not ready");

        var products = GetProducts();
        var cats = GetCats();
        var brands = GetBrands();


        await context.AddRangeAsync(products);
        await context.AddRangeAsync(cats);
        await context.AddRangeAsync(brands);
        await context.SaveChangesAsync();
    }

    public static IEnumerable<Product> GetProducts()
    {
        ICollection<Product> products = new List<Product>();
        Random randomN = new Random();
        int j = 1;

        for (int i = 1; i <= MAX_PRODUCT; i++)
        {
            if (j > 4)
                j = 1;
            var price = (decimal)randomN.Next(1, 100) / 2 + 1;
            var url = $"image/{j}.png";
            var prod = new Product($"product{i}", null, null, price, 5, null, url);
            products.Add(prod);
            j++;
        }
        return products;
    }

    static IEnumerable<Brand> GetBrands()
    {
        return new List<Brand>
    {
      new ("Brand1"),
      new ("Brand2"),
      new ("Brand3"),
      new ("Brand4"),
      new ("Brand5")
    };
    }

    static IEnumerable<Category> GetCats()
    {
        return new List<Category>
    {
      new Category("Cat1",null,null),
      new Category("Cat2",null,null),
      new Category("Cat3",null,null),
      new Category("Cat4",null,null),
      new Category("Cat5",null,null),
    };

    }


}
