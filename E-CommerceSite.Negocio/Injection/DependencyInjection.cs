using E_CommerceSite.DATA;
using E_CommerceSite.Datos.Customer;
using E_CommerceSite.Datos.Seed;
using E_CommerceSite.Negocio.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace E_CommerceSite.Negocio.Injection
{
    public static class DependencyInjection
    {
        public static IServiceCollection SetDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString)
            .UseSeeding(Seeding.Aplicar)
            .UseAsyncSeeding(Seeding.AplicarAsync));

            return services;
        }

        public static IdentityBuilder SetIdentityServices(this IServiceCollection services)
        {
            return services.AddIdentity<UserEntity, IdentityRole>(o => {
                o.SignIn.RequireConfirmedAccount = false;
                o.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        }


        //public static IServiceCollection SetTransient<TService, TImplementation>(this IServiceCollection services) where TService : class where TImplementation : class, TService
        //{
        //    return services.AddTransient<TService, TImplementation>();
        //}

        public static IServiceCollection SetServices(this IServiceCollection services)
        {
            services.AddScoped<IManageUser, ManagerUser>();
            services.AddScoped<ISignInManager, ManageSignIn>();
            services.AddScoped<ICanHandleCustomers, CanHandleCustomers>();
            services.AddScoped<IICanHandleProducts, ICanHandleProducts>();
            services.AddScoped<IICanHandleCategories, ICanHandleCategories>();
            services.AddScoped<IICanHandleOrders, ICanHandleOrders>();

            return services;
        }
    }
}
