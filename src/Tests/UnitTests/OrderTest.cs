using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Entities.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
