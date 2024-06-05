using Estore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests;
public class Product
{
  //public int Id { get; set; }
  public string Name { get; private set; }
  public string? Description { get; private set; }
  public Category? Category { get; private set; }
  public int? CategoryId { get; private set; }
  public Brand? Brand { get; private set; }
  public int? BrandId { get; private set; }
  public decimal Price { get; private set; }
  public int Qt { get; private set; }
  public string? PictureUri { get; private set; }
}
