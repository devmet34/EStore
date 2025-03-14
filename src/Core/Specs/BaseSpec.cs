using EStore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Specs;
public abstract class BaseSpec<T> : ISpec<T> where T : class
{ 
  public Expression<Func<T, bool>>? WhereExp { get; private set; }
  public List<Expression<Func<T, object>>> Includes { get; private set; } = new List<Expression<Func<T, object>>>();
  public QueryBuilder<T> Query = new();
  /// <summary>
  /// Set query here adding where expression, includes etc . 
  /// e.g: protected override void SetQuery() 
  /// {base.AddWhere(x => x.Id == 2).AddInclude(x => x.Brand).AddInclude(x => x.Category); }
  /// </summary>
  protected abstract void SetQuery();
  //public BaseSpec() {  SetQuery(); }
  public BaseSpec<T> AddWhere(Expression<Func<T, bool>> where) 
  { 
    WhereExp=where;
    return this;
  }
  public BaseSpec<T> AddInclude(Expression<Func<T, object>> include)
  {
    Includes.Add(include);
    return this;
  }
  //protected abstract void SetWhereExp(Expression<Func<T, bool>> WhereExp); 
  //protected abstract void AddIncludes();
}
