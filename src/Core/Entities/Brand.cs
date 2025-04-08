using EStore.Core.Interfaces;

namespace EStore.Core.Entities;
public class Brand : BaseEntity, IAggregateRoot
{
  public string Name { get; private set; }

  public Brand(string name)
  {
    Name = name;

  }
}
