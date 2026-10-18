using E_CommerceSite.DATA;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using E_CommerceSite.Negocio.Injection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using E_CommerceSite.Datos.Customer;
using E_CommerceSite.Negocio.Services;
using E_CommerceSite.Datos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

builder.Services.AddControllersWithViews(opc => opc.Filters.Add(new AuthorizeFilter(policy)));


builder.Services.SetServices();//Set all Iservice that i created for mi use in the app
builder.Services.AddScoped<ManagerUser>();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!
    ?? throw new InvalidOperationException("No se encontro la cadena de connecion con el nombre DefaultConnection");

builder.Services.SetDbContext(connectionString);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Constantes.Admin, policy => policy.RequireRole(Constantes.Admin));
});

//builder.Services.AddDbContext<GlobalDBContext>(o => o.UseSqlServer("name=DefaultConnection"));

builder.Services.SetIdentityServices();

//builder.Services.AddIdentity<UserCustomer, IdentityRole>(o =>

//    o.SignIn.RequireConfirmedAccount = false

//).AddEntityFrameworkStores<GlobalDBContext>().AddDefaultTokenProviders();

var app = builder.Build();

//set default admin test@test.com
using (var scope = app.Services.CreateScope())
{
    var adminEmail = builder.Configuration.GetSection("AdminUser")["defaultAdminEmail"];
    var adminPassword = builder.Configuration.GetSection("AdminUser")["defaultAdminPassword"];

    if (adminEmail == null || adminPassword == null)
    {
        throw new InvalidOperationException("No se ha establecido un email para el administrador por defecto");
    }

    var userManager = scope.ServiceProvider.GetRequiredService<ManagerUser>();

    var adminUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);

    if (adminUser == null)
    {
        var result = await userManager.CreateAsync("Administrator", adminEmail, adminEmail, adminPassword);

        if (!result.Succeeded)
            throw new InvalidOperationException("Ha ocurido un error al momento de crear el usuario administrador por defecto");

        adminUser = userManager.Users.FirstOrDefault(u => u.Email == adminEmail);

        if(adminUser == null) throw new InvalidOperationException($"{adminEmail} no existe en la DB");
        
        var isInRole = await userManager.IsInRoleAsync(adminUser, Constantes.Admin);

        if (isInRole == false)
        {
            await userManager.AddToRoleAsync (adminUser, Constantes.Admin);
        }
    }
    else
    {
        var isInRole = await userManager.IsInRoleAsync(adminUser, Constantes.Admin);

        if (isInRole == false)
        {
            await userManager.AddToRoleAsync(adminUser, Constantes.Admin);
        }
    }
}


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
