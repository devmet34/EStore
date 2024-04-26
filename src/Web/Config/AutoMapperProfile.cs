﻿using AutoMapper;
using Estore.Core.Entities;
using EStore.Web.Models;

namespace EStore.Web.Config;

public class AutoMapperProfile:Profile
{
  public AutoMapperProfile() 
  {
    CreateMap<Product,ProductVM>();
    CreateMap<Basket,BasketVM>();
  }
}
