using System;
using System.Collections.Generic;
using System.Text;

namespace E_CommerceSite.Datos.Entities.DeletedEntity
{
    public class DeletedEntities
    {
        public Guid Id { get; set; }
        public Guid DeletedEntityId { get; set; }
        public DateTime DeletedAt { get; set; }
        public required string EntityType { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string DeletedByUserId { get; set; }
    }
}
