using E_CommerceSite.DATA;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using E_CommerceSite.Negocio.Injection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

builder.Services.AddControllersWithViews(opc => opc.Filters.Add(new AuthorizeFilter(policy)));


builder.Services.SetServices();//Set all Iservice that i created for mi use in the app

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
    ?? throw new InvalidOperationException("No se encontro la cadena de connecion con el nombre DefaultConnection");

builder.Services.SetDbContext(connectionString);

//builder.Services.AddDbContext<GlobalDBContext>(o => o.UseSqlServer("name=DefaultConnection"));

builder.Services.SetIdentityServices();

//builder.Services.AddIdentity<UserCustomer, IdentityRole>(o =>

//    o.SignIn.RequireConfirmedAccount = false

//).AddEntityFrameworkStores<GlobalDBContext>().AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
