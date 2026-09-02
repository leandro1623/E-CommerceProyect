namespace E_CommerceSite.Models
{
    public class OrderViewModel
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal Total { get; set; } = 50;

        public string Status { get; set; } = "Completado";

        public Guid CustomerId { get; set; }

        public CustomerViewModel? Customer { get; set; }
    }
}
