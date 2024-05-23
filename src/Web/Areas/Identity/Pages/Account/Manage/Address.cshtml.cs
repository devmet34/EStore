using EStore.Infra.EF;
using Estore.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Estore.Core.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http.HttpResults;
using EStore.Web.Models;
using AutoMapper;
using NuGet.Protocol;

namespace EStore.Web.Areas.Identity.Pages.Account.Manage
{
    public class AddressModel : PageModel
  {
    private readonly EStoreDbContext _dbcontext;
    private readonly ILogger<AddressModel> logger;
    private readonly IMapper _mapper;
    

    [BindProperty]
    public AddressVM? Address { get; set; }

    public AddressModel(EStoreDbContext context, ILogger<AddressModel> logger,IMapper mapper)
    {
      this.logger=  logger;
      _dbcontext = context;
      _mapper = mapper;
    }

    [TempData]
    public string Status { get; set; }

    public async Task OnGetAsync()
    {      
      var customerAddress=await GetCustomerAddress();
      Address=_mapper.Map<AddressVM>(customerAddress);
    }


    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
        throw new ArgumentException();
      var customerAddress=await GetCustomerAddress();
      var address=_mapper.Map<AddressVM>(customerAddress);
      
      if (Helper.AreObjectsEqual(address,Address!))
      {
        return Page();
      }

      if (Address!=null)
        customerAddress!.Update(Address.Street!,Address.Province,Address.City!,Address.Country!,Address.ZipCode);
      
      await _dbcontext.SaveChangesAsync();

      Status = "success";
      return RedirectToPage();
      
    }

    private async Task< CustomerAddress?> GetCustomerAddress()
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      return await _dbcontext.CustomerAddresses.Where(c => c.UserId == userId).FirstOrDefaultAsync();
    }

  }
}
