using E_CommerceSite.DATA;
using E_CommerceSite.Datos.Category;
using E_CommerceSite.Datos.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_CommerceSite.Negocio.Services
{
    public interface IICanHandleCategories
    {
        Task<int> Create(Category category);
        Task<int> Delete(Guid id);
        Task<int> Edit(Category category);
        Task<List<Category>> GetAll();
        Task<List<Category>> GetAllAsNotTraking();
    }

    public class ICanHandleCategories : IICanHandleCategories
    {
        private readonly ApplicationDbContext DbContext;

        public ICanHandleCategories(ApplicationDbContext applicationDbContext)
        {
            this.DbContext = applicationDbContext;
        }

        public async Task<List<Category>> GetAll()
        {
            var categories = await DbContext.Categories.ToListAsync();

            return categories;
        }

        public async Task<List<Category>> GetAllAsNotTraking()
        {
            var categories = await DbContext.Categories.AsNoTracking().ToListAsync();

            return categories;
        }

        public async Task<int> Create(Category category)
        {
            DbContext.Categories.Add(category);

            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Edit(Category category)
        {
            if (category == null || category.Id == Guid.Empty || string.IsNullOrEmpty(category.Name) || string.IsNullOrEmpty(category.Description))
            {
                return -1; //Campos incompletos en el parametro enviado
            }

            var categoryDb = DbContext.Categories.Where(c=>c.Id == category!.Id).AsTracking().FirstOrDefault();

            if (categoryDb == null)
            {
                return -2; //-2 no existe la categoria en la DB
            }

            // asignacion de campos editables

            categoryDb.Name = category.Name;
            categoryDb.Description = category.Description;

            var result = await DbContext.SaveChangesAsync();

            return result;
        }

        public async Task<int> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return -1; //El parametro pasado esta vacio
            }

            var category = await DbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if(category == null)
            {
                return -2;//-2 no existe la categoria en la Db
            }

            var IsUsed = await DbContext.Products.Where(p=> p.CategoryId == category.Id).AnyAsync();

            if(IsUsed)
            {
                return -3;// la categoria que se desea eliminar tiene productos asociados, por el momento no sera posible eliminar esta hasta que este asociada a ningun producto
            }

            DbContext.Remove(category); // se marca para eliminar

            var result = await DbContext.SaveChangesAsync(); // se envian cambios para elimanar de la db

            return result;
        }
    }
}
