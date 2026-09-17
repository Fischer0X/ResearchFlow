using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResearchFlow.Data;
using ResearchFlow.Models;
using ResearchFlow.Services;



var builder = WebApplication.CreateBuilder(args);

var cs = 
    builder.Configuration.GetConnectionString("DefaultConnection") ?? 
    throw new InvalidOperationException("Missing connection string");



builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(cs));


builder.Services.AddDefaultIdentity<ApplicationUser>(o => 
{ 
    o.SignIn.RequireConfirmedAccount = false;
    o.Password.RequiredLength = 8;
    o.Password.RequireNonAlphanumeric = false; 

})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews(o => o.Filters.Add(new Microsoft.AspNetCore.Mvc.AutoValidateAntiforgeryTokenAttribute()));


builder.Services.AddRazorPages();

builder.Services.AddScoped<StudyService>(); 

builder.Services.AddScoped<PhaseService>(); 

builder.Services.AddScoped<ReportService>();

var app = builder.Build(); 

if (!app.Environment.IsDevelopment()) 
{ 
    app.UseExceptionHandler("/Home/Error"); 
    app.UseHsts(); 

}


app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseRouting(); 

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); 

app.MapRazorPages();


using (var scope = app.Services.CreateScope()) 
{ 
    await DbInitializer.InitializeAsync(scope.ServiceProvider); 
}
app.Run();