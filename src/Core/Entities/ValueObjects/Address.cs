using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities.ValueObjects;
public class Address // ValueObject
{
  public string Street { get; private set; }

  public string City { get; private set; }

  public string Country { get; private set; }

  public string ZipCode { get; private set; }

  #pragma warning disable CS8618 // Required by Entity Framework
  private Address() { }

  public Address(string street, string city, Enums.Country country, string zipcode)
  {
    Street = street;
    City = city;   
    Country = country.ToString();
    ZipCode = zipcode;
  }
}
