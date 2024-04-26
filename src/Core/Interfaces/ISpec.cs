using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Interfaces;
public interface ISpec<T> where T : class
{

  public Expression<Func<T,bool>>? WhereExp { get;  }
  public List<Expression<Func<T,object>>>? Includes { get;  }
  
}
