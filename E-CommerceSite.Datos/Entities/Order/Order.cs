using E_CommerceSite.Datos.Entities.OrderNameSpace;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Datos.OrderNameSpace
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(30)]
        public string OrderNumber { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Precision(18, 2)]
        public decimal Subtotal { get; set; }

        [Precision(18, 2)]
        public decimal Discount { get; set; }

        [Precision(18, 2)]
        public decimal Tax { get; set; }

        [Precision(18, 2)]
        public decimal Total { get; set; }

        //public bool IsDeleted { get; set; } = false;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Guid CustomerId { get; set; }

        public Customer.Customer Customer { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set; }
            = new List<OrderItem>();

    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Completed,
        Cancelled
    }
}

