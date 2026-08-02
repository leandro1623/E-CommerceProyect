namespace E_CommerceSite.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal Total { get; set; } = 50;

        public string Status { get; set; } = "Completado";

        public Guid CustomerId { get; set; }

        public Customer? Customer { get; set; }
    }
}
