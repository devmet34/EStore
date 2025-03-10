using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities.OrderAggregate;
public class OrderItem : BaseEntity
{
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; } = null!;
    public int Qt { get; private set; }
    public decimal Price { get; private set; }


    private OrderItem() { }  //For Ef core
    public OrderItem(int orderId, int productId, string productName, int qt, decimal price)
    {
        OrderId = orderId;
        ProductId = productId;
        ProductName = productName;
        Qt = qt;
        Price = price;
    }
}
