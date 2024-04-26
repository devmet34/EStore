
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace EStore.Infra.EF.Identity;

public class EstoreIdentityDbContextSeed
{
  public static async Task SeedAsync(EstoreIdentityDbContext identityDbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
  {  

    if (identityDbContext.Database.IsSqlServer())
    {
      identityDbContext.Database.Migrate();
    }

    await roleManager.CreateAsync(new IdentityRole(ConstIdent.ADMIN_ROLE));
    var defaultUserName = ConstIdent.DEFAULT_USERNAME;
    var defaultPass = ConstIdent.DEFAULT_PASS;
    var defaultUser = new AppUser { UserName = defaultUserName, Email = defaultUserName};
    string adminUserName = ConstIdent.DEFAULT_ADMIN;
    var adminUser = new AppUser { UserName = adminUserName,Email=adminUserName };

    var t=await userManager.CreateAsync(defaultUser, defaultPass);
    await userManager.CreateAsync(adminUser, defaultPass);
    adminUser = await userManager.FindByNameAsync(adminUserName);
    if (adminUser != null)
    {
      await userManager.AddToRoleAsync(adminUser, ConstIdent.ADMIN_ROLE);
    }
  }
}

