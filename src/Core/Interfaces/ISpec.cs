using System.Linq.Expressions;

namespace EStore.Core.Interfaces;
public interface ISpec<T> where T : class
{

  public Expression<Func<T, bool>>? WhereExp { get; }
  public List<Expression<Func<T, object>>>? Includes { get; }

}
