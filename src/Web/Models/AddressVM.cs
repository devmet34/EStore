using EStore.Core;
using EStore.Core.Interfaces;
namespace EStore.Web.Models;

public class AddressVM
{
  public string? Street { get; set; } = "";
  public string? Province { get;  set; } = "";

  public string? City { get;  set; } = "";

  public string? Country { get;  set; } = "";

  public int ZipCode { get;  set; }

  public string? UserId {  get; set; }
}
