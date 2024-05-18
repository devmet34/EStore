using Estore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Estore.Core.Entities;
public class CustomerAddress:BaseEntity
{
  public string UserId {  get; private set; }
  
  public string Street { get; private set; }
  public string? Province {  get; private set; }

  public string City { get; private set; }

  public string Country { get; private set; }

  public int ZipCode { get; private set; }

 

  public CustomerAddress(string userId, string street, string? province, string city, string country, int zipCode)
  {
    UserId = userId;
    Street = street;
    Province = province;
    City = city;    
    Country = country;
    ZipCode = zipCode;
  }

  public void Update(string street, string? province, string city, string country,int zipcode) {
    Street = street;
    Province=province;
    City = city;
    Country = country;
    ZipCode = zipcode;
  
  }
}
