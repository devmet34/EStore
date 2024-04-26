using Estore.Core.Entities.ValueObjects;
using Estore.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities;
public class Customer:BaseEntity
{
  public string CustomerId { get; private set; }
  public string CustomerName { get; private set; }
  public Address? Address { get; private set; }
  public decimal Credit { get; private set; }

  public Customer(string customerId, string customerName, Address? address, decimal credit=100)
  {
    CustomerId = customerId;
    CustomerName = customerName;
    Address = address;
    Credit = credit;
  }

  public void UpdateCredit(decimal credit) 
  {
    credit.GuardNegative();
    Credit = credit;
  }
}
