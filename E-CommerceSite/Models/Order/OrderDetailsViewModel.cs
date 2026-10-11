using E_CommerceSite.Datos.OrderNameSpace;
using E_CommerceSite.Models.Order;

namespace E_CommerceSite.Models
{
    public class OrderDetailsViewModel
    {
        public Guid Id { get; set; }

        public string OrderNumber { get; set; } = string.Empty;

        //public string CustomerName { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Tax { get; set; }

        public decimal Total { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItemDetailsViewModel> Items { get; set; } = [];

        public ProductViewModel Products { get; set; } = new();

        public CustomerViewModel Customers { get; set; } = new();
        public List<OrderDetailsViewModel> OrderList { get; set; } = [];



        //public CreateOrderViewModel CreateOrder { get; set; } = new();
    }
}
