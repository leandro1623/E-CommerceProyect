using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Datos.Order
{
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        [Precision(18,2)]
        public decimal Total { get; set; } = 50;

        public string Status { get; set; } = "Completado";

        public Guid CustomerId { get; set; }

        public Customer.Customer? Customer { get; set; }
    }
}
