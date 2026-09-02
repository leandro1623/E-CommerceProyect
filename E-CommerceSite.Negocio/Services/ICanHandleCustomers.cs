using E_CommerceSite.DATA;
using E_CommerceSite.Datos.Customer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace E_CommerceSite.Negocio.Services
{
    public interface ICanHandleCustomers
    {
        Task<Guid> CreateCustomer(string? phone, string? address, DateTime? createDate);
        Task<int> Delete(Guid id);
        Task<int> Delete(Guid id, string currentSignedUser);
        Task<int> EditUser(Guid userId, string fullName, string email, string phone, string address);
        Task<List<Customer>> GetCustomers();
    }

    public class CanHandleCustomers : ICanHandleCustomers
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ISignInManager signInManager;

        public CanHandleCustomers(ApplicationDbContext applicationDbContext, ISignInManager signInManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.signInManager = signInManager;
        }

        public async Task<Guid> CreateCustomer(string? phone, string? address, DateTime? createDate)
        {
            Customer customer = new Customer()
            {
                //Phone = phone,
                Address = address,
                CreateDate = createDate != null ? DateTime.Parse(createDate.ToString()!) : DateTime.Now
            };

            await applicationDbContext.Customer.AddAsync(customer);
            return customer.Id;
            //return await applicationDbContext.SaveChangesAsync();

        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await applicationDbContext.Customer.Include(c=>c.UserEntity).ToListAsync();
        }

        public async Task<int> EditUser(Guid userId, string fullName, string email, string phone, string address)
        {
            var customer = await applicationDbContext.Customer.Include(c=>c.UserEntity).Where(c=>c.Id == userId).AsTracking().FirstOrDefaultAsync();

            if (customer == null)
            {
                return 0;
            }
            
            customer.UserEntity.Fullname = fullName;
            customer.UserEntity.Email = email;
            customer.UserEntity.NormalizedEmail = email.ToUpper();
            customer.UserEntity.UserName = email;
            customer.UserEntity.NormalizedUserName = email.ToUpper();
            customer.UserEntity.PhoneNumber = phone;
            customer.Address = address;

            return await applicationDbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(Guid id)
        {
            var customer = applicationDbContext.Customer.Where(c=>c.Id == id).Include(c=>c.UserEntity).FirstOrDefault();

            if(customer == null)
            {
                return 0;
            }

            if (signInManager.GetSignInManager().IsSignedIn(signInManager.GetSignInManager().Context.User) == true) await signInManager.GetSignInManager().SignOutAsync();//si el usuario que esta logueado es el mismo que se esta eliminando se desloguea al usuario

            applicationDbContext.Customer.Remove(customer);
            return await applicationDbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(Guid id, string currentSignedUser)
        {
            var customer = applicationDbContext.Customer.Where(c => c.Id == id).Include(c => c.UserEntity).FirstOrDefault();

            if (customer == null)// puede haber un error por que UserEntity puede ser null y no se esta validando actualmente
            {
                return -2; //-2 significa que el usuario que se intenta eliminar no existe en la DB
            }

            if (currentSignedUser == customer.UserEntity.Id)
            {
                return -1; // -1 Significa que se esta intentando eliminar el mismo usuario logueado
            }

            applicationDbContext.Customer.Remove(customer);
            return await applicationDbContext.SaveChangesAsync();
        }
    }
}
