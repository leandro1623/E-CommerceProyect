using E_CommerceSite.Models;
using E_CommerceSite.Models.Index;
using E_CommerceSite.Negocio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_CommerceSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IICanHandleProducts handleProducts;
        private readonly ICanHandleCustomers handleCustomers;
        private readonly IICanHandleOrders handleOrders;

        public HomeController(
            IICanHandleProducts handleProducts,
            ICanHandleCustomers handleCustomers,
            IICanHandleOrders handleOrders)
        {
            this.handleProducts = handleProducts;
            this.handleCustomers = handleCustomers;
            this.handleOrders = handleOrders;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel
            {
                TotalProducts =
                    await handleProducts.GetTotalCount(),

                TotalCustomers =
                    await handleCustomers.GetTotalCount(),

                TotalOrders =
                    await handleOrders.GetTotalCount(),

                TotalSales =
                    await handleOrders.GetTotalSales(),

                MonthlySales =
                    await handleOrders.GetMonthlySales(6),

                OrdersByStatus =
                    await handleOrders.GetOrdersByStatus()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
