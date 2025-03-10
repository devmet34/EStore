using Estore.Core.Entities;
using Estore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Specs;
public class ProductSpec : BaseSpec<Product>
{
  //public Expression<Func<Product, bool>> Where { get; set; }= x => x.Id == 1; 
  //public Expression<Func<Product, object>> Include { get; set; } = x => x.Brand;

  public int ProductId { get; set; }
  
  protected override void SetQuery()
  {

    base.AddWhere(x => x.Id == ProductId)
      .AddInclude(x => x.Brand!)
      .AddInclude(x => x.Category!);
      
    /*
    Query.Where(Where)
      .Include(Include);
    */
  }

 


  //void Add() { base.AddWhere(Where).AddInclude(Include); }


}
