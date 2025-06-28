using EStore.App.Services;
using EStore.Core.Entities;
using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Entities.OrderAggregate;
using EStore.Core.Extensions;
using EStore.Infra.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;


namespace IntegrationTests.AutomaticTests
{
  public class OrderTests
  {

    const int _productId = 1;

    ITestOutputHelper _output;
    EStoreDbContext _dbContext;
    IConfiguration _config;
    ProductService _productService;
    string? _userId;
    bool _isConcurrencyTest = false;

    Basket _basket;
    Product _product;

    public OrderTests(ITestOutputHelper output)
    {
      _output = output;
      var scope = Helper4Tests.GetServiceScope();
      _dbContext = scope.ServiceProvider.GetRequiredService<EStoreDbContext>();
      _config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
      _productService = scope.ServiceProvider.GetRequiredService<ProductService>();
      _userId = _config["userId"] ?? throw new ArgumentNullException();
      InitBasket(); //mc, Set basket and add product for tests
    }

    private void InitBasket()
    {
      _basket = new Basket(_userId!);
      _product = new Product("test", 3, null, 5.50m, 100);
      _basket.SetBasketItem(_productId, 2, _product.Price, _product);
    }

    [Fact]
    public async Task TestProductPriceChange()
    {
      //using var scope = Helper4Tests.GetServiceScope();

      //var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
      //var userId = config["userId"];//mc; getting config from secret or appsettings.json

      //var _product = new Product("test", 3, null, 5.50m, 100);
      //_product.Id = 1;

      //var _basket=new Basket(_userId!);
      //_basket.SetBasketItem(_product.Id, 2, _product.Price);
      //var dbContext = scope.ServiceProvider.GetRequiredService<EStoreDbContext>();

      SqlChangeProductPrice(_dbContext);

      try
      {
        await CreateOrderAsync(_dbContext, _basket, 1);
      }
      catch (Exception ex)
      {
        _output.WriteLine(ex.Message);
        Assert.Contains("Price of product", ex.Message);
      }

      finally { SqlRollBackChangeProductPrice(_dbContext); }


    }

    [Fact]
    public async Task TestQtNotEnough()
    {
      SqlChangeProductQt(_dbContext);
      try
      {
        await CreateOrderAsync(_dbContext, _basket, 1);
      }
      catch (Exception ex)
      {
        _output.WriteLine(ex.Message);
        Assert.Contains("quantity not enough", ex.Message);
      }
      finally { SqlRollBackChangeProductQt(_dbContext); }

    }

    [Fact]
    public async Task TestConcurrencyException()
    {
      try
      {
        _isConcurrencyTest = true;
        await CreateOrderAsync(_dbContext, _basket, 1);
      }
      catch (Exception ex)
      {
        _output.WriteLine(ex.Message);
        if (ex is DbUpdateConcurrencyException)
          return;
        throw;

      }
      Assert.Fail();
    }

    [Fact]
    public async Task MakeOrder()
    {
      await CreateOrderAsync(_dbContext, _basket, 1);
    }



    private async Task CreateOrderAsync(EStoreDbContext context, Basket basket, int addressId)
    {
      basket.GuardNull();
      basket.BasketItems.GuardNull();
      addressId.GuardZero();

      bool hasOrderCached = false;
      bool saved = false;
      bool concurrencyException = false;
      //mc, Saved flag will be false until order created/saved to db. When there is a change in related data, loop will start again. Within the loop, items (product) will be checked for price/qt etc. constraints. 
      while (!saved)
      {

        foreach (var basketItem in basket.BasketItems)
        {
          //todo ef doesnt track projections thus no concurrency protection take place if used, any workaround maybe table splitting?
          var productOnDB = await context.Products.Where(p => p.Id == basketItem.ProductId).FirstOrDefaultAsync();
          productOnDB.GuardNull();
          if (_isConcurrencyTest)
            _dbContext.Database.ExecuteSql($"update Products set qt={productOnDB?.Qt - 1} where id={productOnDB?.Id}");

          //basketItem.Product = productOnDB;

          //context.Attach(productOnDB!);
          //mc, if concurrencyException was thrown and item state modified, reload ef tracked/cached entity from db. It looks like even rows/items not changed seem changed probably because it belongs to entity type that changed, bug? or expected behaviour?  
          if (concurrencyException)
          {
            var entry = context.Entry(productOnDB!);
            if (entry.State == EntityState.Modified)
              await entry.ReloadAsync();
            else
              continue;
          }


          if (basketItem.Price != productOnDB!.Price)
            throw new Exception($"Price of product {basketItem.Product?.Name} changed");
          if (productOnDB!.Qt < basketItem.Qt)
            throw new Exception($"Product {basketItem.Product?.Name} quantity not enough");

          //mc, decrease product qt by basket item qt 
          //basketItem.Product.UpdateQt(-basketItem.Qt);
          productOnDB!.UpdateQt(-basketItem.Qt);
        }

        //mc, to prevent creating duplicate orders
        if (!hasOrderCached)
        {
          var order = new Order(basket, addressId);
          context.Orders.Add(order);
          hasOrderCached = true;
        }

        try
        {
          await context.SaveChangesAsync();
          saved = true;
        }
        catch (DbUpdateConcurrencyException)
        {
          concurrencyException = true;
          throw;
          //foreach (var entry in ex.Entries) //mc, this only returns single entry, a bug?
          //entry.Reload();

        }

      }

    }

    private void SqlChangeProductPrice(EStoreDbContext context)
    {
      context.Database.ExecuteSql($"update Products set Price=1 where id={_productId}");
    }

    private void SqlRollBackChangeProductPrice(EStoreDbContext context)
    {
      context.Database.ExecuteSql($"update Products set Price=5.50 where id={_productId}");
    }

    private void SqlChangeProductQt(EStoreDbContext context)
    {
      context.Database.ExecuteSql($"update Products set qt=0 where id={_productId}");
    }

    private void SqlRollBackChangeProductQt(EStoreDbContext context)
    {
      context.Database.ExecuteSql($"update Products set qt=50 where id={_productId}");
    }

  }//eo cls
}
