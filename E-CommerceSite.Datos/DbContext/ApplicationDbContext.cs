using E_CommerceSite.Datos.Customer;
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
    }
}
