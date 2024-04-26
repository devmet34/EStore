using Estore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Specs;
public class BasketSpec : BaseSpec<Basket>
{
  public string? BuyerId {  get; set; }
  public bool IsIncludeItems {  get; set; }

  public BasketSpec(string? buyerId, bool isIncludeItems=true)
  {
    BuyerId = buyerId;
    IsIncludeItems = isIncludeItems;
    SetQuery();
  }
  protected override void SetQuery()
  {
    
    base.AddWhere(b => b.BuyerId == BuyerId);
    if (IsIncludeItems)
      base.AddInclude(b => b.BasketItems);
    
    
  }


}
