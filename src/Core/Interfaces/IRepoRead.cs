using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Estore.Core.Interfaces;
public interface IRepoRead
{
  public IQueryable<TEntity> Query<TEntity>() where TEntity : class;
}
