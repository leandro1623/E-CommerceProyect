using E_CommerceSite.DATA;
using E_CommerceSite.Datos.Entities.DeletedEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_CommerceSite.Negocio.Services
{
    public interface IICanHandleDeletedEntities
    {
        Task<int> Create(DeletedEntities entity);
    }

    public class ICanHandleDeletedEntities : IICanHandleDeletedEntities
    {
        private readonly ApplicationDbContext applicationDbContext;

        public ICanHandleDeletedEntities(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<int> Create(DeletedEntities entity)
        {
            if (entity == null) throw new ArgumentNullException("Hubo un error al crear registro de eliminacion");


            var result = applicationDbContext.DeletedEntities.Add(entity);

            return await applicationDbContext.SaveChangesAsync();
        }

    }
}
