using E_CommerceSite.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSite.DATA
{
    public class GlobalDBContext : IdentityDbContext
    {
        public GlobalDBContext(DbContextOptions options) : base(options)
        {
        }

        protected GlobalDBContext()
        {
        }

        public DbSet<UserCustomer> UserCustomer { get; set; }
    }
}
