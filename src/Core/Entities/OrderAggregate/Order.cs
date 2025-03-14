using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Extensions;
using EStore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Entities.OrderAggregate;
public class Order : BaseEntity, IAggregateRoot
{
    public string BuyerId { get; private set; } = null!;
  public DateTime CreatedAt { get; private set; }   
    public int? CustomerAddressId { get; private set; }
    public decimal TotalPrice { get; private set; }
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    //mc required for ef core
    private Order() { }

    public Order(Basket basket, int addressId)
    {
        basket.BasketItems.GuardNull();
        BuyerId = basket.BuyerId;
        CreatedAt = DateTime.Now;
        CustomerAddressId = addressId;
        TotalPrice = basket.TotalPrice;
        SetOrderItems(basket);

    }

    private void SetOrderItems(Basket basket)
    {
        foreach (var item in basket.BasketItems)
        {
            decimal totalPrice = item.Product!.Price * item.Qt;
            _orderItems.Add(
              new OrderItem(Id, item.ProductId, item.Product!.Name, item.Qt, item.Product!.Price)
              );



        }
    }

}
