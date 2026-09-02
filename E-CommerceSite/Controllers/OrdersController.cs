using E_CommerceSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSite.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            dynamic customerId = Guid.NewGuid();
            var orders = new List<OrderViewModel>();
            orders.Add(new OrderViewModel()
            {
                Id = Guid.NewGuid(),
                Total = 50,
                OrderDate = DateTime.Now,
                Status = "Completado",
                Customer = new CustomerViewModel() { Id = customerId, Name = "TestName"},
                CustomerId = customerId
            });

            return View(orders);
        }
    }
}
