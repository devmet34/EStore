using EStore.Core.Interfaces;

namespace EStore.Core.Entities;
public class Category : BaseEntity, IAggregateRoot
{
  public string MainCat { get; private set; }
  public string? SubCat { get; private set; }
  public string? Description { get; private set; }

  public Category(string mainCat, string? subCat, string? description)
  {
    MainCat = mainCat;
    SubCat = subCat;
    Description = description;
  }
}
