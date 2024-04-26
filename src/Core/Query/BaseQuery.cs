using Estore.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Query;

public abstract class BaseQuery<T> where T : class
{
  IRepo<T> _repo;
  internal IQueryable<T> Query { get; set; }  
  
  private BaseQuery(IRepo<T> repo) 
  {
    _repo = repo;
    Query = repo.Query();
  
  }
}
