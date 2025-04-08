using EStore.Core.Entities.BasketAggregate;
using Xunit.Abstractions;

namespace UnitTests;
public class BasketTest
{

  ITestOutputHelper _output;
  public BasketTest(ITestOutputHelper output)
  {
    _output = output;
  }

  [Fact]
  public void TestBasket()
  {
    var basket = new Basket("test");
    basket.SetBasketItem(1, 3, 4m);
    BasketMock basketMock = new BasketMock("buyer1", 3);
    /*
    basket.AddItem(3);
    basket.AddItem(3);
    basket.AddItem(4);
    basket.AddItem(4);
    */
    /*
    Assert.True(basket.BasketItems.Count == 2);
    Assert.True(basket.BasketItems.Where(b=>b.ProductId == 3).FirstOrDefault().Qt==2);
    Assert.True(basket.BasketItems.Where(b => b.ProductId == 4).FirstOrDefault().Qt == 1);

    */

  }
}
