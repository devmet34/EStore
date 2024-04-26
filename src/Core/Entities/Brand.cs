using Estore.Core.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Entities;
public class Brand:BaseEntity
{
  public string Name { get; private set; }
 
  public Brand(string name)
  {
    Name = name;
    
  }
}
