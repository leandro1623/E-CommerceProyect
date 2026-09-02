using E_CommerceSite.Datos.Customer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_CommerceSite.Negocio.Services
{
    public interface IManageUser
    {
        UserManager<UserEntity> GetUserManager();
        public Task<IdentityResult> CreateAsync(string fullName, string email, string userName, string password);
    }

    public class ManagerUser : UserManager<UserEntity>, IManageUser
    {
        private readonly ICanHandleCustomers handleCustomers;

        public ManagerUser(IUserStore<UserEntity> store, IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<UserEntity> passwordHasher, IEnumerable<IUserValidator<UserEntity>> userValidators,
            IEnumerable<IPasswordValidator<UserEntity>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<UserEntity>> logger,
            ICanHandleCustomers handleCustomers) 
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.handleCustomers = handleCustomers;
        }

        public async Task<IdentityResult> CreateAsync(string fullName, string email, string userName, string password)
        {
            var customerId = await handleCustomers.CreateCustomer(null, null, null);
            UserEntity user = new UserEntity() { Fullname = fullName, Email = email, UserName = userName, CustomerId = customerId };
            
            
            return await base.CreateAsync(user, password);
        }

        public UserManager<UserEntity> GetUserManager()
        {
            return this;
        }
    }
}
