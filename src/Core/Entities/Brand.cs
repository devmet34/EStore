using EStore.Core.Entities.ValueObjects;
using EStore.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Entities;
public class Brand:BaseEntity, IAggregateRoot
{
  public string Name { get; private set; }
 
  public Brand(string name)
  {
    Name = name;
    
  }
}
