using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Core.Exceptions;
public class RedisGenericException:Exception 
{
  public RedisGenericException() : base(Constants.redisGenericException) { }
  
}
