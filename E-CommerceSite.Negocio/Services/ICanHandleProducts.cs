using E_CommerceSite.DATA;
using E_CommerceSite.Datos.Entities.DeletedEntity;
using E_CommerceSite.Datos.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace E_CommerceSite.Negocio.Services
{
    public interface IICanHandleProducts
    {
        Task<int> Create(Product product);
        Task<int> Delete(Guid id);
        Task<int> Delete(Guid id, string userId);
        Task<int> Edit(Product product);
        Task<List<Product>> GetAllAsNotTraking();
        Task<int> GetTotalCount();
    }

    public class ICanHandleProducts : IICanHandleProducts
    {
        private readonly ApplicationDbContext dbContext;

        public ICanHandleProducts(ApplicationDbContext applicationDbContext)
        {
            this.dbContext = applicationDbContext;
        }

        public async Task<List<Product>> GetAllAsNotTraking()
        {
            var products = await dbContext.Products.Where(p => p.IsDeleted == false).Include(p => p.Category).AsNoTracking().ToListAsync();

            return products;
        }

        public async Task<int> Create(Product product)
        {
            dbContext.Products.Add(product);

            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> Edit(Product product)
        {
            var productIntoDb = dbContext.Products.Where(p=>p.Id == product.Id).AsTracking().FirstOrDefault();

            if(productIntoDb == null)
            {
                return -1;//no existe el producto en la base de datos
            }

            //establecemos los cambios
            productIntoDb.Name = product.Name;
            productIntoDb.Description = product.Description;
            productIntoDb.Price = product.Price;
            productIntoDb.Stock = product.Stock;
            productIntoDb.CategoryId = product.CategoryId;

            return await dbContext.SaveChangesAsync();
    
        }

        public async Task<int> GetTotalCount()
        {
            return await dbContext.Products.CountAsync();
        }

        public async Task<int> Delete(Guid id)
        {
            if (id == Guid.Empty) { return -1; }//el valor parametro recibido esta vacio, no se elimina

            var productIntoDb = await dbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (productIntoDb == null) { return -2; }// el producto no existe en la DB, no se elimina

            //verificamos si hay alguna orden vinculada a este producto, en caso de que si no permite la eliminacion del mismo por ahora
            var isInOrder = await dbContext.Orders.Include(o=>o.Items)
                .Where(o => o.Status == Datos.OrderNameSpace.OrderStatus.Pending || o.Status == Datos.OrderNameSpace.OrderStatus.Processing)
                .Where(o=>o.Items.Any(oi=>oi.ProductId == id))
                .FirstOrDefaultAsync();

            if (isInOrder != null) { return -3; }// existe almenos 1 orden asociada a este producto, no se elimina

            dbContext.Remove(productIntoDb);//marcamos para eliminarlo

            return await dbContext.SaveChangesAsync();//eliminamos
        }


        public async Task<int> Delete(Guid id, string userId)
        {
            if (id == Guid.Empty) { return -1; }//el valor parametro recibido esta vacio, no se elimina

            var productIntoDb = await dbContext.Products.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (productIntoDb == null) { return -2; }// el producto no existe en la DB, no se elimina

            //verificamos si hay alguna orden vinculada a este producto, en caso de que si no permite la eliminacion del mismo por ahora
            var isInOrder = await dbContext.Orders.Include(o => o.Items)
                .Where(o => o.Status == Datos.OrderNameSpace.OrderStatus.Pending || o.Status == Datos.OrderNameSpace.OrderStatus.Processing)
                .Where(o => o.Items.Any(oi => oi.ProductId == id))
                .FirstOrDefaultAsync();

            if (isInOrder != null) { return -3; }// existe almenos 1 orden asociada a este producto, no se elimina

            //Marcamos como eliminado
            productIntoDb.IsDeleted = true;

            //
            var deletedEntity = new DeletedEntities()
            {
                Id = Guid.NewGuid(),
                DeletedAt = DateTime.UtcNow,
                DeletedByUserId = userId,
                //deleted entity
                DeletedEntityId = productIntoDb.Id,
                EntityType = nameof(Product),
                Reason = ""
            };

            dbContext.DeletedEntities.Add(deletedEntity);

            return await dbContext.SaveChangesAsync();//eliminamos
        }


    }
}
