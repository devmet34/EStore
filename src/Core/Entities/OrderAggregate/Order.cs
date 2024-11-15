﻿using Estore.Core.Entities.BasketAggregate;
using Estore.Core.Extensions;
using Estore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities.OrderAggregate;
public class Order : BaseEntity, IAggregateRoot
{
    public string BuyerId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    //public CustomerAddress Address { get; private set; }
    public decimal TotalPrice { get; private set; }
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    //mc required for ef core
    private Order() { }

    public Order(Basket basket)
    {
        basket.BasketItems.GuardNull();
        BuyerId = basket.BuyerId;
        CreatedAt = DateTime.Now;
        //Address = address;
        TotalPrice = basket.TotalPrice;
        SetOrderItems(basket);

    }

    private void SetOrderItems(Basket basket)
    {
        foreach (var item in basket.BasketItems)
        {
            decimal totalPrice = item.Price * item.Qt;
            _orderItems.Add(
              new OrderItem(Id, item.ProductId, item.ProductName, item.Qt, item.Price)
              );



        }
    }

}
