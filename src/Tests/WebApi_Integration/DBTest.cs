﻿using Estore.Core.Entities;
using Estore.Core.Services;
using Estore.Core.Specs;
using EStore.Infra.EF;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace WebApi_Integration;
public class DBTest
{
  private ITestOutputHelper _output;
  
 
  //private EfRepo<Product> _repo;
  public DBTest(ITestOutputHelper output ) 
  { 
    _output = output;
    
    //_repo = repo;
    
  }


}
