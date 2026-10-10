using E_CommerceSite.Datos.Category;
using E_CommerceSite.Datos.Customer;
using E_CommerceSite.Datos.Entities.OrderNameSpace;
using E_CommerceSite.Datos.OrderNameSpace;
using E_CommerceSite.Datos.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSite.DATA
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        public DbSet<UserEntity> UserCustomer { get; set; }
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}
