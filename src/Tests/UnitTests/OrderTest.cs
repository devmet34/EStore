using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Entities.OrderAggregate;

namespace UnitTests;
public class OrderTest
{

  [Fact]
  public void Test()
  {
    var basket = new Basket("test");
    var order = new Order(basket, 55);

  }


}
