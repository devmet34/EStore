﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EStore.Infra.EF.Identity
{
  public class ConstIdent
  {
    //todo const identity user pass
    public const string DEFAULT_USERNAME = "user1@estore.com";
    public const string DEFAULT_PASS = "Pass@Word1";
    public const string DEFAULT_ADMIN = "admin@estore.com";

    public const string ADMIN_ROLE = "Administrators";

    
    public const int JWT_EXP_DAYS = 7;


    
  }
}
