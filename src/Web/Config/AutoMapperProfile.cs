using AutoMapper;
using EStore.Core.Entities;
using EStore.Core.Entities.BasketAggregate;
using EStore.Core.Models;
using EStore.Web.Models;

namespace EStore.Web.Config;

public class AutoMapperProfile:Profile
{
  public AutoMapperProfile() 
  {
    CreateMap<Product,ProductVM>();
    CreateMap<Basket, Models.BasketVM>();
    CreateMap<Basket, Core.Models.BasketVM>();
    CreateMap<CustomerAddress,AddressVM>();
    CreateMap<AddressVM, CustomerAddress>();
    CreateMap<BasketItem, BasketItemVM>();
    
  }
}
