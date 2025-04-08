namespace EStore.Core.Exceptions;
public class RedisGenericException : Exception
{
  public RedisGenericException() : base(Constants.redisGenericException) { }

}
