using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Identity
{
  public class AppLoginModel
  {
    public string? UserName { get; set;}
    public string? Password { get; set;}

    public AppLoginModel(string? userName, string? password)
    {
      UserName = userName;
      Password = password;
    }

  }
}
