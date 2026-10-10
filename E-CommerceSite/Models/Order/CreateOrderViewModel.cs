using System.ComponentModel.DataAnnotations;

namespace E_CommerceSite.Models.Order
{
    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Selecciona un cliente")]
        public Guid CustomerId { get; set; }

        public List<CreateOrderCustomerViewModel> Customers { get; set; } = [];

        public List<CreateOrderProductViewModel> Products { get; set; } = [];

    }

    public class CreateOrderCustomerViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    public class CreateOrderProductViewModel
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public int Stock { get; set; }

        public int Quantity { get; set; }
    }
}
