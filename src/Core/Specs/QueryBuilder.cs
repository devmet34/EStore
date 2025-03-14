using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Specs;
public class QueryBuilder<T> where T : class
{
  public Expression<Func<T, bool>>? WhereExp { get; private set; }
  public List<Expression<Func<T, object>>> Includes { get; } = new();

  public QueryBuilder<T> Where(Expression<Func<T, bool>> where) { WhereExp = where; return this; }
  public QueryBuilder<T> Include(Expression<Func<T, object>> include) 
  {
    Includes.Add(include);
    return this;
  }

}
