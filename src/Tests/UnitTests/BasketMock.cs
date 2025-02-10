using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Entities;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UnitTests;
public class BasketMock
{
  public int Id { get; set; }
  public string BuyerId { get; private set; }

  public DateTime BasketCreatedAt { get; init; }
  [JsonInclude]
  public decimal TotalPrice { get; private set; }
  [JsonInclude]
  public ICollection<BasketItemMock> BasketItems { get; private set; } = new List<BasketItemMock>();

  /// <summary>
  ///mc, Basketmock with given string buyerId and basket itemcount. eg: ("buyer1", 3) will create a basket with 3 mocked basketitems.
  /// </summary>
  /// <param name="buyerId"></param>
  /// 
  /// <param name="itemCount"></param>
  public BasketMock(string buyerId, int itemCount)
  {
    Id = Helper.GetRandomInt(1, 100);
    BuyerId = buyerId.GuardNullOrEmpty();
    BasketCreatedAt = DateTime.Now;
    TotalPrice = 50;
    for (int i = 0; i < itemCount; i++)
    {
      BasketItems.Add(new BasketItemMock(Id));
    }

  }
}
