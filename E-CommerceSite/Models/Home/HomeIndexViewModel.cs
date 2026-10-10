using E_CommerceSite.Negocio.Services;

namespace E_CommerceSite.Models.Index
{
    public class HomeIndexViewModel
    {
        public int TotalProducts { get; set; }
    
        public int TotalCustomers { get; set; }
    
        public int TotalOrders { get; set; }
    
        public decimal TotalSales { get; set; }
    
        public List<MonthlySaleViewModel> MonthlySales { get; set; }
            = new();
    
        public List<OrderStatusSummaryViewModel> OrdersByStatus { get; set; }
            = new();
    }
    
    //public class MonthlySaleViewModel
    //{
    //    public string Label { get; set; } = string.Empty;
    
    //    public decimal Total { get; set; }
    //}
    
    //public class OrderStatusSummaryViewModel
    //{
    //    public string Status { get; set; } = string.Empty;
    
    //    public int Quantity { get; set; }
    //}
}
