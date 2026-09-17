using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResearchFlow.Models;
namespace ResearchFlow.Data;
public static class DbInitializer
{ 
    public static async Task InitializeAsync(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<ApplicationDbContext>();
      
        if ((await db.Database.GetPendingMigrationsAsync()).Any())
            await db.Database.MigrateAsync();
        
     
        else await db.Database.EnsureCreatedAsync();
      
        var rm = sp.GetRequiredService<RoleManager<IdentityRole>>();
      
        var um = sp.GetRequiredService<UserManager<ApplicationUser>>();
       
        foreach (var r in new[] { "LabManager", "Researcher" }) 
            if (!await rm.RoleExistsAsync(r))
                await rm.CreateAsync(new(r));
        var rows = new[] 
        { 
            ("manager@researchflow.local", "مدير المختبر", "LabManager"), ("researcher1@researchflow.local", "الباحث الأول", "Researcher"), ("researcher2@researchflow.local", "الباحث الثاني", "Researcher"), ("assistant@researchflow.local", "مساعد الباحث", "Researcher")
        };
     
        foreach (var (email, name, role) in rows)
        { 
        
            var u = await um.FindByEmailAsync(email);
        
            if (u is null) 
            {
                u = new() 
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = name 
                };
 
                var result = await um.CreateAsync(u, "Lab@2026");
               
                if (!result.Succeeded) 
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
            } 
           
            if (!await um.IsInRoleAsync(u, role)) 
                await um.AddToRoleAsync(u, role);
        }
    }
}