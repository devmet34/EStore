using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web
{
  public class Constants
  {

    //App---------------

    public const string EstoreDbConnectionString = "Server=127.0.0.1,1434;Database=EStore; user id=admin; password=000000; MultipleActiveResultSets=true;TrustServerCertificate=True";

    public const int applicationCookieTimeoutHours = 2;

    public const string DEFAULT_USERNAME = "user1@estore.com";
    public const string DEFAULT_PASS = "Pass@Word1";
    public const string DEFAULT_ADMIN = "admin@estore.com";

    public const string ADMIN_ROLE = "Administrators";

    
    public const int JWT_EXP_DAYS = 7;


    
  }
}
