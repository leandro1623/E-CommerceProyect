using E_CommerceSite.Datos.OrderNameSpace;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace E_CommerceSite.Datos.Entities.OrderNameSpace
{
    public class OrderItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderId { get; set; }

        public Order Order { get; set; } = null!;

        public Guid ProductId { get; set; }

        public Product.Product Product { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal UnitPrice { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Precision(18, 2)]
        public decimal LineTotal { get; set; }

        [Precision(18, 2)]
        public decimal Subtotal { get; set; }
    }
}
